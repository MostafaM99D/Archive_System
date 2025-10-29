using Archive.BLL.Services;
using Archive.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Archive.API.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet("GetAll", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var res = await UserService.GetAllUsers();
            if (!res.Any())
                return NotFound("No users founded");
            return Ok(res);
        }

        [HttpGet("Get/{Id}", Name = "GetUserByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> GetStudentByID(int Id)
        {
            if (Id < 0) return BadRequest();
            var res = await UserService.GetUserByID(Id);
            if (res == null) return NotFound("This user isn't exist");
            return Ok(res);
        }

        [HttpPost("AddNew", Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddNewUser(User newUser)
        {
            if (newUser == null
                || string.IsNullOrEmpty(newUser.Username)
                || string.IsNullOrEmpty(newUser.FirstName)
                || string.IsNullOrEmpty(newUser.LastName)
                || string.IsNullOrEmpty(newUser.Password))
            {
                return BadRequest("Invalid data inputs");
            }


            var user = new UserService(newUser);
            if (await user.Save())
            {
                newUser.UserID = user.UserID;
                return CreatedAtRoute("GetUserByID", new { userId = newUser.UserID }, newUser);
            }
            else
                return BadRequest("User not saved there exists an error");

        }

        [HttpPut("Update/{id}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateUser(int id, User updatedUser)
        {
            if (updatedUser == null) return BadRequest("User is null.");

            if (id < 1 || string.IsNullOrEmpty(updatedUser.Username)
                || string.IsNullOrEmpty(updatedUser.FirstName)
                || string.IsNullOrEmpty(updatedUser.LastName)
                || string.IsNullOrEmpty(updatedUser.Password))
                return BadRequest("Invalid data");
            var user = await UserService.GetUserByID(id);

            if (user == null) return NotFound($"User with UserId : {id} not founded.");
            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;
            user.FirstName = updatedUser.FirstName;
            user.Permissions = updatedUser.Permissions;
            user.LastName = updatedUser.LastName;
            user.IsActive = updatedUser.IsActive;

            if (await user.Save())
                return Ok(user);
            else
                return BadRequest("User not saved there exists an error");
        }
        [HttpDelete("Delete/{Id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUser(int Id)
        {
            if (Id < 1) return BadRequest("Invalid data");
            bool Exsist = await UserService.IsUserExist(Id);
            if (!Exsist) return NotFound($"User with UserId : {Id} not founded.");
            if (await UserService.DeleteUser(Id))
                return Ok("User deleted successfully");
            else
                return BadRequest("Error In Server");
        }

    }
}
