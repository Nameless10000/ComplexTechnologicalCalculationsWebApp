using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations.SlagModeDB
{
    /// <inheritdoc />
    public partial class HistoryEventDeduplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HistoryEventId",
                table: "Responses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Responses_HistoryEventId",
                table: "Responses",
                column: "HistoryEventId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Responses_HistoryEventId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "HistoryEventId",
                table: "Responses");
        }
    }
}
