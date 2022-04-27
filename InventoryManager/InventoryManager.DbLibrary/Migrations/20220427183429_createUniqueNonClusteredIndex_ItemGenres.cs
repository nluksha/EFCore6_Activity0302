using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManager.DbLibrary.Migrations
{
    public partial class createUniqueNonClusteredIndex_ItemGenres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemGenges_ItemId",
                table: "ItemGenges");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGenges_ItemId_GenreId",
                table: "ItemGenges",
                columns: new[] { "ItemId", "GenreId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemGenges_ItemId_GenreId",
                table: "ItemGenges");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGenges_ItemId",
                table: "ItemGenges",
                column: "ItemId");
        }
    }
}
