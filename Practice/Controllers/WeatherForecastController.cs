using Microsoft.AspNetCore.Mvc;
using Services.CarFactory;
using Services.Contracts;
using Services.MessagePublisher;

namespace Practice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly EventHubPublisher _eventHubPublisher;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            EventHubPublisher eventHubPublisher,
            ILogger<WeatherForecastController> logger)
        {
            _eventHubPublisher = eventHubPublisher;
            _logger = logger;
        }

        //[HttpGet(Name = "NoName")]
        //public async Task<string> TestRabbitMq()
        //{
        //    await _eventHubPublisher.SendMessage("this is message from client side------");

        //    return "Hello goldfish";
        //}

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            await _eventHubPublisher.SendMessage("this is message from client side------");
            var carFact = new CarFactory();
            ICar car = await carFact.CreateCar("suv");
            //await car.Drive();
            //Console.WriteLine(cleaned);
            BogusProcessor bogus = new();
            //bogus.TestBogus();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //TODO: design the ouput to deal with invalid input.

        //Brute force O(n^2): Time complexity. Space complexity O(1)
        static int[] TwoSomeBruteForce(int[] nums, int target)
        {
            if (!IsInputValid(nums)) return [];

            int inputLength = nums.Length;

            for (int i = 0; i < inputLength; i++)
            {
                for (int j = i + 1; j < inputLength; j++)
                {
                    if (nums[i] + nums[j] == target)
                    {
                        return [i, j];
                    }
                }
            }

            return [];
        }

        //Time complexity: O(n), Space complexity: O(n)
        static int[] TwoSumHashMap(int[] nums, int target)
        {
            if (!IsInputValid(nums)) return [];

            Dictionary<int, int> hashSet = new();

            for (int i = 0; i < nums.Length; i++)
            {
                int diff = target - nums[i];

                if (hashSet.ContainsKey(diff))
                {
                    return [hashSet[diff], i];
                }

                if (!hashSet.ContainsKey(nums[i])) //avoid overwrite index of duplicates.
                    hashSet[nums[i]] = i;
            }

            return [];
        }

        //Time complexity: O(n log n), Space complexity: O(n)
        static int[]? TwoSumTwoPointers(int[] nums, int target)
        {
            if (!IsInputValid(nums)) return null;

            var sortedNums = nums
                .Select((num, index) => new { num, index })
                .OrderBy(x => x.num)
                .ToArray();

            var left = 0;
            var right = sortedNums.Length - 1;

            while (left < right)
            {
                var total = sortedNums[left].num + sortedNums[right].num;

                if (total == target)
                {
                    return [sortedNums[left].index, sortedNums[right].index];
                }
                else if (total > target)
                {
                    right--;
                }
                else
                {
                    left++;
                }
            }

            return [];
        }

        static bool IsInputValid(int[]? input) => input is not null && input.Length >= 2;
    }
}

