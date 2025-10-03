using Domain.Entities;
using Microsoft.EntityFrameworkCore;

// Este debe ser el namespace donde EF Core lo espera
namespace Infrastructure.Repositories.Sql
{
   
    public class StoreDbContext : DbContext
    {
        public DbSet<Automovil> Automoviles { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }

        protected StoreDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<DummyEntity>().ToTable("DummyEntity");
            modelBuilder.Entity<Automovil>().ToTable("Automovil");
        }
    }
}
