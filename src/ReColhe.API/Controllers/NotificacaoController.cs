using Microsoft.AspNetCore.Mvc;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class NotificacaoController : ControllerBase
{
    private readonly INotificacaoRepository _repo;

    public NotificacaoController(INotificacaoRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> ListarPorUsuario(int usuarioId)
    {
        var lista = await _repo.ListarPorUsuarioAsync(usuarioId);
        return Ok(lista);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var notificacao = await _repo.BuscarPorIdAsync(id);
        if (notificacao == null)
            return NotFound();
        return Ok(notificacao);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] Notificacao input)
    {
        await _repo.CriarAsync(input);
        return Ok(input);
    }

    [HttpPut("marcar-como-lida")]
    public async Task<IActionResult> MarcarComoLida(int usuarioId, int notificacaoId)
    {
        await _repo.MarcarComoLidaAsync(usuarioId, notificacaoId);
        return NoContent();
    }
}
