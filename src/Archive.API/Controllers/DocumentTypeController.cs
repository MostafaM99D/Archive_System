using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Archive.API.Controllers
{
    [Route("api/DocumentTypes")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DocumentType>>> GetAllDocumentTypes()
        {
            var types = await DocumentTypeService.GetAllDocumentTypes();
            if (!types.Any())
                return NotFound("No document types found.");

            return Ok(types);
        }

        [HttpGet("Get/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DocumentType>> GetDocumentTypeByID(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID.");

            var documentType = await DocumentTypeService.GetDocumentTypeByID(id);
            if (documentType == null)
                return NotFound("Document type not found.");

            return Ok(documentType);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddNewDocumentType([FromBody] DocumentType documentType)
        {
            if (documentType == null || string.IsNullOrEmpty(documentType.TypeName))
                return BadRequest("Invalid input data.");

            var newDocumentType = new DocumentTypeService(documentType);
            if (await newDocumentType.Save())
                return CreatedAtAction(nameof(GetDocumentTypeByID), new { id = newDocumentType.DocumentTypeID }, documentType);

            return StatusCode(StatusCodes.Status500InternalServerError, "Error while saving document type.");
        }

        [HttpPut("Update/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateDocumentType(int id, [FromBody] DocumentType updatedType)
        {
            if (id <= 0 || updatedType == null || string.IsNullOrEmpty(updatedType.TypeName))
                return BadRequest("Invalid data.");

            var existing = await DocumentTypeService.GetDocumentTypeByID(id);
            if (existing == null)
                return NotFound($"Document type with ID {id} not found.");

            existing.TypeName = updatedType.TypeName;

            if (await existing.Save())
                return Ok(existing);

            return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating document type.");
        }

        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteDocumentType(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID.");

            var exists = await DocumentTypeService.IsDocumentTypeExist(id);
            if (!exists)
                return NotFound($"Document type with ID {id} not found.");

            if (await DocumentTypeService.DeleteDocumentType(id))
                return Ok("Document type deleted successfully.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Error while deleting document type.");
        }
    }
}
