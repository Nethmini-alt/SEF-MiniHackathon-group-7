namespace EWasteManagement.API.Models
{
    public class PickupRequest
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string ItemType { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public string Location { get; set; } = string.Empty;

        public DateTime PreferredDate { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}