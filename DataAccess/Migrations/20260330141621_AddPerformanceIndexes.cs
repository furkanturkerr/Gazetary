using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CategorySlug",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "BlogPosts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedDate",
                table: "Comments",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IsStatus",
                table: "Comments",
                column: "IsStatus");

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_CommentId_AppUserId",
                table: "CommentLikes",
                columns: new[] { "CommentId", "AppUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategorySlug",
                table: "Categories",
                column: "CategorySlug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_CreatedDate",
                table: "BlogPosts",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_Slug",
                table: "BlogPosts",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_Status",
                table: "BlogPosts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_ViewCount",
                table: "BlogPosts",
                column: "ViewCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_CreatedDate",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_IsStatus",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_CommentLikes_CommentId_AppUserId",
                table: "CommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategorySlug",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_CreatedDate",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_Slug",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_Status",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_ViewCount",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<string>(
                name: "CategorySlug",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
