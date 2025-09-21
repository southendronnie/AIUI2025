window.renderCandleChart = function (containerId, seriesData) {
    debugger;
    Highcharts.stockChart(containerId, {
        rangeSelector: {
            selected: 1
        },
        title: {
            text: 'Market Activity'
        },
        xAxis: {
            type: 'datetime'
        },
        series: [{
            type: 'candlestick',
            name: 'Price',
            data: seriesData,
            tooltip: {
                valueDecimals: 2
            }
        }]
    });
};