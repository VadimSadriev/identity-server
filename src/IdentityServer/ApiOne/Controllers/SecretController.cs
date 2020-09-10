using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOne.Controllers
{
    [Route("api/[controller]")]
    public class SecretController : ControllerBase
    {
        [Authorize]
        public string Index()
        {
            return "Secret message from api one";
        }
    }
}
