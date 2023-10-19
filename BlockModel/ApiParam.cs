using BlockModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModel
{
    public class ApiParam
    {
        public List<Block> Blocks { get; set; }
        public TransactionDTO? TransactionDTO { get; set; }

    }
}
