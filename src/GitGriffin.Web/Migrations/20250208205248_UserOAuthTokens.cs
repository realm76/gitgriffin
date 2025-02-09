using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GitGriffin.Web.Migrations
{
    /// <inheritdoc />
    public partial class UserOAuthTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "github_config");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "AspNetUserTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "login_provider",
                table: "AspNetUserTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "provider_key",
                table: "AspNetUserLogins",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "login_provider",
                table: "AspNetUserLogins",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "user_o_auth_tokens",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    provider = table.Column<string>(type: "text", nullable: false),
                    access_token = table.Column<string>(type: "text", nullable: false),
                    refresh_token = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_o_auth_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_o_auth_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_o_auth_tokens_user_id",
                table: "user_o_auth_tokens",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_o_auth_tokens");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "AspNetUserTokens",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "login_provider",
                table: "AspNetUserTokens",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "provider_key",
                table: "AspNetUserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "login_provider",
                table: "AspNetUserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "github_config",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    access_token = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    repositories = table.Column<List<string>>(type: "text[]", nullable: false),
                    user_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_github_config", x => x.id);
                    table.ForeignKey(
                        name: "fk_github_config_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_github_config_user_id",
                table: "github_config",
                column: "user_id",
                unique: true);
        }
    }
}
