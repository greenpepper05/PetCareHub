using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PropertyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRecord_Procedure_ProcedureId",
                table: "ServiceRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRecordStep_Procedure_ProcedureId",
                table: "ServiceRecordStep");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRecord_ProcedureId",
                table: "ServiceRecord");

            migrationBuilder.DropColumn(
                name: "ProcedureId",
                table: "ServiceRecord");

            migrationBuilder.AlterColumn<int>(
                name: "ProcedureId",
                table: "ServiceRecordStep",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRecordStep_Procedure_ProcedureId",
                table: "ServiceRecordStep",
                column: "ProcedureId",
                principalTable: "Procedure",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRecordStep_Procedure_ProcedureId",
                table: "ServiceRecordStep");

            migrationBuilder.AlterColumn<int>(
                name: "ProcedureId",
                table: "ServiceRecordStep",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRecordStep_Procedure_ProcedureId",
                table: "ServiceRecordStep",
                column: "ProcedureId",
                principalTable: "Procedure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
