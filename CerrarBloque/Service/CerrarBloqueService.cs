using BlockModel.Model;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace CerrarBloque.Service
{
    public class CerrarBloqueService : ICerrarBloqueService
    {

        private static string prevHash = "0000000000000000000000000000000000000000000000000000000000000000";
        private static int idBlock = 1;
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
            return chain;
        }

        public List<Block> CerrarBlock(List<Block> blocks) {

            idBlock += 1;
            string jsonString = JsonConvert.SerializeObject(blocks.Last());
            Chain chain = GenerateChain(jsonString);
            blocks.Last().Chain = chain;
            var blockNew = new Block();
            blockNew.Chain = new Chain();
            blockNew.Transactions = new List<TransactionChain>();
            blockNew.Id = idBlock;
            blocks.Add(blockNew);
            return blocks;

        }

    }

    public interface ICerrarBloqueService
    {
        List<Block> CerrarBlock(List<Block> blocks);
    }

}
