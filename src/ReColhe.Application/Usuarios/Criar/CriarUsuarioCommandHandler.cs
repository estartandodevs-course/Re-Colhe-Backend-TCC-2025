using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using MediatR;
using System.Net;

namespace ReColhe.Application.Usuarios.Criar
{
    public class CriarUsuarioCommandHandler : IRequestHandler<CriarUsuarioCommand, CommandResponse<CriarUsuarioCommandResponse>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public CriarUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<CommandResponse<CriarUsuarioCommandResponse>> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
        {
            if (!request.Validar())
            {
                return CommandResponse<CriarUsuarioCommandResponse>.ErroValidacao(request.ResultadoDasValidacoes!);
            }

            try
            {
                var emailJaUtilizado = await _usuarioRepository.EmailJaUtilizado(request.Email);

                if (emailJaUtilizado)
                {
                    return CommandResponse<CriarUsuarioCommandResponse>.AdicionarErro(
                        statusCode: HttpStatusCode.Conflict,
                        mensagem: "O email informado já está em uso."
                    );
                }

                // Validação específica para colaboradores
                if (request.TipoUsuarioId == 2)
                {
                    var empresaExiste = await _usuarioRepository.EmpresaExiste(request.EmpresaId!.Value);
                    if (!empresaExiste)
                    {
                        return CommandResponse<CriarUsuarioCommandResponse>.AdicionarErro(
                            statusCode: HttpStatusCode.BadRequest,
                            mensagem: "A empresa informada não existe."
                        );
                    }
                }

                // Criar nossa entidade Usuario
                var usuario = new Usuario
                {
                    Nome = request.Nome,
                    Email = request.Email,
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
                    TipoUsuarioId = request.TipoUsuarioId,
                    EmpresaId = request.EmpresaId,
                    Cep = request.Cep
                };

                // Salvar o usuario no banco de dados
                await _usuarioRepository.Adicionar(usuario);
                await _usuarioRepository.UnitOfWork.CommitAsync(cancellationToken);

                var response = new CriarUsuarioCommandResponse(usuario.UsuarioId);

                // Retornar uma resposta pro usuário 
                return CommandResponse<CriarUsuarioCommandResponse>.Sucesso(response, statusCode: HttpStatusCode.Created);

            }
            catch (Exception ex)
            {
                return CommandResponse<CriarUsuarioCommandResponse>.ErroCritico(
                    mensagem: $"Ocorreu um erro ao criar o usuário: {ex.Message}"
                );
            }
        }
    }
}