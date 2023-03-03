using Azure.Messaging.WebPubSub;
using Microsoft.AspNetCore.Mvc;
using System;
using static WebApplication1.Controllers.Breakfast;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreakfastSynchronousController : ControllerBase
    {

        private readonly WebPubSubServiceClient _webPubSubServiceClient;
        private readonly IConfiguration _configuration;

        public BreakfastSynchronousController(IConfiguration configuration)
        {
            _webPubSubServiceClient = new WebPubSubServiceClient(configuration.GetValue<string>("pubsub-connection"), "myHub1");
        }


        // GET: api/<BreakfastController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _webPubSubServiceClient.SendToAll("Starting Stopwatch....");

            Console.WriteLine("The stopwatch, press S to begin and Q to stop");
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            var done = false;


            _webPubSubServiceClient.SendToAll("Starting Sync Breakfast....");
            //return new string[] { "value1", "value2" };
            Coffee cup = Breakfast.PourCoffee();
            Console.WriteLine("coffee is ready");
            _webPubSubServiceClient.SendToAll("coffee is ready");

            _webPubSubServiceClient.SendToAll("starting eggs....");
            Egg eggs = FryEggs(2);
            Console.WriteLine("eggs are ready");
            _webPubSubServiceClient.SendToAll("eggs are ready");

            _webPubSubServiceClient.SendToAll("starting bacon....");
            Bacon bacon = FryBacon(3);
            Console.WriteLine("bacon is ready");
            _webPubSubServiceClient.SendToAll("bacon is ready");

            _webPubSubServiceClient.SendToAll("starting toast....");
            Toast toast = ToastBread(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");
            _webPubSubServiceClient.SendToAll("toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");
            _webPubSubServiceClient.SendToAll("oj is ready");
            _webPubSubServiceClient.SendToAll("Breakfast is ready");

            stopWatch.Stop();
            done = true;

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value. 
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 2);
            Console.WriteLine("RunTime " + elapsedTime);

            _webPubSubServiceClient.SendToAll("RunTime " + elapsedTime);

            return new string[] { "value1", "value2" };
        }

    }

    static class Breakfast{

        internal class Bacon { }
        internal class Coffee { }
        internal class Egg { }
        internal class Juice { }
        internal class Toast { }


        public static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        public static void ApplyJam(Toast toast) =>
            Console.WriteLine("Putting jam on the toast");

        public static void ApplyButter(Toast toast) =>
            Console.WriteLine("Putting butter on the toast");

        public static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        public static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            Task.Delay(3000).Wait();
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        public static Egg FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            Task.Delay(3000).Wait();
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        public static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }

    }
}
