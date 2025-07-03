using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RolesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7f480c80-5b4f-4384-8126-aa8daf7a4322", null, "Admin", "ADMIN" },
                    { "8cc557f6-1962-44ad-b702-d9563995a2c2", null, "SuperAdmin", "SUPERADMIN" },
                    { "b567050d-6775-4d0c-8e96-950bdbac7167", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7f480c80-5b4f-4384-8126-aa8daf7a4322");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8cc557f6-1962-44ad-b702-d9563995a2c2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b567050d-6775-4d0c-8e96-950bdbac7167");
        }
    }
}
