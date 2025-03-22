using Application.Services.Email;
using Domain.Models;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Practice.Filters;
using Practice.LongRunningProcessSimulation;
using Services.Contracts;
using Services.Implementations;
using Services.Implementations.Cars;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Practice.Controllers;

[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(ApiCustomFilter))]
public class CheckoutController : ControllerBase
{
    static ConcurrentQueue<int> _queue = new();
    private static int _counter = 0;
    private const int _numIterations = 10;
    private static object _lockObj = new();
    private static readonly SemaphoreSlim _semaphoreLock = new SemaphoreSlim(1, 2);
    private static Mutex _mutex = new();
    private static TimeSpan _timeout = TimeSpan.FromSeconds(3);
    private readonly ICheckoutService _checkoutSer;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _environment;
    public static string ApplicationAreaHeader = "APPLICATION_AREA";

    public CheckoutController(
        IWebHostEnvironment environment,
        ICheckoutService checkoutSer,
        IHttpContextAccessor httpContextAccessor
        )
    {
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
        _checkoutSer = checkoutSer;
    }

    [HttpPost(Name = "Checkout")]
    public async Task<IActionResult> Checkout([FromBody] OrderVM order, [FromServices] IEmailService emailSer)
    {
        var appArea = GetParticularDataExample();

        var convertedOrder = new OrderModel()
        {
            OrderId = order.OrderId,
            OrderDetail = order.OrderDetail
        };

        var paymentMethod = new StripePaymentService();

        await _checkoutSer.CheckoutAsync(convertedOrder, paymentMethod);

        await emailSer.SendMailAsync();

        //assumption that process successfully!
        return Ok(new
        {
            Message = "Checkout successful"
        });
    }

    [HttpGet(Name = "Checkout")]
    public async Task<IActionResult> Checkout(string factoryType = "eco", string carType = "eco-sedan")
    {
        ICarFactory carFactory = factoryType.ToLower() switch
        {
            "lux" => new LuxuryCarFactory(),
            "eco" => new EconomyCarFactory(),
            _ => throw new ArgumentException("Invalid factory type")
        };

        ICar car = await carFactory.CreateCar(carType);
        await car.Drive();

        ICar nextCarType = await carFactory.CreateCar("eco-hatchback");
        await nextCarType.Drive();

        if (carFactory is IEconomyCarFactory economyFactory)
        {
            Console.WriteLine($"Is fuel-efficient? {await economyFactory.IsFuelEfficient()}");
        }

        //Continue creating luxury car
        factoryType = "lux";
        carType = "lux-suv";
        ICarFactory nextCarFactory = factoryType.ToLower() switch
        {
            "lux" => new LuxuryCarFactory(),
            "eco" => new EconomyCarFactory(),
            _ => throw new ArgumentException("Invalid factory type")
        };

        ICar nextCar = await nextCarFactory.CreateCar(carType);

        if (nextCarFactory is ILuxuryCarFactory luxuryCarFactory)
        {
            Console.WriteLine($"Has premium features? {await luxuryCarFactory.HasPremiumFeatures()}");
        }

        return Ok("Produced necessary cars!");
    }

    private async Task GetJsonString()
    {
        using HttpClient client = new HttpClient();
        string s = await client.GetStringAsync("https://coderbyte.com/api/challenges/json/json-cleaning");
        Console.WriteLine(CleanObject(s));
    }

    [HttpGet("json-clean-practice")]
    public async Task<string> JsonCleanPractice()
    {
        await GetJsonString();

        return "Happy ending!";
    }

    public static string WordSplit(string[] strArr)
    {

        // code goes here  
        if (strArr == null || strArr.Length != 2)
            return "not possible";

        string sequence = strArr[0];
        HashSet<string> dictionary = new HashSet<string>(strArr[1].Split(','));

        for (int i = 1; i < sequence.Length; i++)
        {
            string firstPart = sequence.Substring(0, i);
            string secondPart = sequence.Substring(i);

            if (dictionary.Contains(firstPart) && dictionary.Contains(secondPart))
            {
                return firstPart + "," + secondPart;
            }
        }

        return "not possible";

    }

    public static string CleanObject(string jsonString)
    {
        try
        {
            JObject jsonObject = JObject.Parse(jsonString);
            JObject cleanedObject = (JObject)Clean(jsonObject);
            return JsonConvert.SerializeObject(cleanedObject, Formatting.Indented);
        }
        catch (Exception)
        {
            return "Invalid JSON input";
        }
    }

    private static JToken Clean(JToken token)
    {
        if (token.Type == JTokenType.Object)
        {
            JObject obj = (JObject)token;
            JObject cleanedObject = new JObject();
            foreach (var property in obj.Properties())
            {
                JToken cleanedValue = Clean(property.Value);
                if (!IsRemovableValue(cleanedValue))
                {
                    cleanedObject[property.Name] = cleanedValue;
                }
            }
            return cleanedObject;
        }
        else if (token.Type == JTokenType.Array)
        {
            JArray array = (JArray)token;
            JArray cleanedArray = new JArray();
            foreach (JToken item in array)
            {
                JToken cleanedItem = Clean(item);
                if (!IsRemovableValue(cleanedItem))
                {
                    cleanedArray.Add(cleanedItem);
                }
            }
            return cleanedArray;
        }
        return token;
    }

    private static bool IsRemovableValue(JToken token)
    {
        return token.Type == JTokenType.String && (token.ToString() == "N/A" || token.ToString() == "-" || token.ToString() == "")
            || (token.Type == JTokenType.Array && !token.HasValues);
    }

    [HttpGet("test")]
    public async Task<string> Test()
    {
        var longRunningProcess = new LongRunningProcessFaker();

        //try
        //{
        //    var task = Task.Run(() => longRunningProcess.LongRunningProcess());
        //    await task.ContinueWith(t => HandlingSuccess(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        //}
        //catch (Exception ex)
        //{
        //    HandleFailure(ex);
        //}

        var task = Task.Run(() => longRunningProcess.LongRunningProcess());

        task.ContinueWith(t => HandlingSuccess(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        task.ContinueWith(t => HandleFailure(t.Exception!.InnerException!), TaskContinuationOptions.OnlyOnFaulted);

        await task;

        return "Test happy!";
    }

    public static void HandlingSuccess(string result)
    {
        Console.WriteLine($"Success: {result}");
    }

    public static void HandleFailure(Exception result)
    {
        Console.WriteLine($"Task failed: {result}");
    }

    public static void HandleCancel(bool result)
    {

    }

    private static async Task TestLockMechanisms()
    {
        Stopwatch sw = new Stopwatch();

        Task producer = Task.Run(() => Producer());
        Task consumer = Task.Run(() => Consumer());

        Task.WaitAll(producer, consumer);

        sw.Start();
        Parallel.Invoke(async () => await SemaphoreSlimLock(), async () => await SemaphoreSlimLock()); //synchronous action.
        sw.Stop();

        Task task1 = Task.Run(() => SemaphoreSlimLock());
        Task task2 = Task.Run(() => SemaphoreSlimLock());

        await Task.WhenAll(task1, task2);

        Console.WriteLine($"lock: {sw.ElapsedMilliseconds} Milliseconds, Counter={_counter}");

        Console.WriteLine($"Expected counter value: {_numIterations * 2}");
        Console.WriteLine($"Actual counter value: {_counter}");
    }

    static void Producer()
    {
        for (int i = 1; i <= _numIterations; i++)
        {
            lock (_lockObj)
            {
                _queue.Enqueue(i);
                Console.WriteLine($"Produced: {i}");

                // Notify the waiting thread
                Monitor.Pulse(_lockObj);
            }

            Thread.Sleep(2000); // Simulate work
        }
    }

    static void Consumer()
    {
        for (int i = 0; i < _numIterations; i++)
        {
            lock (_lockObj)
            {
                while (_queue.Count == 0)
                {
                    Console.WriteLine("Queue empty, waiting...");
                    Monitor.Wait(_lockObj);  // Wait until notified
                }

                var consequence = _queue.TryDequeue(out int item);
                Console.WriteLine($"\tConsumed: {item}");
            }
        }

        Console.WriteLine("Consumer done!");
    }

    private static void LockIncrement()
    {
        for (int i = 0; i < _numIterations; i++)
        {
            lock (_lockObj)
            {
                _counter++;
            }
        }
    }

    private async static Task SemaphoreSlimLock()
    {
        for (int i = 0; i < _numIterations; i++)
        {
            await _semaphoreLock.WaitAsync();
            try
            {
                _counter++;
            }
            finally
            {
                _semaphoreLock.Release();
            }
        }
    }

    public static void SafeMethodWithTryEnterMonitor()
    {
        for (int i = 0; i < _numIterations; i++)
        {
            bool lockAcquired = false;

            try
            {
                while (!lockAcquired)
                {
                    lockAcquired = Monitor.TryEnter(_lockObj, TimeSpan.FromMilliseconds(10));
                }

                _counter++;
            }
            finally
            {
                if (lockAcquired)
                    Monitor.Exit(_lockObj);
            }

            //Monitor.Pulse(_lockObj);
        }
    }

    public static void SafeMethodWithEnterMonitor()
    {
        for (int i = 0; i < _numIterations; i++)
        {
            Monitor.Enter(_lockObj);

            try
            {
                _counter++;
            }
            finally
            {
                Monitor.Exit(_lockObj);
            }
            //Monitor.Pulse(_lockObj);
        }
    }

    private static async Task IncrementCounter()
    {
        for (int i = 0; i < _numIterations; i++)
        {
            _mutex.WaitOne();
            try
            {
                _counter++;
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
    }

    private string GetParticularDataExample()
    {
        if (_httpContextAccessor?.HttpContext == null)
        {
            throw new UnauthorizedAccessException();
        }
        var test = _environment.EnvironmentName;
        _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(ApplicationAreaHeader.ToLower(), out StringValues applicationArea);

        string searchString = ".";
        int index = applicationArea.ToString().IndexOf(searchString);

        if (index >= 0)
        {
            string result = applicationArea.ToString().Remove(index, searchString.Length);
            return result;
        }

        return applicationArea!;
    }
}
