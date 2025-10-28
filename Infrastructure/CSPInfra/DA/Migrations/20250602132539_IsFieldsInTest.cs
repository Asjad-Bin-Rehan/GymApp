using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class IsFieldsInTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDispatch",
                table: "Quantitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIncoming",
                table: "Quantitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsQcCritical",
                table: "Quantitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsQcFloor",
                table: "Quantitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsQcLab",
                table: "Quantitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrial",
                table: "Quantitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDispatch",
                table: "Qualitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIncoming",
                table: "Qualitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsQcCritical",
                table: "Qualitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsQcFloor",
                table: "Qualitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsQcLab",
                table: "Qualitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrial",
                table: "Qualitative_Inspection_Mapping",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDispatch",
                table: "Quantitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "IsIncoming",
                table: "Quantitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "IsQcCritical",
                table: "Quantitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "IsQcFloor",
                table: "Quantitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "IsQcLab",
                table: "Quantitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "IsTrial",
                table: "Quantitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "IsDispatch",
                table: "Qualitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "IsIncoming",
                table: "Qualitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "IsQcCritical",
                table: "Qualitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "IsQcFloor",
                table: "Qualitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "IsQcLab",
                table: "Qualitative_Inspection_Mapping");

            migrationBuilder.DropColumn(
                name: "IsTrial",
                table: "Qualitative_Inspection_Mapping");
        }
    }
}
