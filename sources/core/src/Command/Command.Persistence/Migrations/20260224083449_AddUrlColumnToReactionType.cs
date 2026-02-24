using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Command.Persistence.Migrations;

/// <inheritdoc />
public partial class AddUrlColumnToReactionType : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Url",
            table: "ReactionTypes",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.UpdateData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("31574891-5b59-49bd-a697-eca5b6b5dc94"),
            columns: new[] { "Icon", "Name", "Url" },
            values: new object[] { "fire", "Fire", "/icons/reactions/fire.svg" });

        migrationBuilder.UpdateData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("72b5c96b-4859-40e7-b87f-210440e629fd"),
            columns: new[] { "Icon", "Name", "Url" },
            values: new object[] { "heart", "Like", "/icons/reactions/like.svg" });

        migrationBuilder.UpdateData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("904d8bed-c0b6-4d28-90bb-b539d3074bc7"),
            columns: new[] { "Icon", "Name", "Url" },
            values: new object[] { "exploding", "ExplodingHead", "/icons/reactions/exploding-head.svg" });

        migrationBuilder.UpdateData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("97de8548-8026-4a6a-a1c7-c4b25a4e6e2e"),
            columns: new[] { "Icon", "Name", "Url" },
            values: new object[] { "hands", "RaisedHands", "/icons/reactions/raised-hands.svg" });

        migrationBuilder.UpdateData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("ea58565b-1456-4169-bb47-37102c635710"),
            columns: new[] { "Icon", "Name", "Url" },
            values: new object[] { "unicorn", "Unicorn", "/icons/reactions/unicorn.svg" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Url",
            table: "ReactionTypes");

        migrationBuilder.UpdateData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("31574891-5b59-49bd-a697-eca5b6b5dc94"),
            columns: new[] { "Icon", "Name" },
            values: new object[] { "exploding", "ExplodingHead" });

        migrationBuilder.UpdateData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("72b5c96b-4859-40e7-b87f-210440e629fd"),
            columns: new[] { "Icon", "Name" },
            values: new object[] { "unicorn", "Unicorn" });

        migrationBuilder.UpdateData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("904d8bed-c0b6-4d28-90bb-b539d3074bc7"),
            columns: new[] { "Icon", "Name" },
            values: new object[] { "hands", "RaisedHands" });

        migrationBuilder.UpdateData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("97de8548-8026-4a6a-a1c7-c4b25a4e6e2e"),
            columns: new[] { "Icon", "Name" },
            values: new object[] { "fire", "Fire" });

        migrationBuilder.UpdateData(
            table: "ReactionTypes",
            keyColumn: "Id",
            keyValue: new Guid("ea58565b-1456-4169-bb47-37102c635710"),
            columns: new[] { "Icon", "Name" },
            values: new object[] { "heart", "Like" });
    }
}
