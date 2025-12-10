document.addEventListener("DOMContentLoaded", function () {
    const estados = ["Pendiente", "Procesando", "Enviado", "Entregado", "Cancelado"];
    const select = document.querySelector("#estadoSelect");

    estados.forEach(estado => {
        const option = document.createElement("option");
        option.value = estado;
        option.textContent = estado;
        if (estado === select.dataset.selected) {
            option.selected = true;
        }
        select.appendChild(option);
    });
});
