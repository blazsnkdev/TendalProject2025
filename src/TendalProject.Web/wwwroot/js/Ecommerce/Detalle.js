document.addEventListener('DOMContentLoaded', function() {
    
    const cantidadInput = document.querySelector('input[name="cantidad"]');
    const maxStock = parseInt(cantidadInput.max) || 0;
    
    cantidadInput.addEventListener('input', function() {
        let value = parseInt(this.value) || 0;
        
        if (value < 1) {
            this.value = 1;
            value = 1;
        } else if (value > maxStock) {
            this.value = maxStock;
            value = maxStock;
        }
        
        updateStockIndicator(value);
        
        if (value > maxStock * 0.8) {
            this.classList.remove('border-warning', 'border-danger');
            this.classList.add('border-success');
        } else if (value > maxStock * 0.3) {
            this.classList.remove('border-success', 'border-danger');
            this.classList.add('border-warning');
        } else {
            this.classList.remove('border-success', 'border-warning');
            this.classList.add('border-danger');
        }
    });
    
    function updateStockIndicator(quantity) {
        const stockElement = document.querySelector('dd:nth-child(10)');
        if (stockElement) {
            const remaining = maxStock - quantity;
            
            stockElement.innerHTML = `
                <span class="stock-counter">
                    <span class="stock-indicator ${remaining < 10 ? 'stock-low' : ''} ${remaining === 0 ? 'stock-out' : ''}"></span>
                    ${remaining} disponibles de ${maxStock} unidades
                </span>
            `;
            
            
            if (remaining < 10 && remaining > 0) {
                showStockAlert(remaining);
            } else if (remaining === 0) {
                showOutOfStockAlert();
            } else {
                hideStockAlert();
            }
        }
    }
    
    updateStockIndicator(1);
    
    
    function showStockAlert(remaining) {
        let alertElement = document.getElementById('stock-alert');
        if (!alertElement) {
            alertElement = document.createElement('div');
            alertElement.id = 'stock-alert';
            alertElement.className = 'alert alert-warning mt-3';
            cantidadInput.parentNode.appendChild(alertElement);
        }
        
        alertElement.innerHTML = `
            <i class="fas fa-exclamation-triangle me-2"></i>
            <strong>Stock bajo:</strong> Solo quedan ${remaining} unidades disponibles.
            <br><small>Aprovecha antes de que se agoten.</small>
        `;
        alertElement.style.display = 'block';
    }
    
    function showOutOfStockAlert() {
        let alertElement = document.getElementById('stock-alert');
        if (!alertElement) {
            alertElement = document.createElement('div');
            alertElement.id = 'stock-alert';
            alertElement.className = 'alert alert-danger mt-3';
            cantidadInput.parentNode.appendChild(alertElement);
        }
        
        alertElement.innerHTML = `
            <i class="fas fa-ban me-2"></i>
            <strong>Agotado:</strong> No hay unidades disponibles.
            <br><small>Vuelve a consultar más tarde.</small>
        `;
        alertElement.style.display = 'block';
        
        const addButton = document.querySelector('.btn-primary-tendal');
        if (addButton) {
            addButton.disabled = true;
            addButton.style.opacity = '0.6';
            addButton.style.cursor = 'not-allowed';
            addButton.textContent = 'Agotado';
        }
    }
    
    function hideStockAlert() {
        const alertElement = document.getElementById('stock-alert');
        if (alertElement) {
            alertElement.style.display = 'none';
        }
    }
    
    const addToCartForm = document.querySelector('form[action*="Agregar"]');
    if (addToCartForm) {
        addToCartForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            const quantity = parseInt(cantidadInput.value);
            
            if (quantity < 1) {
                cantidadInput.focus();
                cantidadInput.classList.add('is-invalid');
                return false;
            }
            
            if (quantity > maxStock) {
                alert(`Solo hay ${maxStock} unidades disponibles.`);
                cantidadInput.focus();
                cantidadInput.classList.add('is-invalid');
                return false;
            }
            
            const submitBtn = this.querySelector('button[type="submit"]');
            const originalText = submitBtn.innerHTML;
            submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> Agregando...';
            submitBtn.disabled = true;
            submitBtn.classList.add('btn-loading');
            
            setTimeout(() => {
                
                showAddToCartMessage(quantity);
                
                
                submitBtn.innerHTML = originalText;
                submitBtn.disabled = false;
                submitBtn.classList.remove('btn-loading');
                
                
                this.submit();
            }, 1500);
        });
    }
    
    function showAddToCartMessage(quantity) {
        
        const message = document.createElement('div');
        message.className = 'added-to-cart-message';
        message.innerHTML = `
            <i class="fas fa-check-circle fa-lg"></i>
            <div>
                <strong>¡Agregado al carrito!</strong>
                <div style="font-size: 0.9rem; opacity: 0.9;">
                    ${quantity} unidad${quantity > 1 ? 'es' : ''} de 
                    <strong>${document.querySelector('dd:nth-child(2)').textContent.trim()}</strong>
                </div>
            </div>
        `;
        
        document.body.appendChild(message);
        
        setTimeout(() => {
            if (message.parentNode) {
                message.parentNode.removeChild(message);
            }
        }, 3000);
    }
    
    const ratingElement = document.querySelector('dd:nth-child(14)');
    if (ratingElement) {
        const ratingValue = parseFloat(ratingElement.textContent.trim()) || 0;
        createStarRating(ratingValue, ratingElement);
    }
    
    function createStarRating(rating, container) {
        const fullStars = Math.floor(rating);
        const hasHalfStar = rating % 1 >= 0.5;
        
        container.innerHTML = `
            <div class="rating-container">
                <div class="rating-stars">
                    ${Array.from({length: 5}, (_, i) => {
                        if (i < fullStars) {
                            return '<span class="star full">★</span>';
                        } else if (i === fullStars && hasHalfStar) {
                            return '<span class="star half">★</span>';
                        } else {
                            return '<span class="star empty">☆</span>';
                        }
                    }).join('')}
                </div>
                <span class="rating-value">${rating.toFixed(1)}</span>
            </div>
        `;
    }
    
    const imageElement = document.querySelector('dd:nth-child(12)');
    if (imageElement) {
        const imageSrc = imageElement.textContent.trim();
        if (imageSrc) {
            imageElement.innerHTML = `
                <div class="product-image-container">
                    <img src="${imageSrc}" 
                         alt="${document.querySelector('dd:nth-child(2)').textContent.trim()}"
                         class="product-image"
                         onerror="this.src='/images/default-product.png'">
                    <div class="stock-badge">${maxStock} disponibles</div>
                </div>
            `;
            
            const img = imageElement.querySelector('img');
            img.style.cursor = 'pointer';
            img.addEventListener('click', function() {
                openImageModal(this.src, this.alt);
            });
        }
    }
    
    function openImageModal(src, alt) {
        const modal = document.createElement('div');
        modal.className = 'image-modal';
        modal.innerHTML = `
            <span class="close-modal">&times;</span>
            <img src="${src}" alt="${alt}" class="modal-image">
        `;
        
        document.body.appendChild(modal);
        modal.style.display = 'flex';
        
        modal.querySelector('.close-modal').addEventListener('click', function() {
            modal.style.display = 'none';
            setTimeout(() => {
                if (modal.parentNode) {
                    modal.parentNode.removeChild(modal);
                }
            }, 300);
        });
        
        modal.addEventListener('click', function(e) {
            if (e.target === modal) {
                modal.style.display = 'none';
                setTimeout(() => {
                    if (modal.parentNode) {
                        modal.parentNode.removeChild(modal);
                    }
                }, 300);
            }
        });
        
        document.addEventListener('keydown', function closeOnEsc(e) {
            if (e.key === 'Escape') {
                modal.style.display = 'none';
                setTimeout(() => {
                    if (modal.parentNode) {
                        modal.parentNode.removeChild(modal);
                    }
                }, 300);
                document.removeEventListener('keydown', closeOnEsc);
            }
        });
    }
    
    cantidadInput.addEventListener('input', function() {
        const quantity = parseInt(this.value) || 1;
        const priceElement = document.querySelector('dd:nth-child(8)');
        const priceText = priceElement.textContent.trim();
        const unitPrice = parseFloat(priceText.replace(/[^0-9.]/g, ''));
        
        if (!isNaN(unitPrice)) {
            const totalPrice = unitPrice * quantity;
            
            const originalContent = priceElement.innerHTML;
            priceElement.dataset.originalContent = originalContent;
            priceElement.innerHTML = `
                <span style="color: var(--primary-color);">S/ ${unitPrice.toFixed(2)}</span>
                <small style="display: block; color: var(--gray-color); font-size: 0.9rem;">
                    × ${quantity} = <strong>S/ ${totalPrice.toFixed(2)}</strong>
                </small>
            `;
            
            clearTimeout(priceElement.restoreTimeout);
            priceElement.restoreTimeout = setTimeout(() => {
                if (priceElement.dataset.originalContent) {
                    priceElement.innerHTML = priceElement.dataset.originalContent;
                }
            }, 3000);
        }
    });
    
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
    
    document.querySelectorAll('dt, dd, form, a').forEach(el => {
        observer.observe(el);
    });
    
    const ratingValue = parseFloat(document.querySelector('dd:nth-child(14)').textContent.trim()) || 0;
    if (ratingValue >= 4.5) {
        const imageContainer = document.querySelector('.product-image-container');
        if (imageContainer) {
            const featuredBadge = document.createElement('div');
            featuredBadge.className = 'featured-badge';
            featuredBadge.textContent = 'Destacado ⭐';
            imageContainer.appendChild(featuredBadge);
        }
    }
});