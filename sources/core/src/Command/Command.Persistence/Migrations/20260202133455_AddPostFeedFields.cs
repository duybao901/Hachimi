using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Command.Persistence.Migrations;

/// <inheritdoc />
public partial class AddPostFeedFields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "ViewCount",
            table: "Post",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Slug",
            table: "Post",
            type: "nvarchar(250)",
            maxLength: 250,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<int>(
            name: "ReadingTimeMinutes",
            table: "Post",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "PostStatus",
            table: "Post",
            type: "int",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int",
            oldDefaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "CommentCount",
            table: "Post",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "Excerpt",
            table: "Post",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<double>(
            name: "FeedScore",
            table: "Post",
            type: "float",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "Post",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "LikeCount",
            table: "Post",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "PublishedAt",
            table: "Post",
            type: "datetimeoffset(3)",
            precision: 3,
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Post_PostStatus_FeedScore",
            table: "Post",
            columns: new[] { "PostStatus", "FeedScore" });

        migrationBuilder.CreateIndex(
            name: "IX_Post_PostStatus_PublishedAt",
            table: "Post",
            columns: new[] { "PostStatus", "PublishedAt" });

        migrationBuilder.CreateIndex(
            name: "IX_Post_Slug",
            table: "Post",
            column: "Slug",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Post_PostStatus_FeedScore",
            table: "Post");

        migrationBuilder.DropIndex(
            name: "IX_Post_PostStatus_PublishedAt",
            table: "Post");

        migrationBuilder.DropIndex(
            name: "IX_Post_Slug",
            table: "Post");

        migrationBuilder.DropColumn(
            name: "CommentCount",
            table: "Post");

        migrationBuilder.DropColumn(
            name: "Excerpt",
            table: "Post");

        migrationBuilder.DropColumn(
            name: "FeedScore",
            table: "Post");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "Post");

        migrationBuilder.DropColumn(
            name: "LikeCount",
            table: "Post");

        migrationBuilder.DropColumn(
            name: "PublishedAt",
            table: "Post");

        migrationBuilder.AlterColumn<int>(
            name: "ViewCount",
            table: "Post",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int",
            oldDefaultValue: 0);

        migrationBuilder.AlterColumn<string>(
            name: "Slug",
            table: "Post",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(250)",
            oldMaxLength: 250);

        migrationBuilder.AlterColumn<int>(
            name: "ReadingTimeMinutes",
            table: "Post",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<int>(
            name: "PostStatus",
            table: "Post",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int");
    }
}
