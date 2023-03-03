using Azure.Messaging.WebPubSub;
using Microsoft.AspNetCore.Mvc;
using static WebApplication1.Controllers.Breakfast2Async;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreakfastAsynchronousController : ControllerBase
    {

        private readonly WebPubSubServiceClient _webPubSubServiceClient;
        private readonly IConfiguration _configuration;

        public BreakfastAsynchronousController(IConfiguration configuration)
        {
            _webPubSubServiceClient = new WebPubSubServiceClient(configuration.GetValue<string>("pubsub-connection"), "myHub1");
        }


        // GET: api/<BreakfastAsync>
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            _webPubSubServiceClient.SendToAll("Starting Stopwatch....");
            Console.WriteLine("The stopwatch, press S to begin and Q to stop");
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            var done = false;


            _webPubSubServiceClient.SendToAll("Starting Async Breakfast....");

            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            _webPubSubServiceClient.SendToAll("starting eggs, bacon and toast asyncronously....");
            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = MakeToastWithButterAndJamAsync(2);
            var cntStart = 3;
            var cntCur = 0;
            var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };
            while (breakfastTasks.Count > 0)
            {
                cntCur = breakfastTasks.Count;
                Task finishedTask = await Task.WhenAny(breakfastTasks);
                if (finishedTask == eggsTask)
                {
                    Console.WriteLine("eggs are ready");
                    _webPubSubServiceClient.SendToAll("eggs are ready");
                }
                else if (finishedTask == baconTask)
                {
                    Console.WriteLine("bacon is ready");
                    _webPubSubServiceClient.SendToAll("bacon is ready");
                }
                else if (finishedTask == toastTask)
                {
                    Console.WriteLine("toast is ready");
                    _webPubSubServiceClient.SendToAll("toast is ready");
                }
                await finishedTask;
                breakfastTasks.Remove(finishedTask);

              //  if(cntCur

            }

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");
            _webPubSubServiceClient.SendToAll("oj is ready");
            _webPubSubServiceClient.SendToAll("<strong>Breakfast is ready!</strong>");

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

    static class Breakfast2Async
    {

        internal class Bacon { }
        internal class Coffee { }
        internal class Egg { }
        internal class Juice { }
        internal class Toast { }


        public static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var toast = await ToastBreadAsync(number);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }

        public static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        public static void ApplyJam(Toast toast) =>
            Console.WriteLine("Putting jam on the toast");

        public static void ApplyButter(Toast toast) =>
            Console.WriteLine("Putting butter on the toast");

        public static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            await Task.Delay(3000);
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        public static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(3000);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            await Task.Delay(3000);
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        public static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000);
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(3000);
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
