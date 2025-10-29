using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ServiceRecordEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcedureId",
                table: "ServiceRecord",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRecord_ProcedureId",
                table: "ServiceRecord",
                column: "ProcedureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRecord_Procedure_ProcedureId",
                table: "ServiceRecord",
                column: "ProcedureId",
                principalTable: "Procedure",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRecord_Procedure_ProcedureId",
                table: "ServiceRecord");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRecord_ProcedureId",
                table: "ServiceRecord");

            migrationBuilder.DropColumn(
                name: "ProcedureId",
                table: "ServiceRecord");
        }
    }
}
