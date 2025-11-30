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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioRol>()
            .HasKey(ur => new { ur.UsuarioId, ur.RolId });

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuariosRoles)
                .HasForeignKey(ur => ur.UsuarioId);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuariosRoles)
                .HasForeignKey(ur => ur.RolId);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Cliente)
                .WithOne(c => c.Usuario)
                .HasForeignKey<Cliente>(c => c.UsuarioId);
        }
    }
}
