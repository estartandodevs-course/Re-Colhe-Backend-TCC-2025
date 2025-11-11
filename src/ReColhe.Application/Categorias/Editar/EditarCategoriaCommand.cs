using MediatR;
using ReColhe.Application.Mediator;
using System.Text.Json.Serialization;

namespace ReColhe.Application.Categorias.Editar
{
    public class EditarCategoriaCommand : IRequest<CommandResponse<Unit>>
    {
        [JsonIgnore]
        public int CategoriaId { get; private set; }
        public string Nome { get; set; }
        
        public void SetCategoriaId(int id) => CategoriaId = id;
    }
}