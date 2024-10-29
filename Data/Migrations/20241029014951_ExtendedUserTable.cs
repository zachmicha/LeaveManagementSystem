using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a918535-cabe-451f-b212-8c11c7c78dee",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "198084e2-f3eb-48d6-b94d-b248b9752780", new DateOnly(1980, 10, 10), "Default", "Admin", "AQAAAAIAAYagAAAAEDQpZBJnodaY+O1lvzIQa4KCYeeE1rK5HzeOt9OI+x0Qa2dGp4uq+Wjo8em1SPFrjg==", "2431c2d4-1cb3-4727-82a0-d969f5d9f2b8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a918535-cabe-451f-b212-8c11c7c78dee",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "426eecf4-13a0-4dbc-a3e8-e7fc50d3b890", "AQAAAAIAAYagAAAAEGiisr+8tEuiVBMdzLTr26pnhiOOlyFls/YvGGApNfUBye2xEhUpinAuU2JRwHozcg==", "f0b3f399-d2c1-434c-9f1b-c6937d47b744" });
        }
    }
}
