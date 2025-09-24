window.renderBacktestChart = (candles, markers) => {
    Highcharts.stockChart('backtestChart', {
        rangeSelector: { selected: 1 },
        title: { text: 'Backtest Trade Overlay' },
        series: [{
            type: 'candlestick',
            name: 'Price',
            data: candles,
            tooltip: { valueDecimals: 2 }
        }],
        annotations: [{
            labels: markers.map(m => ({
                point: { xAxis: 0, yAxis: 0, x: m.x },
                text: m.text,
                backgroundColor: m.color,
                style: { color: 'white' }
            }))
        }]
    });
};
