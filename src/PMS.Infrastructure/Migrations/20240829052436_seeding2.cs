using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seeding2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProduct_Products_ProductsId",
                table: "CategoryProduct");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "CategoryProduct",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryProduct_ProductsId",
                table: "CategoryProduct",
                newName: "IX_CategoryProduct_ProductId");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Products",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "BottomDescription", "Description", "Name" },
                values: new object[] { 3, "A bottom description", "A description texts", "iPhone 12" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Currency",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Currency",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Currency",
                value: "");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Color", "Currency", "Description", "Ean", "Material", "Name", "Price", "ProductGroup", "ProductType", "Sku", "SpecialPrice" },
                values: new object[] { 4, "Black", "EUR", "A black iPhone 12 / 12 Pro cover", "EAN090909", "Silicone / TPU", "iPhone 12 / 12 Pro cover - Black", 17f, "Smartphone", "Cover", "LC01-76-1038-1", 12.95f });

            migrationBuilder.InsertData(
                table: "CategoryProduct",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[] { 3, 4 });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProduct_Products_ProductId",
                table: "CategoryProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProduct_Products_ProductId",
                table: "CategoryProduct");

            migrationBuilder.DeleteData(
                table: "CategoryProduct",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CategoryProduct",
                newName: "ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryProduct_ProductId",
                table: "CategoryProduct",
                newName: "IX_CategoryProduct_ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProduct_Products_ProductsId",
                table: "CategoryProduct",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
