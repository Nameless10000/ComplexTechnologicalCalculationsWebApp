using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AglomResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AglomResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cocksicks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    PercentZola = table.Column<double>(type: "double precision", nullable: false),
                    PercentSera = table.Column<double>(type: "double precision", nullable: false),
                    PercentValotiles = table.Column<double>(type: "double precision", nullable: false),
                    PercentC = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cocksicks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FluxAdditionss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IzvestnyakCaO = table.Column<double>(type: "double precision", nullable: false),
                    IzvestnyakSiO2 = table.Column<double>(type: "double precision", nullable: false),
                    IzvestnyakAl2O3 = table.Column<double>(type: "double precision", nullable: false),
                    IzvestnyakMgO = table.Column<double>(type: "double precision", nullable: false),
                    IzvestnyakPMPP = table.Column<double>(type: "double precision", nullable: false),
                    DolomyteCaO = table.Column<double>(type: "double precision", nullable: false),
                    DolomyteSiO2 = table.Column<double>(type: "double precision", nullable: false),
                    DolomyteAl2O3 = table.Column<double>(type: "double precision", nullable: false),
                    DolomyteMgO = table.Column<double>(type: "double precision", nullable: false),
                    DolomytePMPP = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxAdditionss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StartEnters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    osnovnost = table.Column<double>(type: "double precision", nullable: false),
                    FeOinAgl = table.Column<double>(type: "double precision", nullable: false),
                    DolomyteInAgl = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartEnters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZolaOfCocksicks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fe = table.Column<double>(type: "double precision", nullable: false),
                    CaO = table.Column<double>(type: "double precision", nullable: false),
                    SiO2 = table.Column<double>(type: "double precision", nullable: false),
                    Al2O3 = table.Column<double>(type: "double precision", nullable: false),
                    MgO = table.Column<double>(type: "double precision", nullable: false),
                    P = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZolaOfCocksicks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentName = table.Column<string>(type: "text", nullable: false),
                    ReportComponentOfShihta = table.Column<double>(type: "double precision", nullable: false),
                    ReportFe = table.Column<double>(type: "double precision", nullable: false),
                    ReportS = table.Column<double>(type: "double precision", nullable: false),
                    ReportP = table.Column<double>(type: "double precision", nullable: false),
                    ReportFeO = table.Column<double>(type: "double precision", nullable: false),
                    ReportCaO = table.Column<double>(type: "double precision", nullable: false),
                    ReportSiO2 = table.Column<double>(type: "double precision", nullable: false),
                    ReportAl2O3 = table.Column<double>(type: "double precision", nullable: false),
                    ReportMgO = table.Column<double>(type: "double precision", nullable: false),
                    ReportMnO = table.Column<double>(type: "double precision", nullable: false),
                    ReportTiO2 = table.Column<double>(type: "double precision", nullable: false),
                    ReportZn = table.Column<double>(type: "double precision", nullable: false),
                    ReportPMPP = table.Column<double>(type: "double precision", nullable: false),
                    ReportFe2O3 = table.Column<double>(type: "double precision", nullable: false),
                    ReportOxideSum = table.Column<double>(type: "double precision", nullable: false),
                    ReportCaO_SiO2 = table.Column<double>(type: "double precision", nullable: false),
                    AglomResponseID = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Components_AglomResponses_AglomResponseID",
                        column: x => x.AglomResponseID,
                        principalTable: "AglomResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AglomRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZolaOfCocksickID = table.Column<int>(type: "integer", nullable: false),
                    CocksickID = table.Column<int>(type: "integer", nullable: false),
                    FluxAdditionsID = table.Column<int>(type: "integer", nullable: false),
                    StartEnterID = table.Column<int>(type: "integer", nullable: false),
                    AglomResponseID = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AglomRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AglomRequests_AglomResponses_AglomResponseID",
                        column: x => x.AglomResponseID,
                        principalTable: "AglomResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AglomRequests_Cocksicks_CocksickID",
                        column: x => x.CocksickID,
                        principalTable: "Cocksicks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AglomRequests_FluxAdditionss_FluxAdditionsID",
                        column: x => x.FluxAdditionsID,
                        principalTable: "FluxAdditionss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AglomRequests_StartEnters_StartEnterID",
                        column: x => x.StartEnterID,
                        principalTable: "StartEnters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AglomRequests_ZolaOfCocksicks_ZolaOfCocksickID",
                        column: x => x.ZolaOfCocksickID,
                        principalTable: "ZolaOfCocksicks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShihtaComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    Wet = table.Column<double>(type: "double precision", nullable: false),
                    PMPP = table.Column<double>(type: "double precision", nullable: false),
                    Fe = table.Column<double>(type: "double precision", nullable: false),
                    FeO = table.Column<double>(type: "double precision", nullable: false),
                    CaO = table.Column<double>(type: "double precision", nullable: false),
                    SiO2 = table.Column<double>(type: "double precision", nullable: false),
                    MgO = table.Column<double>(type: "double precision", nullable: false),
                    Al2O3 = table.Column<double>(type: "double precision", nullable: false),
                    TiO2 = table.Column<double>(type: "double precision", nullable: false),
                    S = table.Column<double>(type: "double precision", nullable: false),
                    P = table.Column<double>(type: "double precision", nullable: false),
                    Cr = table.Column<double>(type: "double precision", nullable: false),
                    Zn = table.Column<double>(type: "double precision", nullable: false),
                    MnO = table.Column<double>(type: "double precision", nullable: false),
                    AglomRequestID = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShihtaComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShihtaComponents_AglomRequests_AglomRequestID",
                        column: x => x.AglomRequestID,
                        principalTable: "AglomRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AglomRequests_AglomResponseID",
                table: "AglomRequests",
                column: "AglomResponseID");

            migrationBuilder.CreateIndex(
                name: "IX_AglomRequests_CocksickID",
                table: "AglomRequests",
                column: "CocksickID");

            migrationBuilder.CreateIndex(
                name: "IX_AglomRequests_FluxAdditionsID",
                table: "AglomRequests",
                column: "FluxAdditionsID");

            migrationBuilder.CreateIndex(
                name: "IX_AglomRequests_StartEnterID",
                table: "AglomRequests",
                column: "StartEnterID");

            migrationBuilder.CreateIndex(
                name: "IX_AglomRequests_ZolaOfCocksickID",
                table: "AglomRequests",
                column: "ZolaOfCocksickID");

            migrationBuilder.CreateIndex(
                name: "IX_Components_AglomResponseID",
                table: "Components",
                column: "AglomResponseID");

            migrationBuilder.CreateIndex(
                name: "IX_ShihtaComponents_AglomRequestID",
                table: "ShihtaComponents",
                column: "AglomRequestID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "ShihtaComponents");

            migrationBuilder.DropTable(
                name: "AglomRequests");

            migrationBuilder.DropTable(
                name: "AglomResponses");

            migrationBuilder.DropTable(
                name: "Cocksicks");

            migrationBuilder.DropTable(
                name: "FluxAdditionss");

            migrationBuilder.DropTable(
                name: "StartEnters");

            migrationBuilder.DropTable(
                name: "ZolaOfCocksicks");
        }
    }
}
