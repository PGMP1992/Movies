using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Data.Migrations
{
    /// <inheritdoc />
    public partial class MovieList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Playlists_PlaylistId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_PlaylistId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "MoviePlaylist",
                columns: table => new
                {
                    MovieListId = table.Column<int>(type: "int", nullable: false),
                    PlaylistListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviePlaylist", x => new { x.MovieListId, x.PlaylistListId });
                    table.ForeignKey(
                        name: "FK_MoviePlaylist_Movies_MovieListId",
                        column: x => x.MovieListId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviePlaylist_Playlists_PlaylistListId",
                        column: x => x.PlaylistListId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoviePlaylist_PlaylistListId",
                table: "MoviePlaylist",
                column: "PlaylistListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviePlaylist");

            migrationBuilder.AddColumn<int>(
                name: "PlaylistId",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_PlaylistId",
                table: "Movies",
                column: "PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Playlists_PlaylistId",
                table: "Movies",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id");
        }
    }
}
