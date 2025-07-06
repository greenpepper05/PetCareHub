using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePetEnity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AspNetUsers_OwnerId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_OwnerId",
                table: "Pets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0caf1fd0-cf4d-4d7f-a01f-633884b514e1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16f06a27-7f37-4b65-9bf3-aa90b1e3b5aa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47294b53-875c-4b70-9bde-69b453e49e29");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09748a9b-1c59-4553-b8ea-5a0f02006e70", null, "Admin", "ADMIN" },
                    { "30020fff-f236-45ce-9cf9-db50d1e5652d", null, "SuperAdmin", "SUPERADMIN" },
                    { "fce8640b-aac0-4a8c-80fa-6f99ad2f1bbf", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09748a9b-1c59-4553-b8ea-5a0f02006e70");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "30020fff-f236-45ce-9cf9-db50d1e5652d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fce8640b-aac0-4a8c-80fa-6f99ad2f1bbf");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Pets",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0caf1fd0-cf4d-4d7f-a01f-633884b514e1", null, "Customer", "CUSTOMER" },
                    { "16f06a27-7f37-4b65-9bf3-aa90b1e3b5aa", null, "SuperAdmin", "SUPERADMIN" },
                    { "47294b53-875c-4b70-9bde-69b453e49e29", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_OwnerId",
                table: "Pets",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AspNetUsers_OwnerId",
                table: "Pets",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
