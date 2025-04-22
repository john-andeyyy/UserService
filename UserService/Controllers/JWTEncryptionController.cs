using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model.Manager;
using UserService.Helper;
using UserService.Models.Request;

namespace UserService.Controllers
{
    [Route("api/jwt/")]
    [ApiController]
    public class JWTEncryptionController : Controller
    {
        private readonly JwtSettings jwtSettings;
        public JWTEncryptionController(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }

        [HttpGet("UserEncrypt/{Id}")]
        public IActionResult GetUserById([FromRoute] int Id)
        {
            var manager = new UserManager();
            var user = manager.GetUserById(Id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var tokenExpiration = TimeSpan.FromSeconds(jwtSettings.ExpirationSeconds);
            string token = JWTEncryption_OJT.JWTEncryption(jwtSettings, user, tokenExpiration);

            return Ok(new { Message = "User found", JWTToken = token });
        }


        [HttpPost("UserDecrypt")]
        public IActionResult DecryptUserFromToken([FromBody] JwtToken jwt)
        {

            if (string.IsNullOrEmpty(jwt.Token))
                return BadRequest( new {message = "jwtToken is Missing!" });
            try
            {
                var user = JWTEncryption_OJT.JWTDecryption(jwt.Token);
                return Ok(new
                {
                    Message = "User found",
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Invalid token", Error = ex.Message });
            }
        }



    }
}
