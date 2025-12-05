function confirmarCambioEstado(estadoActual) {

    const activando = estadoActual !== "Activo";

    Swal.fire({
        title: activando
            ? "¿Quieres activar este proveedor?"
            : "¿Quieres desactivar este proveedor?",

        text: activando
            ? "El proveedor volverá a estar disponible para la venta."
            : "El proveedor quedará inactivo y no se podrá vender.",

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