using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Command.Persistence.Migrations;

/// <inheritdoc />
public partial class RemoveLikeCountsColumn : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "LikeCount",
            table: "Post");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "LikeCount",
            table: "Post",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }
}
