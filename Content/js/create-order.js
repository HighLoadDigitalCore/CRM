$().ready(function () {
    $('#ProductForm input[name="Article"]').val('');
    $('#ProductForm input[name="Name"]').val('');
    $('#ProductForm input[name="Price"]').val('');
    $('#ProductForm input[name="Amount"]').val('1');

    initOrders();
});

function addProduct() {
    $.get('/Shop/ProductForm', { ID: $('#OrderID').val(), ShopID: $('#ShopID').val() }, function (d) {
        $('#ProductForm').replaceWith(d);
        autoCompleteProduct();
    });
    return false;
}

function refreshOrderProducts() {
    $.get('/Shop/ProductsList', { ID: $('#OrderID').val() }, function (d) {
        $('#OrdersTable').replaceWith(d);
        initOrders();
    });
    $.get('/Shop/SubmitForm', { ID: $('#OrderID').val() }, function (d) {
        $('#SubmitForm').replaceWith(d);
    });
}

function deleteOrderedProduct(obj) {
    $.post('/Shop/DeleteProduct', { ID: $(obj).attr('arg') }, function (d) {
        refreshOrderProducts();
    });
    return false;
}

function editOrderedProduct(obj) {
    $.get('/Shop/ProductForm', { ID: $('#OrderID').val(), OrderedProductID: $(obj).attr('arg'), ShopID: $('#ShopID').val() }, function (d) {
        $('#ProductForm').replaceWith(d);
        autoCompleteProduct();
    });
    return false;
}

function saveChar(obj) {
    $.post('/Shop/AddCharToDB', { name: $(obj).parents('.form-group').find('label').text().trim(), value: $(obj).parents('.form-group').find('input').val(), id: $('#ProductForm input[name="ID"]').val(), shop: $('select#ShopID').val(), type: 1 }, function (d) {
        if (d == '1') {
            $(obj).remove();
            document.location.reload();
        }
    });

    return false;
}

function saveCharOrder(obj) {
    var txt = $(obj).parents('.form-group').find('label').text().trim();
    $.post('/Shop/AddCharToDB', { name: txt.substr(0, txt.length - 1), value: $(obj).parents('.form-group').find('input').val(), id: $('#ProductForm input[name="ID"]').val(), shop: $('select#ShopID').val(), type: 3 }, function (d) {
        if (d == '1') {
            $(obj).remove();
            document.location.reload();
        }
    });

    return false;
}

function addChar() {
    var count = parseInt($('#charCount').val());
    count++;
    var html = '<div class="form-group" rel="char"><div class="col-sm-4"><input type="text" class="form-control" placeholder="Название" name="CharName_' + count + '"/></div><div class="col-sm-6"><input type="text" class="form-control" placeholder="Значение" value="" name="Char_' + count + '"></div><div class="col-sm-2"><a href="#" onclick="return removeLine(this);" class="glyphicon glyphicon-remove"></a></div></div>';
    $('#ProductForm').find('div[rel="char"]:last').after(html);
    $('#charCount').val(count);
    return false;
}

function addOrderChar() {
    //debugger;
    var count = parseInt($('#charOrderCount').val());
    count++;
    var html = '<div class="form-group" rel="char"><div class="col-sm-4"><input type="text" class="form-control" placeholder="Название" name="CharName_' + count + '"/></div><div class="col-sm-6"><input type="text" class="form-control" placeholder="Значение" value="" name="Char_' + count + '"></div><div class="col-sm-2"><a href="#" onclick="return removeLine(this);" class="glyphicon glyphicon-remove"></a></div></div>';
    $('#WarningBlock').find('div[rel="char"]:last').after(html);
    $('#charOrderCount').val(count);
    return false;
}

function removeLine(obj) {
    var txt = $(obj).parents('.form-group').find('label').text().trim();
    $.post('/Shop/RemoveChar', { name: txt, type: 1, order: $('#WarningBlock input[name="ID"]').val(), shop: $('select#ShopID').val() }, function (d) {
        if (d == '1') {
            $(obj).parents('.form-group').remove();
        }
    });

    return false;
}

function removeLineOrder(obj) {
    var txt = $(obj).parents('.form-group').find('label').text().trim();
    $.post('/Shop/RemoveChar', { name: txt.substr(0, txt.length - 1), type: 3, order: $('#WarningBlock input[name="ID"]').val(), shop: $('select#ShopID').val() }, function (d) {
        if (d == '1') {
            $(obj).parents('.form-group').remove();
        }
    });

    return false;
}

function initOrders() {
    initDataTableHelper();
    autoCompleteProduct();
    autoCompleteUser();
}

function split(val) {
    return val.split(/,\s*/);
}

function extractLast(term) {
    return split(term).pop();
}


function autoCompleteProduct() {

    $("#ProductForm input[name='Article']")
        .autocomplete({
            source: function (request, response) {
                $.getJSON("/Shop/ArtSearch", {
                    query: extractLast(request.term)
                }, response);
            },
            search: function () {
                var term = extractLast(this.value);
                if (term.length < 1) {
                    return false;
                }
            },
            focus: function () {
                return false;
            },
            select: function (event, ui) {

                loadProductByID(ui.item.value);

                return false;
            }
        });

    $("#ProductForm input[name='Name']")
        .autocomplete({
            source: function (request, response) {
                $.getJSON("/Shop/NameSearch", {
                    query: extractLast(request.term)
                }, response);
            },
            search: function () {
                var term = extractLast(this.value);
                if (term.length < 1) {
                    return false;
                }
            },
            focus: function () {
                return false;
            },
            select: function (event, ui) {

                loadProductByID(ui.item.value);

                return false;
            }
        });


}


function autoCompleteUser() {

    /*$('input[name="Street"]').keydown(function() {

        if ($(this).val().length > 1) {
            var str = $('input[name="Region"]').val();
            if (str.length)
                str += ', ';
            str += $('input[name="Town"]').val();
            if ($('input[name="Town"]').val().length)
                str += ', ';
            str += $(this).val();

            ymaps.suggest(str).then(function(items) {
                console.log(items);
                // items - массив поисковых подсказок
            });
        }
    });*/

    $('input[name="Street"]').autocomplete({
        source: function (request, response) {
            var street = $('input[name="Street"]');
            if (street.val().length > 1) {
                var str = $('input[name="Region"]').val();
                if (str.length)
                    str += ', ';
                str += $('input[name="Town"]').val();
                if ($('input[name="Town"]').val().length)
                    str += ', ';
                str += street.val();

                ymaps.suggest(str).then(response/*function (items) {
                    console.log(items);
                    // items - массив поисковых подсказок
                }*/);
            }

        },
        search: function () {
            var term = extractLast(this.value);
            if (term.length < 1) {
                return false;
            }
        },
        focus: function (event, ui) {
            $("#project").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            if (ui.item.displayName.indexOf(',') > 0) {
                $('input[name="Street"]').val(ui.item.displayName.substr(0, ui.item.displayName.indexOf(',')));
                $('input[name="Town"]').val($.trim(ui.item.displayName.substr(ui.item.displayName.indexOf(',') + 1, ui.item.displayName.substr(ui.item.displayName.indexOf(',') + 1).indexOf(','))));
            } else {
                $('input[name="Street"]').val(ui.item.value);
            }
            //console.log(ui.item.displayName.substr(0, ui.item.displayName.indexOf(',')));
            //$('input[name="Street"]').val(ui.item.value);
            //loadUserByID(ui.item.value);
            return false;
        }
    });

    
    $('input[name="Region"]').autocomplete({
        source: function (request, response) {
            $.getJSON("/Shop/RegionSearch", {
                query: extractLast(request.term)
            }, response);
        },
        search: function () {
            var term = extractLast(this.value);
            if (term.length < 1) {
                return false;
            }
        },
        focus: function () {
            return false;
        },
        select: function (event, ui) {
            $('input[name="Region"]').val(ui.item.value);
            //loadUserByID(ui.item.value);
            return false;
        }
    });
    $('input[name="Town"]').autocomplete({
        source: function (request, response) {
            $.getJSON("/Shop/CitySearch", {
                query: extractLast(request.term),
                region: $('input[name="Region"]').val()
            }, response);
        },
        search: function () {
            var term = extractLast(this.value);
            if (term.length < 1) {
                return false;
            }
        },
        focus: function () {
            return false;
        },
        select: function (event, ui) {
            $('input[name="Town"]').val(ui.item.value);
            //loadUserByID(ui.item.value);
            return false;
        }
    });


    $("input[name='Surname']")
        .autocomplete({
            source: function (request, response) {
                $.getJSON("/Shop/UserSearch", {
                    query: extractLast(request.term)
                }, response);
            },
            search: function () {
                var term = extractLast(this.value);
                if (term.length < 1) {
                    return false;
                }
            },
            focus: function () {
                return false;
            },
            select: function (event, ui) {
                loadUserByID(ui.item.value);
                return false;
            }
        });

    $("input[name='Phone']")
        .autocomplete({
            source: function (request, response) {
                if (request.term.length > 3) {
                    $.getJSON("/Shop/UserSearchByPhone", {
                        query: extractLast(request.term)
                    }, response);
                }
            },
            search: function () {
                var term = extractLast(this.value);
                if (term.length < 3) {
                    return false;
                }
            },
            focus: function () {
                return false;
            },
            select: function (event, ui) {
                loadUserByID(ui.item.value);
                return false;
            }
        });
    $("input[name='Email']")
        .autocomplete({
            source: function (request, response) {
                $.getJSON("/Shop/UserSearchByEmail", {
                    query: extractLast(request.term)
                }, response);
            },
            search: function () {
                var term = extractLast(this.value);
                if (term.length < 2) {
                    return false;
                }
            },
            focus: function () {
                return false;
            },
            select: function (event, ui) {
                loadUserByID(ui.item.value);
                return false;
            }
        });


}

var ro = null;
function saveReadOnly() {
    ro = $('input, select').filter('[disabled="disabled"]');
    ro.removeAttr('disabled');
    setTimeout(function() {
        ro.attr('disabled', 'disabled');
    }, 200);
}

function calculateContractor(obj) {
    
    if ($(obj).val().length) {
        $('#ContractorCostCell').show();
        if ($(obj).find('option:selected').attr('price')) {
            $('input[name="ContractorCost"]').val($(obj).find('option:selected').attr('price'));
            $('input[name="ContractorCost"]').attr('disabled', 'disabled');
            $('select[name="ContractorCostType"]').val('0');
            $('select[name="ContractorCostType"]').attr('disabled', 'disabled');
        } else {
            $('input[name="ContractorCost"]').removeAttr('disabled');
            $('select[name="ContractorCostType"]').removeAttr('disabled');
        }
    } else {
        $('#ContractorCostCell').hide();
    }
}

function loadUserByID(id) {


    $.get('/Shop/UserInfo', { id: id }, function (data) {
        $('#CustomerForm input[name="Surname"]').val(data.Surname);
        $('#CustomerForm input[name="Sex"]').prop('checked', false);
        $('#CustomerForm input[name="Sex"]').filter('[value="' + data.Sex + '"]').prop('checked', true);
        $('#CustomerForm input[name="Name"]').val(data.Name);
        $('#CustomerForm input[name="Patrinomic"]').val(data.Patrinomic);
        $('#CustomerForm input[name="Phone"]').val(data.Phone);
        $('#CustomerForm input[name="AddPhone"]').val(data.AddPhone);
        $('#CustomerForm input[name="Email"]').val(data.Email);
        $('#CustomerForm input[name="CustomerID"]').val(data.CustomerID);
        $('#DeliveryBlock input[name="Region"]').val(data.DeliveryAddress.Region);
        $('#DeliveryBlock input[name="Town"]').val(data.DeliveryAddress.Town);
        $('#DeliveryBlock input[name="Street"]').val(data.DeliveryAddress.Street);
        $('#DeliveryBlock input[name="House"]').val(data.DeliveryAddress.House);
        $('#DeliveryBlock input[name="Building"]').val(data.DeliveryAddress.Building);
        $('#DeliveryBlock input[name="Section"]').val(data.DeliveryAddress.Section);
        $('#DeliveryBlock input[name="Doorway"]').val(data.DeliveryAddress.Doorway);
        $('#DeliveryBlock input[name="Code"]').val(data.DeliveryAddress.Code);
        $('#DeliveryBlock input[name="Floor"]').val(data.DeliveryAddress.Floor);
        $('#DeliveryBlock input[name="Flat"]').val(data.DeliveryAddress.Flat);
    });
}

function loadProductByID(id) {
    $.get('/Shop/ProductInfo', { id: id, shop: $('select#ShopID').val() }, function (data) {
        $('#ProductForm input[name="Article"]').val(data.Article);
        $('#ProductForm input[name="Name"]').val(data.Name);
        $('#ProductForm input[name="Price"]').val(data.Price);
        $('#ProductForm input[name="Amount"]').val('1');
        $('#ProductForm input[name="Weight"]').val('1');
        $('#ProductForm select[name="UnitCode"]').val(data.UnitCode);
        $('#ProductForm input[name="SelfCost"]').val(data.SelfCost);
        $('#ProductForm input[name="OptCost"]').val(data.OptCost);

        $('#ProductForm .form-group[arg="char-cell"]').remove();

        for (var i = 0; i < data.Chars.length; i++) {
            var html = '<div arg="char-cell" rel="char" class="form-group">' +
                '<label class="col-sm-4 control-label">' + data.Chars[i].Name + '</label>' +
                '<input type="hidden" value="' + data.Chars[i].Name + '" name="CharName_' + data.Chars[i].Name + '"/>' +
                '<div class="col-sm-6"><input type="text" name="Char_' + data.Chars[i].Name + '" value="' + data.Chars[i].Value + '" class="form-control"></div><div class="col-sm-2">';
            if (!data.Chars[i].Exist) {
                html += '<a style="margin-right: 10px;" onclick="return saveChar(this)" title="Добавить характеристику в настройки базы" class="glyphicon glyphicon-plus" href="#"></a>';
            }
            html += '<a title="Удалить из списка" onclick="return removeLine(this)" arg="' + data.Chars[i].Name + '" class="glyphicon glyphicon-remove" href="#"></a></div></div>';
            $(html).insertAfter('#ProductForm .form-group[rel="char"]:last');
            /*
                                var label = $('label:contains("' + data.Chars[i].Name + '")');
                                if (label.length) {
                                    label.parent().find('input').val(data.Chars[i].Value);
                                } else {

                                }
            */
        }
        $('#charCount').val(data.Chars.length);
    });
}

