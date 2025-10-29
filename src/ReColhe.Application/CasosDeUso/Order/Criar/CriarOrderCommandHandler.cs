using ReColhe.Domain.Repository;

namespace ReColhe.Application.CasosDeUso.Order.Criar;

public class CriarOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public CriarOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CriarOrderCommandResponse> Handle(CriarOrderCommand command, CancellationToken cancellationToken)
    {
        // Verificar se a order já existe
        var existingOrder = await _orderRepository.GetById(command.OrderId, cancellationToken);
        if (existingOrder != null)
        {
            return new CriarOrderCommandResponse
            {
                Success = false,
                Message = $"Order with ID '{command.OrderId}' already exists"
            };
        }

        // Criar a entidade
        var order = command.ToEntity();

        // Salvar
        await _orderRepository.Add(order, cancellationToken);

        return new CriarOrderCommandResponse
        {
            Success = true,
            Order = order
        };
    }
}

