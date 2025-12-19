const estadosPedido = [
    { value: 'Pendiente', text: 'Pendiente' },
    { value: 'Procesando', text: 'Procesando' },
    { value: 'Enviado', text: 'Enviado' },
    { value: 'Entregado', text: 'Entregado' },
    { value: 'Cancelado', text: 'Cancelado' },
    { value: 'Pagado', text: 'Pagado' }
];

function cargarEstados(button) {
    const pedidoId = button.getAttribute('data-pedidoid');
    const estadoActual = button.getAttribute('data-estadoactual');

    document.getElementById('modalPedidoId').value = pedidoId;


    document.getElementById('estadoActualDisplay').value = estadoActual;


    const select = document.getElementById('EstadoSeleccionado');
    select.innerHTML = '<option value="">-- Seleccione un estado --</option>';


    estadosPedido.forEach(estado => {
        if (estado.value !== estadoActual) {
            const option = document.createElement('option');
            option.value = estado.value;
            option.textContent = estado.text;
            select.appendChild(option);
        }
    });
}


document.getElementById('cambiarEstadoModal').addEventListener('hidden.bs.modal', function () {
    document.getElementById('EstadoSeleccionado').selectedIndex = 0;
});