using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantPMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableRawProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnidPrice",
                table: "RawProducts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnidPrice",
                table: "RawProducts");
        }
    }
}
