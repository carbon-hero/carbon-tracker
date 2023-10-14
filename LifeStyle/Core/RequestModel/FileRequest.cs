using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestModel
{
    public class FileRequest
    {
        public string Type { get; set; }
        public IFormFile File { get; set; }
      
        public FileRequest()
        {
            Type = Constants.FileType.Image;
        }
    }


    public class MultipleFileRequest
    {
        public string Type { get; set; }
        public List<IFormFile> File { get; set; }
        public MultipleFileRequest()
        {
            Type = Constants.FileType.Image;
        }
    }
}
