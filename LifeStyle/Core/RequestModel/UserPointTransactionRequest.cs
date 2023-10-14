using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestModel
{
    public class UserPointTransactionRequest
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public int Points { get; set; }
        public UserPointTransactionRequest()
        {
            TransactionType = Constants.TransactionType.Earned.ToString();
        }
    }
}
