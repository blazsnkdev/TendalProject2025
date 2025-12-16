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
