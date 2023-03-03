using Azure.Messaging.WebPubSub;
using Azure;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;
using Microsoft.AspNetCore.SignalR;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable Get()
        {

            Thread.Sleep(1000); //will sleep for 5 sec

            Random rnd = new Random();
            int num = rnd.Next();

            //  var serviceClient = new WebPubSubServiceClient(new Uri("https://pubsub-testmnb.webpubsub.azure.com"), "myHub1", new AzureKeyCredential("OhTvi1hB1NKftf9TtEgBlkC/+PXHu2HiTrmlkg3nTq4="));
            var serviceClient = new WebPubSubServiceClient("Endpoint=https://pubsub-testmnb.webpubsub.azure.com;AccessKey=OhTvi1hB1NKftf9TtEgBlkC/+PXHu2HiTrmlkg3nTq4=;Version=1.0;", "myHub1");
            serviceClient.SendToAll("message from swagger endpoint " + num.ToString());

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
