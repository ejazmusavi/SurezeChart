using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SurezeChart.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Strips",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric", nullable: false),
                    CTGID = table.Column<decimal>(type: "numeric", nullable: false),
                    PATIENTID = table.Column<decimal>(type: "numeric", nullable: false),
                    SIGNAL = table.Column<decimal>(type: "numeric", nullable: false),
                    HRA = table.Column<decimal>(type: "numeric", nullable: false),
                    HRB = table.Column<decimal>(type: "numeric", nullable: false),
                    TOCO = table.Column<decimal>(type: "numeric", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strips", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Strips");
        }
    }
}
