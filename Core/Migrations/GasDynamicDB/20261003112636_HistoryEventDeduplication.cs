using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations.GasDynamicDB
{
    /// <inheritdoc />
    public partial class HistoryEventDeduplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HistoryEventId",
                table: "CalculationModels",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalculationModels_HistoryEventId",
                table: "CalculationModels",
                column: "HistoryEventId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CalculationModels_HistoryEventId",
                table: "CalculationModels");

            migrationBuilder.DropColumn(
                name: "HistoryEventId",
                table: "CalculationModels");
        }
    }
}
