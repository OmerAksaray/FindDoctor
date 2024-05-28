using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindDoctor.Migrations
{
    /// <inheritdoc />
    public partial class AddDescription3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiseaseDetection",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DiseaseDetection",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiseaseDetection",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiseaseDetection",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
