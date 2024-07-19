using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviePlaylist");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Playlists_PlaylistId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_PlaylistId",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "MoviePlaylist",
                columns: table => new
                {
                    MoviesListId = table.Column<int>(type: "int", nullable: false),
                    PlaylistListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviePlaylist", x => new { x.MoviesListId, x.PlaylistListId });
                    table.ForeignKey(
                        name: "FK_MoviePlaylist_Movies_MoviesListId",
                        column: x => x.MoviesListId,
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
