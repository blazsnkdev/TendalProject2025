function VaciarCarritoConfirmacion() {
    Swal.fire({
        title: "¿Estás seguro?",
        text: "Se eliminarán todos los items de tu carrito. Esta acción no se puede deshacer.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, vaciar carrito",
        cancelButtonText: "Cancelar",
        confirmButtonColor: "#d33",
        cancelButtonColor: "#6c757d",
    }).then((result) => {
        if (result.isConfirmed) {
            // Envía el form que tiene el botón de vaciar carrito
            document.getElementById("formVaciarCarrito").submit();
        }
    });
}
