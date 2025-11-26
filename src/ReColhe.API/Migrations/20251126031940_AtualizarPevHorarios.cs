using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReColhe.API.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarPevHorarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorarioFuncionamento",
                table: "Pevs");

            migrationBuilder.AddColumn<string>(
                name: "CloseTime",
                table: "Pevs",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OpenTime",
                table: "Pevs",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OpeningDays",
                table: "Pevs",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "Pevs");

            migrationBuilder.DropColumn(
                name: "OpenTime",
                table: "Pevs");

            migrationBuilder.DropColumn(
                name: "OpeningDays",
                table: "Pevs");

            migrationBuilder.AddColumn<string>(
                name: "HorarioFuncionamento",
                table: "Pevs",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
