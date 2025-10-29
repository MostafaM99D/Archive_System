using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Archive.API.Controllers
{
    [Route("api/Departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments()
        {
            var departments = await DepartmentService.GetAllDepartments();
            if (!departments.Any())
                return NotFound("No departments found.");

            return Ok(departments);
        }

        [HttpGet("Get/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Department>> GetDepartmentByID(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID.");

            var department = await DepartmentService.GetDepartmentByID(id);
            if (department == null)
                return NotFound("Department not found.");

            return Ok(department);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddNewDepartment([FromBody] Department department)
        {
            if (department == null || string.IsNullOrEmpty(department.DepartmentName))
                return BadRequest("Invalid input data.");

            var newDepartment = new DepartmentService(department);
            if (await newDepartment.Save())
                return CreatedAtAction(nameof(GetDepartmentByID), new { id = newDepartment.DepartmentID }, department);

            return StatusCode(StatusCodes.Status500InternalServerError, "Error while saving department.");
        }

        [HttpPut("Update/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateDepartment(int id, [FromBody] Department updatedDepartment)
        {
            if (id <= 0 || updatedDepartment == null || string.IsNullOrEmpty(updatedDepartment.DepartmentName))
                return BadRequest("Invalid data.");

            var existing = await DepartmentService.GetDepartmentByID(id);
            if (existing == null)
                return NotFound($"Department with ID {id} not found.");

            existing.DepartmentName = updatedDepartment.DepartmentName;

            if (await existing.Save())
                return Ok(existing);

            return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating department.");
        }

        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID.");

            var exists = await DepartmentService.IsDepartmentExist(id);
            if (!exists)
                return NotFound($"Department with ID {id} not found.");

            if (await DepartmentService.DeleteDepartment(id))
                return Ok("Department deleted successfully.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Error while deleting department.");
        }
    }
}
