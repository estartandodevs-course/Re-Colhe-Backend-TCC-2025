using ReColhe.Application.CasosDeUso.Order.Criar;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using ReColhe.API.Models;

namespace ReColhe.API.Controllers;

/// <summary>
/// Controller for managing orders
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderRepository _orderRepository;

    public OrdersController(ILogger<OrdersController> logger, IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Get all orders
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderEntity>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderEntity>>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving all orders");
        var orders = await _orderRepository.GetAll(cancellationToken);
        return Ok(orders);
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderEntity), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderEntity>> GetById(string id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving order {OrderId}", id);
        
        var order = await _orderRepository.GetById(id, cancellationToken);
        if (order == null)
        {
            return NotFound(new { message = $"Order with ID '{id}' not found" });
        }

        return Ok(order);
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] OrderDto orderDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new CriarOrderCommand
        {
            OrderId = orderDto.OrderId,
            CustomerId = orderDto.CustomerId,
            TotalAmount = orderDto.TotalAmount,
            Items = orderDto.Items,
            OrderDate = orderDto.OrderDate
        };

        var handler = new CriarOrderCommandHandler(_orderRepository);
        var response = await handler.Handle(command, cancellationToken);

        if (!response.Success)
        {
            return Conflict(new { message = response.Message });
        }

        return CreatedAtAction(nameof(GetById), new { id = response.Order!.Id }, response.Order);
    }

    /// <summary>
    /// Update an existing order
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(OrderEntity), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderEntity>> Update(string id, [FromBody] OrderDto orderDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingOrder = await _orderRepository.GetById(id, cancellationToken);
        if (existingOrder == null)
        {
            return NotFound(new { message = $"Order with ID '{id}' not found" });
        }

        existingOrder.CustomerId = orderDto.CustomerId;
        existingOrder.TotalAmount = orderDto.TotalAmount;
        existingOrder.Items = orderDto.Items;
        existingOrder.OrderDate = orderDto.OrderDate;
        existingOrder.UpdatedAt = DateTime.UtcNow;

        await _orderRepository.Update(existingOrder, cancellationToken);

        _logger.LogInformation("Order {OrderId} updated successfully", id);
        return Ok(existingOrder);
    }

    /// <summary>
    /// Delete an order
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting order {OrderId}", id);

        var existingOrder = await _orderRepository.GetById(id, cancellationToken);
        if (existingOrder == null)
        {
            return NotFound(new { message = $"Order with ID '{id}' not found" });
        }

        await _orderRepository.Delete(id, cancellationToken);

        _logger.LogInformation("Order {OrderId} deleted successfully", id);
        return NoContent();
    }

    /// <summary>
    /// Get order statistics
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetStats(CancellationToken cancellationToken)
    {
        var totalOrders = await _orderRepository.GetTotalCount(cancellationToken);
        var totalRevenue = await _orderRepository.GetTotalRevenue(cancellationToken);
        
        var stats = new
        {
            totalOrders,
            totalRevenue,
            averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0,
            lastUpdated = DateTime.UtcNow
        };

        _logger.LogInformation("Retrieving order statistics");
        return Ok(stats);
    }
}
