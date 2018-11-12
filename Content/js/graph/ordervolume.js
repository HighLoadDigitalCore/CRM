var ov = null;
$().ready(function () {
    ov = new OrderVolume();
    graphs.push({ name: "OrderVolume", object: ov });
});


function OrderVolume() {
    var _this = this;
    var chart = null;

    this.loadPeriod = function(obj) {
        $(obj).parents('#morris').find('.period').removeClass('active');
        $(obj).addClass('active');
        _this.loadGraph($('#PointsList').val());
        return false;
    }

    this.loadGraph = function(items) {
        var my = $('#ordervolume').parents('#morris');
        $.get('/Graph/OrderVolumePoints', { period: my.find('.period.active').attr('arg'), points: items }, function(data) {
            if (chart == null) {

                chart = Morris.Bar({
                    element: 'ordervolume',
                    data: data,
                    xkey: 'period',
                    ykeys: ['man', 'woman'],
                    labels: ['Мужчины', 'Женщины'],
                    barRatio: 0.4,
                    /*xLabelAngle: 35,*/
/*
                xLabelFormat: function (x) {
                    return x;
                },
*/
                    hideHover: 'auto',
                    barColors: ['#ff6c60', '#a9d86e']
                });

            } else {
                chart.setData(data);
            }
        });
    }
}