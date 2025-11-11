using Microsoft.AspNetCore.Mvc;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaRepository _repo;

    public CategoriaController(ICategoriaRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var lista = await _repo.ListarAsync();
        return Ok(lista);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var categoria = await _repo.BuscarPorIdAsync(id);
        if (categoria == null)
            return NotFound();
        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] Categoria input)
    {
        await _repo.CriarAsync(input);
        return CreatedAtAction(nameof(BuscarPorId), new { id = input.CategoriaId }, input);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Categoria input)
    {
        if (id != input.CategoriaId)
            return BadRequest();
        await _repo.AtualizarAsync(input);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        await _repo.RemoverAsync(id);
        return NoContent();
    }
}
