using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaApp.Repository.Migrations
{
    public partial class Final12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketsAvailable",
                table: "Projections",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketsAvailable",
                table: "Projections");
        }
    }
}
