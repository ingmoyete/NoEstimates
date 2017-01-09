/// <reference path="../Vendor/_references.js" />

// Example of arrayOfSeries
//var d1 = [];
//for (var i = 0; i < 14; i += 0.5) {
//    d1.push([i, Math.sin(i)]);
//}
//arrayOfSeries = [d1];

(function ($) {
    // Objects =================================
    $.fn.Serie = function (json, pointColor, lineColor, label) {

        // Set object with the data and return it
        var ret = buildSerie(json, pointColor, lineColor, label);

        return ret;
    }

    function buildSerie(json, pointColor, lineColor, label) {
        // Get colors
        var pointColor = (pointColor) ? pointColor : '#000000';
        var lineColor = (lineColor) ? lineColor : '#A9A9A9';


        // Set object with the data and return it
        var ret =
            {
                data: json,
                color: lineColor,
                lines: {
                    fill: false,
                    lineWidth: 3
                },
                points: {
                    show: true,
                    fillColor: pointColor
                }

            };

        // Get label
        var retOptions;
        if (label) {
            var labelValue = label;

            retOptions = {
                label: labelValue
            };

            $.extend(ret, retOptions);
        }

        return ret;
    }

    // Jquery plugin =================================
    $.fn.plot = function (arrayOfSeries, markingJson, axisLimitJson, labelsPosition)
    {
        // Default options
        var optionsDefault = {
            series: {
                lines: {
                    show: true
                },
                points: {
                    show: true 
                }
            },
            grid: {
                hoverable: true //IMPORTANT! this is needed for tooltip to work
            },
            legend: { position: "se" }, //"ne" or "nw" or "se" or "sw"
            tooltip: true,
            tooltipOpts: {
                content: "X = %x.4, Y = %y.4",
                shifts: {
                    x: -60,
                    y: 25
                }
            }
        };

        // Options with label
        
        // Set user options
        var options;
        options = markingJson ? getMarkings(markingJson, options) : options; // markings
        options = axisLimitJson ? getAxisLimits(axisLimitJson, options) : options; // axis limit
        options = labelsPosition ? getLabel(labelsPosition, options) : options; // Labels positions

        // traverse all nodes
        options = $.extend(optionsDefault, options);
        this.each(function () {

            // express a single node as a jQuery object
            var $t = $(this);

            // Call plot function
            plotData($t, arrayOfSeries, options);

        });

        // Allow jquery chaining
        return this;
    }

    // Private Methods =================================
    // Plot data
    function plotData($placeHolder, arrayOfSeries, options) {
        $.plot($placeHolder, arrayOfSeries, options);
    }

    // Get the marking for a graph
    function getMarkings(json, options) {
        var markings = [];

        var retOptions;
        if (json) {
            // Loop through all the elements in the json and prepare the markings
            $.each(json, function (index, value) {
                if (value.IsY)
                    markings.push({ yaxis: { from: value.YLine, to: value.YLine }, color: value.Color });
                if (value.IsX)
                    markings.push({ xaxis: { from: value.XLine, to: value.XLine }, color: value.Color });
            });

            // Set options and return it
            retOptions = {
                grid: {
                    hoverable: true,
                    markings: markings
                }
            };
        }
        
        return $.extend(retOptions, options);
    }

    // Get limits of the graph
    function getAxisLimits(json, options) {
        var axisX = {min: '', max: ''};
        var axisY = { min: '', max: '' };

        var retOptions;
        if (json) {

            // Set options and return it
            retOptions = {
                xaxis: axisX,
                yaxis: axisY
            };

            // Set the axis the the max and the min value
            if (json.ThereIsY) {
                axisY.min = json.FromY;
                axisY.max = json.ToY;

                delete retOptions.xaxis;
            }
            if (json.ThereIsX) {
                axisX.min = json.FromX;
                axisX.max = json.ToX;

                delete retOptions.yaxis;
            }
        }

        return $.extend(retOptions, options);
    }

    // Get label
    function getLabel(labelPosition, options) {
        if (labelPosition) {
            var retOptions = {
                legend : { position: labelPosition }
            };
        }

        return $.extend(retOptions, options);
    }

})(jQuery);