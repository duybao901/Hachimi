using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationAPI.Migrations;

/// <inheritdoc />
public partial class UpdateAppUserTable : Migration
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
            name: "Address",
            table: "AppUsers");

        migrationBuilder.DropColumn(
            name: "DateOfBirth",
            table: "AppUsers");

        migrationBuilder.DropColumn(
            name: "IsDirector",
            table: "AppUsers");

        migrationBuilder.DropColumn(
            name: "IsHeadOfDepartment",
            table: "AppUsers");

        migrationBuilder.DropColumn(
            name: "IsReceipient",
            table: "AppUsers");

        migrationBuilder.DropColumn(
            name: "ManagerId",
            table: "AppUsers");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "AppUsers");

        migrationBuilder.RenameColumn(
            name: "LastName",
            table: "AppUsers",
            newName: "Password");

        migrationBuilder.RenameColumn(
            name: "FirstName",
            table: "AppUsers",
            newName: "Name");

        migrationBuilder.AlterColumn<string>(
            name: "UserName",
            table: "AppUsers",
            type: "nvarchar(256)",
            maxLength: 256,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
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

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
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

        migrationBuilder.RenameColumn(
            name: "Password",
            table: "AppUsers",
            newName: "LastName");

        migrationBuilder.RenameColumn(
            name: "Name",
            table: "AppUsers",
            newName: "FirstName");

        migrationBuilder.AlterColumn<string>(
            name: "UserName",
            table: "AppUsers",
            type: "nvarchar(450)",
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

        migrationBuilder.AddColumn<string>(
            name: "Address",
            table: "AppUsers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<DateTime>(
            name: "DateOfBirth",
            table: "AppUsers",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<bool>(
            name: "IsDirector",
            table: "AppUsers",
            type: "bit",
            nullable: true,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsHeadOfDepartment",
            table: "AppUsers",
            type: "bit",
            nullable: true,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "IsReceipient",
            table: "AppUsers",
            type: "int",
            nullable: false,
            defaultValue: -1);

        migrationBuilder.AddColumn<Guid>(
            name: "ManagerId",
            table: "AppUsers",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<Guid>(
            name: "UserId",
            table: "AppUsers",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            name: "IX_AppUsers_Email",
            table: "AppUsers",
            column: "Email",
            unique: true,
            filter: "[Email] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AppUsers_UserName",
            table: "AppUsers",
            column: "UserName",
            unique: true,
            filter: "[UserName] IS NOT NULL");
    }
}
