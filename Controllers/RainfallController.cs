using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RainfallApi.Model;
using RainfallApi.Model.ResultsView;
using RainfallApi.Service;
using System.Text.Json.Serialization;

namespace RainfallApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallService _rainfallService;



        public RainfallController(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService;
        }

        [HttpGet("/rainfall/id/{stationId}/readings", Name = "get-rainfall")]
        public async Task<ActionResult<IEnumerable<RainfallReading>>> GetRainfallReadingsAsync(string stationId, [FromQuery] int count = 10)
        {
            return await _rainfallService.GetRainfallReadingsAsync(stationId, count);
        }
    }
}
