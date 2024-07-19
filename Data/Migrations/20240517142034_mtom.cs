using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Data.Migrations
{
    /// <inheritdoc />
    public partial class mtom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviePlaylist_Playlists_PlaylistId",
                table: "MoviePlaylist");

            migrationBuilder.RenameColumn(
                name: "PlaylistId",
                table: "MoviePlaylist",
                newName: "PlaylistListId");

            migrationBuilder.RenameIndex(
                name: "IX_MoviePlaylist_PlaylistId",
                table: "MoviePlaylist",
                newName: "IX_MoviePlaylist_PlaylistListId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePlaylist_Playlists_PlaylistListId",
                table: "MoviePlaylist",
                column: "PlaylistListId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviePlaylist_Playlists_PlaylistListId",
                table: "MoviePlaylist");

            migrationBuilder.RenameColumn(
                name: "PlaylistListId",
                table: "MoviePlaylist",
                newName: "PlaylistId");

            migrationBuilder.RenameIndex(
                name: "IX_MoviePlaylist_PlaylistListId",
                table: "MoviePlaylist",
                newName: "IX_MoviePlaylist_PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePlaylist_Playlists_PlaylistId",
                table: "MoviePlaylist",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
