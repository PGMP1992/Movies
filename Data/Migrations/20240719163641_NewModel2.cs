using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Playlists",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Playlists_PlaylistId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_PlaylistId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "Movies");
        }
    }
}
