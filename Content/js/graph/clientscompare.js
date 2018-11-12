var cc = null;

$().ready(function() {
    cc = new ClientCompare();
    graphs.push({ name: "ClientCompare", object: cc });
});

function ClientCompare() {
    var _this = this;
    this.loadPeriod = function(obj) {


        $(obj).parents('#morris').find('.period').removeClass('active');
        $(obj).addClass('active');
        _this.loadGraph($('#PointsList').val());
        return false;
    }

    var chart = null;
    this.loadGraph = function(items) {
        var my = $('#clientscompare').parents('#morris');
        //$('#hero-area').html('');
        $.get('/Graph/ClientsComparePoints', { period: my.find('.period.active').attr('arg'), points: items }, function(data) {
            if (chart == null) {
/*
            chart = Morris.Area({
                element: 'hero-area',
                data: data.Points,

                xkey: 'period',
                ykeys: /*['iphone', 'ipad', 'itouch']#1#data.YKeys,
                labels: /*['iPhone', 'iPad', 'iPod Touch']#1#data.Labels,
                hideHover: 'auto',
                xLabelFormat: function(x) {
                    return ''; //x.toString();
                },
                lineWidth: 1,
                pointSize: 5,
                lineColors: ['#cccccc', '#4a8bc2', '#ff6c60', '#a9d86e', '#ff00ff'],
                fillOpacity: 0.5,
                smooth: true
            });
*/
                chart = Morris.Bar({
                    element: 'clientscompare',
                    data: data,
                    xkey: 'period',
                    ykeys: ['good', 'bad'],
                    labels: ['Удовлетворенные', 'Не удовлетворенные'],
                    barRatio: 0.4,
                    /*xLabelAngle: 35,*/
/*
                xLabelFormat: function (x) {
                    return x;
                },
*/
                    hideHover: 'auto',
                    barColors: ['#a9d86e', '#ff6c60']
                });

            } else {
                chart.setData(data);
            }
        });
    }
}