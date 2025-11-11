using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using ReColhe.API.Infrastructure;
using ReColhe.Domain;
public class CategoriaRepository : ICategoriaRepository
{
    private readonly ApplicationDbContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public CategoriaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> ListarAsync()
    {
        return await _context.Categorias.ToListAsync();
    }

    public async Task<Categoria> BuscarPorIdAsync(int categoriaId)
    {
        return await _context.Categorias.FindAsync(categoriaId);
    }

    public async Task CriarAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
    }

    public async Task AtualizarAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
    }

    public async Task RemoverAsync(Categoria categoria)
    {
        _context.Categorias.Remove(categoria);
    }

    public async Task<Categoria> ObterPorIdAsync(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }
}