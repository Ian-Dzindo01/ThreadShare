using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadShare.Migrations
{
    /// <inheritdoc />
    public partial class FKbetweencommentandforum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForumId",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ForumId",
                table: "Comments",
                column: "ForumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Forums_ForumId",
                table: "Comments",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Forums_ForumId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ForumId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ForumId",
                table: "Comments");
        }
    }
}
