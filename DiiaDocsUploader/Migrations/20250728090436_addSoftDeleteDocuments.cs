using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiiaDocsUploader.Migrations
{
    /// <inheritdoc />
    public partial class addSoftDeleteDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DocumentMetadatas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DocumentMetadatas");
        }
    }
}
