using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [TableName("Friends")]
    [PrimaryKey("Id")]
    public class Friend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
        [ResultColumn]
        public string CreatedBy { get; set; }
        [ResultColumn]
        public string FriendName { get; set; }
        public Friend()
        {
            CreatedBy = "";
            FriendName = "";
        }
    }
}


