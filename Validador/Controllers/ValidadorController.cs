using BlockModel;
using Microsoft.AspNetCore.Mvc;

namespace Validador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidadorController : ControllerBase
    {
        [Route("IsValidTransaction")]
        [HttpPost]
        public async Task<IActionResult> IsValidTransaction([FromBody] ApiParam request)
        {
            Console.WriteLine("Entra");
            var blocks = request.Blocks;
            var transaction = request.TransactionDTO;

            var clientBlocks = blocks
                .Where(block => block.Transactions.Any(t => t.Receiver.Id == transaction.Receiver.Id) ||
                                block.Transactions.Any(t => t.Sender?.Id == transaction.Sender?.Id))
                .ToList();

            ApiResponse response = new ApiResponse()
            {
                IsValid = false,
                Response = "Transacción inválida"
            };

            if (clientBlocks.Any() && transaction.Money > 0 && transaction.Receiver.Id != transaction.Sender.Id)
            {
                response.IsValid = true;
                response.Response = "";
            }

            Console.WriteLine();
            return Ok(response);
        }

        [HttpPost("IsValidClient/{idClient}")]
        public IActionResult IsValiClient(int idClient, [FromBody] ApiParam request)
        {
            Console.WriteLine();
            var blocks = request.Blocks;
            var clientBlocks = blocks
                .Where(block => block.Transactions.Any(t => t.Receiver.Id == idClient) ||
                                block.Transactions.Any(t => t.Sender?.Id == idClient))
                .ToList();

            ApiResponse response = new ApiResponse()
            {
                IsValid = false,
                Response = "Cliente no existe"
            };

            if (clientBlocks.Any())
            {
                response.IsValid = true;
                response.Response = "";
            }

            Console.WriteLine();
            return Ok(response);
        }
    }
}
