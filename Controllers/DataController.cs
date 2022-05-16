using Microsoft.AspNetCore.Mvc;

namespace elestio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpGet(Name = "data")]
        public ActionResult Get()
        {
            
            return Ok("{ 'status' : 'OK'}");
        }
    }
}
