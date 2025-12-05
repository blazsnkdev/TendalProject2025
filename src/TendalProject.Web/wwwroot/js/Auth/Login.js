// Esperar a que el DOM esté completamente cargado
document.addEventListener('DOMContentLoaded', function () {
    // Elementos del DOM
    const loginForm = document.getElementById('loginForm');
    const togglePassword = document.getElementById('togglePassword');
    const passwordInput = document.querySelector('input[name="Password"]');
    const submitBtn = document.getElementById('submitBtn');
    const buttonText = document.getElementById('buttonText');
    const spinner = document.getElementById('spinner');

    // Si existen los elementos
    if (togglePassword && passwordInput) {
        // Mostrar/ocultar contraseña
        togglePassword.addEventListener('click', function () {
            const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordInput.setAttribute('type', type);
            this.innerHTML = type === 'password'
                ? '<i class="fas fa-eye"></i>'
                : '<i class="fas fa-eye-slash"></i>';
        });
    }

    // Manejar envío del formulario
    if (loginForm) {
        loginForm.addEventListener('submit', function (e) {
            // Validación básica del lado del cliente
            if (!validateForm()) {
                e.preventDefault();
                return;
            }

            // Mostrar estado de carga
            if (submitBtn && buttonText && spinner) {
                submitBtn.disabled = true;
                buttonText.style.opacity = '0';
                spinner.style.display = 'block';
            }

            // Permitir que el formulario se envíe normalmente
            // ASP.NET se encargará de la validación del servidor
        });
    }

    // Validación del formulario
    function validateForm() {
        let isValid = true;

        // Validar email
        const emailInput = document.querySelector('input[name="Email"]');
        if (emailInput && !emailInput.value.trim()) {
            showFieldError(emailInput, 'El correo electrónico es requerido');
            isValid = false;
        } else if (emailInput && !isValidEmail(emailInput.value.trim())) {
            showFieldError(emailInput, 'Ingresa un correo electrónico válido');
            isValid = false;
        } else {
            clearFieldError(emailInput);
        }

        // Validar contraseña
        if (passwordInput && !passwordInput.value) {
            showFieldError(passwordInput, 'La contraseña es requerida');
            isValid = false;
        } else if (passwordInput && passwordInput.value.length < 6) {
            showFieldError(passwordInput, 'La contraseña debe tener al menos 6 caracteres');
            isValid = false;
        } else {
            clearFieldError(passwordInput);
        }

        return isValid;
    }

    // Funciones de ayuda
    function isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    function showFieldError(input, message) {
        // Remover clases de error anteriores
        input.classList.remove('input-validation-error');

        // Agregar clase de error
        input.classList.add('input-validation-error');

        // Buscar o crear elemento de error
        let errorSpan = input.parentNode.nextElementSibling;
        if (!errorSpan || !errorSpan.classList.contains('field-validation-error')) {
            errorSpan = document.createElement('span');
            errorSpan.className = 'field-validation-error';
            input.parentNode.parentNode.insertBefore(errorSpan, input.parentNode.nextSibling);
        }

        // Mostrar mensaje de error
        errorSpan.textContent = message;
        errorSpan.style.display = 'block';
    }

    function clearFieldError(input) {
        // Remover clase de error
        input.classList.remove('input-validation-error');

        // Ocultar mensaje de error
        const errorSpan = input.parentNode.nextElementSibling;
        if (errorSpan && errorSpan.classList.contains('field-validation-error')) {
            errorSpan.style.display = 'none';
        }
    }

    // Funciones para login social (simuladas)
    window.loginWithGoogle = function () {
        alert('Redirigiendo a Google para autenticación (simulación)');
        // Aquí iría la lógica real de OAuth
    };

    window.loginWithMicrosoft = function () {
        alert('Redirigiendo a Microsoft para autenticación (simulación)');
        // Aquí iría la lógica real de OAuth
    };
});

// Para cuando el formulario tiene errores de validación del servidor
// Mostrar automáticamente los errores si existen
window.onload = function () {
    const errorElements = document.querySelectorAll('.field-validation-error');
    errorElements.forEach(element => {
        if (element.textContent.trim() !== '') {
            element.style.display = 'block';
            const input = element.previousElementSibling?.querySelector('.form-control');
            if (input) {
                input.classList.add('input-validation-error');
            }
        }
    });
};