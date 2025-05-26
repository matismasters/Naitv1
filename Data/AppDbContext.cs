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
        public DbSet<Ciudad> Ciudades { get; set; }
    }
}
