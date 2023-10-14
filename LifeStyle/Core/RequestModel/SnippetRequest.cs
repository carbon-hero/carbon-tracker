using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestModel
{
    public class SnippetRequest
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Content { get; set; }

        public SnippetRequest()
        {
            Key = "";
            Content = "";
        }

    }
}

