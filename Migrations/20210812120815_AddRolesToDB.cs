using Microsoft.EntityFrameworkCore.Migrations;

namespace CompanyEmployee.Migrations
{
    public partial class AddRolesToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "be4549f4-ae00-465d-8e4e-d386df699a78", "2bbba3fd-9771-41ec-acd4-84c65a2322f1", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "74d03484-a180-4d0a-8562-68bf257dbd70", "438542d8-9388-4c0d-ab09-5d4536e03188", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "60d37f79-d6d0-425f-89ba-78a89db9aeee", "65f5b68f-c585-44a3-bd61-260c6c9a0d18", "Employee", "EMPLOYEE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60d37f79-d6d0-425f-89ba-78a89db9aeee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74d03484-a180-4d0a-8562-68bf257dbd70");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be4549f4-ae00-465d-8e4e-d386df699a78");
        }
    }
}
