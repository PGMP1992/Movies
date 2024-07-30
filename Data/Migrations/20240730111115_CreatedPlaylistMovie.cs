using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatedPlaylistMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Movies_MovieId",
                table: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_MovieId",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Playlists");

            migrationBuilder.CreateTable(
                name: "PlaylistMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaylistId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistMovies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistMovies_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistMovies_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistMovies_MovieId",
                table: "PlaylistMovies",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistMovies_PlaylistId",
                table: "PlaylistMovies",
                column: "PlaylistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistMovies");

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Playlists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_MovieId",
                table: "Playlists",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Movies_MovieId",
                table: "Playlists",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
