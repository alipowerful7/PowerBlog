using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBlog.Site.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationToOfferPay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferWord",
                table: "Orders");

            migrationBuilder.AddColumn<long>(
                name: "OfferPayId",
                table: "Orders",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OfferPayId",
                table: "Orders",
                column: "OfferPayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OfferPays_OfferPayId",
                table: "Orders",
                column: "OfferPayId",
                principalTable: "OfferPays",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OfferPays_OfferPayId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OfferPayId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OfferPayId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "OfferWord",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
