using Microsoft.EntityFrameworkCore;
using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> TblUsuario => Set<Usuario>();
        public DbSet<Rol> TblRol => Set<Rol>();
        public DbSet<Cliente> TblCliente => Set<Cliente>();
        public DbSet<UsuarioRol> TblUsuarioRol => Set<UsuarioRol>();
        public DbSet<Articulo> TblArticulo => Set<Articulo>();
        public DbSet<Categoria> TblCategoria => Set<Categoria>();
        public DbSet<Proveedor> TblProveedor => Set<Proveedor>();
        public DbSet<Carrito> TblCarrito => Set<Carrito>();
        public DbSet<Item> TblItem => Set<Item>();
        public DbSet<Pedido> TblPedido => Set<Pedido>();
        public DbSet<DetallePedido> TblDetallePedido => Set<DetallePedido>();
        public DbSet<Venta> TblVenta => Set<Venta>();
        public DbSet<Reseña> TblReseña => Set<Reseña>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Usuario-Rol (many-to-many)
            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.UsuarioId, ur.RolId });

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuariosRoles)
                .HasForeignKey(ur => ur.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuariosRoles)
                .HasForeignKey(ur => ur.RolId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cliente - Usuario (1:1)
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithOne(u => u.Cliente)
                .HasForeignKey<Cliente>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Articulo - Categoria y Proveedor
            modelBuilder.Entity<Articulo>()
                .HasOne(a => a.Categoria)
                .WithMany(c => c.Articulos)
                .HasForeignKey(a => a.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Articulo>()
                .HasOne(a => a.Proveedor)
                .WithMany(p => p.Articulos)
                .HasForeignKey(a => a.ProveedorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            modelBuilder.Entity<Articulo>().HasIndex(a => a.Nombre);
            modelBuilder.Entity<Articulo>().HasIndex(a => a.Codigo).IsUnique();
            modelBuilder.Entity<Categoria>().HasIndex(c => c.Nombre).IsUnique();
            modelBuilder.Entity<Proveedor>().HasIndex(p => p.Nombre).IsUnique();

            // Default
            modelBuilder.Entity<Articulo>()
                .Property(a => a.Destacado)
                .HasDefaultValue(false);

            // Cliente - Carrito (1:1)
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Carrito)
                .WithOne(ca => ca.Cliente)
                .HasForeignKey<Carrito>(ca => ca.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cliente - Pedidos (1:N)
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Pedidos)
                .WithOne(p => p.Cliente)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Carrito - Items (1:N)
            modelBuilder.Entity<Carrito>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Carrito)
                .HasForeignKey(i => i.CarritoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Item - Articulo (N:1)
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Articulo)
                .WithMany()
                .HasForeignKey(i => i.ArticuloId)
                .OnDelete(DeleteBehavior.Restrict);

            // Pedido - Detalles (1:N)
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Detalles)
                .WithOne(d => d.Pedido)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            // DetallePedido - Articulo (N:1)
            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Articulo)
                .WithMany()
                .HasForeignKey(d => d.ArticuloId)
                .OnDelete(DeleteBehavior.Restrict);

            // Venta - Pedido (N:1)
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Pedido)
                .WithMany()
                .HasForeignKey(v => v.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Precisión
            modelBuilder.Entity<Item>()
                .Property(i => i.PrecioUnitario)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DetallePedido>()
                .Property(dp => dp.PrecioUnitario)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.SubTotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Igv)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasPrecision(10, 2);

            //Articulo - Reseñas (1:N)
            modelBuilder.Entity<Reseña>()
                .HasOne(r => r.Articulo)
                .WithMany(a => a.Reseñas)
                .HasForeignKey(r => r.ArticuloId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cliente - Reseñas (1:N)
            modelBuilder.Entity<Reseña>()
                .HasOne(r => r.Cliente)
                .WithMany(c => c.Reseñas)
                .HasForeignKey(r => r.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
