using dotnet.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace dotnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        static HttpClient client = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public class IpResponse
        {
            public string? Ip { get; set; }
            public string? Country { get; set; }
            public string? City { get; set; }
            public string? Region { get; set; }

        }
        public async Task<IActionResult> IndexAsync()
        {
            HttpContext context = HttpContext;
            string ip = string.Empty;
            if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"]))
            {
                ip = context.Request.Headers["X-Forwarded-For"];
            }
            else
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                ip = context.Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            ViewBag.ClientIp = ip;
            ViewBag.Host = context.Request?.Host;
            ViewBag.Location = await GetLocation(ip);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<String> GetLocation(string ip)
        {
            IpResponse ipResponse = new()
            {
                Ip = ip,
                City = "",
                Country = "",
                Region = ""
            };
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri("https://ipinfo.io/");
            }
            
            HttpResponseMessage response = await client.GetAsync(ip + "/json");
            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();
                try
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable SYSLIB0020 // Type or member is obsolete
                    ipResponse = await JsonSerializer.DeserializeAsync<IpResponse>(contentStream, options: new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
#pragma warning restore SYSLIB0020 // Type or member is obsolete
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    if (ipResponse.Country != null && ipResponse.City != null)
                    {
                        return ipResponse.Country + " " + ipResponse.City;
                    }
                    else
                        return "?";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
                catch (JsonException) // Invalid JSON
                {
                    Console.WriteLine("Invalid JSON.");
                    return "?";
                }
            }
            else return "?";
        }

    }
}