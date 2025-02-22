using ConfigChecker.Models;
using ConfigChecker.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ConfigChecker.Persistence
{
  public class FindingsDbContext : DbContext
  {
    public FindingsDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Finding> Findings => Set<Finding>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Using IEntityTypeConfiguration<T> to de-couple model from database entity.
      modelBuilder.ApplyConfiguration<Finding>(new FindingsConfiguration());

      base.OnModelCreating(modelBuilder);
    }
  }
}
