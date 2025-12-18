document.addEventListener('DOMContentLoaded', function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    const autoSubmitElements = document.querySelectorAll('[onchange*="submit"]');

    autoSubmitElements.forEach(element => {
        element.addEventListener('change', function () {
            const form = document.getElementById('filterForm');
            if (form) {
                form.submit();
            }
        });
    });

    const searchForm = document.getElementById('searchForm');
    if (searchForm) {
        searchForm.addEventListener('submit', function (e) {
            const filterForm = document.getElementById('filterForm');
            if (filterForm) {
                const searchInput = this.querySelector('input[name="q"]');
                const filterSearchInput = filterForm.querySelector('input[name="q"]');

                if (searchInput && filterSearchInput) {
                    filterSearchInput.value = searchInput.value;
                }

                // Enviar el formulario de filtros
                filterForm.submit();
                e.preventDefault();
            }
        });
    });

// Validación de rango de precio
const minPrecio = document.querySelector('input[name="minPrecio"]');
const maxPrecio = document.querySelector('input[name="maxPrecio"]');

if (minPrecio && maxPrecio) {
    const filterForm = document.getElementById('filterForm');

    filterForm.addEventListener('submit', function (e) {
        const min = parseFloat(minPrecio.value);
        const max = parseFloat(maxPrecio.value);

        if (min && max && min > max) {
            alert('El precio mínimo no puede ser mayor al precio máximo');
            e.preventDefault();
            minPrecio.focus();
            return false;
        }
    });
}

// Actualizar URLs de paginación con parámetros actuales
function updatePaginationLinks() {
    const pageLinks = document.querySelectorAll('.pagination a');
    const currentParams = new URLSearchParams(window.location.search);

    pageLinks.forEach(link => {
        const href = link.getAttribute('href');
        if (href && href.includes('pagina=')) {
            const url = new URL(href, window.location.origin);
            const params = new URLSearchParams(url.search);

            // Mantener todos los parámetros excepto la página actual
            currentParams.forEach((value, key) => {
                if (key !== 'pagina') {
                    params.set(key, value);
                }
            });

            // Actualizar el href
            url.search = params.toString();
            link.setAttribute('href', url.pathname + url.search);
        }
    });
}

updatePaginationLinks();

// Efecto hover mejorado para cards
const productCards = document.querySelectorAll('.product-card');

productCards.forEach(card => {
    card.addEventListener('mouseenter', function () {
        this.style.transition = 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)';
    });

    card.addEventListener('mouseleave', function () {
        this.style.transition = 'all 0.3s ease';
    });
});

// Manejar stock bajo
const stockBadges = document.querySelectorAll('.stock-badge');

stockBadges.forEach(badge => {
    if (badge.textContent.includes('Últimas')) {
        badge.style.animation = 'pulse 2s infinite';
        badge.style.backgroundColor = 'rgba(255, 193, 7, 0.2)';
        badge.style.color = '#ffc107';
    }
});

// Agregar animación de pulso para stock bajo
const style = document.createElement('style');
style.textContent = `
        @keyframes pulse {
            0% { opacity: 1; }
            50% { opacity: 0.7; }
            100% { opacity: 1; }
        }
    `;
document.head.appendChild(style);

// Mostrar modal de carga al hacer clic en "Seleccionar"
const selectButtons = document.querySelectorAll('.btn-seleccionar');
const loadingModal = document.getElementById('loadingModal');

if (loadingModal) {
    const modal = new bootstrap.Modal(loadingModal);

    selectButtons.forEach(button => {
        button.addEventListener('click', function (e) {
            // Solo mostrar modal si el producto está disponible
            const stockBadge = this.closest('.card-body').querySelector('.stock-badge');
            if (!stockBadge || !stockBadge.textContent.includes('Agotado')) {
                modal.show();
            }
        });
    });

    // Ocultar modal cuando se carga la página de detalle
    window.addEventListener('pagehide', function () {
        modal.hide();
    });
}

// Actualizar contador de productos en tiempo real
function updateProductCount() {
    const productCount = document.querySelectorAll('.product-card').length;
    const countElement = document.querySelector('.text-muted.mb-0');

    if (countElement) {
        const match = countElement.textContent.match(/(\d+)/);
        if (match) {
            countElement.textContent = countElement.textContent.replace(
                match[1],
                productCount.toString()
            );
        }
    }
}

// Inicializar contador
updateProductCount();

});