using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VidhayakApp.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Dob = table.Column<DateTime>(type: "date", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: false),
                    Ward = table.Column<string>(type: "longtext", nullable: false),
                    MobileNumber = table.Column<string>(type: "longtext", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    Education = table.Column<string>(type: "longtext", nullable: true),
                    AadharNumber = table.Column<string>(type: "longtext", nullable: true),
                    SamagraID = table.Column<string>(type: "longtext", nullable: true),
                    VoterID = table.Column<string>(type: "longtext", nullable: true),
                    Caste = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
