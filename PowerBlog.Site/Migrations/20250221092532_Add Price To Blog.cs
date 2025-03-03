using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBlog.Site.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceToBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Blogs");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Blogs",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Blogs");

            migrationBuilder.AddColumn<float>(
                name: "Rate",
                table: "Blogs",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
