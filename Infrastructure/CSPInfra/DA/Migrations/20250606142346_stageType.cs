using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class stageType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StageType",
                table: "Purchase_QC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StageType",
                table: "Production_QC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StageType",
                table: "Production_QA",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageType",
                table: "Purchase_QC");

            migrationBuilder.DropColumn(
                name: "StageType",
                table: "Production_QC");

            migrationBuilder.DropColumn(
                name: "StageType",
                table: "Production_QA");
        }
    }
}
