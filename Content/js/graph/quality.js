
var qu = null;
$().ready(function() {
    qu = new Quality();
    graphs.push({ name: "Quality", object: qu });
});

function Quality() {
    var _this = this;
    this.loadPeriod = function(obj) {

        $(obj).parents('#morris').find('.period').removeClass('active');
        $(obj).addClass('active');
        _this.loadGraph($('#PointsList').val());
        return false;
    }

    var chart = null;

    this.loadGraph = function(items) {
        var my = $('#quality').parents('#morris');
        $.get('/Graph/QualityPoints', { period: my.find('.period.active').attr('arg'), points: items }, function(data) {
            if (chart == null) {
                chart = Morris.Line({
                    element: 'quality',
                    data: data.Points,

                    xkey: 'period',
                    ykeys: /*['iphone', 'ipad', 'itouch']*/data.YKeys,
                    labels: /*['iPhone', 'iPad', 'iPod Touch']*/data.Labels,
                    hideHover: 'auto',
                    xLabelFormat: function(x) {
                        return ''; //x.toString();
                    },
                    hoverCallback: function(index, options, content) {
                        return (content);
                    },
                    lineWidth: 1,
                    pointSize: 5,
                    lineColors: ['#cccccc', '#4a8bc2', '#ff6c60', '#a9d86e', '#ff00ff'],
                    fillOpacity: 0.5,
                    smooth: true
                });
            } else {
                chart.setData(data.Points);
            }
        });
    }
}