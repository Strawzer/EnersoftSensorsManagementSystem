using EnersoftSensorsManagementSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnersoftSensorsManagementSystem.Infrastructure.Configurations;

public class SensorTypeConfiguration : IEntityTypeConfiguration<SensorType>
{
    public void Configure(EntityTypeBuilder<SensorType> builder)
    {
        builder.ToTable("SensorTypes");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
    }
}