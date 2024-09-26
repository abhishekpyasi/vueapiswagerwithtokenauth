using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vueapi.Migrations
{
    public partial class removediSdeletedassn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Assignments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Assignments",
                type: "bit",
                nullable: true);
        }
    }
}
