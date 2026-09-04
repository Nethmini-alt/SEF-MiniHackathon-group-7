namespace EWasteManagement.API.Models;

public class Center
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string District { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string AcceptedItemType { get; set; } = string.Empty;

    public string OpeningHours { get; set; } = string.Empty;
}