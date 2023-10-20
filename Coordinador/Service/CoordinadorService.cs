using BlockModel;
using BlockModel.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Coordinador.Service
{
    public class CoordinadorService : ICoordinadorService
    {
        private readonly HttpClient httpClient;

        public CoordinadorService()
        {
            httpClient = new HttpClient();
        }

        public List<Block> GetBlocks()
        {
            var apiUrl = "http://localhost:5030/api/BlockChain";
            var response = httpClient.GetAsync(apiUrl).Result;

            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            var blocks = JsonConvert.DeserializeObject<List<Block>>(jsonResponse);

            return blocks;
        }

        public ApiResponse IsValidClient(int idClient)
        {
            var blocks = GetBlocks();
            ApiParam apiParam = new ApiParam();
            apiParam.Blocks = blocks;

            var json = JsonConvert.SerializeObject(apiParam);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiUrl = $"http://localhost:5243/api/Validador//IsValidClient/{idClient}";

            HttpResponseMessage response = httpClient.PostAsync(apiUrl, content).Result;
            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            var responseApi = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

            return responseApi;
        }

        public async Task<ApiResponse> IsValidTransaction(TransactionDTO transaction)
        {
            List<Block> blocks = GetBlocks();

            ApiParam apiParam = new ApiParam()
            {
                Blocks = blocks,
                TransactionDTO = transaction,
            };

            var json = JsonConvert.SerializeObject(apiParam);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiUrl = "https://localhost:7295/api/Validador/IsValidTransaction";

            HttpResponseMessage response =await httpClient.PostAsync(apiUrl, content);
            Console.WriteLine(response);
            string jsonResponse =await response.Content.ReadAsStringAsync();
            var responseApi = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);
            Console.WriteLine();
            return responseApi;
        }

        public TransactionChain SendMoney(TransactionDTO transaction)
        {
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiUrl = "http://localhost:5030/api/BlockChain/SaveTranstaction";

            HttpResponseMessage response = httpClient.PostAsync(apiUrl, content).Result;
            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            var responseApi = JsonConvert.DeserializeObject<TransactionChain>(jsonResponse);

            return responseApi;
        }

        public ApiResponse QueryTotal(int idClient)
        {
            var apiUrl = $"http://localhost:5030/api/BlockChain/QueryTotal/{idClient}";
            var response = httpClient.GetAsync(apiUrl).Result;

            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            var totalResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

            return totalResponse;
        }
    }

    public interface ICoordinadorService
    {
        List<Block> GetBlocks();
        ApiResponse IsValidClient(int idClient);
        Task<ApiResponse> IsValidTransaction(TransactionDTO transaction);
        TransactionChain SendMoney(TransactionDTO transaction);
        ApiResponse QueryTotal(int idClient);
    }
}
