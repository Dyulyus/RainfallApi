using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RainfallApi.Model;
using RainfallApi.Model.ResultsView;
using System.Text.Json.Serialization;

namespace RainfallApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RainfallController : ControllerBase
    {

        private readonly ILogger<RainfallController> _logger;

        public RainfallController(ILogger<RainfallController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/rainfall/id/{stationId}/readings", Name = "get-rainfall")]
        public async Task<ActionResult<IEnumerable<RainfallReading>>> GetRainfallReadingsAsync(string stationId, [FromQuery] int count = 10)
        {
            if (count <= 0 || count > 100)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.Detail.Add(new ErrorDetail { Message = "count should be between 1 to 100", PropertyName = "Count" });
                errorResponse.Message = "Invalid Request";
                return BadRequest(errorResponse);

            }

            try
            {


                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"https://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/readings?latest&_limit={count}";

                    HttpResponseMessage responseMessage = client.GetAsync(apiUrl).Result;


                    if (responseMessage.IsSuccessStatusCode)
                    {
                        string responseBody = await responseMessage.Content.ReadAsStringAsync();

                        RootObject data = JsonConvert.DeserializeObject<RootObject>(responseBody);

                        RainfallReadingResponse readingResponse = new RainfallReadingResponse();



                        data.Items.ForEach(x =>
                        readingResponse.Readings.Add(new RainfallReading { AmountMeasured = x.Value, DateMeasured = x.DateTime })
                        );

                        if (!readingResponse.Readings.Any())
                        {

                            ErrorResponse errorResponse = new ErrorResponse();
                            errorResponse.Message = "No Data Found";
                            return NotFound(errorResponse);
                        }

                        return Ok(readingResponse);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.Message = "Server Error";
                return StatusCode(500, errorResponse);
            }

            return Ok();
        }

    }
}
