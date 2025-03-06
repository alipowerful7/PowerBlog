using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBlog.Site.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisableToOfferPay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisable",
                table: "OfferPays",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisable",
                table: "OfferPays");
        }
    }
}
