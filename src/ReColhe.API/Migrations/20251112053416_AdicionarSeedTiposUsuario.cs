using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReColhe.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarSeedTiposUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Empresas",
                columns: new[] { "EmpresaId", "Cnpj", "EmailContato", "NomeFantasia", "TelefoneContato" },
                values: new object[] { 1, "00.000.000/0001-00", "contato@empresa.com", "Empresa Teste", "99999-9999" });

            migrationBuilder.InsertData(
                table: "TiposUsuario",
                columns: new[] { "TipoUsuarioId", "Nome" },
                values: new object[,]
                {
                    { 1, "Comum" },
                    { 2, "Colaborador" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Empresas",
                keyColumn: "EmpresaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposUsuario",
                keyColumn: "TipoUsuarioId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposUsuario",
                keyColumn: "TipoUsuarioId",
                keyValue: 2);
        }
    }
}
