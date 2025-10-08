using MAC.Models;
using Microsoft.EntityFrameworkCore;

namespace MAC.Models
{
    public class MACDbContext : DbContext
    {
        public MACDbContext(DbContextOptions<MACDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Proceso> Procesos { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<RolProceso> RolProcesos { get; set; }
        public DbSet<UsuarioProceso> UsuariosProceso { get; set; }
        public DbSet<UsuarioRol> UsuariosRol { get; set; }
        public DbSet<Municipio> Municipios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Clave compuesta para RolProceso
            modelBuilder.Entity<RolProceso>()
                .HasKey(rp => new { rp.RolId, rp.ProcesoId });

            modelBuilder.Entity<RolProceso>().ToTable("rolproceso");

            // Clave compuesta para UsuarioRol
            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.UsuarioId, ur.RolId });

            modelBuilder.Entity<UsuarioRol>().ToTable("usuariorol");

            // Clave compuesta para UsuarioProceso
            modelBuilder.Entity<UsuarioProceso>()
                .HasKey(up => new { up.UsuarioId, up.ProcesoId });

            modelBuilder.Entity<UsuarioProceso>().ToTable("usuarioproceso");


            modelBuilder.Entity<Usuario>().HasOne(u => u.mpoInfo)
                        .WithMany()
                        .HasForeignKey(u => u.municipio);
            modelBuilder.Entity<Usuario>().HasOne(u => u.rolInfo)
                       .WithMany()
                       .HasForeignKey(u => u.rol);


            // Puedes agregar más configuraciones aquí si es necesario
        }
    }
}
