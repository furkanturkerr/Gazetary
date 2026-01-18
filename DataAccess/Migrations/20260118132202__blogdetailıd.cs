using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _blogdetailıd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogPostId",
                table: "BlogPostDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostDetails_BlogPostId",
                table: "BlogPostDetails",
                column: "BlogPostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostDetails_BlogPosts_BlogPostId",
                table: "BlogPostDetails",
                column: "BlogPostId",
                principalTable: "BlogPosts",
                principalColumn: "BlogPostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostDetails_BlogPosts_BlogPostId",
                table: "BlogPostDetails");

            migrationBuilder.DropIndex(
                name: "IX_BlogPostDetails_BlogPostId",
                table: "BlogPostDetails");

            migrationBuilder.DropColumn(
                name: "BlogPostId",
                table: "BlogPostDetails");
        }
    }
}
