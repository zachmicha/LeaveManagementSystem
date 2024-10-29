using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1506ec9c-09c8-4c16-a22d-8ee1320ee8c8", null, "Employee", "EMPLOYEE" },
                    { "2a918535-cabe-451f-b212-8c11c7c78dee", null, "Administrator", "ADMINISTRATOR" },
                    { "d9467a8b-7c26-4caa-a7dd-2dd5f30885b1", null, "Supervisor", "SUPERVISOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2a918535-cabe-451f-b212-8c11c7c78dee", 0, "426eecf4-13a0-4dbc-a3e8-e7fc50d3b890", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEGiisr+8tEuiVBMdzLTr26pnhiOOlyFls/YvGGApNfUBye2xEhUpinAuU2JRwHozcg==", null, false, "f0b3f399-d2c1-434c-9f1b-c6937d47b744", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2a918535-cabe-451f-b212-8c11c7c78dee", "2a918535-cabe-451f-b212-8c11c7c78dee" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1506ec9c-09c8-4c16-a22d-8ee1320ee8c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9467a8b-7c26-4caa-a7dd-2dd5f30885b1");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2a918535-cabe-451f-b212-8c11c7c78dee", "2a918535-cabe-451f-b212-8c11c7c78dee" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a918535-cabe-451f-b212-8c11c7c78dee");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a918535-cabe-451f-b212-8c11c7c78dee");
        }
    }
}
