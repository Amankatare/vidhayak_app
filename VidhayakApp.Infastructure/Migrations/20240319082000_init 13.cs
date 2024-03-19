using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VidhayakApp.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class init13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoPath",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "VoterCount",
                table: "UserDetails",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoterCount",
                table: "UserDetails");

            migrationBuilder.AddColumn<string>(
                name: "VideoPath",
                table: "Items",
                type: "longtext",
                nullable: true);
        }
    }
}
