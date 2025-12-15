document.addEventListener("DOMContentLoaded", function () {

    const abrirModal = document.getElementById("abrirModalPassword");

    if (abrirModal) {
        const modalElement = document.getElementById("cambiarPasswordModal");
        if (modalElement) {
            const modal = new bootstrap.Modal(modalElement);
            modal.show();
        }
    }
});
