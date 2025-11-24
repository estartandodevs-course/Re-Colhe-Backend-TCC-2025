using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using ReColhe.API.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReColhe.API.Infrastructure.Repository
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly ApplicationDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public EmpresaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empresa>> ListarAsync()
        {
            return await _context.Empresas
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Empresa?> BuscarPorIdAsync(int empresaId)
        {
            return await _context.Empresas
                .FirstOrDefaultAsync(e => e.EmpresaId == empresaId);
        }

        public async Task<Empresa?> BuscarPorCnpjAsync(string cnpj)
        {
            return await _context.Empresas
                .FirstOrDefaultAsync(e => e.Cnpj == cnpj);
        }

        public async Task CriarAsync(Empresa empresa)
        {
            await _context.Empresas.AddAsync(empresa);
        }

        public async Task AtualizarAsync(Empresa empresa)
        {
            _context.Empresas.Update(empresa);
        }

        public async Task RemoverAsync(Empresa empresa)
        {
            _context.Empresas.Remove(empresa);
        }

        public async Task<bool> CnpjJaUtilizadoAsync(string cnpj, int? empresaIdIgnorar = null)
        {
            var query = _context.Empresas.Where(e => e.Cnpj == cnpj);

            if (empresaIdIgnorar.HasValue)
            {
                query = query.Where(e => e.EmpresaId != empresaIdIgnorar.Value);
            }

            return await query.AnyAsync();
        }
    }
}