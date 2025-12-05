// Esperar a que el DOM esté completamente cargado
document.addEventListener('DOMContentLoaded', function () {
    // Elementos del DOM
    const registerForm = document.getElementById('registerForm');
    const submitBtn = document.getElementById('submitBtn');
    const buttonText = document.getElementById('buttonText');
    const spinner = document.getElementById('spinner');
    const acceptTerms = document.getElementById('acceptTerms');

    // Manejar toggle de contraseñas
    document.querySelectorAll('.toggle-password').forEach(button => {
        button.addEventListener('click', function () {
            const targetField = this.getAttribute('data-target');
            const input = document.querySelector(`input[name="${targetField}"]`);

            if (input) {
                const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
                input.setAttribute('type', type);
                this.innerHTML = type === 'password'
                    ? '<i class="fas fa-eye"></i>'
                    : '<i class="fas fa-eye-slash"></i>';
            }
        });
    });

    // Validación de fecha de nacimiento
    const fechaInput = document.querySelector('input[name="FechaNacimiento"]');
    if (fechaInput) {
        // Establecer fecha máxima (hoy)
        const today = new Date().toISOString().split('T')[0];
        fechaInput.setAttribute('max', today);

        // Establecer fecha mínima (personas mayores de 18 años)
        const minDate = new Date();
        minDate.setFullYear(minDate.getFullYear() - 100);
        fechaInput.setAttribute('min', minDate.toISOString().split('T')[0]);

        // Validar al cambiar
        fechaInput.addEventListener('change', function () {
            validateDateOfBirth(this);
        });
    }

    // Validación de teléfono
    const phoneInput = document.querySelector('input[name="NumeroCelular"]');
    if (phoneInput) {
        phoneInput.addEventListener('input', function () {
            // Solo permitir números
            this.value = this.value.replace(/\D/g, '');

            // Validar longitud
            if (this.value.length > 15) {
                this.value = this.value.slice(0, 15);
            }
        });
    }

    // Validación de contraseña
    const passwordInput = document.querySelector('input[name="Password"]');
    const confirmPasswordInput = document.querySelector('input[name="ConfirmarPassword"]');

    if (passwordInput && confirmPasswordInput) {
        // Validar fortaleza de contraseña
        passwordInput.addEventListener('input', function () {
            validatePasswordStrength(this.value);

            // Si el campo de confirmación tiene valor, validar coincidencia
            if (confirmPasswordInput.value) {
                validatePasswordMatch();
            }
        });

        // Validar coincidencia de contraseñas
        confirmPasswordInput.addEventListener('input', validatePasswordMatch);
    }

    // Manejar envío del formulario
    if (registerForm) {
        registerForm.addEventListener('submit', function (e) {
            // Validar términos y condiciones
            if (acceptTerms && !acceptTerms.checked) {
                e.preventDefault();
                showNotification('Debes aceptar los términos y condiciones', 'error');
                acceptTerms.focus();
                return;
            }

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

    // Funciones de validación
    function validateForm() {
        let isValid = true;

        // Validar campos requeridos
        const requiredFields = registerForm.querySelectorAll('[required]');
        requiredFields.forEach(field => {
            if (!field.value.trim() && field.type !== 'checkbox') {
                showFieldError(field, 'Este campo es requerido');
                isValid = false;
            } else {
                clearFieldError(field);
            }
        });

        // Validar email
        const emailInput = document.querySelector('input[name="Email"]');
        if (emailInput && !isValidEmail(emailInput.value.trim())) {
            showFieldError(emailInput, 'Ingresa un correo electrónico válido');
            isValid = false;
        }

        // Validar fecha de nacimiento
        if (fechaInput && !validateDateOfBirth(fechaInput)) {
            isValid = false;
        }

        // Validar teléfono
        if (phoneInput && phoneInput.value && !isValidPhone(phoneInput.value)) {
            showFieldError(phoneInput, 'Ingresa un número de teléfono válido');
            isValid = false;
        }

        // Validar contraseña
        if (passwordInput) {
            const passwordError = validatePasswordRequirements(passwordInput.value);
            if (passwordError) {
                showFieldError(passwordInput, passwordError);
                isValid = false;
            }
        }

        // Validar coincidencia de contraseñas
        if (passwordInput && confirmPasswordInput) {
            const matchError = validatePasswordMatch();
            if (matchError) {
                showFieldError(confirmPasswordInput, matchError);
                isValid = false;
            }
        }

        return isValid;
    }

    function validateDateOfBirth(input) {
        const selectedDate = new Date(input.value);
        const today = new Date();
        const minDate = new Date();
        minDate.setFullYear(minDate.getFullYear() - 100);

        if (selectedDate > today) {
            showFieldError(input, 'La fecha de nacimiento no puede ser futura');
            return false;
        }

        if (selectedDate < minDate) {
            showFieldError(input, 'La edad máxima permitida es 100 años');
            return false;
        }

        // Calcular edad
        const age = today.getFullYear() - selectedDate.getFullYear();
        const monthDiff = today.getMonth() - selectedDate.getMonth();
        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < selectedDate.getDate())) {
            age--;
        }

        if (age < 13) {
            showFieldError(input, 'Debes tener al menos 13 años para registrarte');
            return false;
        }

        clearFieldError(input);
        return true;
    }

    function isValidEmail(email) {
        if (!email) return false;
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    function isValidPhone(phone) {
        // Validación básica de teléfono (solo números, longitud mínima)
        const phoneRegex = /^\d{9,15}$/;
        return phoneRegex.test(phone);
    }

    function validatePasswordRequirements(password) {
        if (!password) return 'La contraseña es requerida';
        if (password.length < 6) return 'La contraseña debe tener al menos 6 caracteres';
        if (password.length > 50) return 'La contraseña no puede exceder los 50 caracteres';

        // Opcional: agregar más validaciones de fortaleza
        // if (!/[A-Z]/.test(password)) return 'Debe contener al menos una letra mayúscula';
        // if (!/[0-9]/.test(password)) return 'Debe contener al menos un número';
        // if (!/[^A-Za-z0-9]/.test(password)) return 'Debe contener al menos un carácter especial';

        return null;
    }

    function validatePasswordStrength(password) {
        const strengthBar = document.getElementById('passwordStrength');
        if (!strengthBar) return;

        let strength = 0;
        if (password.length >= 6) strength += 25;
        if (/[A-Z]/.test(password)) strength += 25;
        if (/[0-9]/.test(password)) strength += 25;
        if (/[^A-Za-z0-9]/.test(password)) strength += 25;

        strengthBar.style.width = strength + '%';

        let color = '#e74c3c'; // Rojo
        let text = 'Débil';

        if (strength >= 50) {
            color = '#f39c12'; // Naranja
            text = 'Media';
        }
        if (strength >= 75) {
            color = '#27ae60'; // Verde
            text = 'Fuerte';
        }

        strengthBar.style.backgroundColor = color;
        document.getElementById('strengthText').textContent = text;
    }

    function validatePasswordMatch() {
        if (!passwordInput || !confirmPasswordInput) return null;

        if (passwordInput.value !== confirmPasswordInput.value) {
            return 'Las contraseñas no coinciden';
        }

        clearFieldError(confirmPasswordInput);
        return null;
    }

    // Funciones auxiliares de UI
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

        // Scroll a campo con error
        input.scrollIntoView({ behavior: 'smooth', block: 'center' });
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

    function showNotification(message, type = 'info') {
        // Crear notificación
        const notification = document.createElement('div');
        notification.className = `notification notification-${type}`;
        notification.innerHTML = `
            <div class="notification-content">
                <i class="fas ${type === 'error' ? 'fa-exclamation-circle' : type === 'success' ? 'fa-check-circle' : 'fa-info-circle'}"></i>
                <span>${message}</span>
            </div>
            <button class="notification-close">&times;</button>
        `;

        // Estilos para la notificación
        notification.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            background: ${type === 'error' ? '#e74c3c' : type === 'success' ? '#27ae60' : '#3498db'};
            color: white;
            padding: 15px 20px;
            border-radius: 8px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.2);
            display: flex;
            align-items: center;
            justify-content: space-between;
            min-width: 300px;
            max-width: 400px;
            z-index: 1000;
            animation: slideIn 0.3s ease;
        `;

        // Agregar al body
        document.body.appendChild(notification);

        // Auto-remover después de 5 segundos
        setTimeout(() => {
            notification.style.animation = 'slideOut 0.3s ease';
            setTimeout(() => notification.remove(), 300);
        }, 5000);

        // Botón para cerrar
        notification.querySelector('.notification-close').addEventListener('click', () => {
            notification.remove();
        });
    }

    // Funciones para registro social (simuladas)
    window.registerWithGoogle = function () {
        showNotification('Redirigiendo a Google para registro (simulación)', 'info');
        // Aquí iría la lógica real de OAuth
    };

    window.registerWithMicrosoft = function () {
        showNotification('Redirigiendo a Microsoft para registro (simulación)', 'info');
        // Aquí iría la lógica real de OAuth
    };
});

// Para cuando el formulario tiene errores de validación del servidor
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

    // Mostrar notificación si hay errores de modelo
    const modelErrors = document.querySelector('.general-error');
    if (modelErrors && modelErrors.textContent.trim() !== '') {
        const firstError = modelErrors.querySelector('li');
        if (firstError) {
            showNotification(firstError.textContent, 'error');
        }
    }
};

// Animaciones CSS para notificaciones
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from {
            transform: translateX(100%);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
    
    @keyframes slideOut {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(100%);
            opacity: 0;
        }
    }
    
    .notification-content {
        display: flex;
        align-items: center;
        gap: 10px;
        flex: 1;
    }
    
    .notification-close {
        background: none;
        border: none;
        color: white;
        font-size: 20px;
        cursor: pointer;
        padding: 0;
        margin-left: 15px;
        opacity: 0.8;
        transition: opacity 0.3s ease;
    }
    
    .notification-close:hover {
        opacity: 1;
    }
`;
document.head.appendChild(style);