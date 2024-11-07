using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixDateOnlyMigrationWithoutFixingThisShit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Days",
                table: "LeaveAllocation",
                newName: "NumberOfDays");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a918535-cabe-451f-b212-8c11c7c78dee",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e520c6e5-56f6-4453-aed4-78ac2ea4d6b1", "AQAAAAIAAYagAAAAEOY+mul4qmwVvJqadEn9JfkGgt0Z4r5mdkgyrFx2cnVSA3KUGwdpMaj8iP76k+g8tQ==", "125ab781-c07e-4e76-96aa-26203ed74440" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfDays",
                table: "LeaveAllocation",
                newName: "Days");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a918535-cabe-451f-b212-8c11c7c78dee",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "383c16f0-3ac2-4694-851e-0b3605251cea", "AQAAAAIAAYagAAAAEEv01hlSnMfAZnVRBkwaWC+8waxxw9SSuj9WgveYehCROlXaCHTeXZ9u4vvEthmy1A==", "37c8f182-e680-469b-9476-1ee6c104e8e2" });
        }
    }
}
