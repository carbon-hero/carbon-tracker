using System;
using System.Collections.Generic;
using System.Text;

namespace Core.RequestModel
{
    public class TemplateRequest
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }

        public TemplateRequest()
        {
            Key = "";
            Content = "";
            Subject = "";
        }
    }

}

