namespace RainfallApi.Model.ResultsView
{
    public class ErrorResponse
    {


        public ErrorResponse()
        {
            Detail = new List<ErrorDetail>();
        }

        public string Message { get; set; }
        public List<ErrorDetail> Detail { get; set; }
    }
}
