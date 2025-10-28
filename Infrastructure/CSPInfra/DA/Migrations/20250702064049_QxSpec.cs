using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class QxSpec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuantitativeSpec",
                table: "Quantitative_Inspection_Mapping",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QualitativeSpec",
                table: "Qualitative_Inspection_Mapping",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantitativeSpec",
                table: "Quantitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "QualitativeSpec",
                table: "Qualitative_Inspection_Mapping");
        }
    }
}
