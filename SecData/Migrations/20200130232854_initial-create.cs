using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SecData.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cik = table.Column<string>(nullable: true),
                    FileDate = table.Column<DateTime>(nullable: false),
                    FormType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Cik = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funds", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filings");

            migrationBuilder.DropTable(
                name: "Funds");
        }
    }
}
