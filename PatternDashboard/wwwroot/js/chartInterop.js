window.renderCandleChart = function (containerId, seriesData) {
    Highcharts.stockChart(containerId, {
        rangeSelector: { selected: 1 },
        title: { text: 'Market Activity' },
        series: [{
            type: 'candlestick',
            name: 'Price',
            data: seriesData,
            tooltip: { valueDecimals: 2 }
        }]
    });
};