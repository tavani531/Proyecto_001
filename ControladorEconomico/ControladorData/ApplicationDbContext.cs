using Microsoft.EntityFrameworkCore;
using ControladorEntities;

namespace ControladorData
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<CategoriasEntities> Categorias { get; set; }
        public DbSet<ObjetivosEntities> Objetivos { get; set; }
        public DbSet<MovimientosEntities> Movimientos { get; set; }
    }
}