using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Task4MovieLibraryApi.Controllers
{
    /// <summary>
    /// Identity server controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        /// <summary>
        /// Get: api/identity
        /// Get request to the Identity server
        /// </summary>
        /// <returns>Return any data</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
