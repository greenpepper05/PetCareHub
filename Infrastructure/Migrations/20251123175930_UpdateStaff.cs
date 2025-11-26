using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRecord_Staff_StaffId",
                table: "ServiceRecord");

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRecord_Staff_StaffId",
                table: "ServiceRecord",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRecord_Staff_StaffId",
                table: "ServiceRecord");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Staff");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRecord_Staff_StaffId",
                table: "ServiceRecord",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
