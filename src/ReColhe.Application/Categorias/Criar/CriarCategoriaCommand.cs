using MediatR;
using ReColhe.Application.Mediator;

namespace ReColhe.Application.Categorias.Criar
{
    public class CriarCategoriaCommand : IRequest<CommandResponse<CriarCategoriaCommandResponse>>
    {
        public string Nome { get; set; }
        
    }
}