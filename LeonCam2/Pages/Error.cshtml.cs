// Error.cshtml.cs by Gradzka & Kazimierczak

namespace LeonCam2.Pages
{
    using System.Diagnostics;
    using LeonCam2.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class Error : PageModel
    {
        private readonly ILogger<Error> logger;

        public Error(ILogger<Error> logger)
        {
            this.logger = logger;
        }

        public string RequestId { get; set; }

        public bool ShowRequestId => !this.RequestId.IsNullOrEmpty();

        public void OnGet()
        {
            this.RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier;
        }
    }
}
