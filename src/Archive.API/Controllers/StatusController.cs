using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Archive.API.Controllers
{
    [Route("api/Status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Status>>> GetAll()
        {
            var res = await StatusService.GetAllStatuses();
            if (!res.Any()) return NotFound("No statuses found");
            return Ok(res);
        }

        [HttpGet("Get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Status>> GetByID(int id)
        {
            if (id < 1) return BadRequest("Invalid ID");
            var res = await StatusService.GetStatusByID(id);
            if (res == null) return NotFound("Status not found");
            return Ok(res);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Add([FromBody] Status newStatus)
        {
            if (newStatus == null || string.IsNullOrEmpty(newStatus.StatusName))
                return BadRequest("Invalid data");

            var status = new StatusService(newStatus);
            if (await status.Save())
                return CreatedAtAction(nameof(GetByID), new { id = status.StatusID }, newStatus);

            return BadRequest("Error saving status");
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(int id, [FromBody] Status updated)
        {
            if (updated == null || string.IsNullOrEmpty(updated.StatusName))
                return BadRequest("Invalid data");

            var existing = await StatusService.GetStatusByID(id);
            if (existing == null) return NotFound($"Status with ID {id} not found");

            existing.StatusName = updated.StatusName;

            if (await existing.Save()) return Ok(existing);
            return BadRequest("Error updating status");
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 1) return BadRequest("Invalid ID");
            if (!await StatusService.IsStatusExist(id)) return NotFound($"Status with ID {id} not found");

            if (await StatusService.DeleteStatus(id)) return Ok("Deleted successfully");
            return BadRequest("Error deleting status");
        }
    }
}
