namespace BlockModel.Model
{
    public class Block
    {

        public int Id { get; set; }
        public List<TransactionChain> Transactions { get; set; }
        public bool IsOpen { get; set; } = true;
        public Chain Chain {get;set;}

    }
}
