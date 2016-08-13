using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace MyTravel.Services
{
    public class GeoCoordsService
    {
        private ILogger<GeoCoordsService> _logger;
        private IConfigurationRoot _config;

        public GeoCoordsService(IConfigurationRoot config, ILogger<GeoCoordsService> logger)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<GeoCoordsResult> GetGoogleMapCoordsAsync(string address)
        {
            var result = new GeoCoordsResult
            {
                Success = false,
                Message = "Failed to get Coordinates"
            };

            var encodeAddr = WebUtility.UrlEncode(address);
            var url = $"http://maps.googleapis.com/maps/api/geocode/json?address={encodeAddr}&language=zh-TW";
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            var results = JObject.Parse(json);

            if (!results["results"].HasValues)
            {
                result.Message = $"Could not find '{address}' as a location";
            }
            else
            {
                result.Latitude = (double)results["results"][0]["geometry"]["location"]["lat"];
                result.Longitude = (double)results["results"][0]["geometry"]["location"]["lng"];
                result.Success = true;
                result.Message = "Success";
            }

            return result;
        }

        public async Task<GeoCoordsResult> GetCoordsAsync(string name)
        {
            var result = new GeoCoordsResult
            {
                Success = false,
                Message = "Failed to get Coordinates"
            };
            var apiKey = _config["Keys:BingKey2"];
            var endcodeName = WebUtility.UrlEncode(name);

            var url = $"http://dev.virtualearch.net/REST/v1/Locations?q={endcodeName}&key={apiKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            // Read out the results
            // Fragile, might need to change if the Bing API changes
            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];
            if (!results["resourceSets"][0]["resources"].HasValues)
            {
                result.Message = $"Could not find '{name}' as a location";
            }
            else
            {
                var confidence = (string)resources[0]["confidence"];
                if (confidence != "High")
                {
                    result.Message = $"Could not find a confident match for '{name}' as a location";
                }
                else
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double)coords[0];
                    result.Longitude = (double)coords[1];
                    result.Success = true;
                    result.Message = "Success";
                }
            }
            return result;
        }
    }
}