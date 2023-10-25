using BlockModel.Model;
using CerrarBloque.Service;
using Microsoft.AspNetCore.Mvc;

namespace CerrarBloque.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CerrarBloqueController : ControllerBase
    {

        ICerrarBloqueService service;
        public CerrarBloqueController(ICerrarBloqueService cerrarBloqueService) 
        {
            service = cerrarBloqueService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] List<Block> blocks)
        {
            service.CerrarBlock(blocks);
            return Ok(blocks);
        }

    }
}
