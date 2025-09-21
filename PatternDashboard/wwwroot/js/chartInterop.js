window.renderCandleChart = function (containerId, seriesData, patternPoints) {
    Highcharts.stockChart(containerId, {
        rangeSelector: { selected: 1 },
        title: { text: 'Market Activity' },
        xAxis: { type: 'datetime' },
        series: [
            {
                type: 'candlestick',
                name: 'Price',
                data: seriesData,
                tooltip: { valueDecimals: 2 }
            },
            {
                type: 'scatter',
                name: 'Patterns',
                data: patternPoints,
                marker: {
                    symbol: 'circle',
                    radius: 5,
                    fillColor: '#e74c3c'
                },
                tooltip: {
                    pointFormat: '<b>{point.label}</b><br/>Confidence: {point.confidence}%'
                },
                dataLabels: {
                    enabled: true,
                    format: '{point.label}',
                    style: {
                        fontWeight: 'bold',
                        color: '#2c3e50'
                    }
                }
            }
        ]
    });
};
window.highlightCandle = function (timestamp) {
    const chart = Highcharts.charts.find(c => c && c.renderTo.id === "chartContainer");
    if (!chart) return;

    chart.xAxis[0].setExtremes(timestamp - 60000, timestamp + 60000); // Zoom to ±1min
};
