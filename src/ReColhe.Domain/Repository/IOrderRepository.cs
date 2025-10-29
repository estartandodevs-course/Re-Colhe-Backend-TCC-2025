using ReColhe.Domain.Entidades;

namespace ReColhe.Domain.Repository;

public interface IOrderRepository
{
    Task<IEnumerable<OrderEntity>> GetAll(CancellationToken cancellationToken);
    Task<OrderEntity?> GetById(string id, CancellationToken cancellationToken);
    Task Add(OrderEntity order, CancellationToken cancellationToken);
    Task Update(OrderEntity order, CancellationToken cancellationToken);
    Task Delete(string id, CancellationToken cancellationToken);
    Task<int> GetTotalCount(CancellationToken cancellationToken);
    Task<decimal> GetTotalRevenue(CancellationToken cancellationToken);
}

