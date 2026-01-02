# 🛒 TendalProject2025 – Sistema eCommerce

**TendalProject2025** es un sistema **eCommerce web para el minimaket Tendal** desarrollado en **ASP.NET Core MVC**, enfocado en buenas prácticas de arquitectura, seguridad y escalabilidad.  
Permite la gestión completa de productos, pedidos y pagos, integrando una pasarela de pago real.

---

## 🚀 Funcionalidades principales

- 🔐 **Autenticación y autorización**
  - Implementación con **Claims Identity**
  - Control de acceso por usuario autenticado

- 👤 **Gestión de clientes**
  - Registro y autenticación
  - Gestión de perfil
  - Historial de pedidos

- 📦 **Gestión de catálogo**
  - CRUD de artículos
  - CRUD de categorías
  - CRUD de proveedores

- 🛍️ **Pedidos y ventas**
  - Creación de pedidos
  - Flujo de compra completo
  - Confirmación de pagos

- 💳 **Pagos en línea**
  - Integración con **MercadoPago**
  - Manejo de estados de pago

---

## 🧱 Arquitectura del proyecto

El sistema está organizado bajo **Arquitectura N-Capas**, separando responsabilidades para facilitar mantenimiento y escalabilidad:

- **Web** → ASP.NET Core MVC (Razor)
- **Negocio** → Reglas de negocio y servicios
- **Datos** → Acceso a datos (EF Core)
- **Entidades** → Modelos de dominio
- **Common** → Clases compartidas (Result, utilidades, constantes)

---

## 🛠️ Patrones y buenas prácticas

- Unit of Work
- Repository Pattern
- Result Pattern
- Inyección de Dependencias
- Manejo de asincronía (`async / await`)
- Uso de **ViewModels** y **DTOs**
- Separación de responsabilidades (SRP)
- Manejo de errores controlado

---

## 🧰 Tecnologías utilizadas

### Backend
- C#
- ASP.NET Core MVC
- Entity Framework Core
- DbContext
- Migraciones
- Claims Identity
- SQL Server

### Frontend
- Razor (vistas)
- HTML5
- CSS3
- JavaScript
- Bootstrap

### Integraciones
- MercadoPago (pasarela de pagos)

---

## ⚙️ Instalación y configuración

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/tu-usuario/TendalProject2025.git
