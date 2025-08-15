using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesApp.Migrations
{
    /// <inheritdoc />
    public partial class NTier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Genre",
                table: "Movies",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Movies",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
