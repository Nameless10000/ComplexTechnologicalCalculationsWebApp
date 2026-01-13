using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Migrations.SlagModeDB
{
    /// <inheritdoc />
    public partial class AddedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CastIrons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Si = table.Column<double>(type: "double precision", nullable: false),
                    S = table.Column<double>(type: "double precision", nullable: false),
                    Mn = table.Column<double>(type: "double precision", nullable: false),
                    C = table.Column<double>(type: "double precision", nullable: false),
                    Ti = table.Column<double>(type: "double precision", nullable: false),
                    Cr = table.Column<double>(type: "double precision", nullable: false),
                    Temp = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CastIrons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChargeComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sourcename = table.Column<string>(type: "text", nullable: false),
                    Consumption = table.Column<double>(type: "double precision", nullable: false),
                    Fe = table.Column<double>(type: "double precision", nullable: false),
                    SiO2 = table.Column<double>(type: "double precision", nullable: false),
                    Al2O3 = table.Column<double>(type: "double precision", nullable: false),
                    CaO = table.Column<double>(type: "double precision", nullable: false),
                    MgO = table.Column<double>(type: "double precision", nullable: false),
                    S = table.Column<double>(type: "double precision", nullable: false),
                    MnO = table.Column<double>(type: "double precision", nullable: false),
                    TiO2 = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeComponents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InputCoke",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Consumption = table.Column<double>(type: "double precision", nullable: false),
                    Sulfur = table.Column<double>(type: "double precision", nullable: false),
                    AshAmount = table.Column<double>(type: "double precision", nullable: false),
                    AshCaOFraction = table.Column<double>(type: "double precision", nullable: false),
                    AshSiO2Fraction = table.Column<double>(type: "double precision", nullable: false),
                    AshAl2O3Fraction = table.Column<double>(type: "double precision", nullable: false),
                    AshMgOFraction = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputCoke", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaO = table.Column<double>(type: "double precision", nullable: false),
                    SiO2 = table.Column<double>(type: "double precision", nullable: false),
                    TiO2 = table.Column<double>(type: "double precision", nullable: false),
                    Al2O3 = table.Column<double>(type: "double precision", nullable: false),
                    MgO = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CastIronID = table.Column<int>(type: "integer", nullable: false),
                    CokeID = table.Column<int>(type: "integer", nullable: false),
                    SlagID = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_CastIrons_CastIronID",
                        column: x => x.CastIronID,
                        principalTable: "CastIrons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_InputCoke_CokeID",
                        column: x => x.CokeID,
                        principalTable: "InputCoke",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Slags_SlagID",
                        column: x => x.SlagID,
                        principalTable: "Slags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChargeComponentRequest",
                columns: table => new
                {
                    ComponentsId = table.Column<int>(type: "integer", nullable: false),
                    RequestsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeComponentRequest", x => new { x.ComponentsId, x.RequestsId });
                    table.ForeignKey(
                        name: "FK_ChargeComponentRequest_ChargeComponents_ComponentsId",
                        column: x => x.ComponentsId,
                        principalTable: "ChargeComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChargeComponentRequest_Requests_RequestsId",
                        column: x => x.RequestsId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SlagBasicity1 = table.Column<double>(type: "double precision", nullable: false),
                    SlagBasicity2 = table.Column<double>(type: "double precision", nullable: false),
                    SlagBasicity3 = table.Column<double>(type: "double precision", nullable: false),
                    SlagBasicityKulikov = table.Column<double>(type: "double precision", nullable: false),
                    SlagOut = table.Column<double>(type: "double precision", nullable: false),
                    MaterialCons = table.Column<double>(type: "double precision", nullable: false),
                    TotalAglo = table.Column<double>(type: "double precision", nullable: false),
                    PropAglo23 = table.Column<double>(type: "double precision", nullable: false),
                    PropAglo4 = table.Column<double>(type: "double precision", nullable: false),
                    PropSsgpo = table.Column<double>(type: "double precision", nullable: false),
                    PropLeb = table.Column<double>(type: "double precision", nullable: false),
                    PropKach = table.Column<double>(type: "double precision", nullable: false),
                    PropMix = table.Column<double>(type: "double precision", nullable: false),
                    PropOre = table.Column<double>(type: "double precision", nullable: false),
                    PropWeldSlag = table.Column<double>(type: "double precision", nullable: false),
                    PropBfAddict = table.Column<double>(type: "double precision", nullable: false),
                    PropMinInclude = table.Column<double>(type: "double precision", nullable: false),
                    Viscosity_1400 = table.Column<double>(type: "double precision", nullable: false),
                    Viscosity_1450 = table.Column<double>(type: "double precision", nullable: false),
                    Viscosity_1500 = table.Column<double>(type: "double precision", nullable: false),
                    Viscosity_1550 = table.Column<double>(type: "double precision", nullable: false),
                    Temp_7_Puaz = table.Column<double>(type: "double precision", nullable: false),
                    Gradient_7_25 = table.Column<double>(type: "double precision", nullable: false),
                    Gradient_1400_1500 = table.Column<double>(type: "double precision", nullable: false),
                    SlagTemperature = table.Column<double>(type: "double precision", nullable: false),
                    SlagTemperature_25Puaz = table.Column<double>(type: "double precision", nullable: false),
                    CurrSlagViscosity = table.Column<double>(type: "double precision", nullable: false),
                    BalSlagMass = table.Column<double>(type: "double precision", nullable: false),
                    CaOBalSlagMass = table.Column<double>(type: "double precision", nullable: false),
                    TotalSInOre = table.Column<double>(type: "double precision", nullable: false),
                    SActivity = table.Column<double>(type: "double precision", nullable: false),
                    SDistribution = table.Column<double>(type: "double precision", nullable: false),
                    SContentInCastIron = table.Column<double>(type: "double precision", nullable: false),
                    CastIronTemp = table.Column<double>(type: "double precision", nullable: false),
                    RequestID = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responses_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChargeComponentRequest_RequestsId",
                table: "ChargeComponentRequest",
                column: "RequestsId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CastIronID",
                table: "Requests",
                column: "CastIronID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CokeID",
                table: "Requests",
                column: "CokeID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SlagID",
                table: "Requests",
                column: "SlagID");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_RequestID",
                table: "Responses",
                column: "RequestID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChargeComponentRequest");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropTable(
                name: "ChargeComponents");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "CastIrons");

            migrationBuilder.DropTable(
                name: "InputCoke");

            migrationBuilder.DropTable(
                name: "Slags");
        }
    }
}
