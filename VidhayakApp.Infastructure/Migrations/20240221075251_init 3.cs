using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VidhayakApp.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AadharNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Caste",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SamagraID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VoterID",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Education = table.Column<string>(type: "longtext", nullable: true),
                    AadharNumber = table.Column<string>(type: "longtext", nullable: true),
                    SamagraID = table.Column<string>(type: "longtext", nullable: true),
                    VoterID = table.Column<string>(type: "longtext", nullable: true),
                    Caste = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_UserDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "AadharNumber",
                table: "Users",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Caste",
                table: "Users",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "Users",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "SamagraID",
                table: "Users",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "VoterID",
                table: "Users",
                type: "longtext",
                nullable: false);
        }
    }
}
