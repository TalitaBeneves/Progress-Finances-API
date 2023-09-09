using Microsoft.EntityFrameworkCore;
using Progress_Finances_API.Model;

namespace Progress_Finances_API.Data
{
    public class DataContext : DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Usuarios> usuarios { get; set; }
        public DbSet<Perguntas> perguntas { get; set; }
        public DbSet<Ativos> ativos { get; set; }
        public DbSet<MetaInvestimentos> meta { get; set; }
    }
}
