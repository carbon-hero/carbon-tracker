using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [TableName("Snippets")]
    [PrimaryKey("Id")]
    public class Snippet
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Content { get; set; }
        public int AddedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime AddedOn { get; set; }
        [ResultColumn]
        public string FirstName { get; set; }
        [ResultColumn]
        public string LastName { get; set; }
        [ResultColumn]
        public string FullName => $"{FirstName} {LastName}";
        public Snippet()
        {
            Key = "";
            Content = "";
        }

    }
}
