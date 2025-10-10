using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class m_10_10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "OrderLine",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Qty",
                table: "OrderLine");
        }
    }
}
