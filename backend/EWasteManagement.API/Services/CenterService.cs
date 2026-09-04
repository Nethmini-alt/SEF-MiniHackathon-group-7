using Microsoft.EntityFrameworkCore;
using EWasteManagement.API.Data;
using EWasteManagement.API.Models;
using EWasteManagement.API.DTOs;

namespace EWasteManagement.API.Services
{
    /// <summary>
    /// Service layer for collection center business logic
    /// </summary>
    public interface ICenterService
    {
        Task<IEnumerable<CenterResponseDto>> GetAllCentersAsync();
        Task<CenterResponseDto> GetCenterByIdAsync(int id);
        Task<CenterResponseDto> CreateCenterAsync(CreateCenterDto createDto);
        Task<CenterResponseDto> UpdateCenterAsync(int id, UpdateCenterDto updateDto);
        Task<bool> DeleteCenterAsync(int id);
        Task<bool> CenterExistsAsync(int id);
    }

    public class CenterService : ICenterService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CenterService> _logger;

        public CenterService(AppDbContext context, ILogger<CenterService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all collection centers
        /// </summary>
        public async Task<IEnumerable<CenterResponseDto>> GetAllCentersAsync()
        {
            try
            {
                var centers = await _context.Centers
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                return centers.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all centers");
                throw;
            }
        }

        /// <summary>
        /// Get a specific center by ID
        /// </summary>
        public async Task<CenterResponseDto> GetCenterByIdAsync(int id)
        {
            try
            {
                var center = await _context.Centers.FindAsync(id);
                if (center == null)
                {
                    throw new KeyNotFoundException($"Center with ID {id} not found");
                }

                return MapToResponseDto(center);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving center with ID {id}");
                throw;
            }
        }

        /// <summary>
        /// Create a new collection center
        /// </summary>
        public async Task<CenterResponseDto> CreateCenterAsync(CreateCenterDto createDto)
        {
            try
            {
                // Check if a center with the same name already exists
                var existingCenter = await _context.Centers
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == createDto.Name.ToLower());

                if (existingCenter != null)
                {
                    throw new InvalidOperationException($"A center with the name '{createDto.Name}' already exists");
                }

                var center = new Center
                {
                    Name = createDto.Name.Trim(),
                    District = createDto.District.Trim(),
                    Address = createDto.Address.Trim(),
                    Phone = createDto.Phone.Trim(),
                    AcceptedItemType = createDto.AcceptedItemType.Trim(),
                    OpeningHours = createDto.OpeningHours.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                _context.Centers.Add(center);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Center created successfully: {center.Name} (ID: {center.Id})");
                return MapToResponseDto(center);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating center");
                throw;
            }
        }

        /// <summary>
        /// Update an existing collection center
        /// </summary>
        public async Task<CenterResponseDto> UpdateCenterAsync(int id, UpdateCenterDto updateDto)
        {
            try
            {
                var center = await _context.Centers.FindAsync(id);
                if (center == null)
                {
                    throw new KeyNotFoundException($"Center with ID {id} not found");
                }

                // Check if another center with the same name exists (excluding current center)
                var existingCenter = await _context.Centers
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == updateDto.Name.ToLower() && c.Id != id);

                if (existingCenter != null)
                {
                    throw new InvalidOperationException($"A center with the name '{updateDto.Name}' already exists");
                }

                // Update properties
                center.Name = updateDto.Name.Trim();
                center.District = updateDto.District.Trim();
                center.Address = updateDto.Address.Trim();
                center.Phone = updateDto.Phone.Trim();
                center.AcceptedItemType = updateDto.AcceptedItemType.Trim();
                center.OpeningHours = updateDto.OpeningHours.Trim();
                center.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Center updated successfully: {center.Name} (ID: {center.Id})");
                return MapToResponseDto(center);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating center with ID {id}");
                throw;
            }
        }

        /// <summary>
        /// Delete a collection center
        /// </summary>
        public async Task<bool> DeleteCenterAsync(int id)
        {
            try
            {
                var center = await _context.Centers.FindAsync(id);
                if (center == null)
                {
                    throw new KeyNotFoundException($"Center with ID {id} not found");
                }

                _context.Centers.Remove(center);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Center deleted successfully: {center.Name} (ID: {center.Id})");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting center with ID {id}");
                throw;
            }
        }

        /// <summary>
        /// Check if a center exists
        /// </summary>
        public async Task<bool> CenterExistsAsync(int id)
        {
            return await _context.Centers.AnyAsync(c => c.Id == id);
        }

        /// <summary>
        /// Map Center entity to CenterResponseDto
        /// </summary>
        private static CenterResponseDto MapToResponseDto(Center center)
        {
            return new CenterResponseDto
            {
                Id = center.Id,
                Name = center.Name,
                District = center.District,
                Address = center.Address,
                Phone = center.Phone,
                AcceptedItemType = center.AcceptedItemType,
                OpeningHours = center.OpeningHours,
                CreatedAt = center.CreatedAt,
                UpdatedAt = center.UpdatedAt
            };
        }
    }
}