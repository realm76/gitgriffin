using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitGriffin.Web.Migrations
{
    /// <inheritdoc />
    public partial class ModifyOAuthToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "refresh_token",
                table: "user_oauth_tokens",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "expires",
                table: "user_oauth_tokens",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expires",
                table: "user_oauth_tokens");

            migrationBuilder.AlterColumn<string>(
                name: "refresh_token",
                table: "user_oauth_tokens",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
