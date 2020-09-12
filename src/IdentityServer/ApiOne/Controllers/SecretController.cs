using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ApiOne.Controllers
{
    [Route("api/[controller]")]
    public class SecretController : ControllerBase
    {
        [Authorize]
        public string Index()
        {
            var claims = User.Claims.ToList();

            return "Secret message from api one";
        }
    }
}
