using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;

namespace ReColhe.API.Infrastructure;

public class InMemoryOrderRepository : IOrderRepository
{
    private static readonly Dictionary<string, OrderEntity> _orders = new();

    public Task<IEnumerable<OrderEntity>> GetAll(CancellationToken cancellationToken)
    {
        return Task.FromResult<IEnumerable<OrderEntity>>(_orders.Values);
    }

    public Task<OrderEntity?> GetById(string id, CancellationToken cancellationToken)
    {
        _orders.TryGetValue(id, out var order);
        return Task.FromResult(order);
    }

    public Task Add(OrderEntity order, CancellationToken cancellationToken)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task Update(OrderEntity order, CancellationToken cancellationToken)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task Delete(string id, CancellationToken cancellationToken)
    {
        _orders.Remove(id);
        return Task.CompletedTask;
    }

    public Task<int> GetTotalCount(CancellationToken cancellationToken)
    {
        return Task.FromResult(_orders.Count);
    }

    public Task<decimal> GetTotalRevenue(CancellationToken cancellationToken)
    {
        var revenue = _orders.Values.Sum(o => o.TotalAmount);
        return Task.FromResult(revenue);
    }
}

