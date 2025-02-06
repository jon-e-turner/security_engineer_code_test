using ConfigChecker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConfigChecker.Persistance.Configurations
{
  public class FindingsConfiguration : IEntityTypeConfiguration<Finding>
  {
    public void Configure(EntityTypeBuilder<Finding> builder)
    {
      // Set the primary key
      builder.HasKey(f => f.Id);

    }
  }
}
