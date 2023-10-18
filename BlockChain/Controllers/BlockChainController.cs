using BlockChain.Model;
using BlockChain.Service;
using Microsoft.AspNetCore.Mvc;

namespace BlockChain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockChainController : ControllerBase
    {
        IBlockChainService _blockChainService;

        public BlockChainController(IBlockChainService blockChainService)
        {
            _blockChainService = blockChainService;
        }

        [HttpPost]
        public IActionResult SaveTranstaction()
        {

        }
    }
}
