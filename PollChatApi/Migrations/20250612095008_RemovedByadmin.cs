using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollChatApi.Migrations
{
    /// <inheritdoc />
    public partial class RemovedByadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainThreads_SubCategory_SubCategoryId",
                table: "MainThreads");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Users_UserId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Warning_Users_UserId",
                table: "Warning");

            migrationBuilder.DropTable(
                name: "SubCategory");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_UserId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_MainThreads_SubCategoryId",
                table: "MainThreads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warning",
                table: "Warning");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Banned",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "MainThreads");

            migrationBuilder.RenameTable(
                name: "Warning",
                newName: "Warnings");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "ProfilePicture");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "MainThreads",
                newName: "Content");

            migrationBuilder.RenameIndex(
                name: "IX_Warning_UserId",
                table: "Warnings",
                newName: "IX_Warnings_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Votes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RemovedByAdmin",
                table: "MainThreads",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "RemovedByAdmin",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Warnings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Warnings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "HandeldAtTime",
                table: "Warnings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHandled",
                table: "Warnings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ObjectsId",
                table: "Warnings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Warnings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warnings",
                table: "Warnings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SubjectUser",
                columns: table => new
                {
                    FavoriteSubjectsId = table.Column<int>(type: "int", nullable: false),
                    SubscribedUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectUser", x => new { x.FavoriteSubjectsId, x.SubscribedUsersId });
                    table.ForeignKey(
                        name: "FK_SubjectUser_Subjects_FavoriteSubjectsId",
                        column: x => x.FavoriteSubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectUser_Users_SubscribedUsersId",
                        column: x => x.SubscribedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectUser_SubscribedUsersId",
                table: "SubjectUser",
                column: "SubscribedUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warnings_Users_UserId",
                table: "Warnings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Warnings_Users_UserId",
                table: "Warnings");

            migrationBuilder.DropTable(
                name: "SubjectUser");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warnings",
                table: "Warnings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RemovedByAdmin",
                table: "MainThreads");

            migrationBuilder.DropColumn(
                name: "RemovedByAdmin",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Warnings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Warnings");

            migrationBuilder.DropColumn(
                name: "HandeldAtTime",
                table: "Warnings");

            migrationBuilder.DropColumn(
                name: "IsHandled",
                table: "Warnings");

            migrationBuilder.DropColumn(
                name: "ObjectsId",
                table: "Warnings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Warnings");

            migrationBuilder.RenameTable(
                name: "Warnings",
                newName: "Warning");

            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "MainThreads",
                newName: "Text");

            migrationBuilder.RenameIndex(
                name: "IX_Warnings_UserId",
                table: "Warning",
                newName: "IX_Warning_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Votes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Banned",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Users",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Subjects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "MainThreads",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warning",
                table: "Warning",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SubCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategory_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubCategory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_UserId",
                table: "Subjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MainThreads_SubCategoryId",
                table: "MainThreads",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategory_SubjectId",
                table: "SubCategory",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategory_UserId",
                table: "SubCategory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MainThreads_SubCategory_SubCategoryId",
                table: "MainThreads",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Users_UserId",
                table: "Subjects",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warning_Users_UserId",
                table: "Warning",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
