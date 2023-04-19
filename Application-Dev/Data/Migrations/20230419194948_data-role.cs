using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application_Dev.Data.Migrations
{
    public partial class datarole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9b1deb4d-3b7d-4bad-9bdd-2b0d7b3d23423", "2", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d", "1", "customer", "customer" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9b1deb4d-3b7d-4bad-9bdd-2b0d7basd23", "3", "storeOwner", "storeOwner" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b1deb4d-3b7d-4bad-9bdd-2b0d7b3d23423");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b1deb4d-3b7d-4bad-9bdd-2b0d7basd23");
        }
    }
}
