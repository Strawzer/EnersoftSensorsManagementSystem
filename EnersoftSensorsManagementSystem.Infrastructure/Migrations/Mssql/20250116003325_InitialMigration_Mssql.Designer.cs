﻿// <auto-generated />
using EnersoftSensorsManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EnersoftSensorsManagementSystem.Infrastructure.Migrations.Mssql
{
    [DbContext(typeof(MssqlSensorDbContext))]
    [Migration("20250116003325_InitialMigration_Mssql")]
    partial class InitialMigration_Mssql
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EnersoftSensorsManagementSystem.Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PasswordHash = "AQAAAAIAAYagAAAAEFF8NmBoxVLAG/cuB+JVzmSHZWxfWy/0jtSBq01cwBbE7sDjYABMpLR/8SDXjYU65A==",
                            Role = "Admin",
                            Username = "admin"
                        },
                        new
                        {
                            Id = 2,
                            PasswordHash = "AQAAAAIAAYagAAAAEDb0VIcCJpSfQFZ2sbadfKIiJjufP07ZsXWSgGIIJOiEn6Qaf6kpHOeu+BasmUvxSA==",
                            Role = "User",
                            Username = "user"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
