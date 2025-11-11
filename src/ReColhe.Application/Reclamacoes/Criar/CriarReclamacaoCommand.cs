using MediatR;
using ReColhe.Application.Mediator;

namespace ReColhe.Application.Reclamacoes.Criar
{
    public class CriarReclamacaoCommand : IRequest<CommandResponse<CriarReclamacaoCommandResponse>>
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int AutorId { get; set; }
        public int CategoriaId { get; set; }
    }
}