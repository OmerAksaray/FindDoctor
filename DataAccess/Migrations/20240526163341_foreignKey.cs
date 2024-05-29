using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindDoctor.Migrations
{
    /// <inheritdoc />
    public partial class foreignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_DoctorId",
                table: "Customers",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Doctors_DoctorId",
                table: "Customers",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Doctors_DoctorId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_DoctorId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Customers");
        }
    }
}
