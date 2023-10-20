using BlockModel.Model;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace BlockChain.Service
{
    public class BlockChainService : IBlockChainService
    {

        private readonly HttpClient httpClient;
        //private static string prevHash = "00004f6470fe351ba7a7c3754788362e8010018f7316a2a64981975253e60498";
        private static string prevHash = "0000000000000000000000000000000000000000000000000000000000000000";
        private static int idTransaction = 1;
        private static int idBlock = 1;
        private static List<Block> blocks = new List<Block>()
        {
            new Block()
            {
                Id = 1,
                /*Chain = new Chain()
                {
                    Data = 10,
                    Nonce = 70964,
                    Prev = ("0000000000000000000000000000000000000000000000000000000000000000"),
                    Hash = ("00004f6470fe351ba7a7c3754788362e8010018f7316a2a64981975253e60498"),
                },*/

                Transactions = new List<TransactionChain>()
                {
                    new TransactionChain()
                    {
                        Id = 1,
                        Receiver = new Client(){
                            Id = 1,
                            Name = "Andres Camilo Rojas"
                        },
                        /*Chain = new Chain()
                        {
                            Data = 10,
                            Nonce = 70964,
                            Prev = ("0000000000000000000000000000000000000000000000000000000000000000"),
                            Hash = ("00004f6470fe351ba7a7c3754788362e8010018f7316a2a64981975253e60498"),
                        }*/
                    }
                }
            }
        };

        public BlockChainService()
        {
            httpClient = new HttpClient();
        }

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

        public TransactionChain SaveTransaction(TransactionDTO transaction)
        {
            Console.WriteLine();
            /*
            var chainTransaction = GenerateChain(transaction.Money);
            if (blocks.Last().IsOpen == false)
            {
                blocks.Add(new Block() { Id = blocks.Last().Id++ });
            }*/

            TransactionChain transactionChain = new TransactionChain()
            {
                Id = idTransaction++,
                //Chain = chainTransaction,
                Description = transaction.Description,
                Receiver = transaction.Receiver,
                Sender = transaction.Sender
            };

            blocks.Last().Transactions.Add(transactionChain);

            if (blocks.Last().Transactions.Count == 5)
            {
                /*var json = JsonConvert.SerializeObject(blocks);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var apiUrl = "http://localhost:5172/api/CerrarBloque";

                HttpResponseMessage response = httpClient.PostAsync(apiUrl, content).Result;
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                blocks = JsonConvert.DeserializeObject<List<Block>>(jsonResponse);*/
            }
            Console.WriteLine();
            return transactionChain;
        }

        public float QueryTotal(int clientId)
        {
            float receivedMoney = (float)blocks
                .SelectMany(block => block.Transactions)
                .Where(transaction => transaction.Receiver.Id == clientId)
                .Sum(transaction => transaction.Chain.Data);

            float sentMoney = (float)blocks
            .SelectMany(block => block.Transactions)
            .Where(transaction => transaction.Sender?.Id == clientId)
            .Sum(transaction => transaction.Chain.Data);

            float balance = receivedMoney - sentMoney;
            Console.WriteLine(balance);
            Console.WriteLine(balance);
            return balance;
        }

        public void CerrarChain(BlockChain block)
        {

            string blockString = block.ToString();
            var Chain = GenerateHash(blockString);
            block.Chain=Chain;

            blocks.Last()=block;
            block.Add(new BlockChain ())

        }

        public List<Block> GetBlockChain()
        {
            Console.WriteLine();
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
        List<Block> GetBlockChain();
        void SetBlockChain(List<Block> _blocks);
    }
}
