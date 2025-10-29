using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.API.Controllers
{
    [Route("api/Documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        [HttpGet("GetAll", Name = "GetAllDocuments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Document>>> GetAllDocuments()
        {
            var res = await DocumentService.GetAllDocuments();
            if (res == null || res.Count == 0)
                return NotFound("No documents found.");
            return Ok(res);
        }

        [HttpGet("Get/{documentId}", Name = "GetDocumentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Document>> GetDocumentByID(int documentId)
        {
            if (documentId < 1)
                return BadRequest("Invalid Document ID.");

            var document = await DocumentService.GetDocumentByID(documentId);
            if (document == null)
                return NotFound($"Document with ID {documentId} not found.");

            return Ok(document);
        }

        [HttpPost("AddNew", Name = "AddNewDocument")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddNewDocument([FromBody] Document newDocument)
        {
            if (newDocument == null || string.IsNullOrEmpty(newDocument.Content))
                return BadRequest("Invalid document data.");

            var documentService = new DocumentService(newDocument);
            if (await documentService.Save())
            {
                newDocument.DocumentID = documentService.DocumentID;
                return CreatedAtRoute("GetDocumentByID", new { documentId = newDocument.DocumentID }, newDocument);
            }
            return BadRequest("Document not saved due to an internal error.");
        }

        [HttpPut("Update/{documentId}", Name = "UpdateDocument")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateDocument(int documentId, [FromBody] Document updatedDocument)
        {
            if (updatedDocument == null || documentId < 1)
                return BadRequest("Invalid document data.");

            var existingDoc = await DocumentService.GetDocumentByID(documentId);
            if (existingDoc == null)
                return NotFound($"Document with ID {documentId} not found.");

            existingDoc.Content = updatedDocument.Content;
            existingDoc.CreatedAt = updatedDocument.CreatedAt;
            existingDoc.Year = updatedDocument.Year;
            existingDoc.Notes = updatedDocument.Notes;
            existingDoc.InternalNumber = updatedDocument.InternalNumber;
            existingDoc.UserID = updatedDocument.UserID;
            existingDoc.DocumentTypeID = updatedDocument.DocumentTypeID;
            existingDoc.DepartmentID = updatedDocument.DepartmentID;

            if (await existingDoc.Save())
                return Ok(existingDoc);
            return BadRequest("Error while updating document.");
        }

        [HttpDelete("Delete/{documentId}", Name = "DeleteDocument")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteDocument(int documentId)
        {
            if (documentId < 1)
                return BadRequest("Invalid Document ID.");

            bool exists = await DocumentService.IsDocumentExist(documentId);
            if (!exists)
                return NotFound($"Document with ID {documentId} not found.");

            if (await DocumentService.DeleteDocument(documentId))
                return Ok("Document deleted successfully.");
            return BadRequest("Error deleting document.");
        }
    }
}
