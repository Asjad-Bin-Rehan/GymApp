using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class packsizeAndProductionDateShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ProductionDate",
                table: "Production_QC",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionShift",
                table: "Production_QC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProductionDate",
                table: "Production_QA",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionShift",
                table: "Production_QA",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PackSize",
                table: "Item",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductionDate",
                table: "Production_QC");

            migrationBuilder.DropColumn(
                name: "ProductionShift",
                table: "Production_QC");

            migrationBuilder.DropColumn(
                name: "ProductionDate",
                table: "Production_QA");

            migrationBuilder.DropColumn(
                name: "ProductionShift",
                table: "Production_QA");

            migrationBuilder.DropColumn(
                name: "PackSize",
                table: "Item");
        }
    }
}
