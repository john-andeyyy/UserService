using Microsoft.AspNetCore.Mvc;
using Model.Manager;
using UserService.Models.Request;

namespace UserService.Controllers
{
    [Route("api/UserService/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("AllUser")]
        public IActionResult GetUsers()
        {
            var manager = new UserManager();
            var Userlist = manager.GetAllUser();
            return Ok(new { Message = "List of all Users", Data = Userlist });
        }

        [HttpGet("User/{id}")]
        public IActionResult GetUser(int id)
        {
            var manager = new UserManager();
            var user = manager.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            return Ok(new { Message = "User found", Data = user });
        }

        [HttpPost("UserRegister")]
        public IActionResult CreateUser([FromBody] UserRegister user)
        {
            var manager = new UserManager();
            var existingUser_Msg = manager.IsUnique(user);
            if (existingUser_Msg != null)
            {
                return BadRequest(new { Message = existingUser_Msg });
            }

            var result = manager.CreateUser(user);
            if (!result)
            {
                return BadRequest(new { Message = "Error creating user" });
            }
            return Ok(new { Message = "User created successfully" });
        }

        [HttpPost("UserLogin")]
        public IActionResult UserLogin([FromBody] UserLogin user)
        {
            var manager = new UserManager();
            var result = manager.UserLogin(user);
            if (result == null)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }
            return Ok(new { Message = "User logged in successfully", Data = result });
        }

        [HttpPut("User")]
        public IActionResult UpdateUser([FromBody] UserUpdate user)
        {
            var manager = new UserManager();
            //var existingUser_Msg = manager.IsUnique(user);
            //if (existingUser_Msg != null)
            //{
                //return BadRequest(new { Message = existingUser_Msg });
            //}
            var result = manager.UpdateUser(user);
            if (!result)
            {
                return BadRequest(new { Message = "Error updating user" });
            }
            return Ok(new { Message = "User updated successfully" });
        }
        [HttpDelete("User/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var manager = new UserManager();
            var IsExcisting = manager.GetUserById(id);
            if (IsExcisting == null)
            {
                return NotFound(new { Message = "User Not Found! "});
            }
            var result = manager.DeleteUser(id);
            if (!result)
            {
                return BadRequest(new { Message = "Error deleting user" });
            }
            return Ok(new { Message = "User deleted successfully" });
        }
    }
}
