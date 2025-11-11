using Microsoft.AspNetCore.Mvc;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ApoioReclamacaoController : ControllerBase
{
    private readonly IApoioReclamacaoRepository _repo;

    public ApoioReclamacaoController(IApoioReclamacaoRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("reclamacao/{reclamacaoId}")]
    public async Task<IActionResult> ListarPorReclamacao(int reclamacaoId)
    {
        var lista = await _repo.ListarPorReclamacaoAsync(reclamacaoId);
        return Ok(lista);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(int usuarioId, int reclamacaoId)
    {
        var apoio = await _repo.BuscarAsync(usuarioId, reclamacaoId);
        if (apoio == null)
            return NotFound();
        return Ok(apoio);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] ApoioReclamacao input)
    {
        await _repo.CriarAsync(input);
        return Ok(input);
    }

    [HttpDelete]
    public async Task<IActionResult> Remover(int usuarioId, int reclamacaoId)
    {
        await _repo.RemoverAsync(usuarioId, reclamacaoId);
        return NoContent();
    }
}
