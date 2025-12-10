function confirmarCambioEstado(estadoActual) {

    const activando = estadoActual !== "Activo";

    Swal.fire({
        title: activando
            ? "¿Quieres activar este Cliente?"
            : "¿Quieres desactivar a este Cliente?",

        text: activando
            ? "El Cliente volverá a estar disponible para la venta."
            : "El Cliente quedará inactivo y no se podrá vender.",

        icon: "warning",
        showCancelButton: true,
        confirmButtonText: activando ? "Sí, activar" : "Sí, desactivar",
        cancelButtonText: "Cancelar",

        confirmButtonColor: activando ? "#28a745" : "#d33",
        cancelButtonColor: "#6c757d",
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById("formEstado").submit();
        }
    });

}