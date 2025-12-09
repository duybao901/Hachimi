using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationAPI.Migrations;

/// <inheritdoc />
public partial class UpdateAppUserAndAddUserProfile : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_AppUsers_Email",
            table: "AppUsers");

        migrationBuilder.DropIndex(
            name: "IX_AppUsers_UserName",
            table: "AppUsers");

        migrationBuilder.DropColumn(
            name: "AvatarUrl",
            table: "AppUsers");

        migrationBuilder.DropColumn(
            name: "Bio",
            table: "AppUsers");

        migrationBuilder.DropColumn(
            name: "Name",
            table: "AppUsers");

        migrationBuilder.AlterColumn<string>(
            name: "UserName",
            table: "AppUsers",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(256)",
            oldMaxLength: 256);

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "AppUsers",
            type: "nvarchar(450)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(256)",
            oldMaxLength: 256);

        migrationBuilder.CreateTable(
            name: "UserProfiles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                AvatarUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDelete = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserProfiles", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AppUsers_Email",
            table: "AppUsers",
            column: "Email",
            unique: true,
            filter: "[Email] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserProfiles");

        migrationBuilder.DropIndex(
            name: "IX_AppUsers_Email",
            table: "AppUsers");

        migrationBuilder.AlterColumn<string>(
            name: "UserName",
            table: "AppUsers",
            type: "nvarchar(256)",
            maxLength: 256,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "AppUsers",
            type: "nvarchar(256)",
            maxLength: 256,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "AvatarUrl",
            table: "AppUsers",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Bio",
            table: "AppUsers",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "AppUsers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "IX_AppUsers_Email",
            table: "AppUsers",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AppUsers_UserName",
            table: "AppUsers",
            column: "UserName",
            unique: true);
    }
}
