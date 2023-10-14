using System;
using System.Collections.Generic;
using System.Text;

namespace Core.RequestModel
{
    public class AppSettingRequest
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsPublic { get; set; }
        public AppSettingRequest()
        {
            Key = "";
            Value = "";
            IsPublic = false;
        }
    }
}

