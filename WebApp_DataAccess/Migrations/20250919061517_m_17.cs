using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class m_17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TestForMigration",
                table: "TestForMigration");

            migrationBuilder.RenameTable(
                name: "TestForMigration",
                newName: "TestForMigration_1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestForMigration_1",
                table: "TestForMigration_1",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TestForMigration_1",
                table: "TestForMigration_1");

            migrationBuilder.RenameTable(
                name: "TestForMigration_1",
                newName: "TestForMigration");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestForMigration",
                table: "TestForMigration",
                column: "Id");
        }
    }
}
