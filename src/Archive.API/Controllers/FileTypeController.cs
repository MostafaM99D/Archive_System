using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Archive.API.Controllers
{
    [Route("api/FileTypes")]
    [ApiController]
    public class FileTypeController : ControllerBase
    {
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<FileType>>> GetAll()
        {
            var res = await FileTypeService.GetAllFileTypes();
            if (!res.Any()) return NotFound("No FileTypes found");
            return Ok(res);
        }

        [HttpGet("Get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FileType>> GetByID(int id)
        {
            if (id < 1) return BadRequest("Invalid ID");
            var res = await FileTypeService.GetFileTypeByID(id);
            if (res == null) return NotFound("FileType not found");
            return Ok(res);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Add([FromBody] FileType newFileType)
        {
            if (newFileType == null || string.IsNullOrEmpty(newFileType.TypeName))
                return BadRequest("Invalid data");

            var fileType = new FileTypeService(newFileType);
            if (await fileType.Save())
                return CreatedAtAction(nameof(GetByID), new { id = fileType.FileTypeID }, newFileType);

            return BadRequest("Error saving FileType");
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(int id, [FromBody] FileType updated)
        {
            if (updated == null || string.IsNullOrEmpty(updated.TypeName))
                return BadRequest("Invalid data");

            var existing = await FileTypeService.GetFileTypeByID(id);
            if (existing == null) return NotFound($"FileType with ID {id} not found");

            existing.TypeName = updated.TypeName;

            if (await existing.Save()) return Ok(existing);
            return BadRequest("Error updating FileType");
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 1) return BadRequest("Invalid ID");
            if (!await FileTypeService.IsFileTypeExist(id)) return NotFound($"FileType with ID {id} not found");

            if (await FileTypeService.DeleteFileType(id)) return Ok("Deleted successfully");
            return BadRequest("Error deleting FileType");
        }
    }
}
