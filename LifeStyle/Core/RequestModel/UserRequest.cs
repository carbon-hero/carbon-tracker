using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestModel
{
    public class UserRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HomeAddress { get; set; }
        public double HomeLat { get; set; }
        public double HomeLong { get; set; }
        public string OfficeAddress { get; set; }
        public double OfficeLat { get; set; }
        public double OfficeLong { get; set; }
        public int ProfilePicId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TimeZone { get; set; }
        public UserRequest()
        {
            FirstName = "";
            LastName = "";
            Email = "";
            Password = "";
            HomeAddress = "";
            OfficeAddress = "";
            OfficeAddress = "";
            TimeZone = "";
        }
    }
}
