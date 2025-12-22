document.addEventListener("DOMContentLoaded", function () {
    const stars = document.querySelectorAll("#rating span");
    const input = document.getElementById("Puntuacion");

    stars.forEach(star => {
        star.addEventListener("click", function () {
            const value = this.getAttribute("data-value");
            input.value = value;

            stars.forEach(s => {
                s.classList.toggle("active", s.getAttribute("data-value") <= value);
            });
        });
    });
});
