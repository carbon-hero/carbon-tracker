using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestModel
{
    public class DataTypeValueRequest
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Value { get; set; }

        public DataTypeValueRequest()
        {
            Value = "";
        }
    }
}
