var dts = new Array();

function initTable($this, index) {

    $this.find('tfoot th').each(function (index) {
        if ($(this).attr('arg') == "sum") {
            var sum = 0;
            $('table[id^="example"] tbody tr').each(function () {
                var td = $(this).find('td:eq(' + index + ')');
                sum += parseFloat($.trim($(td).html()));
            });
            $(this).html('<b>Всего: ' + sum + '</b>');
        }
    });

    $this.wrap("<div class='adv-scroller'></div>");
    $('<button onclick="resetState(' + index + ');" class="ColVis_Button ColVis_MasterButton" style="float:right">Сбросить фильтры</button>').insertAfter($this.parents('.adv-table'));
    var exclude = new Array();
    if ($this.hasClass('exclude-first'))
        exclude.push(0);
    if ($this.hasClass('exclude-second'))
        exclude.push(1);

    if ($this.hasClass('exclude-last'))
        exclude.push($this.find('tr:first-child').find('td').length - 1);


    var filterArray = new Array();
    if ($this.find('.fr').length) {
        $this.find('.fr th').each(function (index) {
            if ($(this).attr('type') == 'dropdown') {

                var values = new Array();
                $(this).parents('table').find('tr').each(function () {
                    var td = $(this).find('td:eq(' + index + ')');
                    var text = td.text().trim();
                    if (text.length && jQuery.inArray(text, values) < 0) {
                        values.push(text);
                    }
                });




                filterArray.push({ type: $(this).attr('type'), values: values });
            } else {
                filterArray.push({ type: $(this).attr('type') });
            }
        });
    }

    var dt = $this.dataTable({
        "sDom": 'C<clear>lfrtip',
        "bStateSave": true,
        "oLanguage": {
            "sProcessing": "Подождите...",
            "sLengthMenu": "Показать _MENU_ записей",
            "sZeroRecords": "Записи отсутствуют.",
            "sInfo": "Записи с _START_ до _END_ из _TOTAL_ записей",
            "sInfoEmpty": "Записи с 0 до 0 из 0 записей",
            "sInfoFiltered": "(отфильтровано из _MAX_ записей)",
            "sInfoPostFix": "",
            "sSearch": "Поиск:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Первая",
                "sPrevious": "Предыдущая",
                "sNext": "Следующая",
                "sLast": "Последняя"
            },
            "oAria": {
                "sSortAscending": ": активировать для сортировки столбца по возрастанию",
                "sSortDescending": ": активировать для сортировки столбцов по убыванию"
            }
        },
        "aLengthMenu": [[20, 50, 100, -1], [20, 50, 100, "Все"]],
        'aoColumnDefs': [
            {
                'bSortable': false,
                'aTargets': ['nosort']
            }
        ],
        /*"scrollX": true,*/
        colVis: {
            exclude: exclude,
            "fnStateChange": function (iColumn, bVisible) {
                $this.removeAttr('style');
                $this.DataTable().columns.adjust().draw();
            }
        },
        "fnDrawCallback": function (oSettings) {
            //console.log('DataTables has redrawn the table');
            var items = '';
            $this.find('td.num').each(function (index) {
                $(this).html(index + 1);
                try {
                    items += $(this).parent().attr('arg') + ';';
                    $('#PointsList').val(items);
                } catch (e) {

                }
            });
            initGrafIfCan(items);
            //$('table[id^="example"]').DataTable().columns.adjust().draw();
        }
    });
    if (filterArray.length) {
        dt.columnFilter({
            sPlaceHolder: "head:after",
            bUseColVis: true,
            aoColumns: filterArray
        });
    } else {
        dt.columnFilter({
            sPlaceHolder: "head:after",
            bUseColVis: true
        });
    }

    new $.fn.dataTable.ColReorder(dt);
    return dt;

}

var graphs = new Array();
function initGrafIfCan(items) {
    for (var i = 0; i < graphs.length; i++) {
        graphs[i].object.loadGraph(items);
    }

    if (typeof loadOrders === "function") {
        loadOrders(items);
    }
}


function viewPeriod(list) {

    $('#ListOverride').val(list);
    $('#SearchFormCell form').submit();
}

function exportToXls() {
    var lst = '';
    $('input[name="groupCbx"]:checked').each(function () {
        lst += $(this).attr('arg') + ";";
    });

    document.location.href = '/Export/XLS?List=' + lst;
}
function exportToTorg() {
    var lst = '';
    $('input[name="groupCbx"]:checked').each(function () {
        lst += $(this).attr('arg') + ";";
    });

    document.location.href = '/Export/Torg12?List=' + lst;
}

function toggleAll(obj) {

    $(obj).parents('table').find('tr td input[type="checkbox"]').prop('checked', $(obj).is(':checked'));
    createActions();
}

function reloadFuncList(obj) {
    $.get('/Common/GetGraphFuncs', { ID: $(obj).val() }, function (d) {
        $('#TypeID').html(d);
    });

}

function showHideGraph(id) {
    
    var cook = $.cookie('Graph_' + id);
    var target = 'hidden';
    if (cook && cook == 'hidden') {
        target = 'showed';
    }
    $.cookie('Graph_' + id, target, { expires: 365, path: '/' });
    
    if (target == 'hidden') {
        $('.graph-cell.panel-body[arg="' + id + '"]').hide();
        $('.graph-cell[arg="' + id + '"]').find('.graph-hide-show').html('Показать');
    } else {
        $('.graph-cell.panel-body[arg="' + id + '"]').show();
        $('.graph-cell[arg="' + id + '"]').find('.graph-hide-show').html('Свернуть');
    }
}

function closeGraphFormAndReload(reload, id) {
    $('#graphCreate').modal('hide');
    $('.modal-backdrop').remove();
    $('body').removeClass('modal-open');

    if (reload == 1) {
        document.location.reload();
    } else {
        var form = $('#GraphFilter_' + id).find('form');
        if (form)
            form.submit();

    }

}
function editGraph(id) {
    showGraphForm(id);
}

function createActions() {
    if ($('input[name="groupCbx"]:checked').length) {
        $('#GroupActions').show();
    } else {
        $('#GroupActions').hide();
    }
    $('input[name="groupCbx"]').each(function () {
        if ($(this).is(':checked')) {
            $(this).parents('tr').addClass('chk');
        } else {
            $(this).parents('tr').removeClass('chk');
        }
    });
}

function reloadGraphicsUI(arg) {

    var selected = arg.split(',');
    for (var i = 0; i < selected.length; i++) {
        var form = $('#GraphFilter_' + selected[i]).find('form');
        if (form)
            form.submit();
    }
}

function initGraphCreateUI() {
    $('#GraphForm select[multiple="multiple"]').multiSelect({
        selectableOptgroup: true,
        afterSelect: function (values) {
            var selected = $('#SerialListPlane').val().split(',');
            if (selected[0] == "")
                selected.splice(0, 1);

            for (var i = 0; i < values.length; i++) {
                if ($.inArray(values[i], selected) < 0) {
                    selected.push(values[i]);
                }
            }
            $('#SerialListPlane').val(selected.join(','));
            console.log($('#SerialListPlane').val());
        },
        afterDeselect: function (values) {

            var selected = $('#SerialListPlane').val().split(',');
            if (selected[0] == "")
                selected.splice(0, 1);

            for (var i = 0; i < values.length; i++) {
                var index = $.inArray(values[i], selected);
                if (index >= 0) {
                    selected.splice(index, 1);
                }
            }

            $('#SerialListPlane').val(selected.join(','));
            console.log($('#SerialListPlane').val());

        }
    });

}
function initSerialCreateUI() {
    $('#SearchSaveContent select[multiple="multiple"]').multiSelect({
        selectableOptgroup: true,
        afterSelect: function (values) {
            var selected = $('#GraphListPlane').val().split(',');
            if (selected[0] == "")
                selected.splice(0, 1);

            for (var i = 0; i < values.length; i++) {
                if ($.inArray(values[i], selected) < 0) {
                    selected.push(values[i]);
                }
            }
            $('#GraphListPlane').val(selected.join(','));
            console.log($('#GraphListPlane').val());
        },
        afterDeselect: function (values) {

            var selected = $('#GraphListPlane').val().split(',');
            if (selected[0] == "")
                selected.splice(0, 1);

            for (var i = 0; i < values.length; i++) {
                var index = $.inArray(values[i], selected);
                if (index >= 0) {
                    selected.splice(index, 1);
                }
            }

            $('#GraphListPlane').val(selected.join(','));
            console.log($('#GraphListPlane').val());

        }
    });

}


function showContrSearch() {
    $('#contrSearch').modal('show');
}


function initRegions() {
    var regions = getRegions();
    $('.serial-item').attr('loaded', '0');
    for (var i = 0; i < regions.length; i++) {
        
        $('.serial-item').each(function() {
            if ($.trim($(this).find('span').text()) == regions[i]) {
                $(this).attr('loaded', '1');
                $(this).find('input').prop('checked', 'checked');
            }
        });
        
    }
    $('.serial-item[loaded="0"]').find('input').removeProp('checked');
}

function deleteRegion(obj) {
    var regions = getRegions();
    var reg = $.trim($(obj).parent().find('span').text());
    var index = $.inArray(reg, regions);
    if (index >= 0) {
        regions.splice(index, 1);
    } 
    $.cookie('Regions', regions.join(';'), { expires: 365, path: document.location.href });
    reloadPage();

}

function toggleRegion(obj) {

    var regions = getRegions();
    var reg = $.trim($(obj).parent().find('span').text());
    var index = $.inArray(reg, regions);
    if (index >= 0 && $(obj).not(':checked')) {

        regions.splice(index, 1);
    } else if(index<0 && $(obj).is(':checked')) {
        regions.push(reg);
    }
    $.cookie('Regions', regions.join(';'), { expires: 365, path: document.location.href });
}

function reloadPage() {
    document.location.reload();
}

function getRegions() {
    var regions = $.cookie('Regions');
    if (!regions)
        regions = '';

    var splitted = regions.split(';');
    return splitted;
}

function showGraphForm(id) {
    $.get('/Common/OrderSearchGraphPopupContent', { id: id }, function (d) {
        $('#GraphForm').replaceWith(d);
        initGraphCreateUI();
        $('#graphCreate').modal('show');
        return false;
    });
    return false;
}

function saveShopRels() {
    $.post('/Products/SaveShops', { ids: $('#ShopListPlane').val(), id: $('#ProductIDForShops').val() }, function(d) {
        $('#shoplist').modal('hide');
        $('.modal-backdrop').remove();
        $('body').removeClass('modal-open');

        reloadShopList();
    });
}

function showTrading(ShopID, ProductID) {
    $.post('/Products/ViewTradingHistoryPartial', { ShopID: ShopID, ProductID: ProductID }, function(d) {
        $('#Trading').replaceWith(d);
        $('#TradingList').modal('show');
        
    });
    return false;
}

function reloadShopList() {
    $.get('/Products/ShopList', { id: $('#Shops').attr('arg') }, function(data) {
        $('#Shops').replaceWith(data);
        dts[1] = initTable($('#Shops').find('#example'), 1);
        initShopForm();
    });
}
function reloadRefillList() {
    $.get('/Products/RefillingList', { id: $('#Refils').attr('arg') }, function (data) {
        $('#Refils').replaceWith(data);
        dts[2] = initTable($('#Refils').find('#example'), 2);
        initRefillForm();
        reloadStoreList();
    });
}
function reloadStoreList() {
    $.get('/Products/StoresList', { id: $('#Stores').attr('arg') }, function (data) {
        $('#Stores').replaceWith(data);
        dts[0] = initTable($('#Stores').find('#example'), 0);
        
    });
}

function initShopForm() {
    $('#ShopList').multiSelect({
        selectableOptgroup: true,
        afterSelect: function (values) {
            var selected = $('#ShopListPlane').val().split(',');
            if (selected[0] == "")
                selected.splice(0, 1);

            for (var i = 0; i < values.length; i++) {
                if ($.inArray(values[i], selected) < 0) {
                    selected.push(values[i]);
                }
            }
            $('#ShopListPlane').val(selected.join(','));
            console.log($('#ShopListPlane').val());
        },
        afterDeselect: function (values) {

            var selected = $('#ShopListPlane').val().split(',');
            if (selected[0] == "")
                selected.splice(0, 1);

            for (var i = 0; i < values.length; i++) {
                var index = $.inArray(values[i], selected);
                if (index >= 0) {
                    selected.splice(index, 1);
                }
            }

            $('#ShopListPlane').val(selected.join(','));
            console.log($('#ShopListPlane').val());

        }
    });
}



function initRefillForm() {
    $('#ShopList').multiSelect({
        selectableOptgroup: true,
        afterSelect: function (values) {
            var selected = $('#ShopListPlane').val().split(',');
            if (selected[0] == "")
                selected.splice(0, 1);

            for (var i = 0; i < values.length; i++) {
                if ($.inArray(values[i], selected) < 0) {
                    selected.push(values[i]);
                }
            }
            $('#ShopListPlane').val(selected.join(','));
            console.log($('#ShopListPlane').val());
        },
        afterDeselect: function (values) {

            var selected = $('#ShopListPlane').val().split(',');
            if (selected[0] == "")
                selected.splice(0, 1);

            for (var i = 0; i < values.length; i++) {
                var index = $.inArray(values[i], selected);
                if (index >= 0) {
                    selected.splice(index, 1);
                }
            }

            $('#ShopListPlane').val(selected.join(','));
            console.log($('#ShopListPlane').val());

        }
    });
}


function initSaveForm() {
    var lst = '';
    $('input[name="groupCbx"]:checked').each(function () {
        lst += $(this).attr('arg') + ";";
    });

    $.get('/Common/OrderSearchSavePopupContent', { uids: lst }, function (d) {
        $('#SearchSaveContent').replaceWith(d);
        $('#SearchSaveContent select[multiple="multiple"]').multiSelect({
            selectableOptgroup: true,
            afterSelect: function (values) {
                var selected = $('#GraphListPlane').val().split(',');
                if (selected[0] == "")
                    selected.splice(0, 1);

                for (var i = 0; i < values.length; i++) {
                    if ($.inArray(values[i], selected) < 0) {
                        selected.push(values[i]);
                    }
                }
                $('#GraphListPlane').val(selected.join(','));
                console.log($('#GraphListPlane').val());
            },
            afterDeselect: function (values) {

                var selected = $('#GraphListPlane').val().split(',');
                if (selected[0] == "")
                    selected.splice(0, 1);

                for (var i = 0; i < values.length; i++) {
                    var index = $.inArray(values[i], selected);
                    if (index >= 0) {
                        selected.splice(index, 1);
                    }
                }

                $('#GraphListPlane').val(selected.join(','));
                console.log($('#GraphListPlane').val());

            }
        });
    });
}

function createDelivery() {
    var lst = '';
    $('input[name="groupCbx"]:checked').each(function () {
        lst += $(this).attr('arg') + ";";
    });
    document.location.href = '/Delivery/DeliveryMap?List=' + lst;
}

function loadSpamContent(template) {
    if (template.length > 0) {
        $.get('/Common/LoadSpamContent', { template: template }, function(d) {
            if (d) {
                $('#SendHeader').val(d.Header);
                $('#SendBody').val(d.Template);
            }
            if ($('#SendBody').length && $('#SendType').val() == 'mail') {
                CKEDITOR.replace('SendBody', {
                    filebrowserBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html',
                    filebrowserImageBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html?type=Images',
                    filebrowserFlashBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html?type=Flash',
                    filebrowserUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files',
                    filebrowserImageUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images',
                    filebrowserFlashUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash'
                });
            } else {
                var editor = CKEDITOR.instances['SendBody'];
                if (editor) { editor.destroy(true); }
            }
        });
    }
    if ($('#SendBody').length && $('#SendType').val() != 'mail') {
        var editor = CKEDITOR.instances['SendBody'];
        if (editor) { editor.destroy(true); }
    }
}

function sendSpam() {
    if ($('#SendHeader').val().trim().length == 0 || $('#SendBody').val().trim().length == 0) {
        $('#spamResult').html('Необходимо указать заголовок и текст сообщения');
        $('#spamResult').css('color', 'red');
        return false;
    }
    var lst = '';
    $('input[name="groupCbx"]:checked').each(function () {
        lst += $(this).attr('arg') + ";";
    });

    $.post('/Shop/SendSpam', { type: $('#SendType').val(), header: $('#SendHeader').val(), message: $('#SendBody').val(), list: lst }, function (d) {
        $('#spamResult').html('Рассылка успешно произведена');
        $('#spamResult').css('color', 'green');
    });
}

function changeStatus(status) {
    var lst = '';
    $('input[name="groupCbx"]:checked').each(function () {
        lst += $(this).attr('arg') + ";";
    });
    $.post('/Shop/ChangeStatus', { Status: status, List: lst }, function () {
        $('input[name="groupCbx"]:checked').each(function () {
            $(this).prop('checked', false);
        });
        $('input[name="toggler"]:checked').each(function () {
            $(this).prop('checked', false);
        });

        document.location.reload();

    });
    return false;
}

function deleteSerial(id) {
    $.post('/Graph/DeleteSerial', { id: id }, function(d) {
        $('.serial-item[arg="' + id + '"]').remove();
    });
}
function showSeqEdit() {
    $.get('/Graph/SequenceEditor', {}, function(d) {
        $('#GraphForm').html(d);
    });
}
function sendToContractor(id) {
    var lst = '';
    $('input[name="groupCbx"]:checked').each(function () {
        lst += $(this).attr('arg') + ";";
    });
    $.post('/Shop/SendContractor', { ID: id, List: lst }, function () {
        $('input[name="groupCbx"]:checked').each(function () {
            $(this).prop('checked', false);
        });
        $('input[name="toggler"]:checked').each(function () {
            $(this).prop('checked', false);
        });
        document.location.reload();
    });
    return false;
}
function changeWarning(status) {
    var lst = '';
    $('input[name="groupCbx"]:checked').each(function () {
        lst += $(this).attr('arg') + ";";
    });
    $.post('/Shop/ChangeWarning', { Status: status, List: lst }, function () {
        $('input[name="groupCbx"]:checked').each(function () {
            $(this).prop('checked', false);
        });
        $('input[name="toggler"]:checked').each(function () {
            $(this).prop('checked', false);
        });

        document.location.reload();
    });
    return false;
}

function resetState(index) {
    var dt = dts[index];
    if (dt != null) {
        dt.api().state.clear();
        window.location.reload();
    }
}

function changeFilterPeriod(obj, period) {
    if ($(obj).hasClass('disabled'))
        return false;

    $(obj).parents('.periods').find('input[name="Days"]').val(period);
    $(obj).parents('.periods').find('.period').removeClass('active');
    $(obj).addClass('active');

    $(obj).parents('form').submit();
}

function saveRefilling() {
    $.post('/Products/Refill', { ID: $('#ProductID').val(), StoreID: $('#StoreID').val(), Date: $('#Date').val(), Price: $('#Price').val(), Amount: $('#Amount').val(), Unit: $('#Unit').val() }, function(d) {
        $('#messageBox').html('<span style="color:red">' + d + '</span>');
        if (!d.length) {
            $('#addproduct').modal('hide');
            $('.modal-backdrop').remove();
            $('body').removeClass('modal-open');
            reloadRefillList();
        }
    });
}

function initSearchForm() {
    $('.autopost input, .autopost select').filter(':not(.skip-auto)').change(function () {
        $(this).parents('form').submit();
    });
    
    if (jQuery.isFunction(jQuery.fn.datepicker)) {
        
        $('input[arg="date"]').datepicker({
            format: 'dd.mm.yyyy',
            today: "Сегодня",
            weekStart: 1
        })

        $('.autopost input[arg="date"]').on('changeDate', function (e) {
            $('input[arg="date"]').datepicker('hide');
            $(this).parents('form').submit();
        }).on('clearDate', function (e) {
            $('input[arg="date"]').datepicker('hide');
            $(this).parents('form').submit();
        });

    }
}
function initDataTableHelper(obj, index) {
    initSearchForm(obj);

    if ($.fn.dataTable) {
        if ($('table[id^="example"]').length) {
            $('table[id^="example"]').each(function (index) {
                var $this = $(this);
                dts[index] = initTable($this, index);

            });

            $('<div class="clear"></div>').insertBefore('.adv-scroller');
        }
    };
}


(function ($) {
    $.fn.dataTable.Api.registerPlural('tables().state.clear()', 'table().state.clear()', function (opts) {
        return this.iterator('table', function (ctx) {
            try {
                (ctx.iStateDuration === -1 ? sessionStorage : localStorage).removeItem(
                    'DataTables_' + ctx.sInstance + '_' + location.pathname
                );
            } catch (e) { }
        });
    });

    $.fn.dataTable.Api.register('state.clearAll()', function () {
        var clearStorage = function (key, storage) {
            if (key.match(/^DataTables_/) !== null) {
                try {
                    storage.removeItem(key);
                } catch (e) { }
            }
        };

        try {
            $.each(window.localStorage, function (key) {
                clearStorage(key, window.localStorage);
            });

            $.each(window.sessionStorage, function (key) {
                clearStorage(key, window.sessionStorage);
            });
        } catch (e) { }

        return this;
    });
}(jQuery));