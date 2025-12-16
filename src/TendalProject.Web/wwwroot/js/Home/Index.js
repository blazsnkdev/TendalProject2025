const ctx = document.getElementById('chartPedidos');

new Chart(ctx, {
    type: 'bar',
    data: {
        labels: ['Pedidos Hoy', 'Pendientes'],
        datasets: [{
            label: 'Cantidad',
            data: [
                window.dashboardData.pedidosHoy,
                window.dashboardData.pedidosPendientes
            ],
            borderWidth: 1
        }]
    },
    options: {
        responsive: true,
        plugins: {
            legend: { display: false }
        },
        scales: {
            y: { beginAtZero: true }
        }
    }
});
