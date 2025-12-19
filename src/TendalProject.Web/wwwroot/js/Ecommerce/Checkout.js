document.addEventListener('DOMContentLoaded', function () {

    const fechaEntregaInput = document.getElementById('FechaEntrega');
    if (!fechaEntregaInput) return;

    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);

    const minDate = tomorrow.toISOString().split('T')[0];
    fechaEntregaInput.min = minDate;

    if (!fechaEntregaInput.value || fechaEntregaInput.value < minDate) {
        fechaEntregaInput.value = minDate;
    }

    fechaEntregaInput.addEventListener('change', function () {
        const selectedDate = new Date(this.value);
        const maxDate = new Date(today);
        maxDate.setDate(maxDate.getDate() + 30);

        if (selectedDate < tomorrow || selectedDate > maxDate) {
            this.classList.add('is-invalid');
            this.classList.remove('is-valid');
            this.value = minDate;
            this.focus();
            return;
        }

        this.classList.remove('is-invalid');
        this.classList.add('is-valid');
    });

});
