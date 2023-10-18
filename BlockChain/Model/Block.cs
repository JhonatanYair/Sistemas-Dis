namespace BlockChain.Model
{
    public class Block
    {

        public int Id { get; set; }
        public List<Transaction> Transactions { get; set; }
        public bool IsOpen { get; set; } = true;

    }
}
