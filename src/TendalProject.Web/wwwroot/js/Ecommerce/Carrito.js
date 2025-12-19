document.addEventListener('DOMContentLoaded', function () {


    const quantityInputs = document.querySelectorAll('input[name="cantidad"]');

    quantityInputs.forEach(input => {
        const originalValue = input.value;

        input.addEventListener('input', function () {
            if (this.value < 1) this.value = 1;

            const updateBtn = this.nextElementSibling;

            if (this.value !== originalValue) {
                this.classList.add('changed');

                if (updateBtn && updateBtn.classList.contains('btn-success')) {
                    updateBtn.style.display = 'inline-flex';
                    updateBtn.innerHTML = '<i class="bi bi-check-lg"></i>';
                }
            } else {
                this.classList.remove('changed');

                if (updateBtn && updateBtn.classList.contains('btn-success')) {
                    updateBtn.style.display = 'none';
                }
            }

            calcularTotales();
        });

        input.addEventListener('blur', function () {
            if (this.classList.contains('changed')) {
                this.closest('form').submit();
            }
        });
    });

    document.querySelectorAll('form[action*="EliminarItem"]').forEach(form => {
        const btn = form.querySelector('button');

        btn.addEventListener('click', function (e) {
            e.preventDefault();

            Swal.fire({
                title: "¿Eliminar producto?",
                text: "El producto se quitará del carrito",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Eliminar",
                cancelButtonText: "Cancelar",
                confirmButtonColor: "#dc3545"
            }).then(result => {
                if (result.isConfirmed) {
                    btn.disabled = true;
                    btn.innerHTML = '<span class="spinner-border spinner-border-sm"></span>';
                    form.submit();
                }
            });
        });
    });

    function calcularTotales() {
        let total = 0;
        let cantidadTotal = 0;

        document.querySelectorAll('tbody tr').forEach(row => {
            const precioText = row.querySelector('td:nth-child(3)')?.textContent;
            const cantidad = parseInt(row.querySelector('input[name="cantidad"]')?.value || 1);
            const subtotalCell = row.querySelector('td:nth-child(5) strong');

            if (!precioText || !subtotalCell) return;

            const precio = parseFloat(precioText.replace('S/', '').trim());
            const subtotal = precio * cantidad;

            subtotalCell.textContent = `S/ ${subtotal.toFixed(2)}`;

            total += subtotal;
            cantidadTotal += cantidad;
        });

        document.querySelector('.resumen-total-items')?.textContent = cantidadTotal;
        document.querySelector('.resumen-total-precio')?.textContent = `S/ ${total.toFixed(2)}`;
    }

    const checkoutBtn = document.querySelector('a[href*="Checkout"]');

    checkoutBtn?.addEventListener('click', function (e) {
        let invalido = false;

        quantityInputs.forEach(input => {
            if (input.value < 1) {
                invalido = true;
                input.focus();
            }
        });

        if (invalido) {
            e.preventDefault();
            Swal.fire({
                icon: "error",
                title: "Cantidad inválida",
                text: "Verifica la cantidad de tus productos"
            });
        }
    });

});
