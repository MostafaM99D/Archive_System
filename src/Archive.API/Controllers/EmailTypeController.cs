using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Archive.API.Controllers
{
    [Route("api/EmailTypes")]
    [ApiController]
    public class EmailTypeController : ControllerBase
    {
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EmailType>>> GetAllEmailTypes()
        {
            var emailTypes = await EmailTypeService.GetAllEmailTypes();
            if (!emailTypes.Any())
                return NotFound("No email types found.");

            return Ok(emailTypes);
        }

        [HttpGet("Get/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmailType>> GetEmailTypeByID(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID.");

            var emailType = await EmailTypeService.GetEmailTypeByID(id);
            if (emailType == null) return NotFound("EmailType not found.");

            return Ok(emailType);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddNewEmailType([FromBody] EmailType emailType)
        {
            if (emailType == null || string.IsNullOrEmpty(emailType.TypeName))
                return BadRequest("Invalid input data.");

            var newEmailType = new EmailTypeService(emailType);
            if (await newEmailType.Save())
                return CreatedAtAction(nameof(GetEmailTypeByID), new { id = newEmailType.EmailTypeID }, emailType);

            return StatusCode(500, "Error while saving EmailType.");
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult> UpdateEmailType(int id, [FromBody] EmailType updatedType)
        {
            if (id <= 0 || updatedType == null || string.IsNullOrEmpty(updatedType.TypeName))
                return BadRequest("Invalid data.");

            var existing = await EmailTypeService.GetEmailTypeByID(id);
            if (existing == null) return NotFound($"EmailType with ID {id} not found.");

            existing.TypeName = updatedType.TypeName;

            if (await existing.Save())
                return Ok(existing);

            return StatusCode(500, "Error while updating EmailType.");
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult> DeleteEmailType(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID.");

            var exists = await EmailTypeService.IsEmailTypeExist(id);
            if (!exists) return NotFound($"EmailType with ID {id} not found.");

            if (await EmailTypeService.DeleteEmailType(id))
                return Ok("EmailType deleted successfully.");

            return StatusCode(500, "Error while deleting EmailType.");
        }
    }
}
