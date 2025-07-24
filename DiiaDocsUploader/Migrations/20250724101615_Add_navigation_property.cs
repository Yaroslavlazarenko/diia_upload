using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiiaDocsUploader.Migrations
{
    /// <inheritdoc />
    public partial class Add_navigation_property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_NameDiia",
                table: "DocumentTypes",
                column: "NameDiia",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_NameUa",
                table: "DocumentTypes",
                column: "NameUa",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DocumentTypes_NameDiia",
                table: "DocumentTypes");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTypes_NameUa",
                table: "DocumentTypes");
        }
    }
}
