using ReColhe.Domain.Entidades;

namespace ReColhe.Application.CasosDeUso.Order.Criar;

public class CriarOrderCommand
{
    public string OrderId { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<string> Items { get; set; } = new();
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public OrderEntity ToEntity()
    {
        return new OrderEntity
        {
            Id = OrderId,
            CustomerId = CustomerId,
            TotalAmount = TotalAmount,
            Items = Items,
            OrderDate = OrderDate
        };
    }
}

