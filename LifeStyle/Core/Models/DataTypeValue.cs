using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [TableName("DataTypeValues")]
    [PrimaryKey("Id")]
    public class DataTypeValue
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Value { get; set; }
        public int AddedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        [ResultColumn]
        public string DataTypeName { get; set; }

        public DataTypeValue()
        {
            Value = "";
            DataTypeName = "";
        }  
    }
}
