
var oa = null;
$().ready(function() {
    oa = new OrderAmount();
    graphs.push({ name: "OrderAmount", object: oa });
});

function OrderAmount() {
    var _this = this;
    this.loadPeriod = function (obj) {
        $(obj).parents('#morris').find('.period').removeClass('active');
        $(obj).addClass('active');
        _this.loadGraph($('#PointsList').val());
        return false;
    }

    var chart = null;

    this.transformData = function (obj) {
        $('#orderamount').parents('#morris').find('.legeng').show();
        $('#orderamount').parents('#morris').find('.legeng').addClass('empty');
        var arr = new Array();
        for (var i = 0; i < obj.length; i++) {
            var no = new Object();
            no.period = obj[i].period;
            for (var j = 0; j < obj[i].details.length; j++) {
                var legend = $('#orderamount').parents('#morris').find('.legeng span[key="' + obj[i].details[j].manager + '"]');
                legend.parent().removeClass('empty');
                //legend.parent().show();
                //no[legend.attr('key')] = obj[i].details[j].value;
                no[obj[i].details[j].manager] = obj[i].details[j].value;
            }
            arr.push(no);
        }
        $('#orderamount').parents('#morris').find('.legeng.empty').hide();
        return arr;
    }

    this.loadGraph = function (items) {
        var my = $('#orderamount').parents('#morris');
        $.get('/Graph/OrderAmountPoints', { period: my.find('.period.active').attr('arg'), points: items }, function(data) {
            data = _this.transformData(data);
            console.log(data);
            if (chart == null) {

                var colors = new Array();
                var labels = new Array();
                var keys = new Array();

                my.find('.legeng').not('.empty').each(function() {
                    colors.push($(this).find('span').attr('style').replace('color:', '').trim());
                    labels.push($(this).find('span').attr('nm'));
                    keys.push($(this).find('span').attr('key'));
                });

                console.log(colors);
                console.log(labels);
                console.log(keys);


                chart = Morris.Line({
                    element: 'orderamount',
                    data: data,

                    xkey: 'period',
                    ykeys: keys,
                    labels: labels,
                    hideHover: 'auto',
                    xLabelFormat: function(x) {
                        return ''; //x.toString();
                    },
                    lineWidth: 1,
                    pointSize: 5,
                    lineColors: colors,
                    fillOpacity: 0.5,
                    smooth: true
                });
            } else {
                chart.setData(data);
            }
        });
    }
}