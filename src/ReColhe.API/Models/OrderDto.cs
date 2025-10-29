using System.ComponentModel.DataAnnotations;

namespace ReColhe.API.Models;

public class OrderDto
{
    [Required(ErrorMessage = "OrderId is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "OrderId must be between 1 and 50 characters")]
    public string OrderId { get; set; } = string.Empty;

    [Required(ErrorMessage = "CustomerId is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "CustomerId must be between 1 and 100 characters")]
    public string CustomerId { get; set; } = string.Empty;

    [Range(0.01, 999999.99, ErrorMessage = "TotalAmount must be between 0.01 and 999999.99")]
    public decimal TotalAmount { get; set; }

    [Required(ErrorMessage = "Items are required")]
    [MinLength(1, ErrorMessage = "Order must contain at least one item")]
    [MaxLength(100, ErrorMessage = "Order cannot contain more than 100 items")]
    public List<string> Items { get; set; } = new();

    [DataType(DataType.DateTime)]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
}
