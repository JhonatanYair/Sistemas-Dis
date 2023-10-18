using BlockChain.Model;
using System.Text;
using System.Security.Cryptography;

namespace BlockChain.Service
{
    public class BlockChainService : IBlockChainService
    {

        private readonly HttpClient httpClient;
        private static Guid prevHash = Guid.Parse("00004f6470fe351ba7a7c3754788362e8010018f7316a2a64981975253e60498");
        private static int idTransction = 1;
        private static List<Block> blocks = new List<Block>()
        {
            new Block()
            {
                Id = 1,
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Id = 1,
                        Receiver = new Client(){
                            Id = 1,
                            Name = "Andres Camilo Rojas"
                        },
                        Chain = new Chain()
                        {
                            Data = 10,
                            Nonce = 70964,
                            Prev = Guid.Parse("0000000000000000000000000000000000000000000000000000000000000000"),
                            Hash = Guid.Parse("00004f6470fe351ba7a7c3754788362e8010018f7316a2a64981975253e60498"),
                        }
                    }
                }
            }
        };

        public BlockChainService()
        {
            /*
             
            httpClient = new HttpClient();
            List<Buzon> list = new List<Buzon>();
            var apiUrl = "https://localhost:7074/api/EP0015ListadoBuzones";
            var response = await httpClient.GetStringAsync(apiUrl);
             
            string json = JsonConvert.SerializeObject(apiResponse, Formatting.Indented);
            //Console.WriteLine(json);
            list = JsonConvert.DeserializeObject<List<Buzon>>(apiResponse);

             */
        }

        private string GenerateHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private Chain GenerateChain(float data)
        {
            int nonce = 0;
            string hash;
            do
            {
                string input = $"{nonce}{data}{prevHash}";
                hash = GenerateHash(input);
                bool startsWith = input.StartsWith("0000");

                if (startsWith == true)
                {
                    break;
                }
                nonce++;
            } while (true);

            Chain chain = new Chain()
            {
                Data = data,
                Hash = Guid.Parse(hash),
                Nonce = nonce,
                Prev = prevHash
            };

            prevHash = Guid.Parse(hash);
            return chain;
        }

        public void SaveTransaction(TransactionDTO transaction)
        {

            var chainTransaction =  GenerateChain(transaction.Money);
            if (blocks.Last().IsOpen == false)
            {
                blocks.Add(new Block());
            }

            blocks.Last().Transactions.Add(new Transaction() 
            {
                Id = idTransction,
                Chain = chainTransaction,
                Description = transaction.Description,
                Receiver = transaction.Receiver,
                Sender = transaction.Sender
            });

            if (blocks.Last().Transactions.Count ==5)
            {
                //cerrar
            }

        }

        public float QueryTotal(int clientId)
        {

            int receivedMoney = (int)blocks
                .SelectMany(block => block.Transactions)
                .Where(transaction => transaction.Receiver.Id == clientId)
                .Sum(transaction => transaction.Chain.Data);

            int sentMoney = (int)blocks
            .SelectMany(block => block.Transactions)
            .Where(transaction => transaction.Sender.Id == clientId)
            .Sum(transaction => transaction.Chain.Data);

            int balance = receivedMoney - sentMoney;

            return balance;
        }

        public List<Block> GetBlockChain()
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

        void SaveTransaction(TransactionDTO transaction);
        float QueryTotal(int clientId);
        List<Block> GetBlockChain();
        void SetBlockChain(List<Block> _blocks);
    }

}
