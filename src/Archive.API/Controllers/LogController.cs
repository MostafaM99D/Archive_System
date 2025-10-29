using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Archive.API.Controllers
{
    [Route("api/Logs")]
    [ApiController]
    public class LogController : ControllerBase
    {
        [HttpGet("GetAll", Name = "GetAllLogs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Log>>> GetAll()
        {
            var res = await LogService.GetAll();
            if (!res.Any()) return NotFound("No logs found.");
            return Ok(res);
        }

        [HttpGet("Get/{id}", Name = "GetLogByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Log>> GetByID(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID.");
            var res = await LogService.GetByID(id);
            if (res == null) return NotFound($"Log with ID {id} not found.");
            return Ok(res);
        }

        [HttpPost("AddNew", Name = "AddNewLog")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddNew([FromBody] Log newLog)
        {
            if (newLog == null || newLog.UserID <= 0 || newLog.OperationID <= 0)
                return BadRequest("Invalid input data.");

            var service = new LogService(newLog);
            if (await service.Save())
                return CreatedAtRoute("GetLogByID", new { id = service.LogID }, newLog);

            return BadRequest("Error adding log.");
        }

        [HttpPut("Update/{id}", Name = "UpdateLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(int id, [FromBody] Log updatedLog)
        {
            if (id <= 0 || updatedLog == null)
                return BadRequest("Invalid data.");

            var existing = await LogService.GetByID(id);
            if (existing == null) return NotFound($"Log with ID {id} not found.");

            existing.OperationID = updatedLog.OperationID;
            existing.DocumentID = updatedLog.DocumentID;
            existing.UserID = updatedLog.UserID;
            existing.EmailID = updatedLog.EmailID;
            existing.StatusID = updatedLog.StatusID;
            existing.DeviceInfo = updatedLog.DeviceInfo;
            existing.LogDate = updatedLog.LogDate;

            if (await existing.Save())
                return Ok(existing);
            return BadRequest("Error updating log.");
        }

        [HttpDelete("Delete/{id}", Name = "DeleteLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID.");
            var existing = await LogService.GetByID(id);
            if (existing == null) return NotFound($"Log with ID {id} not found.");

            if (await LogService.Delete(id))
                return Ok("Log deleted successfully.");

            return BadRequest("Error deleting log.");
        }
    }
}
