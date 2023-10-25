using BlockModel.Model;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace BlockChain.Service
{
    public class BlockChainService : IBlockChainService
    {

        private readonly HttpClient httpClient;
        //private static string prevHash = "0000000000000000000000000000000000000000000000000000000000000000";
        private static int idTransaction = 1;
        private static List<Block> blocks = new List<Block>()
        {
            new Block()
            {
                Id = 1,
                Chain = new Chain (),
                Transactions = new List<TransactionChain>()
                {
                    new TransactionChain()
                    {
                        Id = 1,
                        Receiver = new Client(){
                            Id = 1,
                            Name = "Andres Camilo Rojas"
                        },
                        Sender = new Client()
                        {
                            Id= 2,
                            Name = "Jhonatan Peindao"
                        },
                        Description = "Primera Transaccion",
                        Money = 15,
                    }
                }
            }
        };

        public BlockChainService()
        {
            httpClient = new HttpClient();
        }

        /*
        private string GenerateHash(string input)
        {
            Console.WriteLine();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("x2"));
                }

                Console.WriteLine();
                return builder.ToString();
            }
        }

        private Chain GenerateChain(string data)
        {
            Console.WriteLine("GenerateChain");
            int nonce = 0;
            string hash;
            while (true)
            {
                Console.WriteLine($"nonce {nonce}");

                string input = $"{nonce}{data}{prevHash}";
                hash = GenerateHash(input);
                bool startsWith = hash.Substring(0, 4) == "0000";
                if (startsWith == true)
                {
                    break;
                }
                nonce++;
            };

            Chain chain = new Chain()
            {
                Data = data,
                Hash = (hash),
                Nonce = nonce,
                Prev = prevHash
            };

            prevHash = (hash);
            Console.WriteLine();
            return chain;
        }
        */
        public TransactionChain SaveTransaction(TransactionDTO transaction)
        {

            idTransaction += 1;
            TransactionChain transactionChain = new TransactionChain()
            {
                Id = idTransaction,
                Money = transaction.Money,
                Description = transaction.Description,
                Receiver = transaction.Receiver,
                Sender = transaction.Sender
            };

            blocks.Last().Transactions.Add(transactionChain);

            if (blocks.Last().Transactions.Count == 3)
            {
                var json = JsonConvert.SerializeObject(blocks);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var apiUrl = "http://cerrarbloque/api/CerrarBloque";

                HttpResponseMessage response = httpClient.PostAsync(apiUrl, content).Result;
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                blocks = JsonConvert.DeserializeObject<List<Block>>(jsonResponse);
                Console.WriteLine();
            }
            return transactionChain;
        }

        public float QueryTotal(int clientId)
        {
            float receivedMoney = (float)blocks
                .SelectMany(block => block.Transactions)
                .Where(transaction => transaction.Receiver.Id == clientId)
                .Sum(transaction => transaction.Money);

            float sentMoney = (float)blocks
            .SelectMany(block => block.Transactions)
            .Where(transaction => transaction.Sender?.Id == clientId)
            .Sum(transaction => transaction.Money);

            float balance = receivedMoney - sentMoney;
            return balance;
        }

        public void CerrarChain(Block block)
        {
            string blockString = block.ToString();
            //var Chain = GenerateHash(blockString);
            //block.Chain=Chain;

            //blocks.Last()=block;
            //block.Add(new BlockChain ())
        }

        public async Task<List<Block>> GetBlockChain()
        {
            return blocks;
        }

        public void SetBlockChain(List<Block> _blocks)
        {
            blocks = _blocks;
        }

    }

    public interface IBlockChainService
    {
        TransactionChain SaveTransaction(TransactionDTO transaction);
        float QueryTotal(int clientId);
        Task<List<Block>> GetBlockChain();
        void SetBlockChain(List<Block> _blocks);
    }
}
