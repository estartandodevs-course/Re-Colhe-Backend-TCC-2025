using Microsoft.AspNetCore.Mvc;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ReclamacaoController : ControllerBase
{
    private readonly IReclamacaoRepository _repo;

    public ReclamacaoController(IReclamacaoRepository repo)
    {
        _repo = repo;
    }

    // GET: api/reclamacao
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var lista = await _repo.ListarAsync();
        return Ok(lista);
    }

    // GET: api/reclamacao/Id
    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var reclamacao = await _repo.BuscarPorIdAsync(id);
        if (reclamacao == null)
            return NotFound();
        return Ok(reclamacao);
    }

    // POST: api/reclamacao
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] Reclamacao input)
    {
        await _repo.CriarAsync(input);
        return CreatedAtAction(nameof(BuscarPorId), new { id = input.CategoriaId }, input);
    }

    // PUT: api/reclamacao/Id
    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(int id, [FromBody] Reclamacao input)
    {
        if (id != input.CategoriaId)
            return BadRequest();
        await _repo.EditarAsync(input);
        return NoContent();
    }

    // DELETE: api/reclamacao/Id
    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        await _repo.ExcluirAsync(id);
        return NoContent();
    }
}
