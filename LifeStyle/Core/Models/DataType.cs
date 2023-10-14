using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [TableName("DataTypes")]
    [PrimaryKey("Id")]
    public class DataType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public DataType()
        {
            Name = "";
            AddedOn= DateTime.UtcNow; 
            ModifiedOn= DateTime.UtcNow;
        }
    }
}
