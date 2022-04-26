using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class StatisticImproves : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Requested",
                table: "Genre",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Requested",
                table: "Film",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Requested",
                table: "Genre");

            migrationBuilder.DropColumn(
                name: "Requested",
                table: "Film");
        }
    }
}
