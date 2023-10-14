using NPoco;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    [TableName("Files")]
    [PrimaryKey("Id")]
    public class File
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string Type { get; set; }
        [ResultColumn]
        public string FullFileUrl => $"{Utility.GetBaseUrl()}{(FileUrl.IsNullOrWhiteSpace() ? Utility.Config.GetSection("DefaultImageUrl").Value : FileUrl)}";
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        [ResultColumn]
        public string CreatedName { get; set; }
        public File()
        {
            FileName = "";
            FileUrl = "";
            CreatedName = "";
            Type = "image";
            AddedOn= DateTime.Now;
            ModifiedOn= DateTime.Now;
        }

    }

}

