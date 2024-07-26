using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Data.Migrations
{
    /// <inheritdoc />
    public partial class addMtoM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviePlaylist_Movies_MoviesListId",
                table: "MoviePlaylist");

            migrationBuilder.RenameColumn(
                name: "MoviesListId",
                table: "MoviePlaylist",
                newName: "MoviesId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePlaylist_Movies_MoviesId",
                table: "MoviePlaylist",
                column: "MoviesId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviePlaylist_Movies_MoviesId",
                table: "MoviePlaylist");

            migrationBuilder.RenameColumn(
                name: "MoviesId",
                table: "MoviePlaylist",
                newName: "MoviesListId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviePlaylist_Movies_MoviesListId",
                table: "MoviePlaylist",
                column: "MoviesListId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
