using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_LogTable_IP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropForeignKey(
                name: "FK_LogTable_AspNetUsers_CreatedByUserId",
                table: "LogTable");
            */
            /*
            migrationBuilder.DropIndex(
                name: "IX_LogTable_CreatedByUserId",
                table: "LogTable");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "LogTable",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
            */
            /*
            migrationBuilder.AddColumn<string>(
                name: "_IP_",
                table: "LogTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_IP_",
                table: "LogTable");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "LogTable",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_LogTable_CreatedByUserId",
                table: "LogTable",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogTable_AspNetUsers_CreatedByUserId",
                table: "LogTable",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
