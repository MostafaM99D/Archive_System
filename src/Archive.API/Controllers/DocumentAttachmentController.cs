using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Archive.API.Controllers
{
    [Route("api/DocumentAttachments")]
    [ApiController]
    public class DocumentAttachmentController : ControllerBase
    {
        [HttpGet("GetAll", Name = "GetAllDocumentAttachments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<DocumentAttachment>>> GetAll()
        {
            var res = await DocumentAttachmentService.GetAll();
            if (!res.Any()) return NotFound("No document attachments found.");
            return Ok(res);
        }

        [HttpGet("Get/{id}", Name = "GetDocumentAttachmentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DocumentAttachment>> GetByID(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID.");
            var res = await DocumentAttachmentService.GetByID(id);
            if (res == null) return NotFound($"Attachment with ID {id} not found.");
            return Ok(res);
        }

        [HttpPost("AddNew", Name = "AddNewDocumentAttachment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddNew([FromBody] DocumentAttachment newAttachment)
        {
            if (newAttachment == null || string.IsNullOrEmpty(newAttachment.FileName) || string.IsNullOrEmpty(newAttachment.FilePath))
                return BadRequest("Invalid input data.");

            var service = new DocumentAttachmentService(newAttachment);
            if (await service.Save())
                return CreatedAtRoute("GetDocumentAttachmentByID", new { id = service.DocumentAttachmentID }, newAttachment);

            return BadRequest("Error adding attachment.");
        }

        [HttpPut("Update/{id}", Name = "UpdateDocumentAttachment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, [FromBody] DocumentAttachment updatedAttachment)
        {
            if (id <= 0 || updatedAttachment == null)
                return BadRequest("Invalid data.");

            var existing = await DocumentAttachmentService.GetByID(id);
            if (existing == null) return NotFound($"Attachment with ID {id} not found.");

            existing.FileName = updatedAttachment.FileName;
            existing.FilePath = updatedAttachment.FilePath;
            existing.UploadedAt = updatedAttachment.UploadedAt;
            existing.FileTypeID = updatedAttachment.FileTypeID;
            existing.DocumentID = updatedAttachment.DocumentID;

            if (await existing.Save()) return Ok(existing);
            return BadRequest("Error updating attachment.");
        }

        [HttpDelete("Delete/{id}", Name = "DeleteDocumentAttachment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID.");
            var existing = await DocumentAttachmentService.GetByID(id);
            if (existing == null) return NotFound($"Attachment with ID {id} not found.");

            if (await DocumentAttachmentService.Delete(id))
                return Ok("Attachment deleted successfully.");

            return BadRequest("Error deleting attachment.");
        }
    }
}
