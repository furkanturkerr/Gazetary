using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class user_comment_safe_delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_AspNetUsers_AppUserId",
                table: "CommentLikes");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_AspNetUsers_AppUserId",
                table: "CommentLikes",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_AspNetUsers_AppUserId",
                table: "CommentLikes");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_AspNetUsers_AppUserId",
                table: "CommentLikes",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
