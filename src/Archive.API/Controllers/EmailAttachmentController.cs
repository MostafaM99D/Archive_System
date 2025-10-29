using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Archive.API.Controllers
{
    [Route("api/EmailAttachments")]
    [ApiController]
    public class EmailAttachmentController : ControllerBase
    {
        [HttpGet("GetAll", Name = "GetAllEmailAttachments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<EmailAttachment>>> GetAll()
        {
            var res = await EmailAttachmentService.GetAll();
            if (!res.Any()) return NotFound("No email attachments found.");
            return Ok(res);
        }

        [HttpGet("Get/{id}", Name = "GetEmailAttachmentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmailAttachment>> GetByID(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID.");
            var res = await EmailAttachmentService.GetByID(id);
            if (res == null) return NotFound($"Attachment with ID {id} not found.");
            return Ok(res);
        }

        [HttpPost("AddNew", Name = "AddNewEmailAttachment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddNew([FromBody] EmailAttachment newAttachment)
        {
            if (newAttachment == null || string.IsNullOrEmpty(newAttachment.FileName) || string.IsNullOrEmpty(newAttachment.FilePath))
                return BadRequest("Invalid input data.");

            var service = new EmailAttachmentService(newAttachment);
            if (await service.Save())
                return CreatedAtRoute("GetEmailAttachmentByID", new { id = service.EmailAttachmentID }, newAttachment);

            return BadRequest("Error adding attachment.");
        }

        [HttpPut("Update/{id}", Name = "UpdateEmailAttachment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, [FromBody] EmailAttachment updatedAttachment)
        {
            if (id <= 0 || updatedAttachment == null)
                return BadRequest("Invalid data.");

            var existing = await EmailAttachmentService.GetByID(id);
            if (existing == null) return NotFound($"Attachment with ID {id} not found.");

            existing.FileName = updatedAttachment.FileName;
            existing.FilePath = updatedAttachment.FilePath;
            existing.UploadedAt = updatedAttachment.UploadedAt;
            existing.FileTypeID = updatedAttachment.FileTypeID;
            existing.EmailID = updatedAttachment.EmailID;

            if (await existing.Save()) return Ok(existing);
            return BadRequest("Error updating attachment.");
        }

        [HttpDelete("Delete/{id}", Name = "DeleteEmailAttachment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID.");
            var existing = await EmailAttachmentService.GetByID(id);
            if (existing == null) return NotFound($"Attachment with ID {id} not found.");

            if (await EmailAttachmentService.Delete(id))
                return Ok("Attachment deleted successfully.");

            return BadRequest("Error deleting attachment.");
        }
    }
}
