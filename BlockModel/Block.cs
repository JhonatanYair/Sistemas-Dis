using System.Text.Json.Serialization;

namespace BlockModel.Model
{
    public class Block
    {

        public int? Id { get; set; }
        public virtual List<TransactionChain>? Transactions { get; set; }
        public Chain? Chain {get;set;}

    }
}
