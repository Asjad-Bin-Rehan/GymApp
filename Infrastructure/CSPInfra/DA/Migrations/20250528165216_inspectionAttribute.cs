using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class inspectionAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttributeId",
                table: "Inspection_Characteristic",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Inspection_Attribute",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Tag = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspection_Attribute", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_Characteristic_AttributeId",
                table: "Inspection_Characteristic",
                column: "AttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspection_Characteristic_Inspection_Attribute_AttributeId",
                table: "Inspection_Characteristic",
                column: "AttributeId",
                principalTable: "Inspection_Attribute",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspection_Characteristic_Inspection_Attribute_AttributeId",
                table: "Inspection_Characteristic");

            migrationBuilder.DropTable(
                name: "Inspection_Attribute");

            migrationBuilder.DropIndex(
                name: "IX_Inspection_Characteristic_AttributeId",
                table: "Inspection_Characteristic");

            migrationBuilder.DropColumn(
                name: "AttributeId",
                table: "Inspection_Characteristic");
        }
    }
}
