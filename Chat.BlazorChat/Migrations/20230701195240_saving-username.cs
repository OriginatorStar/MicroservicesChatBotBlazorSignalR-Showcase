using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.BlazorChat.Migrations
{
    public partial class savingusername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "ChatMessages");
        }
    }
}
