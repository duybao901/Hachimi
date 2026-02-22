using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Command.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdateCoverImageUrlPost : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("5b80034d-997d-409b-b6fb-4520fb531fa7"));

        migrationBuilder.DeleteData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("8bb5b7c9-06e7-4c1e-9101-c146228e3e48"));

        migrationBuilder.DeleteData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("c4fc3cef-434e-44b0-8871-9ad46d689424"));

        migrationBuilder.DeleteData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("cd8c5ca0-b7ef-4a0f-8e9f-44fcaef4225d"));

        migrationBuilder.DeleteData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("e06b741b-d1a2-4dbe-ae33-d2ee1ca6901b"));

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "ReactionTypes",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "PostReactions",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AlterColumn<string>(
            name: "CoverImageUrl",
            table: "Post",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.InsertData(
            table: "ReactionTypes",
            columns: new[] { "Id", "Icon", "IsDeleted", "Name" },
            values: new object[,]
            {
                { new Guid("31574891-5b59-49bd-a697-eca5b6b5dc94"), "exploding", false, "ExplodingHead" },
                { new Guid("72b5c96b-4859-40e7-b87f-210440e629fd"), "unicorn", false, "Unicorn" },
                { new Guid("904d8bed-c0b6-4d28-90bb-b539d3074bc7"), "hands", false, "RaisedHands" },
                { new Guid("97de8548-8026-4a6a-a1c7-c4b25a4e6e2e"), "fire", false, "Fire" },
                { new Guid("ea58565b-1456-4169-bb47-37102c635710"), "heart", false, "Like" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("31574891-5b59-49bd-a697-eca5b6b5dc94"));

        migrationBuilder.DeleteData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("72b5c96b-4859-40e7-b87f-210440e629fd"));

        migrationBuilder.DeleteData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("904d8bed-c0b6-4d28-90bb-b539d3074bc7"));

        migrationBuilder.DeleteData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("97de8548-8026-4a6a-a1c7-c4b25a4e6e2e"));

        migrationBuilder.DeleteData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("ea58565b-1456-4169-bb47-37102c635710"));

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "ReactionTypes");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "PostReactions");

        migrationBuilder.AlterColumn<string>(
            name: "CoverImageUrl",
            table: "Post",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

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
    }
}
