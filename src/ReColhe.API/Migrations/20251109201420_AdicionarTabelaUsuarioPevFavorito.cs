using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReColhe.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTabelaUsuarioPevFavorito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioPevFavoritos",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    PevId = table.Column<int>(type: "int", nullable: false),
                    DataAdicao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPevFavoritos", x => new { x.UsuarioId, x.PevId });
                    table.ForeignKey(
                        name: "FK_UsuarioPevFavoritos_Pevs_PevId",
                        column: x => x.PevId,
                        principalTable: "Pevs",
                        principalColumn: "PevId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioPevFavoritos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPevFavoritos_PevId",
                table: "UsuarioPevFavoritos",
                column: "PevId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioPevFavoritos");
        }
    }
}
