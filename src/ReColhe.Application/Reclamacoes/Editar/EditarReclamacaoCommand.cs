using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;

namespace ReColhe.Application.Reclamacoes.Editar
{
    public class EditarReclamacaoCommand : IRequest<CommandResponse<Unit>>
    {
        public int ReclamacaoId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public StatusReclamacao Status { get; set; }
        public int CategoriaId { get; set; }
    }
}