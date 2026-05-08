using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BetTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookmakers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    UrlLogo = table.Column<string>(type: "TEXT", nullable: false),
                    Actif = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateActivation = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmakers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookmakerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    MontantBonus = table.Column<decimal>(type: "real", nullable: false),
                    CoteMinimum = table.Column<decimal>(type: "real", nullable: false),
                    Statut = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateTerminaison = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offres_Bookmakers_BookmakerId",
                        column: x => x.BookmakerId,
                        principalTable: "Bookmakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Calculs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OffreId = table.Column<int>(type: "INTEGER", nullable: false),
                    CoteBook1 = table.Column<decimal>(type: "real", nullable: false),
                    CoteBook2 = table.Column<decimal>(type: "real", nullable: false),
                    MiseBook1 = table.Column<decimal>(type: "real", nullable: false),
                    MiseBook2 = table.Column<decimal>(type: "real", nullable: false),
                    PerteCouverture = table.Column<decimal>(type: "real", nullable: false),
                    EVNette = table.Column<decimal>(type: "real", nullable: false),
                    GoNoGo = table.Column<bool>(type: "INTEGER", nullable: false),
                    Valide = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCalcul = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calculs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calculs_Offres_OffreId",
                        column: x => x.OffreId,
                        principalTable: "Offres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mouvements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookmakerId = table.Column<int>(type: "INTEGER", nullable: false),
                    OffreId = table.Column<int>(type: "INTEGER", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Montant = table.Column<decimal>(type: "real", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mouvements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mouvements_Bookmakers_BookmakerId",
                        column: x => x.BookmakerId,
                        principalTable: "Bookmakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mouvements_Offres_OffreId",
                        column: x => x.OffreId,
                        principalTable: "Offres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Executions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CalculId = table.Column<int>(type: "INTEGER", nullable: false),
                    MiseReelleBook1 = table.Column<decimal>(type: "real", nullable: false),
                    MiseReelleBook2 = table.Column<decimal>(type: "real", nullable: false),
                    CoteReelleBook1 = table.Column<decimal>(type: "real", nullable: false),
                    CoteReelleBook2 = table.Column<decimal>(type: "real", nullable: false),
                    ResultatMatch = table.Column<int>(type: "INTEGER", nullable: false),
                    GainBrut = table.Column<decimal>(type: "real", nullable: false),
                    EVReelle = table.Column<decimal>(type: "real", nullable: false),
                    DateExecution = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Executions_Calculs_CalculId",
                        column: x => x.CalculId,
                        principalTable: "Calculs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Bookmakers",
                columns: new[] { "Id", "Actif", "DateActivation", "Nom", "UrlLogo" },
                values: new object[,]
                {
                    { 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Betclic", "" },
                    { 2, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Winamax", "" },
                    { 3, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unibet", "" },
                    { 4, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMU", "" },
                    { 5, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bwin", "" },
                    { 6, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NetBet", "" },
                    { 7, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ZEbet", "" },
                    { 8, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Parions Sport", "" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calculs_OffreId",
                table: "Calculs",
                column: "OffreId");

            migrationBuilder.CreateIndex(
                name: "IX_Executions_CalculId",
                table: "Executions",
                column: "CalculId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mouvements_BookmakerId",
                table: "Mouvements",
                column: "BookmakerId");

            migrationBuilder.CreateIndex(
                name: "IX_Mouvements_OffreId",
                table: "Mouvements",
                column: "OffreId");

            migrationBuilder.CreateIndex(
                name: "IX_Offres_BookmakerId",
                table: "Offres",
                column: "BookmakerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Executions");

            migrationBuilder.DropTable(
                name: "Mouvements");

            migrationBuilder.DropTable(
                name: "Calculs");

            migrationBuilder.DropTable(
                name: "Offres");

            migrationBuilder.DropTable(
                name: "Bookmakers");
        }
    }
}
