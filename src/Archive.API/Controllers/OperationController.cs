using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Archive.API.Controllers
{
    [Route("api/Operations")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Operation>>> GetAll()
        {
            var res = await OperationService.GetAllOperations();
            if (!res.Any()) return NotFound("No operations found");
            return Ok(res);
        }

        [HttpGet("Get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Operation>> GetByID(int id)
        {
            if (id < 1) return BadRequest("Invalid ID");
            var res = await OperationService.GetOperationByID(id);
            if (res == null) return NotFound("Operation not found");
            return Ok(res);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Add([FromBody] Operation newOp)
        {
            if (newOp == null || string.IsNullOrEmpty(newOp.OperationName))
                return BadRequest("Invalid data");

            var op = new OperationService(newOp);
            if (await op.Save())
                return CreatedAtAction(nameof(GetByID), new { id = op.OperationID }, newOp);

            return BadRequest("Error saving operation");
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(int id, [FromBody] Operation updated)
        {
            if (updated == null || string.IsNullOrEmpty(updated.OperationName))
                return BadRequest("Invalid data");

            var existing = await OperationService.GetOperationByID(id);
            if (existing == null) return NotFound($"Operation with ID {id} not found");

            existing.OperationName = updated.OperationName;

            if (await existing.Save()) return Ok(existing);
            return BadRequest("Error updating operation");
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 1) return BadRequest("Invalid ID");
            if (!await OperationService.IsOperationExist(id)) return NotFound($"Operation with ID {id} not found");

            if (await OperationService.DeleteOperation(id)) return Ok("Deleted successfully");
            return BadRequest("Error deleting operation");
        }
    }
}
