using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class logtable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "LogTable");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId1",
                table: "LogTable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "LogTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId1",
                table: "LogTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
