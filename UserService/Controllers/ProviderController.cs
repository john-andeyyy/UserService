using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Models.Manager;
using UserService.Models.Request;

namespace UserService.Controllers
{
    [Route("api/Provider/")]
    [ApiController]
    [Authorize(Policy = "AuthPolicy")]
    public class ProviderController : Controller
    {
       
        [HttpGet("PrividerList")]
        public IActionResult Provider_DATA()
        {
             var manager = new ProviderManager();
            var providerList = manager.Providers();
            return Ok(new { Message = "List of all Providers", Data = providerList });

        }

        [HttpPost("NewProvider")]
        public IActionResult InsertProvider([FromBody] ProviderRequest provider)
        {
            var manager = new ProviderManager();
            var result = manager.InsertProvider(provider);

            if (!result)
            {
                return BadRequest(new { Message = "Error creating provider" });
            }
            return Ok(new { Message = "Provider created successfully" });
        }

        [HttpGet("Provider/{id}")]
        public IActionResult GetProvider(int id)
        {
            var manager = new ProviderManager();
            var provider = manager.GetProviderById(id);
            if (provider == null)
            {
                return NotFound(new { Message = "Provider not found" });
            }
            return Ok(new { Message = "Provider found", Data = provider });
        }

        [HttpDelete("Provider/{id}")]
        public IActionResult DeleteProvider(int id)
        {
            var manager = new ProviderManager();
            var provider = manager.GetProviderById(id);
            if (provider == null)
            {
                return NotFound(new { Message = "Provider not found" });
            }

            var result = manager.DeleteProvider(id);
            if (!result)
            {
                return NotFound(new { Message = "Provider not found" });
            }
            return Ok(new { Message = "Provider deleted successfully" });
        }

        [HttpPut("Provider")]
        public IActionResult UpdateProvider( [FromBody] ProviderRequest provider)
        {
            var manager = new ProviderManager();
            var existingProvider = manager.GetProviderById(provider.Party_Id);
            if (existingProvider == null)
            {
                return NotFound(new { Message = "Provider not found" });
            }
            var result = manager.UpdateProvider(provider);
            if (!result)
            {
                return BadRequest(new { Message = "Error updating provider" });
            }
            return Ok(new { Message = "Provider updated successfully" });
        }

    }
}
