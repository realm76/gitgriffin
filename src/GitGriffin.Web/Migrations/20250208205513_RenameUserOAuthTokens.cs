using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitGriffin.Web.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserOAuthTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_o_auth_tokens_users_user_id",
                table: "user_o_auth_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_o_auth_tokens",
                table: "user_o_auth_tokens");

            migrationBuilder.RenameTable(
                name: "user_o_auth_tokens",
                newName: "user_oauth_tokens");

            migrationBuilder.RenameIndex(
                name: "ix_user_o_auth_tokens_user_id",
                table: "user_oauth_tokens",
                newName: "ix_user_oauth_tokens_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_oauth_tokens",
                table: "user_oauth_tokens",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_oauth_tokens_users_user_id",
                table: "user_oauth_tokens",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_oauth_tokens_users_user_id",
                table: "user_oauth_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_oauth_tokens",
                table: "user_oauth_tokens");

            migrationBuilder.RenameTable(
                name: "user_oauth_tokens",
                newName: "user_o_auth_tokens");

            migrationBuilder.RenameIndex(
                name: "ix_user_oauth_tokens_user_id",
                table: "user_o_auth_tokens",
                newName: "ix_user_o_auth_tokens_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_o_auth_tokens",
                table: "user_o_auth_tokens",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_o_auth_tokens_users_user_id",
                table: "user_o_auth_tokens",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
