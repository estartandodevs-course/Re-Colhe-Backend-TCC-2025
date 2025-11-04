using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;

namespace ReColhe.API.Infrastructure;

public class MySqlOrderRepository : IOrderRepository
{
    private readonly ReColheDbContext _context;

    public MySqlOrderRepository(ReColheDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderEntity>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Orders
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<OrderEntity?> GetById(string id, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task Add(OrderEntity order, CancellationToken cancellationToken)
    {
        order.CreatedAt = DateTime.UtcNow;
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(OrderEntity order, CancellationToken cancellationToken)
    {
        order.UpdatedAt = DateTime.UtcNow;
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        var order = await GetById(id, cancellationToken);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<int> GetTotalCount(CancellationToken cancellationToken)
    {
        return await _context.Orders.CountAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalRevenue(CancellationToken cancellationToken)
    {
        return await _context.Orders
            .SumAsync(o => o.TotalAmount, cancellationToken);
    }
}
