using EnersoftSensorsManagementSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnersoftSensorsManagementSystem.Infrastructure.Configurations;

public class SensorConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.ToTable("Sensors");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Location).HasMaxLength(200);
        builder.HasOne(s => s.Type)
               .WithMany(t => t.Sensors)
               .HasForeignKey(s => s.TypeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
