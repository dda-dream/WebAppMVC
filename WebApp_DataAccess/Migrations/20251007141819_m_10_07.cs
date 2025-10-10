using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class m_10_07 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Test",
                table: "MyDailyJournal",
                newName: "ExecutionStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExecutionStatus",
                table: "MyDailyJournal",
                newName: "Test");
        }
    }
}
