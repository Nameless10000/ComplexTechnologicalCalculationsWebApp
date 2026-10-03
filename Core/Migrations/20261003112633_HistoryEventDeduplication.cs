using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class HistoryEventDeduplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HistoryEventId",
                table: "AglomRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AglomRequests_HistoryEventId",
                table: "AglomRequests",
                column: "HistoryEventId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AglomRequests_HistoryEventId",
                table: "AglomRequests");

            migrationBuilder.DropColumn(
                name: "HistoryEventId",
                table: "AglomRequests");
        }
    }
}
