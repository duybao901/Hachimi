using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Command.Persistence.Migrations;

/// <inheritdoc />
public partial class AddPostStatusColumn : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsPublished",
            table: "Post");

        migrationBuilder.AddColumn<int>(
            name: "PostStatus",
            table: "Post",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PostStatus",
            table: "Post");

        migrationBuilder.AddColumn<bool>(
            name: "IsPublished",
            table: "Post",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }
}
