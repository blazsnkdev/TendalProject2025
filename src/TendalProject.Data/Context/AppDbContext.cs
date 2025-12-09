using Microsoft.EntityFrameworkCore;
using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }
        public DbSet<Usuario> TblUsuario => Set<Usuario>();
        public DbSet<Rol> TblRol => Set<Rol>();
        public DbSet<Cliente> TblCliente => Set<Cliente>();
        public DbSet<UsuarioRol> TblUsuarioRol => Set<UsuarioRol>();
        public DbSet<Articulo> TblArticulo => Set<Articulo>();
        public DbSet<Categoria> TblCategoria => Set<Categoria>();
        public DbSet<Proveedor> TblProveedor => Set<Proveedor>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithOne(u => u.Cliente)
                .HasForeignKey<Cliente>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade); 

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

            modelBuilder.Entity<Articulo>()
                .HasIndex(a => a.Nombre);

            modelBuilder.Entity<Articulo>()
                .HasIndex(a => a.Codigo)
                .IsUnique();

            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Nombre)
                .IsUnique();

            modelBuilder.Entity<Proveedor>()
                .HasIndex(p => p.Nombre)
                .IsUnique();

            modelBuilder.Entity<Articulo>()
                .Property(a => a.Destacado)
                .HasDefaultValue(false);

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Carrito)
                .WithOne(ca => ca.Cliente)
                .HasForeignKey<Carrito>(ca => ca.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Pedidos)
                .WithOne(p => p.Cliente)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Carrito>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Carrito)
                .HasForeignKey(i => i.CarritoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Articulo)
                .WithMany() 
                .HasForeignKey(i => i.ArticuloId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Detalles)
                .WithOne(d => d.Pedido)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Articulo)
                .WithMany() 
                .HasForeignKey(d => d.ArticuloId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Ventas)
                .WithOne(v => v.Cliente)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Venta>()
                .HasMany(v => v.Detalles)
                .WithOne(dv => dv.Venta)
                .HasForeignKey(dv => dv.VentaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Articulo)
                .WithMany() 
                .HasForeignKey(dv => dv.ArticuloId)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }
    }
}
