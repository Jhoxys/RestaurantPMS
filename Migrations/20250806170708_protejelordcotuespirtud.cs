using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantPMS.Migrations
{
    /// <inheritdoc />
    public partial class protejelordcotuespirtud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredient_Ingredients_IngredientId",
                table: "ProductIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredient_Products_ProductId",
                table: "ProductIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductIngredient",
                table: "ProductIngredient");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "RawProducts");

            migrationBuilder.DropColumn(
                name: "UnidPrice",
                table: "RawProducts");

            migrationBuilder.RenameTable(
                name: "ProductIngredient",
                newName: "ProductIngredients");

            migrationBuilder.RenameColumn(
                name: "ImageFileName",
                table: "RawProducts",
                newName: "ImagenFileName");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredient_ProductId",
                table: "ProductIngredients",
                newName: "IX_ProductIngredients_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredient_IngredientId",
                table: "ProductIngredients",
                newName: "IX_ProductIngredients_IngredientId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RawProducts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RawProducts",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "RawProducts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "RawProducts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RawProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RawProductId",
                table: "ProductIngredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductIngredients",
                table: "ProductIngredients",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductIngredients_RawProductId",
                table: "ProductIngredients",
                column: "RawProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Ingredients_IngredientId",
                table: "ProductIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Products_ProductId",
                table: "ProductIngredients",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_RawProducts_RawProductId",
                table: "ProductIngredients",
                column: "RawProductId",
                principalTable: "RawProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Ingredients_IngredientId",
                table: "ProductIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Products_ProductId",
                table: "ProductIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_RawProducts_RawProductId",
                table: "ProductIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductIngredients",
                table: "ProductIngredients");

            migrationBuilder.DropIndex(
                name: "IX_ProductIngredients_RawProductId",
                table: "ProductIngredients");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RawProducts");

            migrationBuilder.DropColumn(
                name: "RawProductId",
                table: "ProductIngredients");

            migrationBuilder.RenameTable(
                name: "ProductIngredients",
                newName: "ProductIngredient");

            migrationBuilder.RenameColumn(
                name: "ImagenFileName",
                table: "RawProducts",
                newName: "ImageFileName");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredients_ProductId",
                table: "ProductIngredient",
                newName: "IX_ProductIngredient_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredients_IngredientId",
                table: "ProductIngredient",
                newName: "IX_ProductIngredient_IngredientId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RawProducts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RawProducts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "RawProducts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "RawProducts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "RawProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnidPrice",
                table: "RawProducts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductIngredient",
                table: "ProductIngredient",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredient_Ingredients_IngredientId",
                table: "ProductIngredient",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredient_Products_ProductId",
                table: "ProductIngredient",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
