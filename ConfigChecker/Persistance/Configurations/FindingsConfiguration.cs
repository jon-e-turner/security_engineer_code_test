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

      // Configure properties
      builder.Property(f => f.Name)
        .IsRequired()
        .HasMaxLength(200);

      builder.Property(f => f.ReportId)
        .IsRequired();

      builder.Property(f => f.ResourceName)
        .IsRequired();

      builder.Property(f => f.Description)
        .IsRequired();

      builder.Property(f => f.Severity)
        .IsRequired();

      builder.Property(f => f.CweId)
        .IsRequired();

      builder.Property(m => m.Created)
        .IsRequired()
        .ValueGeneratedOnAdd();

      builder.Property(m => m.LastModified)
         .IsRequired()
         .ValueGeneratedOnUpdate();

      // Queries will filter by the ReportId, so use an index to speed up responses.
      builder.HasIndex(f => f.ReportId);
    }
  }
}
