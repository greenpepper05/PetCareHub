using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditedPetServiceHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetServiceHistories_Appointments_AppointmentId",
                table: "PetServiceHistories");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "PetServiceHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PetServiceHistories_Appointments_AppointmentId",
                table: "PetServiceHistories",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetServiceHistories_Appointments_AppointmentId",
                table: "PetServiceHistories");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "PetServiceHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PetServiceHistories_Appointments_AppointmentId",
                table: "PetServiceHistories",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
