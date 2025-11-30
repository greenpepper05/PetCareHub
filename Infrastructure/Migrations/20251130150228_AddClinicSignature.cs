using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClinicSignature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Clinics_ClinicId",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "SignatureUrl",
                table: "Clinics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Clinics_ClinicId",
                table: "Services",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Clinics_ClinicId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "SignatureUrl",
                table: "Clinics");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Clinics_ClinicId",
                table: "Services",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
