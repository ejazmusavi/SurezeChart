using Microsoft.EntityFrameworkCore.Migrations;

namespace SurezeChart.Migrations
{
    public partial class CGID_dropped : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CTGID",
                table: "Strips");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CTGID",
                table: "Strips",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
