using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiiaDocsUploader.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CustomFullName = table.Column<string>(type: "text", nullable: true),
                    CustomFullAddress = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    House = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentMetadatas",
                columns: table => new
                {
                    DeepLinkId = table.Column<Guid>(type: "uuid", nullable: false),
                    MetadataFilePath = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentMetadatas", x => x.DeepLinkId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameDiia = table.Column<string>(type: "text", nullable: false),
                    NameUa = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    BranchId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeepLinkId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentFilePath = table.Column<string>(type: "text", nullable: false),
                    DigitalSignaturePath = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentFiles_DocumentMetadatas_DeepLinkId",
                        column: x => x.DeepLinkId,
                        principalTable: "DocumentMetadatas",
                        principalColumn: "DeepLinkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchDocumentTypes",
                columns: table => new
                {
                    BranchId = table.Column<string>(type: "text", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchDocumentTypes", x => new { x.DocumentTypeId, x.BranchId });
                    table.ForeignKey(
                        name: "FK_BranchDocumentTypes_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchDocumentTypes_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferDocumentTypes",
                columns: table => new
                {
                    OfferId = table.Column<string>(type: "text", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferDocumentTypes", x => new { x.DocumentTypeId, x.OfferId });
                    table.ForeignKey(
                        name: "FK_OfferDocumentTypes_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferDocumentTypes_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "Id", "NameDiia", "NameUa" },
                values: new object[,]
                {
                    { 1, "internal-passport", "Паспорт громадянина України у формі ID-картки" },
                    { 2, "foreign-passport", "Біометричний закордонний паспорт або закордонний паспорт" },
                    { 3, "taxpayer-card", "РНОКПП" },
                    { 4, "user-birth-record", "Свідоцтво про народження користувача" },
                    { 5, "birth-certificate", "Свідоцтво про народження дитини" },
                    { 6, "reference-internally-displaced-person", "Довідка внутрішньо переміщеної особи (ВПО)" },
                    { 7, "student-id-card", "Студентський квиток" },
                    { 8, "pension-card", "Пенсійне посвідчення" },
                    { 9, "name-change-act-record", "Відомості АЗ про зміну імені" },
                    { 10, "marriage-act-record", "Відомості АЗ про укладання шлюбу" },
                    { 11, "divorce-act-record", "Відомості АЗ про розірвання шлюбу" },
                    { 12, "veteran-certificate", "Посвідчення ветерана" },
                    { 13, "education-document", "Освітні документи" },
                    { 14, "residence-permit-permanent", "Е-посвідка на постійне проживання" },
                    { 15, "residence-permit-temporary", "Е-посвідка на тимчасове проживання" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchDocumentTypes_BranchId",
                table: "BranchDocumentTypes",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_DeepLinkId",
                table: "DocumentFiles",
                column: "DeepLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferDocumentTypes_OfferId",
                table: "OfferDocumentTypes",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_BranchId",
                table: "Offers",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchDocumentTypes");

            migrationBuilder.DropTable(
                name: "DocumentFiles");

            migrationBuilder.DropTable(
                name: "OfferDocumentTypes");

            migrationBuilder.DropTable(
                name: "DocumentMetadatas");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
