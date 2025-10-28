using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class BatchNoAndIsBarcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BatchNo",
                table: "Purchase_QC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBarcodeGenerated",
                table: "Purchase_QC",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BatchNo",
                table: "Production_QC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBarcodeGenerated",
                table: "Production_QC",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BatchNo",
                table: "Production_QA",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBarcodeGenerated",
                table: "Production_QA",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchNo",
                table: "Purchase_QC");

            migrationBuilder.DropColumn(
                name: "IsBarcodeGenerated",
                table: "Purchase_QC");

            migrationBuilder.DropColumn(
                name: "BatchNo",
                table: "Production_QC");

            migrationBuilder.DropColumn(
                name: "IsBarcodeGenerated",
                table: "Production_QC");

            migrationBuilder.DropColumn(
                name: "BatchNo",
                table: "Production_QA");

            migrationBuilder.DropColumn(
                name: "IsBarcodeGenerated",
                table: "Production_QA");
        }
    }
}
