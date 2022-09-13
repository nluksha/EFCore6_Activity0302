using InventoryManager.DbLibrary.Migrations.Scripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManager.DbLibrary.Migrations
{
    public partial class magratetoplayerspeoplecompanies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("InventoryManager.DbLibrary.Migrations.Scripts.CustomScripts.MigratePlayers.v0.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FORM Companies; DELETE FROM People");
        }
    }
}
