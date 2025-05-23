using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollChatApi.Migrations
{
    /// <inheritdoc />
    public partial class Firsts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warning_Users_UserId1",
                table: "Warning");

            migrationBuilder.DropIndex(
                name: "IX_Warning_UserId1",
                table: "Warning");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Warning");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Warning",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Warning_UserId",
                table: "Warning",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Warning_Users_UserId",
                table: "Warning",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warning_Users_UserId",
                table: "Warning");

            migrationBuilder.DropIndex(
                name: "IX_Warning_UserId",
                table: "Warning");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Warning",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Warning",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warning_UserId1",
                table: "Warning",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Warning_Users_UserId1",
                table: "Warning",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
