$().ready(function () {
    loadGraphs();
});
var graphList = new Array();

function redrawGraph(id) {


    RGraph.ObjectRegistry.clear("CVS_" + id);
    RGraph.Clear(document.getElementById("CVS_" + id));
    /*
        for (var i = 0; i < graphList.length; i++) {
            if (graphList[i].id == id) {
                
                //graphList[i].obj.draw();
            }
        }
    */


    $.get("/Graph/GetAllGraphsData", { id: id }, function (d) {
        drawData(d);
    });

}

function loadGraphs() {
    $.get("/Graph/GetAllGraphsData", function (d) {
        drawData(d);
    });

}

function toggleSerial(id, gid) {
    $.post('/Graph/ToggleSerial', { id: id }, function (d) {
        var form = $('#GraphFilter_' + gid).find('form');
        if (form)
            form.submit();

    });
}

function drawData(d) {
    for (var i = 0; i < d.length; i++) {

        var labels = new Array();
        var tooltips = new Array();
        if (!d[i].data.length)
            continue;
        for (var j = 0; j < d[i].data[0].PointData.length; j++) {
            labels.push(d[i].data[0].PointData[j].Label);

        }

        var skip = Math.round(labels.length / 4);
        for (var j = 0; j < labels.length; j++) {
            if (j % skip > 0) {
                labels[j] = '';
            }


        }

        var colors = new Array();
        var data = new Array();
        for (var j = 0; j < d[i].data.length; j++) {
            var lineData = new Array();
            colors.push(d[i].data[j].Color);

            for (var k = 0; k < d[i].data[j].PointData.length; k++) {
                lineData.push(d[i].data[j].PointData[k].Value.Value);
                tooltips.push(d[i].data[j].PointData[k].Value.Tick);
            }

            data.push(lineData);
        }
        var graph = null;
        if (d[i].type == "Line") {
            graph = new RGraph.Line({
                id: 'CVS_' + d[i].id,
                data: data,

                options: {
                    tooltips: tooltips,
                    spline: true,
                    /*ymax: 10000,*/
                    noxaxis: true,
                    grid: {
                        dashed: true,
                        color: '#eee'
                    },
                    labels: labels,
                    linewidth: 3,
                    colors: colors,
                    shadow: {
                        color: '#aaa',
                        blur: 5
                    },
                    hmargin: 3,
                    gutter: {
                        left: 50,
                        right: 10,
                        top: 10,
                        bottom: 35
                    },
                    tickmarks: 'circle'
                }
            }).draw();
        }
        else if (d[i].type == "Bar") {
            colors = new Array();
            data = new Array();
            tooltips = new Array();
            if (d[i].data.length && d[i].data[0].PointData.length) {
                var c = d[i].data[0].PointData.length;
                for (var k = 0; k < c; k++) {
                    lineData = new Array();
                    for (var j = 0; j < d[i].data.length; j++) {
                        colors.push(d[i].data[j].Color);
                        lineData.push(d[i].data[j].PointData[k].Value.Value);
                        tooltips.push(d[i].data[j].PointData[k].Value.Tick);
                    }
                    data.push(lineData);
                }

            }
            console.log(data);
            graph = new RGraph.Bar({
                id: 'CVS_' + d[i].id,
                data: data,

                options: {
                    tooltips: tooltips,
                    spline: true,
                    /*ymax: 10000,*/
                    noxaxis: true,
                    grid: {
                        dashed: true,
                        color: '#eee'
                    },
                    labels: labels,
                    linewidth: 1,
                    colors: colors,
                    shadow: {
                        color: '#aaa',
                        blur: 5
                    },
                    hmargin: 3,
                    gutter: {
                        left: 50,
                        right: 10,
                        top: 10,
                        bottom: 35
                    },
                    tickmarks: null
                }
            }).draw();
        }

        else if (d[i].type == "Pie") {


            tooltips = new Array();

            if (!d[i].data.length)
                continue;


            labels = new Array();
            colors = new Array();
            data = new Array();

            var sum = 0;
            //debugger;
            for (var j = 0; j < d[i].data.length; j++) {

                colors.push(d[i].data[j].Color);
                labels.push(d[i].data[j].Name);
                /*
                                for (var k = 0; k < d[i].data[j].PointData.length; k++) {
                                    sum += d[i].data[j].PointData[k].Value.Value;
                                    
                                }
                */
                tooltips.push("<nobr><b>" + d[i].data[j].Name + "</b></nobr><br><nobr>Общая стоимость заказов: <b>" + d[i].data[j].PointData[0].Value.Value + "</b> руб.</nobr>");
                data.push(d[i].data[j].PointData[0].Value.Value);
            }

            graph = new RGraph.Pie({
                id: 'CVS_' + d[i].id,
                data: data,

                options: {
                    strokestyle: '#e8e8e8',
                    linewidth: 5,
                    shadow: {
                        blur: 5,
                        offsety: 15,
                        offsetx: 0,
                        color: '#aaa'
                    },
                    exploded: 10,
                    radius: 80,
                    tooltips: {
                        self: tooltips,
                        coords: {
                            page: true
                        }
                    },
                    labels: {
                        self: labels,
                        sticks: {
                            self: true,
                            length: 15
                        }
                    },
                    text: {
                        size: 7
                    },
                    colors: colors,
                }
            });
            var factor = 1.2;
            // Set the transformation of the canvas - a scale up by the factor (which is 1.5 and a simultaneous translate
            // so that the Pie appears in the center of the canvas
            graph.context.setTransform(factor, 0, 0, 1, ((graph.canvas.width * factor) - graph.canvas.width) * -0.5, 0);
            //pie.draw();
            graph.roundRobin({ frames: 30 });
            /*.grow({frames: 50});*/
        }
        if (graph) {
            for (var l = 0; l < graphList.length; l++) {
                if (graphList[l].id == d[i].id) {
                    graphList[l].obj = graph;
                    return;
                }
            }

            graphList.push({ id: d[i].id, obj: graph });
        }

    }

}