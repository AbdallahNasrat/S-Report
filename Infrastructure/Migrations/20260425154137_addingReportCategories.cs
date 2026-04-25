using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingReportCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCategories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ReportCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "FIRE" },
                    { 2, "INFRASTRUCTURE_DAMAGE" },
                    { 3, "WATER_DISASTER" },
                    { 4, "TRAFFIC_ACCIDENT" },
                    { 5, "CRIME_SCENE" },
                    { 6, "HARASSMENT" },
                    { 7, "MEDICAL_EMERGENCY" },
                    { 8, "NORMAL" },
                    { 9, "Accident" },
                    { 10, "Environmental" },
                    { 11, "Health" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportCategories");
        }
    }
}
