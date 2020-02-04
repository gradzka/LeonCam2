// ErrorDetails.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    using Newtonsoft.Json;

    public class ErrorDetails
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
