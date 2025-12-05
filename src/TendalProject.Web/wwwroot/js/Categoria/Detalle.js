function confirmarCambioEstado(estadoActual) {

    const activando = estadoActual !== "Activo";

    Swal.fire({
        title: activando
            ? "¿Quieres activar esta categoria?"
            : "¿Quieres desactivar esta categoria?",

        text: activando
            ? "La categoria volverá a estar disponible para la venta."
            : "La categoria quedará inactiva y no se podrá vender.",

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