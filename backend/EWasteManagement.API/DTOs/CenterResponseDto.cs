namespace EWasteManagement.API.DTOs
{
    /// <summary>
    /// Response DTO for returning center data to the client
    /// </summary>
    public class CenterResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string AcceptedItemType { get; set; } = string.Empty;
        public string OpeningHours { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}