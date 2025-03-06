using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBlog.Site.Migrations
{
    /// <inheritdoc />
    public partial class AddOfferPay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfferWord",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OfferPays",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferWord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferPercentage = table.Column<double>(type: "float", nullable: true),
                    OfferAmount = table.Column<double>(type: "float", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferPays", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferPays");

            migrationBuilder.DropColumn(
                name: "OfferWord",
                table: "Orders");
        }
    }
}
