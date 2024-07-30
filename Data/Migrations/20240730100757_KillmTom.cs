using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Data.Migrations
{
    /// <inheritdoc />
    public partial class KillmTom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviePlaylist");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
