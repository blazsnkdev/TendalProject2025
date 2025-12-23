function confirmarCambioEstado(estadoActual) {

    const activando = !estadoActual;

    Swal.fire({
        title: activando
            ? "¿Quieres activar este usuario?"
            : "¿Quieres desactivar este usuario?",

        text: activando
            ? "El usuario volverá a tener acceso al sistema."
            : "El usuario quedará inactivo y no podrá ingresar.",

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
