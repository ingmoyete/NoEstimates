/// <reference path="../Vendor/_references.js" />

(function ($) {

    var timeAxisEnum = {
        Hours: 1,
        Days: 2,
        Weeks: 3,
        Months: 4
    };

    var graphEnum = {
        Acumulative : 1,
        Velocity : 2,
        Completion : 3
    };

    // Methods =====================================

    // Get sub label for a serie
    function getSubLabel(timeAxis) {
        var subLabel = timeAxis == timeAxisEnum.Hours ? "Hours"
                : timeAxis == timeAxisEnum.Days ? "Days"
                : timeAxis == timeAxisEnum.Weeks ? "Weeks"
                : timeAxis == timeAxisEnum.Months ? "Months"
                : "Hours";

        return subLabel;
    }

    // Draw acumulative graph
    function drawAcumulativeGraph(timeAxis, subLabel) {
        // Try to get json
        try {
            var acumulativeGraph = JSON.parse($('.acumulativeGraph').val());
        }
        // Exit method if the json could not be parsed.
        catch (e) {
            return null;
        }

        // Choose serie depending on time axis
        var acumulativeData =
            timeAxis == timeAxisEnum.Hours ? acumulativeGraph.AcumulativeSerieHours
            : timeAxis == timeAxisEnum.Days ? acumulativeGraph.AcumulativeSerieDays
            : timeAxis == timeAxisEnum.Weeks ? acumulativeGraph.AcumulativeSerieWeeks
            : timeAxis == timeAxisEnum.Months ? acumulativeGraph.AcumulativeSerieMonths
            : acumulativeGraph.AcumulativeSerieHours

        // Draw graph
        var acumulative = $.fn.Serie(acumulativeData, acumulativeGraph.AcumulativePointsColor, acumulativeGraph.AcumulativeSerieColor, "Y = Complete Percentage " + "<br>" + "X = " + subLabel);
        $('.acumulativeContainer').plot([acumulative], acumulativeGraph.AcumulativeMarkings, acumulativeGraph.AcumulativeAxisLimits);
    }

    // Draw Velocity Graph
    function drawVelocityGraph(timeAxis, subLabel) {
        // Try to get json
        try {
            var velocityGraph = JSON.parse($('.velocityGraph').val());
        }
        // Exit method if the json could not be parsed.
        catch (e) {
            return null;
        }

        // Choose serie depending on time axis
        var velocityData =
            timeAxis == timeAxisEnum.Hours ? velocityGraph.VelocitySerieHours
            : timeAxis == timeAxisEnum.Days ? velocityGraph.VelocitySerieDays
            : timeAxis == timeAxisEnum.Weeks ? velocityGraph.VelocitySerieWeeks
            : timeAxis == timeAxisEnum.Months ? velocityGraph.VelocitySerieMonths
            : acumulativeGraph.AcumulativeSerieHours

        // Choose markings depending on time axis
        var markings =
            timeAxis == timeAxisEnum.Hours ? velocityGraph.VelocityMarkingsHours
            : timeAxis == timeAxisEnum.Days ? velocityGraph.VelocityMarkingsDays
            : timeAxis == timeAxisEnum.Weeks ? velocityGraph.VelocityMarkingsWeeks
            : timeAxis == timeAxisEnum.Months ? velocityGraph.VelocityMarkingsMonths
            : velocityGraph.VelocityMarkingsHours;

        // Draw graph
        var velocity = $.fn.Serie(velocityData, velocityGraph.VelocityPointsColor, velocityGraph.VelocitySerieColor, "Y = Task per " + subLabel + "<br>" + "X = " + subLabel);
        $('.velocityContainer').plot([velocity], markings);
    }

    // Draw completion graph
    function drawCompletionGraph(timeAxis, subLabel) {
        // Try to get json
        try {
            var completionGraph = JSON.parse($('.completionGraph').val());
        }
        // Exit method if the json could not be parsed.
        catch (e) {
            return null;
        }

        // Choose completion serie depending on time axis
        var completionData =
            timeAxis == timeAxisEnum.Hours ? completionGraph.CompletionSerieHours
            : timeAxis == timeAxisEnum.Days ? completionGraph.CompletionSerieDays
            : timeAxis == timeAxisEnum.Weeks ? completionGraph.CompletionSerieWeeks
            : timeAxis == timeAxisEnum.Months ? completionGraph.CompletionSerieMonths
            : acumulativeGraph.AcumulativeSerieHours

        // Choose estimation serie depending on time axis
        var estimationData =
            timeAxis == timeAxisEnum.Hours ? completionGraph.EstimationSerieHours
            : timeAxis == timeAxisEnum.Days ? completionGraph.EstimationSerieDays
            : timeAxis == timeAxisEnum.Weeks ? completionGraph.EstimationSerieWeeks
            : timeAxis == timeAxisEnum.Months ? completionGraph.EstimationSerieMonths
            : acumulativeGraph.AcumulativeSerieHours;

        // Choose the total estimated completion time depending on time axis
        var totalEstimatedCompletion = 
            timeAxis == timeAxisEnum.Hours ? completionGraph.AverageCompletionTimeHours
            : timeAxis == timeAxisEnum.Days ? completionGraph.AverageCompletionTimeDays
            : timeAxis == timeAxisEnum.Weeks ? completionGraph.AverageCompletionTimeWeeks
            : timeAxis == timeAxisEnum.Months ? completionGraph.AverageCompletionTimeMonths
            : acumulativeGraph.AverageCompletionTimeHours;

        var estimationLabel = $('.estimationLabel').val() + totalEstimatedCompletion;

        // Draw graph
        var completion = $.fn.Serie(completionData, completionGraph.CompletionPointsColor, completionGraph.CompletionSerieColor, "Y = Task Remaining" + "<br>" + "X = " + subLabel);
        var estimation = $.fn.Serie(estimationData, undefined, completionGraph.EstimationSerieColor, estimationLabel);
        $('.completionContainer').plot([completion, estimation], completionGraph.CompletionMarkings, completionGraph.CompletionAxisLimits, 'ne');
    }

    // Eventos =====================================
    function main() {
        // On Ready: draw the graphs
        $(document).ready(function () {
            // Get the axis unit (month ,week, ...)
            var timeAxis = $('.timeAxis').val();
            var subLabel = getSubLabel(timeAxis);

            // Acumulative
            drawAcumulativeGraph(timeAxis, subLabel);

            // Velocity  
            drawVelocityGraph(timeAxis, subLabel);

            // Completion
            drawCompletionGraph(timeAxis, subLabel);
        });

        // On Action selected
        $('body').on('click', '.dropdown-menu.pull-right a', function (e) {
            e.preventDefault();

            // Get the selected time axis and selected Graph
            var selectedTimeAxis = $(this).data('time-axis');
            var selectedGraph = $(this).closest('.dropdown-menu.pull-right').data('graph');

            // Get sub-label
            var subLabel = getSubLabel(selectedTimeAxis);

            // Change the time axis of the selected graph
            switch (selectedGraph) {
                case graphEnum.Acumulative:
                    drawAcumulativeGraph(selectedTimeAxis, subLabel);
                    break;

                case graphEnum.Velocity:
                    drawVelocityGraph(selectedTimeAxis, subLabel);
                    break;

                case graphEnum.Completion:
                    drawCompletionGraph(selectedTimeAxis, subLabel);
                    break;

                default: break;

            }

        });
    }

    $(main);

})(jQuery);