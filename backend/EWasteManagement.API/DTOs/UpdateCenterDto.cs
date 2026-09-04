using System.ComponentModel.DataAnnotations;

namespace EWasteManagement.API.DTOs
{
    /// <summary>
    /// Data Transfer Object for updating an existing collection center
    /// </summary>
    public class UpdateCenterDto
    {
        [Required(ErrorMessage = "Center name is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Center name must be between 2 and 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "District is required")]
        [StringLength(100, ErrorMessage = "District cannot exceed 100 characters")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Accepted item types are required")]
        [StringLength(200, ErrorMessage = "Accepted item types cannot exceed 200 characters")]
        public string AcceptedItemType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opening hours are required")]
        [StringLength(100, ErrorMessage = "Opening hours cannot exceed 100 characters")]
        public string OpeningHours { get; set; } = string.Empty;
    }
}