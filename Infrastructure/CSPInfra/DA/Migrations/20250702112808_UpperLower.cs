using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class UpperLower : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LowerLimit",
                table: "Quantitative_Inspection_Mapping",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UpperLimit",
                table: "Quantitative_Inspection_Mapping",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LowerLimit",
                table: "Quantitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "UpperLimit",
                table: "Quantitative_Inspection_Mapping");
        }
    }
}
