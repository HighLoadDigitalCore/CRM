var mp = null;

$().ready(function() {
    mp = new MiddlePrice();
    graphs.push({ name: "MiddlePrice", object: mp });
});

function MiddlePrice() {
    var _this = this;
    this.loadPeriod = function(obj) {


        $(obj).parents('#morris').find('.period').removeClass('active');
        $(obj).addClass('active');
        _this.loadGraph($('#PointsList').val());
        return false;
    }

    var chart = null;
    this.loadGraph = function(items) {
        var my = $('#middleprice').parents('#morris');
        $.get('/Graph/MiddlePricePoints', { period: my.find('.period.active').attr('arg'), points: items }, function(data) {
            if (chart == null) {
                chart = Morris.Area({
                    element: 'middleprice',
                    data: data,

                    xkey: 'period',
                    ykeys: ['point'],
                    labels: ['Средний чек'],
                    hideHover: 'auto',
                    xLabelFormat: function(x) {
                        return ''; //x.toString();
                    },
                    lineWidth: 1,
                    pointSize: 5,
                    lineColors: ['#a9d86e'],
                    fillOpacity: 0.5,
                    smooth: true
                });
            } else {
                chart.setData(data);
            }
        });
    }
}