using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProcedureUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetServiceHistories_Appointments_AppointmentId",
                table: "PetServiceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PetServiceHistories_Clinics_ClinicId",
                table: "PetServiceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PetServiceHistories_Pets_PetId",
                table: "PetServiceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PetServiceHistories_Services_ServiceId",
                table: "PetServiceHistories");

            migrationBuilder.DropIndex(
                name: "IX_Procedure_ServiceId",
                table: "Procedure");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PetServiceHistories",
                table: "PetServiceHistories");

            migrationBuilder.RenameTable(
                name: "PetServiceHistories",
                newName: "PetServiceHistory");

            migrationBuilder.RenameIndex(
                name: "IX_PetServiceHistories_ServiceId",
                table: "PetServiceHistory",
                newName: "IX_PetServiceHistory_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_PetServiceHistories_PetId",
                table: "PetServiceHistory",
                newName: "IX_PetServiceHistory_PetId");

            migrationBuilder.RenameIndex(
                name: "IX_PetServiceHistories_ClinicId",
                table: "PetServiceHistory",
                newName: "IX_PetServiceHistory_ClinicId");

            migrationBuilder.RenameIndex(
                name: "IX_PetServiceHistories_AppointmentId",
                table: "PetServiceHistory",
                newName: "IX_PetServiceHistory_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PetServiceHistory",
                table: "PetServiceHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_ServiceId_Order",
                table: "Procedure",
                columns: new[] { "ServiceId", "Order" });

            migrationBuilder.AddForeignKey(
                name: "FK_PetServiceHistory_Appointments_AppointmentId",
                table: "PetServiceHistory",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PetServiceHistory_Clinics_ClinicId",
                table: "PetServiceHistory",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PetServiceHistory_Pets_PetId",
                table: "PetServiceHistory",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PetServiceHistory_Services_ServiceId",
                table: "PetServiceHistory",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetServiceHistory_Appointments_AppointmentId",
                table: "PetServiceHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PetServiceHistory_Clinics_ClinicId",
                table: "PetServiceHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PetServiceHistory_Pets_PetId",
                table: "PetServiceHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PetServiceHistory_Services_ServiceId",
                table: "PetServiceHistory");

            migrationBuilder.DropIndex(
                name: "IX_Procedure_ServiceId_Order",
                table: "Procedure");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PetServiceHistory",
                table: "PetServiceHistory");

            migrationBuilder.RenameTable(
                name: "PetServiceHistory",
                newName: "PetServiceHistories");

            migrationBuilder.RenameIndex(
                name: "IX_PetServiceHistory_ServiceId",
                table: "PetServiceHistories",
                newName: "IX_PetServiceHistories_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_PetServiceHistory_PetId",
                table: "PetServiceHistories",
                newName: "IX_PetServiceHistories_PetId");

            migrationBuilder.RenameIndex(
                name: "IX_PetServiceHistory_ClinicId",
                table: "PetServiceHistories",
                newName: "IX_PetServiceHistories_ClinicId");

            migrationBuilder.RenameIndex(
                name: "IX_PetServiceHistory_AppointmentId",
                table: "PetServiceHistories",
                newName: "IX_PetServiceHistories_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PetServiceHistories",
                table: "PetServiceHistories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_ServiceId",
                table: "Procedure",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetServiceHistories_Appointments_AppointmentId",
                table: "PetServiceHistories",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PetServiceHistories_Clinics_ClinicId",
                table: "PetServiceHistories",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PetServiceHistories_Pets_PetId",
                table: "PetServiceHistories",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PetServiceHistories_Services_ServiceId",
                table: "PetServiceHistories",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
