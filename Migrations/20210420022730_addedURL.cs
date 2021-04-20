using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieDataBase.Migrations
{
    public partial class addedURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pictureURL",
                table: "Movie",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pictureURL",
                table: "Movie");
        }
    }
}
