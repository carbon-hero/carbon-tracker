using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestModel
{
    public class CommuteHistoryRequest
    {
        public int Id { get; set; }
        public string FromPlaceId { get; set; }
        public double FromLat { get; set; }
        public double FromLong { get; set; }
        public string ToPlaceId { get; set; }
        public double ToLat { get; set; }
        public double ToLong { get; set; }
        public string FriendIds { get; set; }
        public int ModeId { get; set; }
        public int TypeId { get; set; }

        public CommuteHistoryRequest()
        {
            FromPlaceId = "";
            ToPlaceId = "";
            FriendIds = "";
        }
    }
}

