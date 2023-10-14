using NPoco;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    [TableName("AppSettings")]
    [PrimaryKey("Id")]
    public class AppSetting
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int AddedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime AddedOn { get; set; }
        public bool IsPublic { get; set; }
        public AppSetting()
        {
            Key = "";
            Value = "";
            IsPublic = false;
            AddedOn = DateTime.Now;
            ModifiedOn = DateTime.Now;
        }
    }
}


