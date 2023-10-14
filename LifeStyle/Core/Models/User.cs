using NPoco;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Core.Models
{
    [TableName("Users")]
    [PrimaryKey("Id")]
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HomeAddress {get; set; }
        public double HomeLat {get; set; }
        public double HomeLong { get; set; }
        public string OfficeAddress { get; set; }
        public double OfficeLat { get; set; }
        public double OfficeLong { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int ProfilePicId { get; set; }
        public bool IsActive { get; set; }
        public int AddedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string PhoneNumber { get; set; }
        public int TreesPlanted { get; set; }
        public string TimeZone { get; set; }
        [ResultColumn]
        public string FullName => $"{FirstName} {LastName}";
        
        [ResultColumn]
        public string Token { get; set; }
        [ResultColumn]
        public string FileUrl { get; set; }
        [ResultColumn]
        public string FullFileUrl => $"{Utility.GetBaseUrl()}{(FileUrl.IsNullOrWhiteSpace() ? Utility.Config.GetSection("DefaultImageUrl").Value : FileUrl)}";

        public User()
        {
            Token = "";
            IsActive = true;
            FirstName = "";
            Email = "";
            Password = "";
            LastName = "";
            PhoneNumber = "";
            HomeAddress = "";
            OfficeAddress = "";
            TimeZone = "";
            AddedOn = DateTime.Now;
            ModifiedOn = DateTime.Now;        
        }
    }
}





