using BlockModel;
using BlockModel.Model;
using Coordinador.Service;
using Microsoft.AspNetCore.Mvc;

namespace Coordinador.Controllers
{
    public class CoordinadorController : ControllerBase
    {
        ICoordinadorService _coordinadorService;

        public CoordinadorController(ICoordinadorService coordinadorService)
        {
            _coordinadorService = coordinadorService;
        }

        [HttpPost("SendMoney")]
        public async Task<IActionResult> SendMoney([FromBody] TransactionDTO transactionDTO)
        {
            try
            {
                ApiResponse response = await _coordinadorService.IsValidTransaction(transactionDTO);
                Console.WriteLine();
                if (response.IsValid == true)
                {
                    TransactionChain responseTran = _coordinadorService.SendMoney(transactionDTO);
                    return Ok(responseTran);
                }
                else
                {
                    return BadRequest("Transacción inválida");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("QueryTotal{idClient}")]
        public IActionResult QueryTotal(int idClient)
        {
            try
            {
                ApiResponse response = _coordinadorService.IsValidClient(idClient);
                if (response.IsValid == true)
                {
                    return Ok(_coordinadorService.QueryTotal(idClient));
                }
                else
                {
                    return BadRequest("Cliente no existe");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("/BlockChain")]
        public IActionResult GetBlockChain()
        {
            try
            {
                return Ok(_coordinadorService.GetBlocks());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
