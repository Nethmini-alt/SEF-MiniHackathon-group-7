using EWasteManagement.API.Data;
using EWasteManagement.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EWasteManagement.API.Controllers
{
    [ApiController]
    [Route("api/pickup-requests")]
    public class PickupRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PickupRequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/pickup-requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PickupRequest>>> GetPickupRequests()
        {
            var requests = await _context.PickupRequests
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(requests);
        }

        // PUT: api/pickup-requests/{id}/approve
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApprovePickupRequest(int id)
        {
            var request = await _context.PickupRequests.FindAsync(id);

            if (request == null)
            {
                return NotFound(new
                {
                    message = "Pickup request not found."
                });
            }

            if (request.Status != "Pending")
            {
                return BadRequest(new
                {
                    message = $"Request is already {request.Status.ToLower()}."
                });
            }

            request.Status = "Approved";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Pickup request approved successfully.",
                requestId = request.Id,
                status = request.Status
            });
        }

        // PUT: api/pickup-requests/{id}/decline
        [HttpPut("{id}/decline")]
        public async Task<IActionResult> DeclinePickupRequest(int id)
        {
            var request = await _context.PickupRequests.FindAsync(id);

            if (request == null)
            {
                return NotFound(new
                {
                    message = "Pickup request not found."
                });
            }

            if (request.Status != "Pending")
            {
                return BadRequest(new
                {
                    message = $"Request is already {request.Status.ToLower()}."
                });
            }

            request.Status = "Declined";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Pickup request declined successfully.",
                requestId = request.Id,
                status = request.Status
            });
        }
    }
}