using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationAPI.Migrations;

/// <inheritdoc />
public partial class UpdateIsDeletedColumnAuthor : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "IsDelete",
            table: "UserProfiles",
            newName: "IsDeleted");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "IsDeleted",
            table: "UserProfiles",
            newName: "IsDelete");
    }
}
