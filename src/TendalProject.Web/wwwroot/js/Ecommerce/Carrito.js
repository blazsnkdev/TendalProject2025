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


// Archivo: wwwroot/js/Ecommerce/Carrito.js

document.addEventListener('DOMContentLoaded', function () {
    // Inicializar tooltips de Bootstrap
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Manejar cambio de cantidad
    const quantityInputs = document.querySelectorAll('input[name="cantidad"]');

    quantityInputs.forEach(input => {
        const originalValue = input.value;

        input.addEventListener('input', function () {
            if (this.value !== originalValue) {
                this.classList.add('changed');

                // Mostrar botón de actualizar
                const updateBtn = this.nextElementSibling;
                if (updateBtn && updateBtn.classList.contains('btn-success')) {
                    updateBtn.style.display = 'inline-block';
                    updateBtn.innerHTML = '<i class="bi bi-check-lg"></i>';
                }
            } else {
                this.classList.remove('changed');

                // Ocultar botón de actualizar
                const updateBtn = this.nextElementSibling;
                if (updateBtn && updateBtn.classList.contains('btn-success')) {
                    updateBtn.style.display = 'none';
                }
            }

            // Validar valor mínimo
            if (this.value < 1) {
                this.value = 1;
            }
        });

        // Enviar formulario automáticamente al perder foco (si cambió)
        input.addEventListener('blur', function () {
            if (this.classList.contains('changed')) {
                this.closest('form').submit();
            }
        });
    });

    // Confirmación para eliminar items
    const deleteForms = document.querySelectorAll('form[action*="EliminarItem"]');

    deleteForms.forEach(form => {
        const button = form.querySelector('button');

        button.addEventListener('click', function (e) {
            e.preventDefault();

            if (confirm('¿Estás seguro de que quieres eliminar este artículo del carrito?')) {
                // Agregar efecto de carga
                this.classList.add('btn-loading');
                this.disabled = true;

                // Enviar formulario
                form.submit();
            }
        });
    });

    // Función para vaciar carrito con confirmación
    window.VaciarCarritoConfirmacion = function () {
        if (confirm('¿Estás seguro de que quieres vaciar todo el carrito? Esta acción no se puede deshacer.')) {
            // Mostrar modal de confirmación personalizado
            showCustomConfirmModal(
                'Vaciar Carrito',
                '¿Estás completamente seguro? Se eliminarán todos los artículos de tu carrito.',
                'Sí, vaciar carrito',
                'Cancelar',
                function () {
                    // Ejecutar vaciado
                    const form = document.getElementById('formVaciarCarrito');
                    if (form) {
                        form.submit();
                    }
                }
            );
        }
    };

    // Mostrar modal de confirmación personalizado
    function showCustomConfirmModal(title, message, confirmText, cancelText, onConfirm) {
        // Crear modal si no existe
        let modal = document.getElementById('customConfirmModal');

        if (!modal) {
            modal = document.createElement('div');
            modal.id = 'customConfirmModal';
            modal.className = 'modal-confirm';
            modal.innerHTML = `
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">${title}</h5>
                    </div>
                    <div class="modal-body">
                        <p>${message}</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" id="cancelBtn">${cancelText}</button>
                        <button type="button" class="btn btn-danger" id="confirmBtn">${confirmText}</button>
                    </div>
                </div>
            `;
            document.body.appendChild(modal);

            // Agregar event listeners
            document.getElementById('cancelBtn').addEventListener('click', function () {
                modal.style.display = 'none';
            });

            document.getElementById('confirmBtn').addEventListener('click', function () {
                modal.style.display = 'none';
                if (typeof onConfirm === 'function') {
                    onConfirm();
                }
            });

            // Cerrar modal al hacer clic fuera
            modal.addEventListener('click', function (e) {
                if (e.target === modal) {
                    modal.style.display = 'none';
                }
            });
        }

        // Mostrar modal
        modal.style.display = 'flex';
    }

    // Calcular y mostrar total en tiempo real
    function calculateRealTimeTotal() {
        let total = 0;
        let itemCount = 0;

        document.querySelectorAll('tbody tr').forEach(row => {
            const priceText = row.querySelector('td:nth-child(3)').textContent;
            const quantityInput = row.querySelector('input[name="cantidad"]');
            const subtotalCell = row.querySelector('td:nth-child(5) strong');

            if (priceText && quantityInput && subtotalCell) {
                const price = parseFloat(priceText.replace('S/ ', '').replace(',', ''));
                const quantity = parseInt(quantityInput.value) || 1;
                const subtotal = price * quantity;

                // Actualizar subtotal en la tabla
                subtotalCell.textContent = `S/ ${subtotal.toFixed(2)}`;

                total += subtotal;
                itemCount += quantity;
            }
        });

        // Actualizar resumen
        const totalItemsElement = document.querySelector('.resumen-total-items');
        const totalPriceElement = document.querySelector('.resumen-total-precio');

        if (totalItemsElement) {
            totalItemsElement.textContent = itemCount;
        }

        if (totalPriceElement) {
            totalPriceElement.textContent = `S/ ${total.toFixed(2)}`;
        }
    }

    // Actualizar total cuando cambia la cantidad
    quantityInputs.forEach(input => {
        input.addEventListener('input', calculateRealTimeTotal);
    });

    // Efecto visual al agregar/eliminar items
    function addItemAnimation(itemId) {
        const row = document.querySelector(`tr[data-item-id="${itemId}"]`);
        if (row) {
            row.style.animation = 'none';
            setTimeout(() => {
                row.style.animation = 'slideIn 0.3s ease-out';
            }, 10);
        }
    }

    function removeItemAnimation(itemId) {
        const row = document.querySelector(`tr[data-item-id="${itemId}"]`);
        if (row) {
            row.style.animation = 'slideOut 0.3s ease-out';
            setTimeout(() => {
                row.remove();
                calculateRealTimeTotal();
            }, 300);
        }
    }

    // Agregar animación CSS para remover items
    const style = document.createElement('style');
    style.textContent = `
        @keyframes slideOut {
            from {
                opacity: 1;
                transform: translateX(0);
            }
            to {
                opacity: 0;
                transform: translateX(-100%);
            }
        }
        
        @keyframes spinner-border {
            to { transform: rotate(360deg); }
        }
    `;
    document.head.appendChild(style);

    // Validar stock antes de proceder al pago
    const checkoutBtn = document.querySelector('a[href*="Checkout"]');
    if (checkoutBtn) {
        checkoutBtn.addEventListener('click', function (e) {
            // Verificar si hay items con cantidad 0 o negativa
            let invalidItems = false;
            quantityInputs.forEach(input => {
                if (input.value < 1) {
                    invalidItems = true;
                    input.focus();
                    alert('Por favor, ingresa una cantidad válida para todos los artículos.');
                    e.preventDefault();
                }
            });

            if (!invalidItems) {
                // Agregar efecto de carga
                this.classList.add('btn-loading');
                this.querySelector('i').style.display = 'none';
            }
        });
    }

    // Inicializar contador de items
    updateItemCountBadge();

    function updateItemCountBadge() {
        const itemCount = document.querySelectorAll('tbody tr').length;
        const title = document.querySelector('h2');

        // Crear o actualizar badge
        let badge = title.querySelector('.total-badge');
        if (!badge) {
            badge = document.createElement('span');
            badge.className = 'total-badge';
            title.appendChild(badge);
        }

        badge.textContent = itemCount;

        // Ocultar si no hay items
        if (itemCount === 0) {
            badge.style.display = 'none';
        } else {
            badge.style.display = 'inline-flex';
        }
    }

    // Animación para el carrito vacío
    const emptyCartAlert = document.querySelector('.alert-warning');
    if (emptyCartAlert) {
        emptyCartAlert.style.animation = 'pulse 2s infinite';
    }

    // Agregar data attributes para animaciones
    document.querySelectorAll('tbody tr').forEach((row, index) => {
        const itemId = row.querySelector('form[action*="EliminarItem"]')?.getAttribute('action')?.match(/itemId=(\d+)/)?.[1];
        if (itemId) {
            row.setAttribute('data-item-id', itemId);
        }
        row.style.animationDelay = `${index * 0.1}s`;
    });
});