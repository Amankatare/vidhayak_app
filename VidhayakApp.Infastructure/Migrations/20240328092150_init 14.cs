using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VidhayakApp.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class init14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchemeId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_DepartmentId",
                table: "Items",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SchemeId",
                table: "Items",
                column: "SchemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_GovtDepartments_DepartmentId",
                table: "Items",
                column: "DepartmentId",
                principalTable: "GovtDepartments",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_GovtSchemes_SchemeId",
                table: "Items",
                column: "SchemeId",
                principalTable: "GovtSchemes",
                principalColumn: "SchemeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_GovtDepartments_DepartmentId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_GovtSchemes_SchemeId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_DepartmentId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_SchemeId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SchemeId",
                table: "Items");
        }
    }
}
