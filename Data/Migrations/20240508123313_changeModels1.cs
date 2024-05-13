using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeModels1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_AppUserId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_AppUserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "RentPrice",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Playlists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_AddressId",
                table: "Playlists",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Address_AddressId",
                table: "AspNetUsers",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Address_AddressId",
                table: "Playlists",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Address_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Address_AddressId",
                table: "Playlists");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_AddressId",
                table: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Movies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BuyPrice",
                table: "Movies",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RentPrice",
                table: "Movies",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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
        }
    }
}
