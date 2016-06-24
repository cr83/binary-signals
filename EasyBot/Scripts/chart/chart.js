zingchart.THEME = "classic";
var myConfig =
        {
            "type": "mixed",
            //"background-color": "#003849",
            "background-color": "#394142 #1a2021",
            //"border-color": "#3b5d62",
            "utc": true,
            "title": {
                "y": "7px",
                "text": "",
                //"background-color": "#003849",
                "background-color": "none",
                "font-size": "20px",
                "font-color": "white",
                "height": "25px"
            },
            //"preview": { //preview box below chart.  Can be used from zooming and scrolling
            //    //"margin": "20% 8% 14% 12%",
            //    "margin-top":"100%",
            //    "margin-bottom":50,
            //    "height": 50,
            //    "active": {
            //        //"alpha": 0.5,
            //        //"background-color": "red"
            //    }
            //},
            "plotarea": {
                "margin": "20% 8% 14% 12%",
                //"background-color": "#003849",
                "adjust-layout": true /* For automatic margin adjustment. */
            },
            //"legend": {
            //    "layout": "float",
            //    "background-color": "none",
            //    "border-width": 0,
            //    "shadow": 0,
            //    "width": "75%",
            //    "text-align": "middle",
            //    "x": "25%",
            //    "y": "10%",
            //    "item": {
            //        "font-color": "#f6f7f8",
            //        "font-size": "14px"
            //    }
            //},
            "guide": {
                "shared": true,
                "scale-label": {
                    "visible": false
                },
                "plot-label": {
                    "padding": 8,
                    "border-radius": 8,
                    "-background-color-2": "#aaa",
                    "alpha": "0.725",
                    "visible": true,
                    "text": "Close: %v"
                    //"text": "Open: %v0 <br/>High: %v1 <br/>Low: %v2 <br/>Close: %v3"
                }
            },
            "scale-x": {
                "zooming":true,
                "min-value": 1383292800000,
                "shadow": 0,
                "step": 1000,//3600000,
                "line-color": "#f6f7f8",
                "alpha": 0.5,
                "tick": {
                    //"line-color": "#f6f7f8"
                    "line-color": "#949799"
                },
                "guide": {
                    "line-color": "#f6f7f8",
                    "alpha": 0.1
                },
                "item": {
                    "font-color": "#949799"
                    //"font-color": "#f6f7f8"
                },
                "transform": {
                    "type": "date",
                    "all": "%D, %d %M<br />%h:%i:%s %A",
                    "guide": {
                        "visible": false
                    },
                    "item": {
                        "visible": false
                    }
                },
                "label": {
                    "visible": false
                },
                "minor-ticks": 0
            },
            "scale-y": {
                //"values": "0:1000:250",
                "values": "70:75",
                "line-color": "#f6f7f8",
                "alpha": 0.5,
                "shadow": 0,
                "tick": {
                    //"line-color": "#f6f7f8"
                    "line-color": "#949799"
                },
                "guide": {
                    "line-color": "#f6f7f8",
                    "line-style": "dashed",
                    "alpha": 0.1
                },
                "item": {
                    "font-color": "#949799"
                    //"font-color": "#f6f7f8"
                },
                "label": {
                    "text": "pips",
                    //"font-color": "#f6f7f8"
                    "font-color": "#949799"
                },
                "minor-ticks": 0,
                "thousands-separator": ","
            },
            //"crosshair-x": {
            //    "line-color": "#f6f7f8",
            //    "plot-label": {
            //        "border-radius": "5px",
            //        "border-width": "1px",
            //        "border-color": "#f6f7f8",
            //        "padding": "10px",
            //        "font-weight": "bold"
            //    },
            //    "scale-label": {
            //        "font-color": "#00baf0",
            //        "background-color": "#f6f7f8",
            //        "border-radius": "5px"
            //    }
            //},
            "tooltip": {
                "visible": false
            },
            "plot": {
                //"tooltip-text": "Open: %v0 +High: %v1 +Low: %v2 Close: %v3",
                "shadow": 0,
                "line-width": "3px",
                "marker": {
                    "type": "circle",
                    "size": 4
                },
                "hover-marker": {
                    "type": "circle",
                    "size": 5,
                    "border-width": "2px"
                }
            },
            //"refresh":{
            //    "type":"feed",
            //    "transport":"js",
            //    "url":"feed()",
            //    "interval":200
            //},
            "series": [
                {
                    "type": "line",
                    //"type": "area",
                    //"background-color": "rgba(20, 10, 10, .5) none",
                    //rgba(54, 25, 25, .5)
                    //"text": "Close",
                    "line-color": "#007790",
                    //"background-state":{
                    //    "background-color": "#EC6F01 none"
                    //},
                    //"line-color": "#EB6F01",
                    //"line-color": "rgba(233, 110, 1, .9)",
                    //"alpha": 0.9,
                    "legend-marker": {
                        "type": "circle",
                        "size": 5,
                        //"background-color": "#007790",
                        "background-color": "#1DC23D",
                        "border-width": 1,
                        "shadow": 0,
                        //"border-color": "#69dbf1"
                        "border-color": "#EB6F01"
                    },
                    "marker": {
                        "background-color": "#007790",
                        //"background-color": "#394142",
                        "border-width": 2,
                        "shadow": 0,
                        "border-color": "#69dbf1"
                        //"border-color": "#EB6F01"
                    }
                }
            ]
        };

var _init;
function _initChart() {
    zingchart.render({
        id: 'chart',
        data: myConfig,
        height: '100%',
        width: '100%'
    });
}

if (!_init) {
    _initChart();
    _init = true;
}

function drawChart(symbol, candles, step) {
    if (!_init) {
        _initChart();
        _init = true;
    }
    var thevalues = candles.map(function (c) { return c.Close; });
    //var thevalues2 = candles.map(function (c) { return [c.Open, c.High, c.Low, c.Close]; });

    min = Number.MAX_VALUE;
    max = Number.MIN_VALUE;
    for (var i = 0; i < thevalues.length; i++) {
        point = thevalues[i];
        min = Math.min(min, point);
        max = Math.max(max, point);
    }

    var r = max - min;
    max += r;
    min -= r;

    var d = 6;
    do {
        d--;
        r = (r + '').substring(0, d) * 1;
    } while (r != 0)
    
    var scale = 1 / Math.pow(10, d-1);

    max = max.toFixed(4);
    min = min.toFixed(4);

    zingchart.exec('chart', 'modify', {
        graphid: 0,
        data: {
            "title": {
                "text": symbol
            },
            "scale-y": {
                "values": min + ":" + max + ":" + scale
            },
            "scale-x": {
                "step": step,
                "min-value": (new Date(candles[0].Date)).getTime()
            }
        },
        update: false
    });

    zingchart.exec("chart", "setseriesvalues", {
        "values": [thevalues],
        update: false
    });

    zingchart.exec("chart", "update");
}
