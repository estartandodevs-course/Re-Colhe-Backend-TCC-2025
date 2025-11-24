using ReColhe.Domain.Entidades;
using System.Collections.Generic;

namespace ReColhe.Application.Empresas.Listar
{
    public class ListarEmpresasResponse
    {
        public IEnumerable<Empresa> Empresas { get; set; } = new List<Empresa>();
    }
}