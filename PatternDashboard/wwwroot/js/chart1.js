window.renderTickChart = function (data) {
    const ctx = document.getElementById('tickChart').getContext('2d');

    const labels = data.map(d => d.time);
    const prices = data.map(d => d.mid);

    new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Mid Price',
                data: prices,
                borderColor: '#007bff',
                backgroundColor: 'rgba(0,123,255,0.1)',
                fill: true,
                tension: 0.2,
                pointRadius: 0
            }]
        },
        options: {
            animation: false,
            responsive: true,
            scales: {
                x: {
                    ticks: { autoSkip: true, maxTicksLimit: 10 }
                },
                y: {
                    beginAtZero: false
                }
            },
            plugins: {
                legend: { display: false }
            }
        }
    });
};

