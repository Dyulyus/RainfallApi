using Newtonsoft.Json;
using System.Text.Json;

namespace RainfallApi.Model
{
    public class Item
    {
        [JsonProperty("@id")]
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Measure { get; set; }
        public decimal Value { get; set; }
    }
    public class RootObject
    {
        public List<Item> Items { get; set; }
    }

}
