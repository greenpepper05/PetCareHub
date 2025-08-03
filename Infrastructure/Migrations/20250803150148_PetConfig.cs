using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PetConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClinicId",
                table: "Pets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ClinicId",
                table: "Pets",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Clinics_ClinicId",
                table: "Pets",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Clinics_ClinicId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_ClinicId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "Pets");
        }
    }
}
