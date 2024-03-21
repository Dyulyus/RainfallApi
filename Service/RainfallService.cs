using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RainfallApi.Model;
using RainfallApi.Model.ResultsView;

namespace RainfallApi.Service
{

    public interface IRainfallService
    {
        Task<ActionResult<IEnumerable<RainfallReading>>> GetRainfallReadingsAsync(string stationId, int count = 10);
    }

    public class RainfallService : IRainfallService
    {
        private readonly ILogger<RainfallService> _logger;
        private readonly HttpClient _httpClient;

        public RainfallService(ILogger<RainfallService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<ActionResult<IEnumerable<RainfallReading>>> GetRainfallReadingsAsync(string stationId, int count = 10)
        {
            try
            {
                if (count <= 0 || count > 100)
                {
                    ErrorResponse errorResponse = new ErrorResponse
                    {
                        Message = "Invalid Request",
                        Detail = { new ErrorDetail { Message = "Count should be between 1 to 100", PropertyName = "Count" } }
                    };
                    return new BadRequestObjectResult(errorResponse);
                }

                string apiUrl = $"https://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/readings?latest&_limit={count}";
                HttpResponseMessage responseMessage = await _httpClient.GetAsync(apiUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseBody = await responseMessage.Content.ReadAsStringAsync();
                    RootObject data = JsonConvert.DeserializeObject<RootObject>(responseBody);

                    var readings = data.Items.Select(x => new RainfallReading { AmountMeasured = x.Value, DateMeasured = x.DateTime });

                    if (!readings.Any())
                    {
                        ErrorResponse errorResponse = new ErrorResponse { Message = "No Data Found" };
                        return new NotFoundObjectResult(errorResponse);
                    }

                    return new OkObjectResult(readings);
                }
                else
                {
                    ErrorResponse errorResponse = new ErrorResponse { Message = "Server Error" };
                    return new StatusCodeResult(500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching rainfall readings");
                ErrorResponse errorResponse = new ErrorResponse { Message = "An unexpected error occurred" };
                return new StatusCodeResult(500);
            }
        }
    }
}
