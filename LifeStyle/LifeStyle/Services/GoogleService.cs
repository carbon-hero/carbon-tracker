using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Geocoding;
using Geocoding.Google;
using GoogleApi;
using GoogleApi.Entities.Maps.Common;
using GoogleApi.Entities.Maps.Common.Enums;
using GoogleApi.Entities.Maps.Directions.Request;
using GoogleApi.Entities.Maps.DistanceMatrix.Request;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi.Entities.Places.Common;
using GoogleApi.Entities.Places.Details.Request;
using GoogleApi.Entities.Places.Details.Request.Enums;
using GoogleApi.Entities.Places.Details.Response;
using GoogleApi.Entities.Places.Search.Common.Enums;
using GoogleApi.Entities.Places.Search.NearBy.Request;
using GoogleApi.Entities.Places.Search.NearBy.Request.Enums;
using GoogleApi.Entities.Places.Search.NearBy.Response;
using Location = GoogleApi.Entities.Common.Location;
using static GoogleApi.Entities.Places.Details.Request.Enums.FieldTypes;
using StackExchange.Profiling.Internal;
using Core;

namespace LifeStyle.Services
{
    public class GoogleService
    {
        protected static string GOOGLE_API_KEY => new AppSettingService().GetWithDetails("GOOGLE_API_KEY");
        public static DetailsResult GetPlaceDetails(string placeId)
        {
            var req = new PlacesDetailsRequest
            {
                Key = GOOGLE_API_KEY,
                PlaceId = placeId,
                Fields = Basic & Contact & Atmosphere & Website,
            };
            var data = GooglePlaces.Details.Query(req);
            return data.Result;
        }
        public static IEnumerable<NearByResult> GetNearByPlace(double latitude, double longitude, bool isExpandRadius, string query)
        {
            var req = new PlacesNearBySearchRequest
            {
                Key = GOOGLE_API_KEY,
                Location = new GoogleApi.Entities.Places.Search.NearBy.Request.Location(latitude, longitude),
                Radius = 5000
            };
            if (isExpandRadius)
                req.Radius = 10000;
            if (!query.IsNullOrWhiteSpace()) 
                req.Keyword = query;
            var data = GooglePlaces.NearBySearch.Query(req);
            return data.Results;
        }

        public static string GetFullMapRouteMapUrl(double startLat, double startLon, double endLat, double endLon)
        {
            var markers = $"{startLat},{startLon}|{endLat},{endLon}";
            return  $"https://maps.googleapis.com/maps/api/staticmap?size=400x200&key={GOOGLE_API_KEY}&markers={markers}";
        }

        public class LocationInfo
        {
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Address { get; set; }
            public string ZipCode { get; set; }
            public string Name { get; set; }
            public LocationInfo()
            {
                Country = "";
                State = "";
                City = "";
                ZipCode = "";
                Address = "";
            }
        }
        public class DistanceData
        {
            public decimal Distance { get; set; }
            public string DistanceStr { get; set; }
            public int DurationSeconds { get; set; }
            public int DurationMins => DurationSeconds / 60;
            public string DurationStr { get; set; }
            public decimal Duration { get; set; }
            public string DistanceFormatted => $"{Distance} miles";
            public string DurationFormatted { get; set; }
        }


        public static DistanceData GetDistanceMatrix(double startLat, double startLon, double endLat, double endLon)
        {
            var key = $"GetDistanceMatrix-{startLat}-{startLon}-{endLat}-{endLon}";
            if (Cache.Exists(key)) return (DistanceData)Cache.Get(key);
            var req = new DistanceMatrixRequest();
            var distanceData = new DistanceData();
            req.Key = GOOGLE_API_KEY;
            req.Units = GoogleApi.Entities.Maps.Common.Enums.Units.Imperial;
            var origins = new List<GoogleApi.Entities.Common.Location> { new GoogleApi.Entities.Common.Location { Latitude = startLat, Longitude = startLon } };
            req.Origins = origins;

            var dest = new List<GoogleApi.Entities.Common.Location> { new GoogleApi.Entities.Common.Location { Latitude = endLat, Longitude = endLon } };
            req.Destinations = dest;
            var res = GoogleApi.GoogleMaps.DistanceMatrix.Query(req);
            if (res?.Rows != null)
            {
                var data = res.Rows.FirstOrDefault()?.Elements.FirstOrDefault();
                if (data != null)
                {
                    distanceData.Distance = data.Distance?.Value ?? 0;
                    distanceData.DistanceStr = data.Distance?.Text ?? "";
                    distanceData.DurationSeconds = data.Duration?.Value ?? 0;
                    distanceData.DurationStr = data.Duration?.Text ?? "";
                    //convert to miles
                    distanceData.Distance = Convert.ToDecimal(UnitsNet.Length.FromMeters(distanceData.Distance).Miles);

                    //CONVERT TO HOURS IF MORE THAN 60 MINS
                    if (distanceData.DurationMins > 60)
                    {
                        distanceData.Duration = Convert.ToDecimal(UnitsNet.Duration.FromSeconds(distanceData.DurationSeconds).Hours);
                        distanceData.DurationFormatted = $"{distanceData.Duration} hrs";
                    }
                    else
                    {
                        distanceData.DurationFormatted = $"{distanceData.Duration} mins";
                    }
                }
            }

            Cache.Set(key, distanceData);
            return (DistanceData)Cache.Get(key);
        }
        public async Task<LocationInfo> GetLocationInfo(double lat, double lon)
        {
            var geoCoder = new GoogleGeocoder() { ApiKey = GOOGLE_API_KEY };
            var addresses = await geoCoder.ReverseGeocodeAsync(lat, lon);
            var loc = new LocationInfo();
            foreach (var address in addresses)
            {
                foreach (var cmp in address.Components)
                {
                    foreach (var type in cmp.Types)
                    {
                        if (type == GoogleAddressType.Country)
                        {
                            loc.Country = cmp.LongName ?? "";
                        }
                        else if (type == GoogleAddressType.AdministrativeAreaLevel1)
                        {
                            loc.State = cmp.LongName ?? "";
                        }
                        else if (type == GoogleAddressType.Locality)
                        {
                            loc.City = cmp.LongName ?? "";
                        }
                        else if (type == GoogleAddressType.PostalCode)
                        {
                            loc.ZipCode = cmp.LongName ?? "";
                        }
                        
                    }
                }
                if(address.Type == GoogleAddressType.Route)
                     loc.Address = address.FormattedAddress;
            }
            return loc;
        }

      
    }
}
