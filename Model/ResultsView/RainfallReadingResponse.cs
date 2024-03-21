namespace RainfallApi.Model.ResultsView
{
    public class RainfallReadingResponse
    {
        public RainfallReadingResponse() { 
            Readings = new List<RainfallReading>();
        }
        public List<RainfallReading> Readings { get; set; }



    }
}
