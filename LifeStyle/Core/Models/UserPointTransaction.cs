using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [TableName("UserPointTransactions")]
    [PrimaryKey("Id")]
    public class UserPointTransaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TransactionType { get; set; }
        public int Points { get; set; }
        public int AddedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime TransactionDate { get; set; }
        [ResultColumn]
        public string TransactionDateValue => TransactionDate.ToString("MMMM dd yyyy");

        [ResultColumn]
        public int Balance { get; set; }
        public UserPointTransaction()
        {
            TransactionType = Constants.TransactionType.Earned.ToString();
            AddedOn = DateTime.UtcNow; 
            ModifiedOn = DateTime.UtcNow;    
        }
    }

    public class BalanceResponse
    {
        public int Earned { get; set; }
        public int Spent { get; set; }
        public int Balance { get; set; }
    }
}
