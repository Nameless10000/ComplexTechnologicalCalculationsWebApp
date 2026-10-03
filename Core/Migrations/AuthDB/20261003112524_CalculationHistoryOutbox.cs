using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations.AuthDB
{
    /// <inheritdoc />
    public partial class CalculationHistoryOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculationHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Module = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RequestId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CorrelationId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RequestJson = table.Column<string>(type: "jsonb", nullable: false),
                    ResponseJson = table.Column<string>(type: "jsonb", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SourceCalculationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculationHistory_CalculationHistory_SourceCalculationId",
                        column: x => x.SourceCalculationId,
                        principalTable: "CalculationHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalculationOutbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CalculationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Payload = table.Column<string>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NextRetryAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Attempts = table.Column<int>(type: "integer", nullable: false),
                    LastError = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationOutbox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculationOutbox_CalculationHistory_CalculationId",
                        column: x => x.CalculationId,
                        principalTable: "CalculationHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalculationHistory_SourceCalculationId",
                table: "CalculationHistory",
                column: "SourceCalculationId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationHistory_UserId_Module_CreatedAt",
                table: "CalculationHistory",
                columns: new[] { "UserId", "Module", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CalculationOutbox_CalculationId",
                table: "CalculationOutbox",
                column: "CalculationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalculationOutbox_ProcessedAt_NextRetryAt",
                table: "CalculationOutbox",
                columns: new[] { "ProcessedAt", "NextRetryAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculationOutbox");

            migrationBuilder.DropTable(
                name: "CalculationHistory");
        }
    }
}
