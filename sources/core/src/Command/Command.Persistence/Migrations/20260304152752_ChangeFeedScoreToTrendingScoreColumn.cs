using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Command.Persistence.Migrations;

/// <inheritdoc />
public partial class ChangeFeedScoreToTrendingScoreColumn : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "FeedScore",
            table: "Post",
            newName: "TrendingScore");

        migrationBuilder.RenameIndex(
            name: "IX_Post_PostStatus_FeedScore",
            table: "Post",
            newName: "IX_Post_PostStatus_TrendingScore");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "TrendingScore",
            table: "Post",
            newName: "FeedScore");

        migrationBuilder.RenameIndex(
            name: "IX_Post_PostStatus_TrendingScore",
            table: "Post",
            newName: "IX_Post_PostStatus_FeedScore");
    }
}
