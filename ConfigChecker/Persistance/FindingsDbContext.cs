using ConfigChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace ConfigChecker.Persistance
{
  public class FindingsDbContext(DbContextOptions<FindingsDbContext> options) : DbContext(options)
  {
    public DbSet<Finding> Findings => Set<Finding>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(FindingsDbContext).Assembly);
      base.OnModelCreating(modelBuilder);
    }
  }
}
