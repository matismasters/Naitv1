using Microsoft.EntityFrameworkCore;
using Naitv1.Models;

namespace Naitv1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<RegistroEmail> RegistroEmails { get; set; }
        public DbSet<ActividadesUsuarios> ActividadesUsuarios { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; } // Asegurate de incluirlo también
        public DbSet<ConfiguracionReporte> ConfiguracionReportes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Actividad → Ciudad (N:1)
            modelBuilder.Entity<Actividad>()
                .HasOne(a => a.Ciudad)
                .WithMany(c => c.ListActividades)
                .HasForeignKey(a => a.CiudadId)
                .OnDelete(DeleteBehavior.Restrict);

            //    // Relación Actividad → Usuario (Anfitrión) (N:1)
            //    modelBuilder.Entity<Actividad>()
            //        .HasOne(a => a.Anfitrion)
            //        .WithMany(u => u.Actividades) // Esto requiere que Usuario tenga una propiedad ICollection<Actividad>
            //        .HasForeignKey(a => a.AnfitrionId)
            //        .OnDelete(DeleteBehavior.Restrict);
            //}
        }
    }
}
