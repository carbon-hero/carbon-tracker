using NPoco;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    [TableName("Templates")]
    [PrimaryKey("Id")]
    public class Template
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public int AddedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime AddedOn { get; set; }
        [ResultColumn]
        public string FirstName { get; set; }
        [ResultColumn]
        public string LastName { get; set; }
        [ResultColumn]
        public string FullName => $"{FirstName} {LastName}";

        public Template()
        {
            Key = "";
            Content = "";
            Subject = "";
        }
    }
}



