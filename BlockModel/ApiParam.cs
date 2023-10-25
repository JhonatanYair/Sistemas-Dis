using BlockModel.Model;
using System.Text.Json.Serialization;

namespace BlockModel
{
    public class ApiParam
    {
        public virtual List<Block>? Blocks { get; set; }
        public TransactionDTO? TransactionDTO { get; set; }

    }
}
