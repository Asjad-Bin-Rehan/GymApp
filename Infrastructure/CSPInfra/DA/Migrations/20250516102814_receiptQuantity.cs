using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class receiptQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ReceiptQuantity",
                table: "Purchase_QC",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ReceiptQuantity",
                table: "Production_QC",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ReceiptQuantity",
                table: "Production_QA",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptQuantity",
                table: "Purchase_QC");

            migrationBuilder.DropColumn(
                name: "ReceiptQuantity",
                table: "Production_QC");

            migrationBuilder.DropColumn(
                name: "ReceiptQuantity",
                table: "Production_QA");
        }
    }
}
