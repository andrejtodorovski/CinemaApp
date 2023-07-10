using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaApp.Repository.Migrations
{
    public partial class FinalDBMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieInShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShoppingCartId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MoviePrice",
                table: "Movies");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "ShoppingCarts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Projections",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MovieId = table.Column<Guid>(nullable: false),
                    TimeOfProjection = table.Column<DateTime>(nullable: false),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projections_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectionId = table.Column<Guid>(nullable: false),
                    ShoppingCartId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Projections_ProjectionId",
                        column: x => x.ProjectionId,
                        principalTable: "Projections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShoppingCartId",
                table: "Orders",
                column: "ShoppingCartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projections_MovieId",
                table: "Projections",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectionId",
                table: "Tickets",
                column: "ProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ShoppingCartId",
                table: "Tickets",
                column: "ShoppingCartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Projections");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShoppingCartId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<int>(
                name: "MoviePrice",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MovieInShoppingCarts",
                columns: table => new
                {
                    ShoppingCartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TimeOfProjection = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieInShoppingCarts", x => new { x.ShoppingCartId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_MovieInShoppingCarts_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieInShoppingCarts_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShoppingCartId",
                table: "Orders",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieInShoppingCarts_MovieId",
                table: "MovieInShoppingCarts",
                column: "MovieId");
        }
    }
}
