using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollChatApi.Migrations
{
    /// <inheritdoc />
    public partial class warning44 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RepoUser",
                table: "Warnings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Scrap",
                table: "Warnings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "RemovedByAdmin",
                table: "MainThreads",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepoUser",
                table: "Warnings");

            migrationBuilder.DropColumn(
                name: "Scrap",
                table: "Warnings");

            migrationBuilder.AlterColumn<bool>(
                name: "RemovedByAdmin",
                table: "MainThreads",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
