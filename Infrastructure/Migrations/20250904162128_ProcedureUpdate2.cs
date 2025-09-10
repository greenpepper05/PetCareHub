using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProcedureUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Procedure_ServiceId_Order",
                table: "Procedure");

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_ServiceId_Order",
                table: "Procedure",
                columns: new[] { "ServiceId", "Order" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Procedure_ServiceId_Order",
                table: "Procedure");

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_ServiceId_Order",
                table: "Procedure",
                columns: new[] { "ServiceId", "Order" });
        }
    }
}
