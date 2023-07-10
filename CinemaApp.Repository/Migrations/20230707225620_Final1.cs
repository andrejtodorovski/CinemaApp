using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaApp.Repository.Migrations
{
    public partial class Final1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Tickets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Tickets");
        }
    }
}
