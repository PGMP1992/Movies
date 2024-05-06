using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Playlists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Movies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_AppUserId",
                table: "Playlists",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_AppUserId",
                table: "Movies",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_AppUserId",
                table: "Movies",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_AspNetUsers_AppUserId",
                table: "Playlists",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_AppUserId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_AspNetUsers_AppUserId",
                table: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_AppUserId",
                table: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_Movies_AppUserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
