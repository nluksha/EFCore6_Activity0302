using Microsoft.EntityFrameworkCore.Migrations;
using InventoryManager.DbLibrary.Migrations.Scripts;

#nullable disable

namespace InventoryManager.DbLibrary.Migrations
{
    public partial class createFunction_ItemNamesPipeDeliminatedString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("InventoryManager.DbLibrary.Migrations.Scripts.Functions.ItemNamesPipeDeliminatedString.ItemNamesPipeDeliminatedString.v0.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS dbo.ItemNamesPipeDeliminatedString");
        }
    }
}
