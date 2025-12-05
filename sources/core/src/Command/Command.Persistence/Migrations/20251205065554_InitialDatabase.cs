using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Command.Persistence.Migrations;

/// <inheritdoc />
public partial class InitialDatabase : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "OutboxMessages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                OccurredOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ProcessedOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OutboxMessages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Post",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsPublished = table.Column<bool>(type: "bit", nullable: false),
                ViewCount = table.Column<int>(type: "int", nullable: true),
                ReadingTimeMinutes = table.Column<int>(type: "int", nullable: true),
                AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                IsDelete = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Post", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Tags",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                IsDelete = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tags", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PostTags",
            columns: table => new
            {
                PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDelete = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostTags", x => new { x.PostId, x.TagId });
                table.ForeignKey(
                    name: "FK_PostTags_Post_PostId",
                    column: x => x.PostId,
                    principalTable: "Post",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PostTags_Tags_TagId",
                    column: x => x.TagId,
                    principalTable: "Tags",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PostTags_TagId",
            table: "PostTags",
            column: "TagId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "OutboxMessages");

        migrationBuilder.DropTable(
            name: "PostTags");

        migrationBuilder.DropTable(
            name: "Post");

        migrationBuilder.DropTable(
            name: "Tags");
    }
}
