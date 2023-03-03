using Azure.Messaging.WebPubSub;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Message = "have a nice day";
            ViewBag.ConnectionString = "Endpoint=https://pubsub-testmnb.webpubsub.azure.com;AccessKey=OhTvi1hB1NKftf9TtEgBlkC/+PXHu2HiTrmlkg3nTq4=;Version=1.0;";
            ViewBag.Hub = "myHub1";
            ViewBag.EmittedMessages = "start_";

            var pubSubClient = new WebPubSubServiceClient(
                    ViewBag.ConnectionString,
                    ViewBag.Hub);

            var url = pubSubClient.GetClientAccessUri();
            ViewBag.ClientConnection = url;

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
    }
}