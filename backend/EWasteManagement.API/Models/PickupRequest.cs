using System.ComponentModel.DataAnnotations;

namespace EWasteManagement.API.Models;

public class PickupRequest
{
    public int Id { get; set; }

    [Required]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string ItemType { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    public string Location { get; set; } = string.Empty;

    public DateTime? PreferredDate { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}