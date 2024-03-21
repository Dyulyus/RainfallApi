using Newtonsoft.Json;

namespace RainfallApi.Model
{

    public class RainfallReading
    {


        [JsonProperty("dateMeasured")]
        public DateTime DateMeasured { get; set; }

        [JsonProperty("amountMeasured")]
        public decimal AmountMeasured { get; set; }
    }
}
