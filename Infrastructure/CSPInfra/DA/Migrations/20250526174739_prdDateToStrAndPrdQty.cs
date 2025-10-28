using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class prdDateToStrAndPrdQty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductionDate",
                table: "Production_QC",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProducedQuantity",
                table: "Production_QC",
                type: "double precision",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductionDate",
                table: "Production_QA",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProducedQuantity",
                table: "Production_QA",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProducedQuantity",
                table: "Production_QC");

            migrationBuilder.DropColumn(
                name: "ProducedQuantity",
                table: "Production_QA");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProductionDate",
                table: "Production_QC",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProductionDate",
                table: "Production_QA",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
