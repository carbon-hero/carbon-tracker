using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestModel
{
    public class DataTypeRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DataTypeRequest()
        {
            Name = "";
        }
    }
}
