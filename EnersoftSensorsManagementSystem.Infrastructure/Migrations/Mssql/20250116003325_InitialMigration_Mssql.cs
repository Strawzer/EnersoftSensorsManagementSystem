using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EnersoftSensorsManagementSystem.Infrastructure.Migrations.Mssql
{
    /// <inheritdoc />
    public partial class InitialMigration_Mssql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "AQAAAAIAAYagAAAAEFF8NmBoxVLAG/cuB+JVzmSHZWxfWy/0jtSBq01cwBbE7sDjYABMpLR/8SDXjYU65A==", "Admin", "admin" },
                    { 2, "AQAAAAIAAYagAAAAEDb0VIcCJpSfQFZ2sbadfKIiJjufP07ZsXWSgGIIJOiEn6Qaf6kpHOeu+BasmUvxSA==", "User", "user" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
