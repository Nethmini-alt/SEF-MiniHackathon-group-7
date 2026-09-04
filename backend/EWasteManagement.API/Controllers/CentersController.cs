using Microsoft.AspNetCore.Mvc;
using EWasteManagement.API.DTOs;
using EWasteManagement.API.Services;

namespace EWasteManagement.API.Controllers
{
    /// <summary>
    /// API endpoints for managing collection centers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CentersController : ControllerBase
    {
        private readonly ICenterService _centerService;
        private readonly ILogger<CentersController> _logger;

        public CentersController(ICenterService centerService, ILogger<CentersController> logger)
        {
            _centerService = centerService;
            _logger = logger;
        }

        /// <summary>
        /// Get all collection centers
        /// </summary>
        /// <returns>List of all collection centers</returns>
        /// <response code="200">Returns the list of centers</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CenterResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CenterResponseDto>>> GetCenters()
        {
            try
            {
                var centers = await _centerService.GetAllCentersAsync();
                return Ok(centers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all centers");
                return StatusCode(500, "An error occurred while retrieving collection centers");
            }
        }

        /// <summary>
        /// Get a specific collection center by ID
        /// </summary>
        /// <param name="id">Center ID</param>
        /// <returns>The requested center</returns>
        /// <response code="200">Returns the center</response>
        /// <response code="404">Center not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CenterResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CenterResponseDto>> GetCenter(int id)
        {
            try
            {
                var center = await _centerService.GetCenterByIdAsync(id);
                return Ok(center);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting center with ID {id}");
                return StatusCode(500, "An error occurred while retrieving the center");
            }
        }

        /// <summary>
        /// Create a new collection center
        /// </summary>
        /// <param name="createDto">Center data</param>
        /// <returns>The created center</returns>
        /// <response code="201">Center created successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="409">Center with same name already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(CenterResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CenterResponseDto>> CreateCenter([FromBody] CreateCenterDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var center = await _centerService.CreateCenterAsync(createDto);
                return CreatedAtAction(nameof(GetCenter), new { id = center.Id }, center);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating center");
                return StatusCode(500, "An error occurred while creating the collection center");
            }
        }

        /// <summary>
        /// Update an existing collection center
        /// </summary>
        /// <param name="id">Center ID to update</param>
        /// <param name="updateDto">Updated center data</param>
        /// <returns>The updated center</returns>
        /// <response code="200">Center updated successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Center not found</response>
        /// <response code="409">Center with same name already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CenterResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CenterResponseDto>> UpdateCenter(int id, [FromBody] UpdateCenterDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var center = await _centerService.UpdateCenterAsync(id, updateDto);
                return Ok(center);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating center with ID {id}");
                return StatusCode(500, "An error occurred while updating the collection center");
            }
        }

        /// <summary>
        /// Delete a collection center
        /// </summary>
        /// <param name="id">Center ID to delete</param>
        /// <returns>No content on success</returns>
        /// <response code="204">Center deleted successfully</response>
        /// <response code="404">Center not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCenter(int id)
        {
            try
            {
                await _centerService.DeleteCenterAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting center with ID {id}");
                return StatusCode(500, "An error occurred while deleting the collection center");
            }
        }
    }
}