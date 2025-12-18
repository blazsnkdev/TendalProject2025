// Archivo: wwwroot/js/Ecommerce/Checkout.js

document.addEventListener('DOMContentLoaded', function () {
    // Validar fecha de entrega
    const fechaEntregaInput = document.getElementById('FechaEntrega');
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);

    // Establecer fecha mínima (mañana)
    const minDate = tomorrow.toISOString().split('T')[0];
    fechaEntregaInput.min = minDate;

    // Si la fecha actual es menor que la mínima, establecerla
    if (fechaEntregaInput.value < minDate) {
        fechaEntregaInput.value = minDate;
    }

    // Validar fecha al cambiar
    fechaEntregaInput.addEventListener('change', function () {
        const selectedDate = new Date(this.value);
        const maxDate = new Date(today);
        maxDate.setDate(maxDate.getDate() + 30); // Máximo 30 días

        if (selectedDate < tomorrow) {
            alert('La fecha de entrega debe ser al menos mañana.');
            this.value = minDate;
            this.focus();
        } else if (selectedDate > maxDate) {
            alert('La fecha de entrega no puede ser mayor a 30 días.');
            this.value = minDate;
            this.focus();
        }

        // Marcar como válido/inválido visualmente
        if (this.checkValidity()) {
            this.classList.remove('is-invalid');
            this.classList.add('is-valid');
        } else {
            this.classList.remove('is-valid');
            this.classList.add('is-invalid');
        }
    });

    // Formatear precios
    function formatPrice(price) {
        return `S/ ${parseFloat(price).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
    }

    // Actualizar total en tiempo real
    function updateTotalDisplay() {
        const totalElement = document.querySelector('strong:contains("S/")');
        if (totalElement) {
            const total = parseFloat('@Model.Total');
            totalElement.textContent = formatPrice(total);
        }
    }

    // Manejar envío del formulario
    const checkoutForm = document.querySelector('form[action*="Pagar"]');
    if (checkoutForm) {
        checkoutForm.addEventListener('submit', function (e) {
            e.preventDefault();

            // Validar fecha de entrega
            if (!fechaEntregaInput.checkValidity()) {
                fechaEntregaInput.focus();
                fechaEntregaInput.classList.add('is-invalid');
                return false;
            }

            // Mostrar loading en el botón
            const submitBtn = this.querySelector('button[type="submit"]');
            const originalText = submitBtn.innerHTML;
            submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> Procesando pago...';
            submitBtn.disabled = true;
            submitBtn.classList.add('btn-loading');

            // Simular procesamiento (en producción, esto sería real)
            setTimeout(() => {
                // En producción, aquí se enviaría el formulario
                // this.submit();

                // Por ahora solo mostramos un mensaje
                submitBtn.innerHTML = originalText;
                submitBtn.disabled = false;
                submitBtn.classList.remove('btn-loading');

                alert('Procesando pago con Mercado Pago...');
                // Continuar con el envío real
                this.submit();
            }, 1500);
        });
    }

    // Calcular y mostrar fecha estimada de entrega
    function calculateEstimatedDelivery() {
        const selectedDate = new Date(fechaEntregaInput.value);
        const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
        const formattedDate = selectedDate.toLocaleDateString('es-ES', options);

        // Crear o actualizar elemento de fecha estimada
        let deliveryInfo = document.getElementById('delivery-info');
        if (!deliveryInfo) {
            deliveryInfo = document.createElement('div');
            deliveryInfo.id = 'delivery-info';
            deliveryInfo.className = 'alert alert-info mt-3';
            fechaEntregaInput.parentNode.appendChild(deliveryInfo);
        }

        deliveryInfo.innerHTML = `
            <i class="fas fa-truck me-2"></i>
            <strong>Entrega estimada:</strong> ${formattedDate}
            <br>
            <small class="text-muted">El pedido llegará en esta fecha.</small>
        `;
    }

    fechaEntregaInput.addEventListener('change', calculateEstimatedDelivery);

    // Inicializar fecha estimada
    calculateEstimatedDelivery();

    // Agregar clase para precio destacado
    const totalPriceElement = document.querySelector('p.d-flex:last-child strong');
    if (totalPriceElement) {
        totalPriceElement.classList.add('precio-destacado');
    }

    // Agregar badge a cantidad total
    const cantidadElement = document.querySelector('p.d-flex:first-child strong');
    if (cantidadElement) {
        const badge = document.createElement('span');
        badge.className = 'cantidad-badge';
        badge.textContent = '@Model.CantidadTotal';
        cantidadElement.appendChild(badge);
    }

    // Efecto hover para imágenes de productos
    const productImages = document.querySelectorAll('img[alt]');
    productImages.forEach(img => {
        img.addEventListener('mouseenter', function () {
            this.style.transform = 'scale(1.2)';
            this.style.transition = 'transform 0.3s ease';
        });

        img.addEventListener('mouseleave', function () {
            this.style.transform = 'scale(1)';
        });
    });

    // Validar que el carrito no esté vacío
    const itemCount = document.querySelectorAll('tbody tr').length;
    if (itemCount === 0) {
        const checkoutBtn = document.querySelector('button[type="submit"]');
        if (checkoutBtn) {
            checkoutBtn.disabled = true;
            checkoutBtn.textContent = 'Carrito vacío';
            checkoutBtn.style.opacity = '0.6';
            checkoutBtn.style.cursor = 'not-allowed';

            // Agregar mensaje
            const message = document.createElement('div');
            message.className = 'alert alert-warning mt-3';
            message.innerHTML = `
                <i class="fas fa-shopping-cart me-2"></i>
                Tu carrito está vacío. 
                <a href="/Ecommerce/Carrito" class="alert-link">Agrega productos</a> 
                para continuar con la compra.
            `;
            checkoutForm.parentNode.insertBefore(message, checkoutForm);
        }
    }

    // Agregar tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[title]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Crear barra de progreso (opcional)
    function createProgressBar() {
        const progressBar = document.createElement('div');
        progressBar.className = 'checkout-progress';
        progressBar.innerHTML = `
            <div class="checkout-step active">1</div>
            <div class="checkout-step active">2</div>
            <div class="checkout-step">3</div>
        `;

        const container = document.querySelector('.container');
        container.insertBefore(progressBar, container.firstChild);
    }

    // Descomentar para activar la barra de progreso
    // createProgressBar();

    // Agregar animación de entrada para elementos
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animated');
            }
        });
    }, observerOptions);

    // Observar elementos para animación
    document.querySelectorAll('.card, .table tbody tr').forEach(el => {
        observer.observe(el);
    });
});