using System.ComponentModel.DataAnnotations;

namespace EWasteManagement.API.DTOs;

public class UpdatePickupRequestDto
{
    [Required(ErrorMessage = "Item type is required")]
    public string ItemType { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Location is required")]
    public string Location { get; set; } = string.Empty;

    public DateTime? PreferredDate { get; set; }
}