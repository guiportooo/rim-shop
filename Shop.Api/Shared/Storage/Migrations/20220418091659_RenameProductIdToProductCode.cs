using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Api.Shared.Storage.Migrations
{
    public partial class RenameProductIdToProductCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Item",
                newName: "ProductCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductCode",
                table: "Item",
                newName: "ProductId");
        }
    }
}
