using AgileObjects.AgileMapper.Extensions;
using Core.Models;
using Core.RequestModel;
using Geocoding;
using NPoco;
using StackExchange.Profiling.Internal;
using System.Xml.Linq;

namespace LifeStyle.Services
{
    public class CommuteHistoryService
    {

        public CommuteHistory Get(int id)
        {
            var data = Common.Instances.CommuteHistoryInst.Get(id);
            FillData(data);
            return data;
        }

        public Page<CommuteHistory> GetAll(int pageNumber, int pageSize, int userId, string query = "")
        {
            var data = Common.Instances.CommuteHistoryInst.GetAll(pageNumber, pageSize, userId, query);
            foreach(var item in data.Items.ToList())
            {
               FillData(item);
            }
            return data;
        }


        public CommuteHistory FillData(CommuteHistory data)
        {
            if (!data.FromPlaceId.IsNullOrWhiteSpace())
            {
                var fromPlaceDetails = GoogleService.GetPlaceDetails(data.FromPlaceId);
                data.FromAddress = fromPlaceDetails.Vicinity.IsNullOrWhiteSpace() ? fromPlaceDetails.Geometry.Location.Address : fromPlaceDetails.Vicinity;
            }
            if (!data.ToPlaceId.IsNullOrWhiteSpace())
            {
                var toPlaceDetails = GoogleService.GetPlaceDetails(data.ToPlaceId);
                data.ToAddress = toPlaceDetails.Vicinity.IsNullOrWhiteSpace() ? toPlaceDetails.Geometry.Location.Address : toPlaceDetails.Vicinity;
            }
            if (!data.FriendIds.IsNullOrWhiteSpace())
                data.Friends = Common.Instances.UserInst.GetUserByIds(data.FriendIds);
            return data;
        }

        public CommuteHistory Save(CommuteHistoryRequest req, int meId)
        {
            Common.Log.Data($"Saving commute history..", $"Req:-{req}, meId:-{meId}");
            var model = new CommuteHistory();
            if (req.Id > 0) model = Common.Instances.CommuteHistoryInst.Get(req.Id);
            model = req.Map().Over(model);
            if (model.Id <= 0)
            {
                model.AddedOn = DateTime.UtcNow;
                model.AddedBy = meId;
            }
            var distanceData = GoogleService.GetDistanceMatrix(req.FromLat, req.ToLong, req.ToLat, req.ToLong);
            if (distanceData != null)
                model.Distance = distanceData.Distance;

            model.DistanceMapURL = GoogleService.GetFullMapRouteMapUrl(req.FromLat, req.FromLong, req.ToLat, req.ToLong);

            int peopleCount = 0;
            if(!req.FriendIds.IsNullOrWhiteSpace())
                peopleCount = req.FriendIds.Split(',').Select(int.Parse).ToList().Count();

            var mode = Common.Instances.DataTypeInst.Get(req.ModeId);
            model.CarbonEmission = CarbonOffsetService.CalculateOffsetForTravel(Convert.ToDouble(model.Distance), peopleCount, mode.Name.ToLower());

            model.ModifiedOn = DateTime.UtcNow;
            model.ModifiedBy = meId;
            Common.Instances.CommuteHistoryInst.Save(model);
            return model;
        }
    }
}

