using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatServerWebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGlobalChatToUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GlobalChat");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "GlobalChat",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "GlobalChat");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "GlobalChat",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
