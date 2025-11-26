using ReColhe.Domain.Entidades;

namespace ReColhe.Domain.Repository
{
    public interface IEmpresaRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<IEnumerable<Empresa>> ListarAsync();
        Task<Empresa?> BuscarPorIdAsync(int empresaId);
        Task<Empresa?> BuscarPorCnpjAsync(string cnpj);
        Task CriarAsync(Empresa empresa);
        Task AtualizarAsync(Empresa empresa);
        Task RemoverAsync(Empresa empresa);
        Task<bool> CnpjJaUtilizadoAsync(string cnpj, int? empresaIdIgnorar = null);
    }
}