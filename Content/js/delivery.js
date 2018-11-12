var myMap = null;
var zonesList = new Array();

var orders = new Array();
var ordersLoaded = false;
var ordersTimer = null;
var route = null;

var mapReady = false;
var blocksReady = [0, 0, 0];

function isReadyForRoute() {
    for (var i = 0; i < blocksReady.length; i++) {
        if (blocksReady[i] == 0)
            return false;
    }
    return true;
}

ymaps.ready(initMap);
function initMap() {

    //initDeliveryUI();

    //console.log('initMap');
    if (myMap == null) {
        myMap = new ymaps.Map('map', {
            // При инициализации карты обязательно нужно указать
            // её центр и коэффициент масштабирования.
            center: [55.75222, 37.61556], // Москва
            zoom: 11
        });
        myMap.controls.add('zoomControl');
        mapReady = true;
    }
}

$().ready(function () {
    var d = $.cookie('ExactDateDelivery');
    if (!d || !d.length) {
        var dt = new Date();
        d = pad(dt.getDate(), 2) + '.' + pad(dt.getMonth() + 1, 2) + '.' + dt.getFullYear();
        $.cookie('ExactDateDelivery', d, { expires: 365, path: '/' });
    }
    $('input[arg="date"]').val(d);
    refreshBlocks();
    setSelects();
    loadMapInterface();
    $('input[arg="date"]').on('changeDate', function (e) {
        $.cookie('ExactDateDelivery', $('input[arg="date"]').val(), { expires: 365, path: '/' });
        $('input[arg="date"]').datepicker('hide');
        refreshBlocks();
    }).on('clearDate', function (e) {
        $.cookie('ExactDateDelivery', $('input[arg="date"]').val(), { expires: 365, path: '/' });
        $('input[arg="date"]').datepicker('hide');
        refreshBlocks();
    });
});


function changeDelivererForGroup(list, key, type) {
    $.get('/Delivery/ChangeDelivererForGroup', { list: list, group: key, type: type }, function (data) {
        /*$('#DeliveryPopupContent').replaceWith(data);
        $('#DeliveryPopup').modal('show');*/
        showDialog(data);

    });
    return false;
}

function changeWorkerGroup(car, list) {
    $.get('/Delivery/ChangeWorkerGroup', { car: car, list: list }, function (data) {
        /*
                $('#DeliveryPopupContent').replaceWith(data);
                $('#DeliveryPopup').modal('show');
        */
        showDialog(data);
    });
    return false;

}

function saveWorkerGroup(car) {
    var ch = $('.modal-body input[type="radio"]:checked');
    if (!ch.length) {
        $('.modal-body #error').html('Необходимо выбрать экипаж из списка');
        return false;
    }

    $.post('/Delivery/SaveWorkerGroup', { car: car, group: ch.val() }, function (data) {
        refreshBlocks();
    });
    $('#DeliveryPopup').modal('hide');
    return false;
}

function saveDelivererForGroup(list, group, type) {
    var ch = $('.modal-body input[type="radio"]:checked');
    if (!ch.length) {
        $('.modal-body #error').html('Необходимо выбрать доставщика из списка');
        return false;
    }
    $.post('/Delivery/ChangeDelivererForGroup', { list: list, group: group, type: type, target: ch.val(), ttype: ch.attr('tp') }, function (data) {
        refreshBlocks();
    });
    $('#DeliveryPopup').modal('hide');

}

function pad(num, size) {
    var s = num + "";
    while (s.length < size) s = "0" + s;
    return s;
}

function approveList(val) {
    if ($('#errorCellApprove').html() != '' && val == '1') {
        return false;
    } else {
        $.post('/Delivery/ApproveListChange', { date: $('#ExactDateDelivery').val(), val: val }, function(data) {
            if (data.length) {
                $('#errorCellApprove').html(data);
            } else {
                document.location.reload();
            }
        });
    }
}

function setApproveVisible() {
    if (isReadyForRoute()) {
        setApproveVisibleInner();
    } else {
        setTimeout(function () {
            setApproveVisible();
        }, 100);
    }
}

function setApproveVisibleInner() {
    if ($('input[name="IsApproved"]').length) {
        var approved = $('input[name="IsApproved"]').val();
        if (approved == '0') {
            $('#ApproveList').show();
            console.log('HasUndef =' + $('input[name="HasUndef"]').val());
            if ($('input[name="HasUndef"]').val() == '1' || $('input[name="HasUndefCarRoutes"]').val() == '1' || $('input[name="HasUndefCourierRoutes"]').val() == '1' || $('input[name="HasUndefCarDrivers"]').val()=='1') {
                var msg = 'Внимание! Для утверждения маршрутного у всех заказов должен быть выбран доставщик и расположение на карте';
                if ($('input[name="HasUndefCarDrivers"]').val() == '1') {
                    msg += '<br>Внимание! Необходимо выбрать экипажи для всех транспортных средств!';
                }
                $('#errorCellApprove').html(msg);

            } else {
                $('#errorCellApprove').html('');
            }
        } else {
            $('#ApproveList').hide();
            $('#ApproveListRepeat').show();
        }
    } else {
        $('#ApproveList').hide();
    }

}

function addRoutePointsToList(obj) {
    var stores = '';
    var orders = '';
    if ($('#RouteStores').length) {
        $('#RouteStores input[type="checkbox"]:checked').each(function () {
            stores += $(this).val() + ';';
        });
    }
    if ($('#OrderPointList').length) {
        $('#OrderPointList input[type="checkbox"]:checked').each(function () {
            orders += $(this).val() + ";";
        });
    }
    $.post('/Delivery/AddPointsToList', { stores: stores, orders: orders, group: $(obj).attr('group'), type: $(obj).attr('tp'), shopid: $(obj).attr('shop'), date: $('#ExactDateDelivery').val() }, function (d) {
        refreshBlocks();
    });
}

function refreshBlocks() {
    console.log('ExactDateDelivery');
    blocksReady[0] = 0;
    blocksReady[1] = 0;
    blocksReady[2] = 0;

    $.get('/Delivery/CheckList', { Date: $('input[arg="date"]').val(), ShopID: $('#ShopID').val() }, function (data) {
        $.get('/Delivery/CarRoutes', { Date: $('input[arg="date"]').val(), ShopID: $('#ShopID').val() }, function (data) {
            $('#CarRoutes').replaceWith(data);
            
            loadReorder('#CarRoutes');
            blocksReady[0] = 1;
        });
        $.get('/Delivery/CourierRoutes', { Date: $('input[arg="date"]').val(), ShopID: $('#ShopID').val() }, function (data) {
            $('#CourierRoutes').replaceWith(data);
            loadReorder('#CourierRoutes');
            blocksReady[1] = 1;
        });
        $.get('/Delivery/UndefRoutes', { Date: $('input[arg="date"]').val(), ShopID: $('#ShopID').val() }, function (data) {
            $('#UndefRoutes').replaceWith(data);
            blocksReady[2] = 1;
        });

    });
    setApproveVisible();
    drawRoutes();

}

function loadReorder(obj) {
    $(obj).find('.route-info').not('.skip').sortable({
        /*connectWith: ".column",*/
        handle: ".move",
        /*cancel: ".portlet-toggle",*/
        placeholder: "route-placeholder",
        stop: function (event, ui) {
            var lst = '';
            $(ui.item).parent().find('.order-route').each(function (index) {
                lst += $(this).attr('arg') + ';';
            });
            $.post('/Delivery/SaveOrder', { list: lst }, function (data) {
                refreshBlocks();
            });
        }
    });
}

function addRoutePoints(obj) {
    $.post('/Delivery/RoutePoints', { type: $(obj).attr('tp'), group: $(obj).attr('arg'), shopid: $('#ShopID').val() }, function (data) {
        showDialog(data);
        /*
                $('#DeliveryPopupContent').replaceWith(data);
        */
        initAddRouteDatePicker();
        /*
                $('#DeliveryPopup').modal('show');
        */
    });
}

function initAddRouteDatePicker() {
    $('#DeliveryPopupContent #RouteOrderDateFilter').datepicker({
        format: 'dd.mm.yyyy',
        today: "Сегодня",
        weekStart: 1,

    });

    $('#DeliveryPopupContent #RouteOrderDateFilter').on('changeDate', function (e) {

        $('#DeliveryPopupContent #RouteOrderDateFilter').datepicker('hide');
        reloadOrders();
    }).on('clearDate', function (e) {
        $('#DeliveryPopupContent #RouteOrderDateFilter').datepicker('hide');
        reloadOrders();
    });
}

function reloadOrders() {
    var data = JSON.parse($("#RoutePointOrders").attr('data'));
    $.post('/Delivery/RoutePointsOrdersFilter', { Date: $('#DeliveryPopupContent #RouteOrderDateFilter').val(), Group: data.Group, Type: data.Type, ShopID: data.ShopID }, function (d) {
        $('#RoutePointOrders').replaceWith(d);
        initAddRouteDatePicker();
    });
}

function removeFromList(obj) {
    if ($(obj).attr('type') == "store") {
        $.post('/Delivery/RemoveStore', { id: $(obj).attr('arg') }, function (data) {
            refreshBlocks();
        });
    }
    if ($(obj).attr('type') == "order") {
        $.post('/Delivery/RemoveOrder', { id: $(obj).attr('arg') }, function (data) {
            showDialog(data);
            /*$('#DeliveryPopupContent').replaceWith(data);*/
            $('#DeliveryPopupContent input[arg="date"]').datepicker({
                format: 'dd.mm.yyyy',
                today: "Сегодня",
                weekStart: 1,

            });
            /*$('#DeliveryPopup').modal('show');*/
        });
    }
    return false;
}

function changeDeliveryDate(obj) {
    $.post('/Delivery/ChangeDate', { ID: $('#DeliveryPopupContent input[arg="date"]').attr('order'), NewDate: $('#DeliveryPopupContent input[arg="date"]').val(), Notify: $('#DeliveryPopupContent #Notify').val() }, function (data) {
        if (data.length) {
            alert(data);
            return;
        } else {
            $('#DeliveryPopup').modal('hide');
            refreshBlocks();
        }
    });
}

function setDeliveryType(obj) {
    $.post('/Delivery/ChangeType', { target: $(obj).attr('arg'), type: $(obj).val() }, function (data) {
        refreshBlocks();
    });
}

function setDeliverer(obj) {
    $.post('/Delivery/ChangeDeliverer', { target: $(obj).attr('arg'), uid: $(obj).val(), type: $(obj).attr('type') }, function (data) {
        refreshBlocks();
    });
}

function changeReturnPoint(obj) {
    $.post('/Delivery/ChangeReturnPoint', { arg: $(obj).attr('arg'), group: $(obj).attr('group'), value: $(obj).val(), type: $(obj).attr('tp') }, function (data) {
        refreshBlocks();
    });
}

function changeMapShow(obj) {
    $.post('/Delivery/ChangeMapShow', { type: $(obj).attr('tp'), arg: $(obj).attr('arg'), group: $(obj).attr('group'), val: $(obj).is(':checked') ? '1' : '0' }, function (data) {
        refreshBlocks();
    });
}

var adressMap;
var adressPoint;
function detectAdress(obj) {
    $.post('/Delivery/LocationEdit', { OrderID: $(obj).attr('arg') }, function (data) {
        showDialog(data);
        /*$('#DeliveryPopupContent').replaceWith(data);*/
        var d = JSON.parse($('#OrderPointData').val());
        /*if (adressMap == null) {*/
        adressMap = new ymaps.Map('adressMap', {
            // При инициализации карты обязательно нужно указать
            // её центр и коэффициент масштабирования.
            center: [d.PointData.Lat, d.PointData.Lng], // Москва
            zoom: 17
        });
        adressMap.controls.add('zoomControl');
        /*};*/
        /*$('#DeliveryPopup').modal('show');*/

        adressPoint = new ymaps.GeoObject({
            // Описание геометрии.
            geometry: {
                type: "Point",
                coordinates: [d.PointData.Lat, d.PointData.Lng]
            },
            // Свойства.
            properties: {
                // Контент метки.
                iconContent: "Заказ №" + d.Name,
                balloonContent: d.Adress
            }
        }, {
            // Опции.
            // Иконка метки будет растягиваться под размер ее содержимого.
            preset: 'islands#blueStretchyIcon',
            // Метку можно перемещать.
            draggable: true
        });

        adressMap.geoObjects.add(adressPoint);
        adressPoint.events.add('dragend', function (e) {
            console.log(adressPoint.geometry.getCoordinates());
            $('#coords').attr('lat', adressPoint.geometry.getCoordinates()[0]);
            $('#coords').attr('lng', adressPoint.geometry.getCoordinates()[1]);
            $('#coords').html('Координаты: ' + adressPoint.geometry.getCoordinates()[0] + ";" + adressPoint.geometry.getCoordinates()[1]);
        });

    });
    return false;
}

function saveCoords() {
    $.post('/Delivery/SaveCoords', { lat: $('#coords').attr('lat').replace('.', ','), lng: $('#coords').attr('lng').replace('.', ','), id: $('#coords').attr('arg') }, function (d) {
        refreshBlocks();
    });

}

function showDialog(data, width) {
    $('#DeliveryPopupContent').replaceWith(data);
    if (width) {
        $('.modal-dialog').css('width', width + 'px');
    } else {
        $('.modal-dialog').css('width', '');
    }
    $('#DeliveryPopup').modal('show');
}

function showOrderInfo(id) {
    $.get('/Delivery/OrderInfo', { id: id }, function (data) {
        showDialog(data, 900);

    });
    return false;
}

function setSelects() {
    var ZoneFilter = $.cookie('ZoneFilter');
    if (!ZoneFilter || !ZoneFilter.length) {
        ZoneFilter = '0';
        $.cookie('ZoneFilter', '0', { expires: 365, path: '/' });
    }

    $('#ZoneFilter select').val(ZoneFilter);
    $('#ZoneFilter select').change(function () {
        $.cookie('ZoneFilter', $('#ZoneFilter select').val(), { expires: 365, path: '/' });
        drawZones();
    });


    var RouteFilter = $.cookie('RouteFilter');
    if (!RouteFilter || !RouteFilter.length) {
        RouteFilter = '0';
        $.cookie('RouteFilter', '0', { expires: 365, path: '/' });
    }

    $('#RouteFilter select').val(RouteFilter);
    $('#RouteFilter select').change(function () {
        $.cookie('RouteFilter', $('#RouteFilter select').val(), { expires: 365, path: '/' });
        drawRoutes();
    });
}

function loadMapInterface() {
    if (!mapReady) {
        setTimeout(function () {
            loadMapInterface();
        }, 200);
    } else {
        loadMapUI();
    }
}

function drawZones() {
    redrawSectors();
}

function drawRoutes() {
    if (isReadyForRoute() && myMap != null) {
        drawRoutesOnMap();
    } else {
        setTimeout(function () {
            drawRoutes();
        }, 100);
    }
    //drawStores();
}


function loadMapUI() {
    redrawSectors();
}

var routeArray = new Array();
var stores = new Array();
function clearRoutes() {
    for (var i = 0; i < routeArray.length; i++) {
        if (myMap != null && routeArray[i] != null) {
            myMap.geoObjects.remove(routeArray[i].route);
        }
    }
    routeArray = new Array();

    for (var i = 0; i < stores.length; i++) {
        if (myMap != null && stores[i] != null) {
            myMap.geoObjects.remove(stores[i].store);
        }
    }
    stores = new Array();
}

function drawRoutesOnMap() {
    $('.btn').tooltip();
    clearRoutes();
    var filter = $.cookie('RouteFilter');
    if (!filter) {
        filter = '0';
        $.cookie('RouteFilter', '0', { expires: 365, path: '/' });
    }
    $('.delivery-group[skip="0"]').each(function (index) {
        var routeList = $(this);
        var color = $(this).attr('color').replace('#', '');
        var uid = $(this).attr('prefix') + "_" + $(this).attr('arg');
        var rwl = new Array();




        routeList.find('.order-route').each(function () {
            var pos = JSON.parse($(this).attr('pos'));
            rwl.push({ type: 'wayPoint', point: [pos.Lat, pos.Lng] });
            console.log(rwl.length);
            if ($(this).attr('type') == 'store') {
                var store = new ymaps.GeoObject({
                    // Описание геометрии.
                    geometry: {
                        type: "Point",
                        coordinates: [pos.Lat, pos.Lng]
                    },
                    // Свойства.
                    properties: {
                        // Контент метки.
                        iconContent: pos.Name,
                        balloonContent: pos.FullAdress
                    }
                }, {
                    // Опции.
                    // Иконка метки будет растягиваться под размер ее содержимого.
                    preset: 'islands#redStretchyIcon',
                    // Метку можно перемещать.
                    draggable: false
                });
                myMap.geoObjects.add(store);
                stores[index] = { store: store };
            }
        });
        if (routeList.find('#BackRoute').val() != '') {
            try {
                var p = JSON.parse(routeList.find('#BackRoute option:selected').attr('pos'));
                rwl.push({ type: 'wayPoint', point: [p.Lat, p.Lng] });

                var store = new ymaps.GeoObject({
                    // Описание геометрии.
                    geometry: {
                        type: "Point",
                        coordinates: [p.Lat, p.Lng]
                    },
                    // Свойства.
                    properties: {
                        // Контент метки.
                        iconContent: p.Name,
                        balloonContent: p.FullAdress
                    }
                }, {
                    // Опции.
                    // Иконка метки будет растягиваться под размер ее содержимого.
                    preset: 'islands#redStretchyIcon',
                    // Метку можно перемещать.
                    draggable: false
                });

                myMap.geoObjects.add(store);
                stores[index] = { store: store };

            } catch (e) {

            }
        }


        ymaps.route(rwl, {
            mapStateAutoApply: true
            //multiRoute: true
        }).then(function (r) {
            r.getPaths().options.set({
                // в балуне выводим только информацию о времени движения с учетом пробок
                balloonContentBodyLayout: ymaps.templateLayoutFactory.createClass('{{ properties.humanLength }}'),
                // можно выставить настройки графики маршруту
                strokeColor: color,
                opacity: 0.8
            });
            var pathsLen = r.getPaths().getLength();
            for (var l = 0; l < pathsLen; l++) {
                r.getPaths().get(l).options.set({
                    // в балуне выводим только информацию о времени движения с учетом пробок
                    //balloonContentBodyLayout: ymaps.templateLayoutFactory.createClass('{{ properties.humanLength }}'),
                    balloonContentBodyLayout: ymaps.templateLayoutFactory.createClass('{{ properties.humanLength }}'),
                    // можно выставить настройки графики маршруту
                    hasBalloon: true,
                    hasHint: false,
                    //strokeColor: getRandomColor(),
                });
            }

            var points = r.getWayPoints();
            points.options.set('preset', 'islands#grayCircleDotIcon');


            /*
        var pointLen = points.getLength();
        $('.paths-list').html('');
                        var html = '';
                        for (var i = 0; i < pointLen; i++) {
                            if (i < pointLen - 1) {
                                var start = points.get(i).geometry.getCoordinates();
                                var end = points.get(i + 1).geometry.getCoordinates();
                                var path = getAdress(start) + " - " + getAdress(end);
                                var dist = r.getPaths().get(i).getHumanLength();
                                var time = r.getPaths().get(i).getHumanTime();
                                html += '<div class="path">' + (i + 1) + ". " + path + ' (<b>' + dist + ', ' + time + '</b>) </div>';
                            }
                        }
                        $('.paths-list').html(html);
                        $('#allRouteInfo').html('Длина маршрута - ' + Math.round(r.getLength() / 1000) + ' км');*/

            routeList.find('.route-info-len #length').html(Math.round(r.getLength() / 1000));
            if ((filter != '3' && filter == routeList.attr('type')) || filter == '0') {
                if (routeList.find('#ShowOnMap').is(':checked')) {
                    routeArray[index] = { route: r, uid: uid };
                    myMap.geoObjects.add(r);
                }
            }
        });

    });
}

/*
function changeLastSegment(obj) {
    $.cookie('DeliveryLastSegment', $(obj).is(':checked') ? '1' : '0', { expires: 365, domain: '/' });
    loadAllRoutes();
}

function initDeliveryUI() {
    if ($.cookie('DeliveryLastSegment') == '1') {
        $('.show-last-segment input').prop('checked', 'checked');
    }
}
*/




function redrawSectors() {

    for (var i = 0; i < zonesList.length; i++) {
        myMap.geoObjects.remove(zonesList[i].poly);
    }
    zonesList = new Array();

    $.get('/Delivery/SectorList', { ShopID: $('#ShopID').val() }, function (d) {
        var items = $(d).find('.sector-item');
        var filter = $.cookie('ZoneFilter');
        if (filter == '3')
            return;
        draw(items, filter);
        ordersTimer = 0;
    });

}

function draw(o, filter) {
    o.each(function () {
        if (filter == '0' || $(this).attr('type') == filter) {
            var coord = JSON.parse($(this).attr('points'));


            var points = new Array();
            for (var j = 0; j < coord.length; j++) {
                var point = new Array();
                point.push(coord[j].Lat);
                point.push(coord[j].Lng);
                points.push(point);
            }

            var list = new Array();
            list.push(points);

            var p = new ymaps.GeoObject({
                geometry: {
                    type: "Polygon",
                    coordinates: list
                },
                properties: {
                    hintContent: $(this).find('.span-edit').text()
                }

            }, {
                strokeColor: $(this).attr('color'),
                fillColor: $(this).attr('color'),
                fillOpacity: 0.5
            });
            p.Driver = $(this).attr('driver');
            myMap.geoObjects.add(p);
            zonesList.push({ poly: p, id: $(this).attr('arg') });
        }
    });

    myMap.setBounds(myMap.geoObjects.getBounds());
}


function drawRoute(storeid, orderid, obj) {
    if (route) {
        myMap.geoObjects.remove(route);
    }
    $('#showAll').removeClass('active');
    $('.route').removeClass('active');
    $(obj).parents('.route').addClass('active');
    if (stores.length && orders.length && storeid > 0) {
        var store = null;
        for (var i = 0; i < stores.length; i++) {
            if (stores[i].id == storeid) {
                store = stores[i].store;
            }
        }
        if (store == null) return;
        var routePoints = new Array();
        routePoints.push(store.geometry.getCoordinates());

        var order = null;
        for (var i = 0; i < orders.length; i++) {
            if (orders[i].id == orderid) {
                order = orders[i].order;
            }
        }
        if (order == null) return;
        routePoints.push(order.geometry.getCoordinates());



        var multiRouteModel = new ymaps.multiRouter.MultiRouteModel(routePoints, {
            avoidTrafficJams: true,
        });

        var multiRouteView = new ymaps.multiRouter.MultiRoute(multiRouteModel);
        route = multiRouteView;
        myMap.geoObjects.add(multiRouteView);

        // Подписываемся на события модели мультимаршрута.
        multiRouteView.model.events
            .add("requestsuccess", function (event) {
                var routes = event.get("target").getRoutes();
                //console.log("Найдено маршрутов: " + routes.length);
                for (var i = 0, l = routes.length; i < l; i++) {
                    //  console.log("Длина маршрута " + (i + 1) + ": " + routes[i].properties.get("distance").text);
                }
            })
            .add("requestfail", function (event) {
                //console.log("Ошибка: " + event.get("error").message);
            });
    }

}

var routeList = new Array();
function drawRouteDirect(point1, point2) {
    if (point1 && point2) {
        var routePoints = new Array();
        routePoints.push(point1);
        routePoints.push(point2);


        var multiRouteModel = new ymaps.multiRouter.MultiRouteModel(routePoints, {
            avoidTrafficJams: true,
            results: 1
        });

        var multiRouteView = new ymaps.multiRouter.MultiRoute(multiRouteModel);
        myMap.geoObjects.add(multiRouteView);
        routeList.push(multiRouteView);

        // Подписываемся на события модели мультимаршрута.
        multiRouteView.model.events
            .add("requestsuccess", function (event) {
                //var routes = event.get("target").getRoutes();


                var points = event.get("target").getWayPoints();
                if (points.length) {
                    //points.options.set({ opacity: 0 });
                    //// Задаем стиль метки - иконки будут красного цвета, и
                    //// их изображения будут растягиваться под контент
                    //points.options.set('preset', 'twirl#redStretchyIcon');
                    //// Задаем контент меток в начальной и конечной точках
                    //points[0].properties.set('iconContent', 'Точка отправления');
                    //points[1].properties.set('iconContent', 'Точка прибытия');
                    //points[0].properties.set({ opacity: 0 });
                    //points[1].properties.set({ opacity: 0 });
                    //console.log(points[0]);
                }

            })
            .add("requestfail", function (event) {
            });
    }

}



function drawStores() {
    $('.store-data').each(function () {
        var d = JSON.parse($(this).val());
        if (d.Lat > 0) {
            var store = new ymaps.GeoObject({
                // Описание геометрии.
                geometry: {
                    type: "Point",
                    coordinates: [d.Lat, d.Lng]
                },
                // Свойства.
                properties: {
                    // Контент метки.
                    iconContent: $(this).attr('nm'),
                    balloonContent: d.FullAdress
                }
            }, {
                // Опции.
                // Иконка метки будет растягиваться под размер ее содержимого.
                preset: 'islands#redStretchyIcon',
                // Метку можно перемещать.
                draggable: false
            });

            //var store = new ymaps.Placemark([d.Lat, d.Lng], null, { draggable: false });
            store.FullAdress = d.FullAdress;
            myMap.geoObjects.add(store);
            stores.push({ id: $(this).attr('uid') * 1, store: store });
        }

    });
}

function loadOrders(list) {
    return;
    if (ordersTimer == 0) {
        drawOrders(list);
    } else {
        setTimeout(function () {
            loadOrders(list);
        }, 200);
    }

}

function movePointOrder(uid, order) {
    order = order * 1;
    var sorted = orders.sort(function (x, y) {
        return x.num > y.num;
    });

    console.log(sorted);
    var oldnum = 0;
    var target;
    for (var k = 0; k < sorted.length; k++) {
        if (sorted[k].id == uid) {
            target = sorted[k];
            oldnum = sorted[k].num;
            break;
        }
    }
    for (var i = 0; i < sorted.length; i++) {
        if (order > oldnum) {
            if (i > oldnum) {
                sorted[i].num = sorted[i].num - 1;
            }

        }
        else if (order < oldnum) {
            if (i >= order) {
                sorted[i].num = sorted[i].num + 1;
            }

        }
    }
    target.num = order;
    sorted = sorted.sort(function (x, y) {
        return x.num > y.num;
    });


    for (var j = 0; j < sorted.length; j++) {
        sorted[j].num = j;
        $('.route[arg="' + sorted[j].id + '"] .num').html((j + 1) + ".");
    }


    var routes = $('.route').sort(function (x, y) {
        return $(x).find('.num').text().replace('.', '') * 1 > $(y).find('.num').text().replace('.', '') * 1;
    });

    var h = '';
    for (var l = 0; l < routes.length; l++) {
        h += $(routes[l]).get(0).outerHTML;
    }
    $('.routes').html(h);

    //console.log(routes);

    /*
        $('.route').each(function (index) {
            $(this).find('.num').html((index + 1) + '.');
        });
    */

    loadAllRoutes();
}

function createOrderPointGeo(d, index, filter, count) {
    var geocoder = ymaps.geocode(d.PointData.FullAdress);
    geocoder.then(
        function (res) {
            //console.log(d.PointData.FullAdress);


            var select = '';
            for (var j = 0; j < count; j++) {
                select += '<option style="width:25px" ' + (j == index ? 'selected' : '') + ' value="' + j + '">' + (j + 1) + '</option>';
            }
            select = "<br><b>Порядок доставки:</b>&nbsp;&nbsp;<select class='num-changer' uid='" + d.ID + "'>" + select + "</select>";

            console.log(1);
            var geoCoords = res.geoObjects.get(0).geometry.getCoordinates();
            var order = new ymaps.GeoObject({
                // Описание геометрии.
                geometry: {
                    type: "Point",
                    coordinates: geoCoords
                },
                // Свойства.
                properties: {
                    // Контент метки.
                    iconContent: d.Name,
                    balloonContent: "<div class='adress'>" + d.PointData.FullAdress + "</div>" + select
                }
            }, {
                // Опции.
                // Иконка метки будет растягиваться под размер ее содержимого.
                // Метку можно перемещать.
                preset: 'islands#blueStretchyIcon',
                draggable: false
            });

            order.events.add('balloonopen', function (e) {
                var content = order.properties.get('balloonContent');
                var sel = $(content).filter('select');
                var ord = 0;
                var uid = sel.attr('uid') * 1;
                for (var k = 0; k < orders.length; k++) {
                    if (orders[k].id == uid) {
                        sel.val(orders[k].num);
                        ord = orders[k].num;
                        break;
                    }
                }

                var s = '';
                for (var j = 0; j < count; j++) {
                    s += '<option  style="width:25px" ' + (j == ord ? 'selected' : '') + ' value="' + j + '" >' + (j + 1) + '</option>';
                }
                s = "<br><b>Порядок доставки:</b>&nbsp;&nbsp;<select onchange='movePointOrder(" + uid + ", $(this).val())' class='num-changer' uid='" + d.ID + "'>" + s + "</select>";

                var adress = $(content).filter('.adress').text();
                console.log("<div class='adress'>" + adress + "</div>" + s);
                order.properties.set('balloonContent', "<div class='adress'>" + adress + "</div>" + s);

            });

            console.log(2);
            if (!d.Driver.length) {
                for (var i = 0; i < zonesList.length; i++) {

                    if (zonesList[i].poly.geometry.contains(order.geometry.getCoordinates())) {
                        d.Driver = zonesList[i].poly.Driver;
                        $.post('/Delivery/SaveDriver', { ID: d.ID, Driver: zonesList[i].poly.Driver }, function (d) {
                        });
                        break;
                    }

                }
            }
            console.log(3);
            if (d.Driver != filter && filter.length) {

                orderCount--;
                return;
            }


            order.Driver = d.Driver;
            myMap.geoObjects.add(order);
            myMap.setBounds(myMap.geoObjects.getBounds());
            console.log(4);
            orders.push({ id: d.ID, order: order, num: orders.length });
            createRouteLine(d, index, order);

            orderCount--;
        },
        function (err) {
            // обработка ошибки
        }
    );

}
function createOrderPoint(d, index, filter, count) {


    var select = '';
    for (var j = 0; j < count; j++) {
        select += '<option style="width:25px" ' + (j == index ? 'selected' : '') + ' value="' + j + '">' + (j + 1) + '</option>';
    }
    select = "<br><b>Порядок доставки:</b>&nbsp;&nbsp;<select class='num-changer' uid='" + d.ID + "'>" + select + "</select>";


    var order = new ymaps.GeoObject({
        // Описание геометрии.
        geometry: {
            type: "Point",
            coordinates: [d.PointData.Lat, d.PointData.Lng]
        },
        // Свойства.
        properties: {
            // Контент метки.
            iconContent: d.Name,
            balloonContent: "<div class='adress'>" + d.PointData.FullAdress + "</div>" + select
        }
    }, {
        // Опции.
        // Иконка метки будет растягиваться под размер ее содержимого.
        // Метку можно перемещать.
        preset: 'islands#blueStretchyIcon',
        draggable: false
    });

    if (!d.Driver.length) {
        for (var i = 0; i < zonesList.length; i++) {

            if (zonesList[i].poly.geometry.contains(order.geometry.getCoordinates())) {
                d.Driver = zonesList[i].poly.Driver;
                $.post('/Delivery/SaveDriver', { ID: d.ID, Driver: zonesList[i].poly.Driver }, function (d) {
                });
                break;
            }

        }
    }

    if (d.Driver != filter && filter.length) {
        orderCount--;
        return;
    }

    order.Driver = d.Driver;
    myMap.geoObjects.add(order);
    myMap.setBounds(myMap.geoObjects.getBounds());
    orders.push({ id: d.ID, order: order, num: orders.length });
    createRouteLine(d, index, order);
    orderCount--;

}

var orderCount = 0;
function drawOrders(list) {

    $('.routes .route').remove();
    if (route) {
        myMap.geoObjects.remove(route);
    }
    for (var i = 0; i < orders.length; i++) {
        myMap.geoObjects.remove(orders[i].order);

    }
    orders = new Array();

    var filter = $('#DriversFilter').val();
    if (!filter)
        filter = '';

    $.get('/Delivery/OrdersData', { ids: list, ShopID: $('#ShopID').val() }, function (d) {

        $('#Drivers').html('');
        if (!filter.length)
            $('#DriversFilter').html('');
        $('#Drivers').append('<option value=""></option>');
        if (!filter.length)
            $('#DriversFilter').append('<option value="">Все маршруты</option>');
        for (var i = 0; i < d.Drivers.length; i++) {
            $('#Drivers').append('<option value="' + d.Drivers[i] + '">' + d.Drivers[i] + '</option>');
            if (!filter.length)
                $('#DriversFilter').append('<option value="' + d.Drivers[i] + '">' + d.Drivers[i] + '</option>');
        }

        orderCount = d.Points.length;

        for (var i = 0; i < d.Points.length; i++) {
            if (d.Points[i].Lat > 0) {
                createOrderPoint(d.Points[i], i + 1, filter, d.Points.length);

            } else {
                createOrderPointGeo(d.Points[i], i + 1, filter, d.Points.length);

            }

        }
        loadAllRoutes();

    });
}


var reordered = false;
function getRandomColor() {
    var letters = '0123456789ABCDEF'.split('');
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
function drawAllRoutes(obj) {

    if (!stores.length || !orders.length) {
        return;
    }
    $('.route').removeClass('active');
    $('#showAll').addClass('active');
    var pointZero = null;
    if (obj == null) {
        pointZero = stores[0].store.geometry.getCoordinates();
    } else pointZero = obj;

    //if (!reordered) {
    //    autoreorder(pointZero, null, 0);
    //    syncLines();
    //}

    for (var j = 0; j < routeList.length; j++) {
        myMap.geoObjects.remove(routeList[j]);

    }
    var exclude = new Array();
    var rwl = new Array();



    rwl.push({ type: 'wayPoint', point: pointZero });

    var sorted = orders.sort(function (a, b) {
        return a.num > b.num;
    });


    for (var k = 0; k < sorted.length; k++) {
        rwl.push({ type: 'wayPoint', point: sorted[k].order.geometry.getCoordinates() });
    }
    if ($('.show-last-segment input').is(':checked')) {
        rwl.push({ type: 'wayPoint', point: pointZero });
    }



    //do {
    //    var min = 10000;
    //    var cid = 0;
    //    var o = null;
    //    for (var i = 0; i < orders.length; i++) {
    //        if ($.inArray(orders[i].id, exclude) < 0) {
    //            if (orders[i].num < min) {
    //                cid = orders[i].id;
    //                o = orders[i].order;
    //            }
    //        }
    //    }
    //    if (cid > 0) {
    //        exclude.push(cid);
    //        //drawRouteDirect(pointZero, o.geometry.getCoordinates());
    //        pointZero = o.geometry.getCoordinates();
    //        rwl.push({ type: 'wayPoint', point: pointZero });
    //    }

    //} while (cid > 0)

    if (route != null) {
        myMap.geoObjects.remove(route);
        route = null;
    }

    ymaps.route(rwl, {
        mapStateAutoApply: true
        //multiRoute: true
    }).then(function (r) {
        r.getPaths().options.set({
            // в балуне выводим только информацию о времени движения с учетом пробок
            //balloonContentBodyLayout: ymaps.templateLayoutFactory.createClass('{{ properties.humanJamsTime }}'),
            // можно выставить настройки графики маршруту
            //strokeColor: '0000ffff',
            opacity: 0.8
        });
        var pathsLen = r.getPaths().getLength();
        for (var l = 0; l < pathsLen; l++) {
            r.getPaths().get(l).options.set({
                // в балуне выводим только информацию о времени движения с учетом пробок
                //balloonContentBodyLayout: ymaps.templateLayoutFactory.createClass('{{ properties.humanLength }}'),
                balloonContentBodyLayout: ymaps.templateLayoutFactory.createClass('{{ properties.humanLength }}'),
                // можно выставить настройки графики маршруту
                hasBalloon: true, hasHint: false,
                //strokeColor: getRandomColor(),
            });
        }

        var points = r.getWayPoints();
        points.options.set('preset', 'islands#darkBlueCircleDotIcon');
        var pointLen = points.getLength();
        $('.paths-list').html('');
        var html = '';
        for (var i = 0; i < pointLen; i++) {
            if (i < pointLen - 1) {
                var start = points.get(i).geometry.getCoordinates();
                var end = points.get(i + 1).geometry.getCoordinates();
                var path = getAdress(start) + " - " + getAdress(end);
                var dist = r.getPaths().get(i).getHumanLength();
                var time = r.getPaths().get(i).getHumanTime();
                html += '<div class="path">' + (i + 1) + ". " + path + ' (<b>' + dist + ', ' + time + '</b>) </div>';
            }
        }
        $('.paths-list').html(html);
        $('#allRouteInfo').html('Длина маршрута - ' + Math.round(r.getLength() / 1000) + ' км');
        route = r;
        myMap.geoObjects.add(r);
    });



}


function getAdress(coords) {
    for (var i = 0; i < orders.length; i++) {
        var geo = orders[i].order.geometry.getCoordinates();
        if (geo[0] == coords[0] && geo[1] == coords[1]) {
            return $(orders[i].order.properties.get('balloonContent')).filter('div').text();
        }
    }
    for (var j = 0; j < stores.length; j++) {
        var geo = stores[j].store.geometry.getCoordinates();
        if (Math.round(geo[0] * 1000) == Math.round(coords[0] * 1000) && Math.round(geo[1] * 1000) == Math.round(coords[1] * 1000)) {
            return stores[j].store.properties.get('balloonContent');
        }
    }
    return "";
}

function syncLines() {
    $(".routes .route").html($(".routes .route").sort(function (o) {
        //console.log(o);
        for (var i = 0; i < orders.length; i++) {
            //if(orders[i].id == )
        }


    }));
}


function autoreorder(pointZero, restIds, index) {
    index++;
    if (restIds == null) {
        restIds = new Array();
        for (var m = 0; m < orders.length; m++) {
            restIds.push(orders[m].id);
        }
    }
    var distance = new Array();
    for (var i = 0; i < orders.length; i++) {
        if ($.inArray(orders[i].id, restIds) >= 0) {
            var dist = ymaps.coordSystem.geo.getDistance(pointZero, orders[i].order.geometry.getCoordinates());
            distance.push({ id: orders[i].id, distance: dist });
        }
    }
    console.log(distance);
    if (!distance.length)
        return;
    var min = distance[0];
    for (var j = 1; j < distance.length; j++) {
        if (distance[j].distance < min.distance) {
            min = distance[j];
        }
    }
    var arr = new Array();
    for (var k = 0; k < restIds.length; k++) {
        if (restIds[k] != min.id) {
            arr.push(restIds[k]);
        }
    }
    var minOrder = null;
    for (var l = 0; l < orders.length; l++) {
        if (orders[l].id == min.id) {
            orders[l].num = index;
            minOrder = orders[l].order;
        }
    }

    if (minOrder) {
        console.log(min.distance);
        console.log(minOrder.properties);
        autoreorder(minOrder.geometry.getCoordinates(), arr, index);
    }

}

function loadAllRoutes() {
    if (orderCount == 0) {
        $('.routes').sortable({
            placeholder: "sortable-placeholder",
            stop: function (event, ui) {
                $('.route').each(function (index) {
                    $(this).find('.num').html((index + 1) + '.');
                    for (var i = 0; i < orders.length; i++) {
                        if (orders[i].id == $(this).attr('arg') * 1) {
                            orders[i].num = index + 1;
                        }
                    }
                });
                loadAllRoutes();
            }
        });
        if ($('.route').length) {
            $('#showAll').show();
            $('.paths-list').show();
        } else {
            $('#showAll').hide();
            $('.paths-list').hide();
        }
        drawAllRoutes(null, null);
    } else {
        setTimeout(function () {
            loadAllRoutes();
        }, 200);
    }
}


function createRouteLine(d, index, order) {
    var store = null;
    for (var i = 0; i < stores.length; i++) {
        if (stores[i].id == d.StoreID) {
            store = stores[i].store;
            break;
        }
    }
    var source = '';
    if (store) {

        source = store.FullAdress;
    }
    if (source.length) {
        source += ' - ';
    }

    //"
    //onclick = "drawRoute(' + d.StoreID + ', ' + d.ID + ', this)" title="Показать маршрут от склада"
    var h = '<div class="route" arg="' + d.ID + '"><div class="num">' + index + '.</div><div class="name">' + /*+ source + */d.PointData.FullAdress + '</div><div class="driver"><span>Доставщик:</span><span class="driver-name">' + d.Driver + '</span></div></div>';

    var select = $('#Drivers').clone();
    select.show();
    select.attr('oid', d.ID);
    if (!d.Driver.length) {

        for (var i = 0; i < zonesList.length; i++) {

            if (zonesList[i].poly.geometry.contains(order.geometry.getCoordinates())) {
                d.Driver = zonesList[i].poly.Driver;
                $.post('/Delivery/SaveDriver', { ID: d.ID, Driver: zonesList[i].poly.Driver }, function (d) {
                });
                break;
            }

        }
    }
    select.val(d.Driver);

    $('.routes').append(h);
    $('.route[arg="' + d.ID + '"]').find('.driver-name').html(select);

    $('.route').each(function (index) {
        $(this).find('.num').html((index + 1) + '.');
    })
}

function changeDriver(obj) {
    $.post('/Delivery/SaveDriver', { ID: $(obj).attr('oid'), Driver: $(obj).val() }, function (d) {

    });
}

function filterDriver() {
    ordersTimer = 0;
    loadOrders($('#PointsList').val());


}