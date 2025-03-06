using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBlog.Site.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatDateToOfferPay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatDate",
                table: "OfferPays",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatDate",
                table: "OfferPays");
        }
    }
}
