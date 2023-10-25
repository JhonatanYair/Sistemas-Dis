using BlockModel;
using BlockModel.Model;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
//using System.Text.Json;

namespace Coordinador.Service
{
    public class CoordinadorService : ICoordinadorService
    {
        private readonly HttpClient httpClient;

        public CoordinadorService()
        {
            httpClient = new HttpClient();
        }

        public async Task<List<Block>> GetBlocks()
        {
            var apiUrl = "http://blockchain/api/BlockChain";
            var response = httpClient.GetAsync(apiUrl).Result;

            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            var blocks = JsonConvert.DeserializeObject<List<Block>>(jsonResponse);

            return blocks;
        }

        public async Task<ApiResponse> IsValidClient(int idClient)
        {
            var blocks = await GetBlocks();
            ApiParam apiParam = new ApiParam();
            apiParam.Blocks = blocks;

            var json = JsonConvert.SerializeObject(apiParam);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"http://validador/api/Validador/IsValidClient/{idClient}");
            request.Content = content;
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var responseApi = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);
            return responseApi;
        }

        public async Task<ApiResponse> IsValidTransaction(TransactionDTO transaction)
        {
            var blocks = await GetBlocks();

            ApiParam apiParam = new ApiParam()
            {
                Blocks = blocks,
                TransactionDTO = transaction,
            };

            var json = JsonConvert.SerializeObject(apiParam);
            var content = new StringContent(json, null, "application/json");

            var apiUrl = "http://validador/api/Validador/IsValidTransaction";
            var response = await httpClient.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseApi = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

            return responseApi;

        }

        public TransactionChain SendMoney(TransactionDTO transaction)
        {
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiUrl = "http://blockchain/api/BlockChain/SaveTranstaction";

            HttpResponseMessage response = httpClient.PostAsync(apiUrl, content).Result;
            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            var responseApi = JsonConvert.DeserializeObject<TransactionChain>(jsonResponse);

            return responseApi;
        }

        public async Task<string> QueryTotal(int idClient)
        {
            var apiUrl = $"http://blockchain/api/BlockChain/QueryTotal/{idClient}";
            var response = httpClient.GetAsync(apiUrl).Result;
            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            var totalResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);
            return totalResponse.Response;
        }
    }

    public interface ICoordinadorService
    {
        Task<List<Block>> GetBlocks();
        Task<ApiResponse> IsValidClient(int idClient);
        Task<ApiResponse> IsValidTransaction(TransactionDTO transaction);
        TransactionChain SendMoney(TransactionDTO transaction);
        Task<string> QueryTotal(int idClient);
    }
}
