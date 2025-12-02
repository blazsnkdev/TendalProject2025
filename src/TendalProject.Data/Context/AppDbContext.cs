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

            base.OnModelCreating(modelBuilder);
        }
    }
}
