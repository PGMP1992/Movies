using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesApp.Migrations
{
    /// <inheritdoc />
    public partial class changingListNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviePlaylist_Movies_MovieListId",
                table: "MoviePlaylist");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePlaylist_Playlists_PlaylistListId",
                table: "MoviePlaylist");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Playlists");

            migrationBuilder.RenameColumn(
                name: "PlaylistListId",
                table: "MoviePlaylist",
                newName: "PlaylistsId");

            migrationBuilder.RenameColumn(
                name: "MovieListId",
                table: "MoviePlaylist",
                newName: "MoviesId");

            migrationBuilder.RenameIndex(
                name: "IX_MoviePlaylist_PlaylistListId",
                table: "MoviePlaylist",
                newName: "IX_MoviePlaylist_PlaylistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePlaylist_Movies_MoviesId",
                table: "MoviePlaylist",
                column: "MoviesId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePlaylist_Playlists_PlaylistsId",
                table: "MoviePlaylist",
                column: "PlaylistsId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviePlaylist_Movies_MoviesId",
                table: "MoviePlaylist");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviePlaylist_Playlists_PlaylistsId",
                table: "MoviePlaylist");

            migrationBuilder.RenameColumn(
                name: "PlaylistsId",
                table: "MoviePlaylist",
                newName: "PlaylistListId");

            migrationBuilder.RenameColumn(
                name: "MoviesId",
                table: "MoviePlaylist",
                newName: "MovieListId");

            migrationBuilder.RenameIndex(
                name: "IX_MoviePlaylist_PlaylistsId",
                table: "MoviePlaylist",
                newName: "IX_MoviePlaylist_PlaylistListId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Playlists",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePlaylist_Movies_MovieListId",
                table: "MoviePlaylist",
                column: "MovieListId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePlaylist_Playlists_PlaylistListId",
                table: "MoviePlaylist",
                column: "PlaylistListId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
