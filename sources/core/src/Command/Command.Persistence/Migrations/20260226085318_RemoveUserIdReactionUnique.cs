using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Command.Persistence.Migrations;


/// <inheritdoc />
public partial class RemoveUserIdReactionUnique : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_PostReactions_PostId_UserId",
            table: "PostReactions");

        migrationBuilder.CreateIndex(
            name: "IX_PostReactions_PostId_UserId",
            table: "PostReactions",
            columns: new[] { "PostId", "UserId" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_PostReactions_PostId_UserId",
            table: "PostReactions");

        migrationBuilder.CreateIndex(
            name: "IX_PostReactions_PostId_UserId",
            table: "PostReactions",
            columns: new[] { "PostId", "UserId" },
            unique: true);
    }
}
