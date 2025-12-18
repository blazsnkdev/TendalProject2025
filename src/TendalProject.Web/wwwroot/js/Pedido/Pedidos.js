document.addEventListener('DOMContentLoaded', function () {
    // Obtener el estado actual de la URL
    const urlParams = new URLSearchParams(window.location.search);
    const estadoActual = urlParams.get('estado');

    // Definir los estados disponibles
    const estados = [
        { value: '', text: 'Todos los estados' },
        { value: 'Pendiente', text: 'Pendiente' },
        { value: 'Procesando', text: 'Procesando' },
        { value: 'Enviado', text: 'Enviado' },
        { value: 'Entregado', text: 'Entregado' },
        { value: 'Cancelado', text: 'Cancelado' },
        { value: 'Pagado', text: 'Pagado' }
    ];

    const select = document.getElementById('estadoSelect');

    // Poblar el select
    estados.forEach(estado => {
        const option = document.createElement('option');
        option.value = estado.value;
        option.textContent = estado.text;

        // Marcar como seleccionado si coincide
        if (estadoActual === estado.value) {
            option.selected = true;
        }

        select.appendChild(option);
    });

    // Opcional: Limpiar filtro con un botón adicional
    const clearFilterBtn = document.createElement('a');
    clearFilterBtn.href = '@Url.Action("Pedidos", new { clienteId = ViewBag.ClienteId })';
    clearFilterBtn.className = 'btn btn-outline-secondary';
    clearFilterBtn.textContent = 'Limpiar';

    document.querySelector('#filterForm').appendChild(clearFilterBtn);
});