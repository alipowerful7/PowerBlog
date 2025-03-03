using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBlog.Site.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentToReactionBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CommentId",
                table: "ReactionBlogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReactionBlogs_CommentId",
                table: "ReactionBlogs",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionBlogs_Comments_CommentId",
                table: "ReactionBlogs",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionBlogs_Comments_CommentId",
                table: "ReactionBlogs");

            migrationBuilder.DropIndex(
                name: "IX_ReactionBlogs_CommentId",
                table: "ReactionBlogs");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "ReactionBlogs");
        }
    }
}
