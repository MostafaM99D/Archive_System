using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Archive.API.Controllers
{
    [Route("api/Emails")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Email>>> GetAll()
        {
            var res = await EmailService.GetAllEmails();
            if (!res.Any())
                return NotFound("No emails found");
            return Ok(res);
        }

        [HttpGet("Get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Email>> GetByID(int id)
        {
            if (id < 1) return BadRequest("Invalid ID");
            var res = await EmailService.GetEmailByID(id);
            if (res == null) return NotFound($"Email with ID {id} not found");
            return Ok(res);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Add([FromBody] Email newEmail)
        {
            if (newEmail == null ||
                newEmail.UserID < 1 ||
                newEmail.EmailTypeID < 1 ||
                newEmail.DepartmentID < 1 ||
                string.IsNullOrEmpty(newEmail.Content))
            {
                return BadRequest("Invalid input data");
            }

            var email = new EmailService(newEmail);
            if (await email.Save())
                return CreatedAtAction(nameof(GetByID), new { id = email.EmailID }, newEmail);

            return BadRequest("Error saving email");
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, [FromBody] Email updatedEmail)
        {
            if (updatedEmail == null ||
                string.IsNullOrEmpty(updatedEmail.Content) ||
                updatedEmail.UserID < 1 ||
                updatedEmail.EmailTypeID < 1 ||
                updatedEmail.DepartmentID < 1)
            {
                return BadRequest("Invalid input data");
            }

            var existing = await EmailService.GetEmailByID(id);
            if (existing == null) return NotFound($"Email with ID {id} not found");

            existing.Content = updatedEmail.Content;
            existing.CreatedAt = updatedEmail.CreatedAt;
            existing.Year = updatedEmail.Year;
            existing.Notes = updatedEmail.Notes;
            existing.UserID = updatedEmail.UserID;
            existing.EmailTypeID = updatedEmail.EmailTypeID;
            existing.DepartmentID = updatedEmail.DepartmentID;

            if (await existing.Save())
                return Ok(existing);

            return BadRequest("Error updating email");
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 1) return BadRequest("Invalid ID");
            bool exist = await EmailService.IsEmailExist(id);
            if (!exist) return NotFound($"Email with ID {id} not found");

            if (await EmailService.DeleteEmail(id))
                return Ok("Email deleted successfully");

            return BadRequest("Error deleting email");
        }
    }
}
