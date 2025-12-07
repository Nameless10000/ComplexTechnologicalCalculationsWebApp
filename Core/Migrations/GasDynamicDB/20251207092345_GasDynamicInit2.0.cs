#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Migrations.GasDynamicDB
{
    /// <inheritdoc />
    public partial class GasDynamicInit20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AglomContents");

            migrationBuilder.DropTable(
                name: "CalculationOutputs");

            migrationBuilder.DropTable(
                name: "KoksContents");

            migrationBuilder.DropTable(
                name: "OkatContents");

            migrationBuilder.DropTable(
                name: "AglomOutputs");

            migrationBuilder.DropTable(
                name: "BlastFurnanceOutputs");

            migrationBuilder.DropTable(
                name: "CalculationInputs");

            migrationBuilder.DropTable(
                name: "BlastParameters");

            migrationBuilder.DropTable(
                name: "CarbonBalances");

            migrationBuilder.DropTable(
                name: "ChargeAndPackings");

            migrationBuilder.DropTable(
                name: "FurnaceGeometryOutputs");

            migrationBuilder.DropTable(
                name: "HearthGases");

            migrationBuilder.DropTable(
                name: "HydrodynamicsLowers");

            migrationBuilder.DropTable(
                name: "HydrodynamicsUppers");

            migrationBuilder.DropTable(
                name: "IntermediateGases1000");

            migrationBuilder.DropTable(
                name: "MaterialConsumptions");

            migrationBuilder.DropTable(
                name: "ThermalParameters");

            migrationBuilder.DropTable(
                name: "TopGases");

            migrationBuilder.DropTable(
                name: "AglomInputs");

            migrationBuilder.DropTable(
                name: "BlastFurnaceInputs");

            migrationBuilder.DropTable(
                name: "CompositionParameters");

            migrationBuilder.DropTable(
                name: "FuelAndBlastParameters");

            migrationBuilder.DropTable(
                name: "FurnaceGeometryInputs");

            migrationBuilder.DropTable(
                name: "MaterialProperties");

            migrationBuilder.DropTable(
                name: "ProductionParameters");

            migrationBuilder.DropTable(
                name: "ThermalAndPressureParameters");

            migrationBuilder.CreateTable(
                name: "CalculationModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SerializedInput = table.Column<string>(type: "text", nullable: false),
                    SerializedOutput = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    IsPreset = table.Column<bool>(type: "boolean", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_CalculationModels", x => x.Id); });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculationModels");

            migrationBuilder.CreateTable(
                name: "AglomInputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AglomInputs", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "AglomOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AglomPorosity = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    OkatPorosity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AglomOutputs", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "BlastParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Rashod_Dut_Koks = table.Column<double>(type: "double precision", nullable: false),
                    Rashod_Dut_Krit = table.Column<double>(type: "double precision", nullable: false),
                    Rashod_Dut_Minut = table.Column<double>(type: "double precision", nullable: false),
                    Rashod_Dut_Prir_Gaz = table.Column<double>(type: "double precision", nullable: false),
                    Rashod_Dut_Sum = table.Column<double>(type: "double precision", nullable: false),
                    Rashod_Dut_Udeln = table.Column<double>(type: "double precision", nullable: false),
                    Reinolds = table.Column<double>(type: "double precision", nullable: false),
                    Speed_Dut_Furm = table.Column<double>(type: "double precision", nullable: false),
                    Vyazkost_Dut = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_BlastParameters", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "CarbonBalances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    C_Input = table.Column<double>(type: "double precision", nullable: false),
                    C_Out_Chugun = table.Column<double>(type: "double precision", nullable: false),
                    C_Out_Furm = table.Column<double>(type: "double precision", nullable: false),
                    C_Out_Metan = table.Column<double>(type: "double precision", nullable: false),
                    C_Out_Vosstan = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_CarbonBalances", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "ChargeAndPackings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Diam_Aglo = table.Column<double>(type: "double precision", nullable: false),
                    Diam_Koks = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Massa_Nasyp_Shihta = table.Column<double>(type: "double precision", nullable: false),
                    Porozn_Aglo = table.Column<double>(type: "double precision", nullable: false),
                    Porozn_Koks = table.Column<double>(type: "double precision", nullable: false),
                    Porozn_Okat = table.Column<double>(type: "double precision", nullable: false),
                    Porozn_Sloy_Korrekt = table.Column<double>(type: "double precision", nullable: false),
                    Shihta_Diam_Verh = table.Column<double>(type: "double precision", nullable: false),
                    Shihta_Dolya_Aglo = table.Column<double>(type: "double precision", nullable: false),
                    Shihta_Dolya_Koks = table.Column<double>(type: "double precision", nullable: false),
                    Shihta_Dolya_Okat = table.Column<double>(type: "double precision", nullable: false),
                    Shihta_Porozn_Verh = table.Column<double>(type: "double precision", nullable: false),
                    Volume_Aglo_1chugun = table.Column<double>(type: "double precision", nullable: false),
                    Volume_Koks_1chugun = table.Column<double>(type: "double precision", nullable: false),
                    Volume_Okat_1chugun = table.Column<double>(type: "double precision", nullable: false),
                    Volume_Sum_1chugun = table.Column<double>(type: "double precision", nullable: false),
                    Volume_Udeln_Koks = table.Column<double>(type: "double precision", nullable: false),
                    Volume_Udeln_Nasadzka = table.Column<double>(type: "double precision", nullable: false),
                    Volume_Udeln_Ost = table.Column<double>(type: "double precision", nullable: false),
                    Volume_Udeln_Shlak = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_ChargeAndPackings", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "CompositionParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    C_chugun = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Fe_chugun = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Mn_chugun = table.Column<double>(type: "double precision", nullable: false),
                    P_chugun = table.Column<double>(type: "double precision", nullable: false),
                    S_shlak = table.Column<double>(type: "double precision", nullable: false),
                    Si_chugun = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_CompositionParameters", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "FuelAndBlastParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    C_neletuch = table.Column<double>(type: "double precision", nullable: false),
                    C_prir_gaz = table.Column<double>(type: "double precision", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    H2_prir_gaz = table.Column<double>(type: "double precision", nullable: false),
                    Kislorod_dut = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Poteri_dut = table.Column<double>(type: "double precision", nullable: false),
                    Rashod_dut = table.Column<double>(type: "double precision", nullable: false),
                    Stepen_CO = table.Column<double>(type: "double precision", nullable: false),
                    Stepen_pryamogo_vost = table.Column<double>(type: "double precision", nullable: false),
                    Stepen_vodorod = table.Column<double>(type: "double precision", nullable: false),
                    Udeln_koks = table.Column<double>(type: "double precision", nullable: false),
                    Udeln_prir_gaz = table.Column<double>(type: "double precision", nullable: false),
                    Vlazhn_dut = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_FuelAndBlastParameters", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "FurnaceGeometryInputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Diam_furm = table.Column<double>(type: "double precision", nullable: false),
                    Diam_gorn = table.Column<double>(type: "double precision", nullable: false),
                    Diam_koloshnik = table.Column<double>(type: "double precision", nullable: false),
                    Diam_raspar = table.Column<double>(type: "double precision", nullable: false),
                    Dlina_furm = table.Column<double>(type: "double precision", nullable: false),
                    Height_koloshnik = table.Column<double>(type: "double precision", nullable: false),
                    Height_raspar = table.Column<double>(type: "double precision", nullable: false),
                    Height_shahta = table.Column<double>(type: "double precision", nullable: false),
                    Height_zaplechik = table.Column<double>(type: "double precision", nullable: false),
                    Kolvo_furm = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Uroven_zasypi = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_FurnaceGeometryInputs", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "FurnaceGeometryOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Diam_Niz = table.Column<double>(type: "double precision", nullable: false),
                    Diam_Verh = table.Column<double>(type: "double precision", nullable: false),
                    Height_Aktiv = table.Column<double>(type: "double precision", nullable: false),
                    Height_Shihta_Niz = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    S_Sech_Niz = table.Column<double>(type: "double precision", nullable: false),
                    Shihta_Height_Verh = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_FurnaceGeometryOutputs", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "HearthGases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Furmgaz_CO = table.Column<double>(type: "double precision", nullable: false),
                    Furmgaz_CO_V = table.Column<double>(type: "double precision", nullable: false),
                    Furmgaz_H2 = table.Column<double>(type: "double precision", nullable: false),
                    Furmgaz_H2_V = table.Column<double>(type: "double precision", nullable: false),
                    Furmgaz_Koks = table.Column<double>(type: "double precision", nullable: false),
                    Furmgaz_N2 = table.Column<double>(type: "double precision", nullable: false),
                    Furmgaz_N2_V = table.Column<double>(type: "double precision", nullable: false),
                    Furmgaz_Prir_Gaz = table.Column<double>(type: "double precision", nullable: false),
                    Furmgaz_Sum = table.Column<double>(type: "double precision", nullable: false),
                    Furmgaz_Udeln = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Temp_Sredn_Niz = table.Column<double>(type: "double precision", nullable: false),
                    Temp_Teor = table.Column<double>(type: "double precision", nullable: false),
                    Teplosod_Furmgaz = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_HearthGases", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "HydrodynamicsLowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    Davlen_Izb_1000 = table.Column<double>(type: "double precision", nullable: false),
                    Davlen_Niz = table.Column<double>(type: "double precision", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Koef_An = table.Column<double>(type: "double precision", nullable: false),
                    Koef_Proporc_Dut_Filtr_Niz = table.Column<double>(type: "double precision", nullable: false),
                    Koef_Soprot_Niz = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Perepad_Niz_Dolya = table.Column<double>(type: "double precision", nullable: false),
                    Perepad_Niz_Itog = table.Column<double>(type: "double precision", nullable: false),
                    Poteri_Furm = table.Column<double>(type: "double precision", nullable: false),
                    Speed_Filtr_Niz = table.Column<double>(type: "double precision", nullable: false),
                    Tren_Koef = table.Column<double>(type: "double precision", nullable: false),
                    Tren_Sum = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_HydrodynamicsLowers", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "HydrodynamicsUppers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    Davlen_Verh = table.Column<double>(type: "double precision", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Koef_Av = table.Column<double>(type: "double precision", nullable: false),
                    Koef_Proporc_Dut_Filtr_Verh = table.Column<double>(type: "double precision", nullable: false),
                    Koef_Soprot_Verh = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Perepad_Davlen = table.Column<double>(type: "double precision", nullable: false),
                    Speed_Filtr_Gorn = table.Column<double>(type: "double precision", nullable: false),
                    Speed_Filtr_Koloshnik = table.Column<double>(type: "double precision", nullable: false),
                    Speed_Filtr_Raspar = table.Column<double>(type: "double precision", nullable: false),
                    Speed_Filtr_Verh = table.Column<double>(type: "double precision", nullable: false),
                    Speed_Real_Koloshnik = table.Column<double>(type: "double precision", nullable: false),
                    Speed_Real_Raspar = table.Column<double>(type: "double precision", nullable: false),
                    Speed_Real_Verh = table.Column<double>(type: "double precision", nullable: false),
                    Stepen_Urav = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_HydrodynamicsUppers", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "IntermediateGases1000",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Domengaz_CO_1000 = table.Column<double>(type: "double precision", nullable: false),
                    Domengaz_H2_1000 = table.Column<double>(type: "double precision", nullable: false),
                    Domengaz_N2_1000 = table.Column<double>(type: "double precision", nullable: false),
                    Domengaz_Plotn_1000 = table.Column<double>(type: "double precision", nullable: false),
                    Domengaz_Rashod_1000 = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Volume_CO_1000 = table.Column<double>(type: "double precision", nullable: false),
                    Volume_CO_Pvost = table.Column<double>(type: "double precision", nullable: false),
                    Volume_H2_1000 = table.Column<double>(type: "double precision", nullable: false),
                    Volume_N2_1000 = table.Column<double>(type: "double precision", nullable: false),
                    Volume_Sum_1000 = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_IntermediateGases1000", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "MaterialConsumptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Rashod_Prir_Gaz = table.Column<double>(type: "double precision", nullable: false),
                    Udeln_Aglo = table.Column<double>(type: "double precision", nullable: false),
                    Udeln_Izvest = table.Column<double>(type: "double precision", nullable: false),
                    Udeln_Koks_1000 = table.Column<double>(type: "double precision", nullable: false),
                    Udeln_Okat = table.Column<double>(type: "double precision", nullable: false),
                    Udeln_Sum = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_MaterialConsumptions", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "MaterialProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Massa_aglo = table.Column<double>(type: "double precision", nullable: false),
                    Massa_koks_kg = table.Column<double>(type: "double precision", nullable: false),
                    Massa_okat = table.Column<double>(type: "double precision", nullable: false),
                    Plotn_shlak = table.Column<double>(type: "double precision", nullable: false),
                    Porozn_aglo = table.Column<double>(type: "double precision", nullable: false),
                    Porozn_okat = table.Column<double>(type: "double precision", nullable: false),
                    Poteri_prokalivanie = table.Column<double>(type: "double precision", nullable: false),
                    Udeln_vyhod_shlak = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_MaterialProperties", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "ProductionParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Dolya_okat = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Proizvodit_chugun = table.Column<double>(type: "double precision", nullable: false),
                    Stepen_urav_krit = table.Column<double>(type: "double precision", nullable: false),
                    Udeln_izvest = table.Column<double>(type: "double precision", nullable: false),
                    Udeln_zhelezorud = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_ProductionParameters", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "ThermalAndPressureParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    Davlen_izb_dut = table.Column<double>(type: "double precision", nullable: false),
                    Davlen_izb_koloshnik_gaz = table.Column<double>(type: "double precision", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Perepad_niz = table.Column<double>(type: "double precision", nullable: false),
                    Perepad_verh = table.Column<double>(type: "double precision", nullable: false),
                    Temp_dut = table.Column<double>(type: "double precision", nullable: false),
                    Temp_koks = table.Column<double>(type: "double precision", nullable: false),
                    Temp_koloshnik_gaz = table.Column<double>(type: "double precision", nullable: false),
                    Teploemk_koks = table.Column<double>(type: "double precision", nullable: false),
                    Teplota_nepoln_koks = table.Column<double>(type: "double precision", nullable: false),
                    Teplota_nepoln_prir_gaz = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_ThermalAndPressureParameters", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "ThermalParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Temp_Verh = table.Column<double>(type: "double precision", nullable: false),
                    Teploemk_2atom = table.Column<double>(type: "double precision", nullable: false),
                    Teploemk_Voda = table.Column<double>(type: "double precision", nullable: false),
                    Teplosod_Dut = table.Column<double>(type: "double precision", nullable: false),
                    Teplosod_Koks = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_ThermalParameters", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "TopGases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Kolgaz_CH4 = table.Column<double>(type: "double precision", nullable: false),
                    Kolgaz_CO = table.Column<double>(type: "double precision", nullable: false),
                    Kolgaz_CO2 = table.Column<double>(type: "double precision", nullable: false),
                    Kolgaz_H2 = table.Column<double>(type: "double precision", nullable: false),
                    Kolgaz_Minut = table.Column<double>(type: "double precision", nullable: false),
                    Kolgaz_N2 = table.Column<double>(type: "double precision", nullable: false),
                    Kolgaz_Plotn = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    Udeln_Kolgaz = table.Column<double>(type: "double precision", nullable: false),
                    Volume_CH4_Kolgaz = table.Column<double>(type: "double precision", nullable: false),
                    Volume_CO2_Izvest = table.Column<double>(type: "double precision", nullable: false),
                    Volume_CO2_Kolgaz = table.Column<double>(type: "double precision", nullable: false),
                    Volume_CO2_Kvost = table.Column<double>(type: "double precision", nullable: false),
                    Volume_CO_Kolgaz = table.Column<double>(type: "double precision", nullable: false),
                    Volume_H2_Kolgaz = table.Column<double>(type: "double precision", nullable: false),
                    Volume_N2_Kolgaz = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_TopGases", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "AglomContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AglomInputModelId = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FractionPercentage = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    MinFractionSize = table.Column<double>(type: "double precision", nullable: false),
                    Porosity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AglomContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AglomContents_AglomInputs_AglomInputModelId",
                        column: x => x.AglomInputModelId,
                        principalTable: "AglomInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KoksContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AglomInputModelId = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FractionPercentage = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    MinFractionSize = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoksContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KoksContents_AglomInputs_AglomInputModelId",
                        column: x => x.AglomInputModelId,
                        principalTable: "AglomInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OkatContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AglomInputModelId = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FractionPercentage = table.Column<double>(type: "double precision", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true),
                    MinFractionSize = table.Column<double>(type: "double precision", nullable: false),
                    Porosity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OkatContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OkatContents_AglomInputs_AglomInputModelId",
                        column: x => x.AglomInputModelId,
                        principalTable: "AglomInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlastFurnaceInputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompositionId = table.Column<int>(type: "integer", nullable: false),
                    FuelAndBlastId = table.Column<int>(type: "integer", nullable: false),
                    GeometryId = table.Column<int>(type: "integer", nullable: false),
                    MaterialsId = table.Column<int>(type: "integer", nullable: false),
                    ProductionId = table.Column<int>(type: "integer", nullable: false),
                    ThermalAndPressureId = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlastFurnaceInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlastFurnaceInputs_CompositionParameters_CompositionId",
                        column: x => x.CompositionId,
                        principalTable: "CompositionParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnaceInputs_FuelAndBlastParameters_FuelAndBlastId",
                        column: x => x.FuelAndBlastId,
                        principalTable: "FuelAndBlastParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnaceInputs_FurnaceGeometryInputs_GeometryId",
                        column: x => x.GeometryId,
                        principalTable: "FurnaceGeometryInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnaceInputs_MaterialProperties_MaterialsId",
                        column: x => x.MaterialsId,
                        principalTable: "MaterialProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnaceInputs_ProductionParameters_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "ProductionParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnaceInputs_ThermalAndPressureParameters_ThermalAndP~",
                        column: x => x.ThermalAndPressureId,
                        principalTable: "ThermalAndPressureParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlastFurnanceOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BlastParametersId = table.Column<int>(type: "integer", nullable: false),
                    CarbonBalanceId = table.Column<int>(type: "integer", nullable: false),
                    ChargeAndPackingId = table.Column<int>(type: "integer", nullable: false),
                    FurnaceGeometryId = table.Column<int>(type: "integer", nullable: false),
                    HearthGasId = table.Column<int>(type: "integer", nullable: false),
                    HydrodynamicsLowerId = table.Column<int>(type: "integer", nullable: false),
                    HydrodynamicsUpperId = table.Column<int>(type: "integer", nullable: false),
                    IntermediateGas1000Id = table.Column<int>(type: "integer", nullable: false),
                    MaterialConsumptionId = table.Column<int>(type: "integer", nullable: false),
                    ThermalParametersId = table.Column<int>(type: "integer", nullable: false),
                    TopGasId = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlastFurnanceOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlastFurnanceOutputs_BlastParameters_BlastParametersId",
                        column: x => x.BlastParametersId,
                        principalTable: "BlastParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnanceOutputs_CarbonBalances_CarbonBalanceId",
                        column: x => x.CarbonBalanceId,
                        principalTable: "CarbonBalances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnanceOutputs_ChargeAndPackings_ChargeAndPackingId",
                        column: x => x.ChargeAndPackingId,
                        principalTable: "ChargeAndPackings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnanceOutputs_FurnaceGeometryOutputs_FurnaceGeometry~",
                        column: x => x.FurnaceGeometryId,
                        principalTable: "FurnaceGeometryOutputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnanceOutputs_HearthGases_HearthGasId",
                        column: x => x.HearthGasId,
                        principalTable: "HearthGases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnanceOutputs_HydrodynamicsLowers_HydrodynamicsLower~",
                        column: x => x.HydrodynamicsLowerId,
                        principalTable: "HydrodynamicsLowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnanceOutputs_HydrodynamicsUppers_HydrodynamicsUpper~",
                        column: x => x.HydrodynamicsUpperId,
                        principalTable: "HydrodynamicsUppers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnanceOutputs_IntermediateGases1000_IntermediateGas1~",
                        column: x => x.IntermediateGas1000Id,
                        principalTable: "IntermediateGases1000",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnanceOutputs_MaterialConsumptions_MaterialConsumpti~",
                        column: x => x.MaterialConsumptionId,
                        principalTable: "MaterialConsumptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnanceOutputs_ThermalParameters_ThermalParametersId",
                        column: x => x.ThermalParametersId,
                        principalTable: "ThermalParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastFurnanceOutputs_TopGases_TopGasId",
                        column: x => x.TopGasId,
                        principalTable: "TopGases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalculationInputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AglomInputId = table.Column<int>(type: "integer", nullable: false),
                    BlastFurnaceInputId = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsPreset = table.Column<bool>(type: "boolean", nullable: false),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculationInputs_AglomInputs_AglomInputId",
                        column: x => x.AglomInputId,
                        principalTable: "AglomInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalculationInputs_BlastFurnaceInputs_BlastFurnaceInputId",
                        column: x => x.BlastFurnaceInputId,
                        principalTable: "BlastFurnaceInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalculationOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AglomOutputId = table.Column<int>(type: "integer", nullable: false),
                    BlastFurnanceOutputId = table.Column<int>(type: "integer", nullable: false),
                    RequestModelId = table.Column<int>(type: "integer", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorID = table.Column<int>(type: "integer", nullable: false),
                    DeletedByID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEditorID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculationOutputs_AglomOutputs_AglomOutputId",
                        column: x => x.AglomOutputId,
                        principalTable: "AglomOutputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalculationOutputs_BlastFurnanceOutputs_BlastFurnanceOutput~",
                        column: x => x.BlastFurnanceOutputId,
                        principalTable: "BlastFurnanceOutputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalculationOutputs_CalculationInputs_RequestModelId",
                        column: x => x.RequestModelId,
                        principalTable: "CalculationInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AglomContents_AglomInputModelId",
                table: "AglomContents",
                column: "AglomInputModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnaceInputs_CompositionId",
                table: "BlastFurnaceInputs",
                column: "CompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnaceInputs_FuelAndBlastId",
                table: "BlastFurnaceInputs",
                column: "FuelAndBlastId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnaceInputs_GeometryId",
                table: "BlastFurnaceInputs",
                column: "GeometryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnaceInputs_MaterialsId",
                table: "BlastFurnaceInputs",
                column: "MaterialsId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnaceInputs_ProductionId",
                table: "BlastFurnaceInputs",
                column: "ProductionId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnaceInputs_ThermalAndPressureId",
                table: "BlastFurnaceInputs",
                column: "ThermalAndPressureId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnanceOutputs_BlastParametersId",
                table: "BlastFurnanceOutputs",
                column: "BlastParametersId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnanceOutputs_CarbonBalanceId",
                table: "BlastFurnanceOutputs",
                column: "CarbonBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnanceOutputs_ChargeAndPackingId",
                table: "BlastFurnanceOutputs",
                column: "ChargeAndPackingId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnanceOutputs_FurnaceGeometryId",
                table: "BlastFurnanceOutputs",
                column: "FurnaceGeometryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnanceOutputs_HearthGasId",
                table: "BlastFurnanceOutputs",
                column: "HearthGasId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnanceOutputs_HydrodynamicsLowerId",
                table: "BlastFurnanceOutputs",
                column: "HydrodynamicsLowerId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnanceOutputs_HydrodynamicsUpperId",
                table: "BlastFurnanceOutputs",
                column: "HydrodynamicsUpperId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnanceOutputs_IntermediateGas1000Id",
                table: "BlastFurnanceOutputs",
                column: "IntermediateGas1000Id");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnanceOutputs_MaterialConsumptionId",
                table: "BlastFurnanceOutputs",
                column: "MaterialConsumptionId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnanceOutputs_ThermalParametersId",
                table: "BlastFurnanceOutputs",
                column: "ThermalParametersId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastFurnanceOutputs_TopGasId",
                table: "BlastFurnanceOutputs",
                column: "TopGasId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationInputs_AglomInputId",
                table: "CalculationInputs",
                column: "AglomInputId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationInputs_BlastFurnaceInputId",
                table: "CalculationInputs",
                column: "BlastFurnaceInputId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationOutputs_AglomOutputId",
                table: "CalculationOutputs",
                column: "AglomOutputId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationOutputs_BlastFurnanceOutputId",
                table: "CalculationOutputs",
                column: "BlastFurnanceOutputId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationOutputs_RequestModelId",
                table: "CalculationOutputs",
                column: "RequestModelId");

            migrationBuilder.CreateIndex(
                name: "IX_KoksContents_AglomInputModelId",
                table: "KoksContents",
                column: "AglomInputModelId");

            migrationBuilder.CreateIndex(
                name: "IX_OkatContents_AglomInputModelId",
                table: "OkatContents",
                column: "AglomInputModelId");
        }
    }
}