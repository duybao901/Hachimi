using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Command.Persistence.Migrations;

/// <inheritdoc />
public partial class AddReactionTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsDelete",
            table: "Post");

        migrationBuilder.RenameColumn(
            name: "IsDelete",
            table: "Tags",
            newName: "IsDeleted");

        migrationBuilder.RenameColumn(
            name: "IsDelete",
            table: "PostTags",
            newName: "IsDeleted");

        migrationBuilder.CreateTable(
            name: "ReactionTypes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Icon = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ReactionTypes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PostReactions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ReactionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostReactions", x => x.Id);
                table.ForeignKey(
                    name: "FK_PostReactions_Post_PostId",
                    column: x => x.PostId,
                    principalTable: "Post",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PostReactions_ReactionTypes_ReactionTypeId",
                    column: x => x.ReactionTypeId,
                    principalTable: "ReactionTypes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.InsertData(
            table: "ReactionTypes",
            columns: new[] { "Id", "Icon", "Name" },
            values: new object[,]
            {
                { new Guid("5b80034d-997d-409b-b6fb-4520fb531fa7"), "hands", "RaisedHands" },
                { new Guid("8bb5b7c9-06e7-4c1e-9101-c146228e3e48"), "unicorn", "Unicorn" },
                { new Guid("c4fc3cef-434e-44b0-8871-9ad46d689424"), "heart", "Like" },
                { new Guid("cd8c5ca0-b7ef-4a0f-8e9f-44fcaef4225d"), "exploding", "ExplodingHead" },
                { new Guid("e06b741b-d1a2-4dbe-ae33-d2ee1ca6901b"), "fire", "Fire" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_PostReactions_PostId",
            table: "PostReactions",
            column: "PostId");

        migrationBuilder.CreateIndex(
            name: "IX_PostReactions_PostId_UserId",
            table: "PostReactions",
            columns: new[] { "PostId", "UserId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PostReactions_ReactionTypeId",
            table: "PostReactions",
            column: "ReactionTypeId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PostReactions");

        migrationBuilder.DropTable(
            name: "ReactionTypes");

        migrationBuilder.RenameColumn(
            name: "IsDeleted",
            table: "Tags",
            newName: "IsDelete");

        migrationBuilder.RenameColumn(
            name: "IsDeleted",
            table: "PostTags",
            newName: "IsDelete");

        migrationBuilder.AddColumn<bool>(
            name: "IsDelete",
            table: "Post",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }
}
