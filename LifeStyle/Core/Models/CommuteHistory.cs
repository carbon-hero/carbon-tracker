using GoogleApi.Entities.Places.Details.Response;
using NPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CommuteHistory
    {
        public int Id { get; set; }
        public string FromPlaceId { get; set; }
        public double FromLat { get; set; }
        public double FromLong { get; set; }
        public string ToPlaceId { get; set; }
        public double ToLat { get; set; }
        public double ToLong { get; set; }
        public decimal Distance { get; set; }
        public string DistanceMapURL { get; set; }
        public string FriendIds { get; set; }
        public double CarbonEmission { get; set; }
        public int ModeId { get; set; }
        public int TypeId { get; set; }
        public int AddedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        [ResultColumn]
        public string ModeName { get; set; }
        [ResultColumn]
        public string TypeName { get; set; }
        [ResultColumn]
        public string CreatedBy { get; set; }
        [ResultColumn]
        public string FromAddress { get; set; }
        [ResultColumn]
        public string ToAddress{ get; set; }
        [ResultColumn]
        public List<User> Friends { get; set; }
        public CommuteHistory()
        {
            FromPlaceId = "";
            ToPlaceId = "";
            FriendIds = "";
            ModeName = "";
            TypeName = "";
            DistanceMapURL = "";
            AddedOn = DateTime.UtcNow; 
            ModifiedOn = DateTime.UtcNow;
            FromAddress = "";
            ToAddress = "";
            Friends = new List<User>();
        }
    }
}
