using BlockChain.Service;
using BlockModel;
using BlockModel.Model;
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

        [HttpPost("SaveTranstaction")]
        public IActionResult SaveTranstaction([FromBody] TransactionDTO transaction)
        {
            var transactionChain = _blockChainService.SaveTransaction(transaction);
            return Ok(transactionChain);
        }

        [HttpGet("QueryTotal/{clientId}")]
        public IActionResult QueryTotal(int clientId)
        {
            var total = _blockChainService.QueryTotal(clientId);
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Response = total.ToString();
            return Ok(apiResponse);
        }

        [HttpGet]
        public async Task<IActionResult> BlockChain()
        {
            var blocks = await _blockChainService.GetBlockChain();
            return Ok(blocks);
        }

    }
}
