using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inspection_Card",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspection_Card", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inspection_Characteristic",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    SingleCriteria = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspection_Characteristic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemCode = table.Column<string>(type: "text", nullable: true),
                    DocNum = table.Column<string>(type: "text", nullable: true),
                    LineNum = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    GroupCode = table.Column<string>(type: "text", nullable: true),
                    U_QACard = table.Column<string>(type: "text", nullable: true),
                    UoMGroupEntry = table.Column<string>(type: "text", nullable: true),
                    IsEnabledForQA = table.Column<bool>(type: "boolean", nullable: true),
                    GroupName = table.Column<string>(type: "text", nullable: true),
                    ManageBatchNumbers = table.Column<string>(type: "text", nullable: true),
                    IsBatch = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Qualitative_Result",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResultDescription = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualitative_Result", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit_Of_Measure",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UoMcode = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit_Of_Measure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inspection_Characteristic_Mapping",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    InspectionCardId = table.Column<string>(type: "text", nullable: true),
                    CharacteristicId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspection_Characteristic_Mapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspection_Characteristic_Mapping_Inspection_Card_Inspectio~",
                        column: x => x.InspectionCardId,
                        principalTable: "Inspection_Card",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inspection_Characteristic_Mapping_Inspection_Characteristic~",
                        column: x => x.CharacteristicId,
                        principalTable: "Inspection_Characteristic",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Item_Inspection_Card",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardDescription = table.Column<string>(type: "text", nullable: true),
                    ItemDescription = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    ItemId = table.Column<string>(type: "text", nullable: true),
                    InspectionCardId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item_Inspection_Card", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Inspection_Card_Inspection_Card_InspectionCardId",
                        column: x => x.InspectionCardId,
                        principalTable: "Inspection_Card",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Item_Inspection_Card_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Item_Sample",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemDescription = table.Column<string>(type: "text", nullable: true),
                    Flexibility = table.Column<bool>(type: "boolean", nullable: true),
                    ItemId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item_Sample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Sample_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Production_QA",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OverallStatus = table.Column<bool>(type: "boolean", nullable: true),
                    IsPerformed = table.Column<bool>(type: "boolean", nullable: true),
                    IsPostedToSap = table.Column<bool>(type: "boolean", nullable: true),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: true),
                    OperatedBy = table.Column<string>(type: "text", nullable: true),
                    Barcode = table.Column<string>(type: "text", nullable: true),
                    InspectionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AnalyzedBy = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    ReportReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReportNextReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReportRemarks = table.Column<string>(type: "text", nullable: true),
                    QcLotNo = table.Column<string>(type: "text", nullable: true),
                    SampleQuantity = table.Column<double>(type: "double precision", nullable: true),
                    InspectionQuantity = table.Column<double>(type: "double precision", nullable: true),
                    DocNo = table.Column<string>(type: "text", nullable: true),
                    DocDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DocType = table.Column<string>(type: "text", nullable: true),
                    DocEntry = table.Column<string>(type: "text", nullable: true),
                    OpenQuantity = table.Column<double>(type: "double precision", nullable: true),
                    ReceiveQuantity = table.Column<double>(type: "double precision", nullable: true),
                    PlannedQuantity = table.Column<double>(type: "double precision", nullable: true),
                    CompletedQuantity = table.Column<double>(type: "double precision", nullable: true),
                    RejectedQuantity = table.Column<double>(type: "double precision", nullable: true),
                    BMRNo = table.Column<string>(type: "text", nullable: true),
                    BMRLoc = table.Column<string>(type: "text", nullable: true),
                    ItemCode = table.Column<string>(type: "text", nullable: true),
                    ItemDescription = table.Column<string>(type: "text", nullable: true),
                    CardCode = table.Column<string>(type: "text", nullable: true),
                    CardName = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Warehouse = table.Column<string>(type: "text", nullable: true),
                    Vendor = table.Column<string>(type: "text", nullable: true),
                    Variant = table.Column<string>(type: "text", nullable: true),
                    Shift = table.Column<string>(type: "text", nullable: true),
                    ItemWeight = table.Column<string>(type: "text", nullable: true),
                    Cavity = table.Column<string>(type: "text", nullable: true),
                    CavityNo = table.Column<double>(type: "double precision", nullable: true),
                    CycleTime = table.Column<double>(type: "double precision", nullable: true),
                    MachineNo = table.Column<string>(type: "text", nullable: true),
                    MouldNo = table.Column<string>(type: "text", nullable: true),
                    UoM = table.Column<string>(type: "text", nullable: true),
                    InventoryUoM = table.Column<string>(type: "text", nullable: true),
                    ProductionOrderStatus = table.Column<string>(type: "text", nullable: true),
                    ItemId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_QA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Production_QA_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Production_QC",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OverallStatus = table.Column<bool>(type: "boolean", nullable: true),
                    IsPerformed = table.Column<bool>(type: "boolean", nullable: true),
                    IsPostedToSap = table.Column<bool>(type: "boolean", nullable: true),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: true),
                    Barcode = table.Column<string>(type: "text", nullable: true),
                    OperatedBy = table.Column<string>(type: "text", nullable: true),
                    InspectionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AnalyzedBy = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    ReportReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReportNextReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReportRemarks = table.Column<string>(type: "text", nullable: true),
                    QcLotNo = table.Column<string>(type: "text", nullable: true),
                    SampleQuantity = table.Column<double>(type: "double precision", nullable: true),
                    InspectionQuantity = table.Column<double>(type: "double precision", nullable: true),
                    DocNo = table.Column<string>(type: "text", nullable: true),
                    DocDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DocType = table.Column<string>(type: "text", nullable: true),
                    DocEntry = table.Column<string>(type: "text", nullable: true),
                    OpenQuantity = table.Column<double>(type: "double precision", nullable: true),
                    ReceiveQuantity = table.Column<double>(type: "double precision", nullable: true),
                    PlannedQuantity = table.Column<double>(type: "double precision", nullable: true),
                    CompletedQuantity = table.Column<double>(type: "double precision", nullable: true),
                    RejectedQuantity = table.Column<double>(type: "double precision", nullable: true),
                    BMRNo = table.Column<string>(type: "text", nullable: true),
                    BMRLoc = table.Column<string>(type: "text", nullable: true),
                    ItemCode = table.Column<string>(type: "text", nullable: true),
                    ItemDescription = table.Column<string>(type: "text", nullable: true),
                    CardCode = table.Column<string>(type: "text", nullable: true),
                    CardName = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Warehouse = table.Column<string>(type: "text", nullable: true),
                    Vendor = table.Column<string>(type: "text", nullable: true),
                    Variant = table.Column<string>(type: "text", nullable: true),
                    Shift = table.Column<string>(type: "text", nullable: true),
                    ItemWeight = table.Column<string>(type: "text", nullable: true),
                    Cavity = table.Column<string>(type: "text", nullable: true),
                    CavityNo = table.Column<double>(type: "double precision", nullable: true),
                    CycleTime = table.Column<double>(type: "double precision", nullable: true),
                    MachineNo = table.Column<string>(type: "text", nullable: true),
                    MouldNo = table.Column<string>(type: "text", nullable: true),
                    UoM = table.Column<string>(type: "text", nullable: true),
                    InventoryUoM = table.Column<string>(type: "text", nullable: true),
                    ProductionOrderStatus = table.Column<string>(type: "text", nullable: true),
                    ItemId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_QC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Production_QC_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Purchase_QC",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OverallStatus = table.Column<bool>(type: "boolean", nullable: true),
                    IsPerformed = table.Column<bool>(type: "boolean", nullable: true),
                    IsPostedToSap = table.Column<bool>(type: "boolean", nullable: true),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: true),
                    Barcode = table.Column<string>(type: "text", nullable: true),
                    OperatedBy = table.Column<string>(type: "text", nullable: true),
                    InspectionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AnalyzedBy = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    ReportReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReportNextReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReportRemarks = table.Column<string>(type: "text", nullable: true),
                    QcLotNo = table.Column<string>(type: "text", nullable: true),
                    SampleQuantity = table.Column<double>(type: "double precision", nullable: true),
                    InspectionQuantity = table.Column<double>(type: "double precision", nullable: true),
                    DocNo = table.Column<string>(type: "text", nullable: true),
                    LineNo = table.Column<int>(type: "integer", nullable: true),
                    DocEntry = table.Column<string>(type: "text", nullable: true),
                    DocDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DocType = table.Column<string>(type: "text", nullable: true),
                    OpenQuantity = table.Column<double>(type: "double precision", nullable: true),
                    ReceiveQuantity = table.Column<double>(type: "double precision", nullable: true),
                    SapQuantity = table.Column<double>(type: "double precision", nullable: true),
                    CompletedQuantity = table.Column<double>(type: "double precision", nullable: true),
                    RejectedQuantity = table.Column<double>(type: "double precision", nullable: true),
                    BMRNo = table.Column<string>(type: "text", nullable: true),
                    BMRLoc = table.Column<string>(type: "text", nullable: true),
                    ItemCode = table.Column<string>(type: "text", nullable: true),
                    ItemDescription = table.Column<string>(type: "text", nullable: true),
                    CardCode = table.Column<string>(type: "text", nullable: true),
                    CardName = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Warehouse = table.Column<string>(type: "text", nullable: true),
                    Vendor = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    LineStatus = table.Column<string>(type: "text", nullable: true),
                    VatGroup = table.Column<string>(type: "text", nullable: true),
                    UoM = table.Column<string>(type: "text", nullable: true),
                    ItemId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase_QC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchase_QC_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Qualitative_Inspection",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemInspectionCardId = table.Column<string>(type: "text", nullable: true),
                    Qualitative_ResultId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualitative_Inspection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Qualitative_Inspection_Item_Inspection_Card_ItemInspectionC~",
                        column: x => x.ItemInspectionCardId,
                        principalTable: "Item_Inspection_Card",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Qualitative_Inspection_Qualitative_Result_Qualitative_Resul~",
                        column: x => x.Qualitative_ResultId,
                        principalTable: "Qualitative_Result",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Quantitative_Inspection",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemInspectionCardId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quantitative_Inspection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quantitative_Inspection_Item_Inspection_Card_ItemInspection~",
                        column: x => x.ItemInspectionCardId,
                        principalTable: "Item_Inspection_Card",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sampling_Range",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LotSizeMin = table.Column<double>(type: "double precision", nullable: true),
                    LotSizeMax = table.Column<double>(type: "double precision", nullable: true),
                    SampleQty = table.Column<double>(type: "double precision", nullable: true),
                    CriticalDefects = table.Column<double>(type: "double precision", nullable: true),
                    MajorDefects = table.Column<double>(type: "double precision", nullable: true),
                    MinorDefects = table.Column<double>(type: "double precision", nullable: true),
                    ItemSampleId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sampling_Range", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sampling_Range_Item_Sample_ItemSampleId",
                        column: x => x.ItemSampleId,
                        principalTable: "Item_Sample",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Production_QA_Cavity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InspectionBy = table.Column<string>(type: "text", nullable: true),
                    InspectionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MouldNo = table.Column<string>(type: "text", nullable: true),
                    IsToggledOn = table.Column<bool>(type: "boolean", nullable: true),
                    IsCavityPassed = table.Column<bool>(type: "boolean", nullable: true),
                    QaId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_QA_Cavity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Production_QA_Cavity_Production_QA_QaId",
                        column: x => x.QaId,
                        principalTable: "Production_QA",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Production_QC_Sample",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InspectionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InspectionBy = table.Column<string>(type: "text", nullable: true),
                    InspectionQuantity = table.Column<int>(type: "integer", nullable: true),
                    IsSamplePassed = table.Column<bool>(type: "boolean", nullable: true),
                    QcId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_QC_Sample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Production_QC_Sample_Production_QC_QcId",
                        column: x => x.QcId,
                        principalTable: "Production_QC",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Log_Post_Sap",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QType = table.Column<string>(type: "text", nullable: true),
                    QCode = table.Column<int>(type: "integer", nullable: true),
                    BMR = table.Column<string>(type: "text", nullable: true),
                    OverallStatus = table.Column<bool>(type: "boolean", nullable: true),
                    IsPerformed = table.Column<bool>(type: "boolean", nullable: true),
                    IsPostedToSap = table.Column<bool>(type: "boolean", nullable: true),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: true),
                    SamplesPassedCount = table.Column<int>(type: "integer", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    InspectionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AnalyzedBy = table.Column<string>(type: "text", nullable: true),
                    InspectionQuantity = table.Column<double>(type: "double precision", nullable: true),
                    SampleQuantity = table.Column<double>(type: "double precision", nullable: true),
                    ReportReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReportNextReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReportRemarks = table.Column<string>(type: "text", nullable: true),
                    QId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log_Post_Sap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Post_Sap_Production_QA_QId",
                        column: x => x.QId,
                        principalTable: "Production_QA",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Log_Post_Sap_Production_QC_QId",
                        column: x => x.QId,
                        principalTable: "Production_QC",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Log_Post_Sap_Purchase_QC_QId",
                        column: x => x.QId,
                        principalTable: "Purchase_QC",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Purchase_QC_Sample",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InspectionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InspectionBy = table.Column<string>(type: "text", nullable: true),
                    InspectionQuantity = table.Column<int>(type: "integer", nullable: true),
                    IsSamplePassed = table.Column<bool>(type: "boolean", nullable: true),
                    QcId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase_QC_Sample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchase_QC_Sample_Purchase_QC_QcId",
                        column: x => x.QcId,
                        principalTable: "Purchase_QC",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Qualitative_Inspection_Mapping",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QualitativeInspectionId = table.Column<string>(type: "text", nullable: true),
                    CharacteristicId = table.Column<string>(type: "text", nullable: true),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualitative_Inspection_Mapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Qualitative_Inspection_Mapping_Inspection_Characteristic_Ch~",
                        column: x => x.CharacteristicId,
                        principalTable: "Inspection_Characteristic",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Qualitative_Inspection_Mapping_Qualitative_Inspection_Quali~",
                        column: x => x.QualitativeInspectionId,
                        principalTable: "Qualitative_Inspection",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Quantitative_Inspection_Mapping",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QuantitativeInspectionId = table.Column<string>(type: "text", nullable: true),
                    CharacteristicId = table.Column<string>(type: "text", nullable: true),
                    UoMId = table.Column<string>(type: "text", nullable: true),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: true),
                    Target = table.Column<double>(type: "double precision", nullable: true),
                    Max = table.Column<double>(type: "double precision", nullable: true),
                    Min = table.Column<double>(type: "double precision", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quantitative_Inspection_Mapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quantitative_Inspection_Mapping_Inspection_Characteristic_C~",
                        column: x => x.CharacteristicId,
                        principalTable: "Inspection_Characteristic",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Quantitative_Inspection_Mapping_Quantitative_Inspection_Qua~",
                        column: x => x.QuantitativeInspectionId,
                        principalTable: "Quantitative_Inspection",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Quantitative_Inspection_Mapping_Unit_Of_Measure_UoMId",
                        column: x => x.UoMId,
                        principalTable: "Unit_Of_Measure",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Log_Sampling_Range",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LotSizeMin = table.Column<double>(type: "double precision", nullable: true),
                    LotSizeMax = table.Column<double>(type: "double precision", nullable: true),
                    SampleQty = table.Column<double>(type: "double precision", nullable: true),
                    CriticalDefects = table.Column<double>(type: "double precision", nullable: true),
                    MajorDefects = table.Column<double>(type: "double precision", nullable: true),
                    MinorDefects = table.Column<double>(type: "double precision", nullable: true),
                    ItemSampleFlexibility = table.Column<bool>(type: "boolean", nullable: true),
                    ItemSampleId = table.Column<string>(type: "text", nullable: true),
                    SamplingRangeId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log_Sampling_Range", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Sampling_Range_Sampling_Range_SamplingRangeId",
                        column: x => x.SamplingRangeId,
                        principalTable: "Sampling_Range",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Log_Production_QA_Cavity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InspectionBy = table.Column<string>(type: "text", nullable: true),
                    InspectionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MouldNo = table.Column<string>(type: "text", nullable: true),
                    IsToggledOn = table.Column<bool>(type: "boolean", nullable: true),
                    IsCavityPassed = table.Column<bool>(type: "boolean", nullable: true),
                    QaId = table.Column<string>(type: "text", nullable: true),
                    CavityId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log_Production_QA_Cavity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Production_QA_Cavity_Production_QA_Cavity_CavityId",
                        column: x => x.CavityId,
                        principalTable: "Production_QA_Cavity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Production_QA_Cavity_Sample",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InspectionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InspectionBy = table.Column<string>(type: "text", nullable: true),
                    InspectionQuantity = table.Column<int>(type: "integer", nullable: true),
                    IsSamplePassed = table.Column<bool>(type: "boolean", nullable: true),
                    CavityId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_QA_Cavity_Sample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Production_QA_Cavity_Sample_Production_QA_Cavity_CavityId",
                        column: x => x.CavityId,
                        principalTable: "Production_QA_Cavity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Qualitative_Result_Pass_Status",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IntCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QualitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QualitativeResultId = table.Column<string>(type: "text", nullable: true),
                    IsPassed = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualitative_Result_Pass_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Qualitative_Result_Pass_Status_Qualitative_Inspection_Mappi~",
                        column: x => x.QualitativeInspectionMappingId,
                        principalTable: "Qualitative_Inspection_Mapping",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Qualitative_Result_Pass_Status_Qualitative_Result_Qualitati~",
                        column: x => x.QualitativeResultId,
                        principalTable: "Qualitative_Result",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Production_QC_Sample_Result",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QcSampleId = table.Column<string>(type: "text", nullable: true),
                    QualitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QuantitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QualitativeResultId = table.Column<string>(type: "text", nullable: true),
                    IsQualitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    QuantitativeResult = table.Column<double>(type: "double precision", nullable: true),
                    IsQuantitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_QC_Sample_Result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Production_QC_Sample_Result_Production_QC_Sample_QcSampleId",
                        column: x => x.QcSampleId,
                        principalTable: "Production_QC_Sample",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Production_QC_Sample_Result_Qualitative_Inspection_Mapping_~",
                        column: x => x.QualitativeInspectionMappingId,
                        principalTable: "Qualitative_Inspection_Mapping",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Production_QC_Sample_Result_Qualitative_Result_QualitativeR~",
                        column: x => x.QualitativeResultId,
                        principalTable: "Qualitative_Result",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Production_QC_Sample_Result_Quantitative_Inspection_Mapping~",
                        column: x => x.QuantitativeInspectionMappingId,
                        principalTable: "Quantitative_Inspection_Mapping",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Purchase_QC_Sample_Result",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QcSampleId = table.Column<string>(type: "text", nullable: true),
                    QualitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QuantitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QualitativeResultId = table.Column<string>(type: "text", nullable: true),
                    IsQualitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    QuantitativeResult = table.Column<double>(type: "double precision", nullable: true),
                    IsQuantitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase_QC_Sample_Result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchase_QC_Sample_Result_Purchase_QC_Sample_QcSampleId",
                        column: x => x.QcSampleId,
                        principalTable: "Purchase_QC_Sample",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchase_QC_Sample_Result_Qualitative_Inspection_Mapping_Qu~",
                        column: x => x.QualitativeInspectionMappingId,
                        principalTable: "Qualitative_Inspection_Mapping",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchase_QC_Sample_Result_Qualitative_Result_QualitativeRes~",
                        column: x => x.QualitativeResultId,
                        principalTable: "Qualitative_Result",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchase_QC_Sample_Result_Quantitative_Inspection_Mapping_Q~",
                        column: x => x.QuantitativeInspectionMappingId,
                        principalTable: "Quantitative_Inspection_Mapping",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Production_QA_Cavity_Sample_Result",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QaCavitySampleId = table.Column<string>(type: "text", nullable: true),
                    QualitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QuantitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QualitativeResultId = table.Column<string>(type: "text", nullable: true),
                    IsQualitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    QuantitativeResult = table.Column<double>(type: "double precision", nullable: true),
                    IsQuantitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_QA_Cavity_Sample_Result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Production_QA_Cavity_Sample_Result_Production_QA_Cavity_Sam~",
                        column: x => x.QaCavitySampleId,
                        principalTable: "Production_QA_Cavity_Sample",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Production_QA_Cavity_Sample_Result_Qualitative_Inspection_M~",
                        column: x => x.QualitativeInspectionMappingId,
                        principalTable: "Qualitative_Inspection_Mapping",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Production_QA_Cavity_Sample_Result_Qualitative_Result_Quali~",
                        column: x => x.QualitativeResultId,
                        principalTable: "Qualitative_Result",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Production_QA_Cavity_Sample_Result_Quantitative_Inspection_~",
                        column: x => x.QuantitativeInspectionMappingId,
                        principalTable: "Quantitative_Inspection_Mapping",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Log_Production_QC_Sample_Result",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QcSampleId = table.Column<string>(type: "text", nullable: true),
                    QualitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QuantitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QcSampleResultId = table.Column<string>(type: "text", nullable: true),
                    QualitativeResultId = table.Column<string>(type: "text", nullable: true),
                    IsQualitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    QuantitativeResult = table.Column<double>(type: "double precision", nullable: true),
                    IsQuantitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    IsSamplePassed = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log_Production_QC_Sample_Result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Production_QC_Sample_Result_Production_QC_Sample_Result~",
                        column: x => x.QcSampleResultId,
                        principalTable: "Production_QC_Sample_Result",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Log_Purchase_QC_Sample_Result",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QcSampleId = table.Column<string>(type: "text", nullable: true),
                    QualitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QuantitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QcSampleResultId = table.Column<string>(type: "text", nullable: true),
                    QualitativeResultId = table.Column<string>(type: "text", nullable: true),
                    IsQualitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    QuantitativeResult = table.Column<double>(type: "double precision", nullable: true),
                    IsQuantitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    IsSamplePassed = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log_Purchase_QC_Sample_Result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Purchase_QC_Sample_Result_Purchase_QC_Sample_Result_QcS~",
                        column: x => x.QcSampleResultId,
                        principalTable: "Purchase_QC_Sample_Result",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Log_Production_QA_Cavity_Sample_Result",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QaCavitySampleId = table.Column<string>(type: "text", nullable: true),
                    QualitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QuantitativeInspectionMappingId = table.Column<string>(type: "text", nullable: true),
                    QaCavitySampleResultId = table.Column<string>(type: "text", nullable: true),
                    QualitativeResultId = table.Column<string>(type: "text", nullable: true),
                    IsQualitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    QuantitativeResult = table.Column<double>(type: "double precision", nullable: true),
                    IsQuantitativeResultPassed = table.Column<bool>(type: "boolean", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    IsSamplePassed = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log_Production_QA_Cavity_Sample_Result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Production_QA_Cavity_Sample_Result_Production_QA_Cavity~",
                        column: x => x.QaCavitySampleResultId,
                        principalTable: "Production_QA_Cavity_Sample_Result",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_Characteristic_Mapping_CharacteristicId",
                table: "Inspection_Characteristic_Mapping",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_Characteristic_Mapping_InspectionCardId",
                table: "Inspection_Characteristic_Mapping",
                column: "InspectionCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Inspection_Card_InspectionCardId",
                table: "Item_Inspection_Card",
                column: "InspectionCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Inspection_Card_ItemId",
                table: "Item_Inspection_Card",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Sample_ItemId",
                table: "Item_Sample",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Log_Post_Sap_QId",
                table: "Log_Post_Sap",
                column: "QId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_Production_QA_Cavity_CavityId",
                table: "Log_Production_QA_Cavity",
                column: "CavityId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_Production_QA_Cavity_Sample_Result_QaCavitySampleResult~",
                table: "Log_Production_QA_Cavity_Sample_Result",
                column: "QaCavitySampleResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_Production_QC_Sample_Result_QcSampleResultId",
                table: "Log_Production_QC_Sample_Result",
                column: "QcSampleResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_Purchase_QC_Sample_Result_QcSampleResultId",
                table: "Log_Purchase_QC_Sample_Result",
                column: "QcSampleResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_Sampling_Range_SamplingRangeId",
                table: "Log_Sampling_Range",
                column: "SamplingRangeId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QA_ItemId",
                table: "Production_QA",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QA_Cavity_QaId",
                table: "Production_QA_Cavity",
                column: "QaId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QA_Cavity_Sample_CavityId",
                table: "Production_QA_Cavity_Sample",
                column: "CavityId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QA_Cavity_Sample_Result_QaCavitySampleId",
                table: "Production_QA_Cavity_Sample_Result",
                column: "QaCavitySampleId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QA_Cavity_Sample_Result_QualitativeInspectionMap~",
                table: "Production_QA_Cavity_Sample_Result",
                column: "QualitativeInspectionMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QA_Cavity_Sample_Result_QualitativeResultId",
                table: "Production_QA_Cavity_Sample_Result",
                column: "QualitativeResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QA_Cavity_Sample_Result_QuantitativeInspectionMa~",
                table: "Production_QA_Cavity_Sample_Result",
                column: "QuantitativeInspectionMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QC_ItemId",
                table: "Production_QC",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QC_Sample_QcId",
                table: "Production_QC_Sample",
                column: "QcId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QC_Sample_Result_QcSampleId",
                table: "Production_QC_Sample_Result",
                column: "QcSampleId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QC_Sample_Result_QualitativeInspectionMappingId",
                table: "Production_QC_Sample_Result",
                column: "QualitativeInspectionMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QC_Sample_Result_QualitativeResultId",
                table: "Production_QC_Sample_Result",
                column: "QualitativeResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Production_QC_Sample_Result_QuantitativeInspectionMappingId",
                table: "Production_QC_Sample_Result",
                column: "QuantitativeInspectionMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_QC_ItemId",
                table: "Purchase_QC",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_QC_Sample_QcId",
                table: "Purchase_QC_Sample",
                column: "QcId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_QC_Sample_Result_QcSampleId",
                table: "Purchase_QC_Sample_Result",
                column: "QcSampleId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_QC_Sample_Result_QualitativeInspectionMappingId",
                table: "Purchase_QC_Sample_Result",
                column: "QualitativeInspectionMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_QC_Sample_Result_QualitativeResultId",
                table: "Purchase_QC_Sample_Result",
                column: "QualitativeResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_QC_Sample_Result_QuantitativeInspectionMappingId",
                table: "Purchase_QC_Sample_Result",
                column: "QuantitativeInspectionMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Qualitative_Inspection_ItemInspectionCardId",
                table: "Qualitative_Inspection",
                column: "ItemInspectionCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Qualitative_Inspection_Qualitative_ResultId",
                table: "Qualitative_Inspection",
                column: "Qualitative_ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Qualitative_Inspection_Mapping_CharacteristicId",
                table: "Qualitative_Inspection_Mapping",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_Qualitative_Inspection_Mapping_QualitativeInspectionId",
                table: "Qualitative_Inspection_Mapping",
                column: "QualitativeInspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Qualitative_Result_Pass_Status_QualitativeInspectionMapping~",
                table: "Qualitative_Result_Pass_Status",
                column: "QualitativeInspectionMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Qualitative_Result_Pass_Status_QualitativeResultId",
                table: "Qualitative_Result_Pass_Status",
                column: "QualitativeResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Quantitative_Inspection_ItemInspectionCardId",
                table: "Quantitative_Inspection",
                column: "ItemInspectionCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quantitative_Inspection_Mapping_CharacteristicId",
                table: "Quantitative_Inspection_Mapping",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_Quantitative_Inspection_Mapping_QuantitativeInspectionId",
                table: "Quantitative_Inspection_Mapping",
                column: "QuantitativeInspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Quantitative_Inspection_Mapping_UoMId",
                table: "Quantitative_Inspection_Mapping",
                column: "UoMId");

            migrationBuilder.CreateIndex(
                name: "IX_Sampling_Range_ItemSampleId",
                table: "Sampling_Range",
                column: "ItemSampleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inspection_Characteristic_Mapping");

            migrationBuilder.DropTable(
                name: "Log_Post_Sap");

            migrationBuilder.DropTable(
                name: "Log_Production_QA_Cavity");

            migrationBuilder.DropTable(
                name: "Log_Production_QA_Cavity_Sample_Result");

            migrationBuilder.DropTable(
                name: "Log_Production_QC_Sample_Result");

            migrationBuilder.DropTable(
                name: "Log_Purchase_QC_Sample_Result");

            migrationBuilder.DropTable(
                name: "Log_Sampling_Range");

            migrationBuilder.DropTable(
                name: "Qualitative_Result_Pass_Status");

            migrationBuilder.DropTable(
                name: "Production_QA_Cavity_Sample_Result");

            migrationBuilder.DropTable(
                name: "Production_QC_Sample_Result");

            migrationBuilder.DropTable(
                name: "Purchase_QC_Sample_Result");

            migrationBuilder.DropTable(
                name: "Sampling_Range");

            migrationBuilder.DropTable(
                name: "Production_QA_Cavity_Sample");

            migrationBuilder.DropTable(
                name: "Production_QC_Sample");

            migrationBuilder.DropTable(
                name: "Purchase_QC_Sample");

            migrationBuilder.DropTable(
                name: "Qualitative_Inspection_Mapping");

            migrationBuilder.DropTable(
                name: "Quantitative_Inspection_Mapping");

            migrationBuilder.DropTable(
                name: "Item_Sample");

            migrationBuilder.DropTable(
                name: "Production_QA_Cavity");

            migrationBuilder.DropTable(
                name: "Production_QC");

            migrationBuilder.DropTable(
                name: "Purchase_QC");

            migrationBuilder.DropTable(
                name: "Qualitative_Inspection");

            migrationBuilder.DropTable(
                name: "Inspection_Characteristic");

            migrationBuilder.DropTable(
                name: "Quantitative_Inspection");

            migrationBuilder.DropTable(
                name: "Unit_Of_Measure");

            migrationBuilder.DropTable(
                name: "Production_QA");

            migrationBuilder.DropTable(
                name: "Qualitative_Result");

            migrationBuilder.DropTable(
                name: "Item_Inspection_Card");

            migrationBuilder.DropTable(
                name: "Inspection_Card");

            migrationBuilder.DropTable(
                name: "Item");
        }
    }
}
