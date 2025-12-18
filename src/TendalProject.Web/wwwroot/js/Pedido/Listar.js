function confirmarEnvioPedido(estadoActual) {

    if (estadoActual !== "Pagado") {
        Swal.fire({
            title: "Acción no permitida",
            text: "Solo los pedidos PAGADOS pueden ser enviados.",
            icon: "error",
            confirmButtonColor: "#d33"
        });
        return;
    }

    Swal.fire({
        title: "¿Enviar pedido?",
        text: "El pedido cambiará su estado a ENVIADO.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, enviar",
        cancelButtonText: "Cancelar",
        confirmButtonColor: "#28a745",
        cancelButtonColor: "#6c757d",
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById("formEnviarPedido").submit();
        }
    });
}
// Archivo: wwwroot/js/Pedido/Listar.js

document.addEventListener('DOMContentLoaded', function () {
    // Marcar filas próximas a entregar
    const rows = document.querySelectorAll('table tbody tr');
    const today = new Date();

    rows.forEach(row => {
        const fechaEntregaCell = row.querySelector('td:nth-child(4)');
        if (fechaEntregaCell && fechaEntregaCell.textContent.trim() !== '-') {
            const fechaTexto = fechaEntregaCell.textContent.trim();
            const [day, month, year] = fechaTexto.split('/');
            const fechaEntrega = new Date(year, month - 1, day);

            // Calcular diferencia en días
            const diffTime = fechaEntrega - today;
            const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

            // Si está entre 1 y 3 días próximos
            if (diffDays >= 0 && diffDays <= 3) {
                row.setAttribute('data-proximo-entrega', 'true');
                row.style.borderLeft = '4px solid #FFA500';
                row.style.backgroundColor = 'rgba(255, 165, 0, 0.05)';
            }
        }
    });

    // Mejorar validación de fechas
    const fechaInicio = document.querySelector('input[name="fechaInicio"]');
    const fechaFin = document.querySelector('input[name="fechaFin"]');

    if (fechaInicio && fechaFin) {
        fechaInicio.addEventListener('change', function () {
            if (fechaFin.value && new Date(this.value) > new Date(fechaFin.value)) {
                fechaFin.value = this.value;
            }
        });

        fechaFin.addEventListener('change', function () {
            if (fechaInicio.value && new Date(this.value) < new Date(fechaInicio.value)) {
                this.value = fechaInicio.value;
            }
        });
    }

    // Mostrar/ocultar opciones de estado según estado actual
    const estadoSelects = document.querySelectorAll('select[name="EstadoSeleccionado"]');

    estadoSelects.forEach(select => {
        const row = select.closest('tr');
        const estadoCell = row.querySelector('td:nth-child(6)');
        const estadoActual = estadoCell.textContent.trim();

        // Limpiar opciones existentes
        select.innerHTML = '';

        // Agregar opciones según estado actual
        switch (estadoActual) {
            case 'Pagado':
                select.innerHTML = `
                    <option value="2">Enviar</option>
                    <option value="4">Cancelar</option>
                `;
                break;
            case 'Enviado':
                select.innerHTML = `
                    <option value="3">Entregar</option>
                `;
                break;
            default:
                select.style.display = 'none';
                select.nextElementSibling.style.display = 'none'; // Ocultar botón también
        }
    });
});