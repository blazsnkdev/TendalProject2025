function confirmarEntrega() {

    Swal.fire({
        title: "¿Marcar pedido como recepcionado?",
        text: "Esta opción no es reversible.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, marcar",
        cancelButtonText: "Cancelar",
        confirmButtonColor: "#28a745",
        cancelButtonColor: "#6c757d",
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById("formEstado").submit();
        }
    });

}
