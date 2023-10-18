using CerrarBloque.Model;
using Microsoft.AspNetCore.Mvc;

namespace CerrarBloque.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CerrarBloqueController : ControllerBase
    {

        [HttpPost]
        public IActionResult Post([FromBody] List<Block> blocks)
        {
            blocks.Last().IsOpen = false;
            return Ok(blocks);
        }

    }
}
