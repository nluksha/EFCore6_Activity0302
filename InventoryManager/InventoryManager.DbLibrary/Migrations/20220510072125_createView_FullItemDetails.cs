using InventoryManager.DbLibrary.Migrations.Scripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManager.DbLibrary.Migrations
{
    public partial class createView_FullItemDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("InventoryManager.DbLibrary.Migrations.Scripts.Views.FullItemDetails.FullItemDetails.v0.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS dbo.vwFullItemDetails");
        }
    }
}
