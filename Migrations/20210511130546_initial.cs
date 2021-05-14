using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieDataBase.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    imdbID = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    Rated = table.Column<string>(nullable: true),
                    Released = table.Column<string>(nullable: true),
                    Runtime = table.Column<string>(nullable: true),
                    Genre = table.Column<string>(nullable: true),
                    Director = table.Column<string>(nullable: true),
                    Writer = table.Column<string>(nullable: true),
                    Actors = table.Column<string>(nullable: true),
                    Plot = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Awards = table.Column<string>(nullable: true),
                    Poster = table.Column<string>(nullable: true),
                    Metascore = table.Column<string>(nullable: true),
                    imdbRating = table.Column<string>(nullable: true),
                    imdbVotes = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    DVD = table.Column<string>(nullable: true),
                    BoxOffice = table.Column<string>(nullable: true),
                    Production = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Response = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.imdbID);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    MovieimdbID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Movie_MovieimdbID",
                        column: x => x.MovieimdbID,
                        principalTable: "Movie",
                        principalColumn: "imdbID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_MovieimdbID",
                table: "Ratings",
                column: "MovieimdbID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Movie");
        }
    }
}
