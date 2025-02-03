using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitGriffin.Blazor.Migrations
{
    /// <inheritdoc />
    public partial class GithubConfigAddUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "GithubConfig",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "GithubConfig");
        }
    }
}
