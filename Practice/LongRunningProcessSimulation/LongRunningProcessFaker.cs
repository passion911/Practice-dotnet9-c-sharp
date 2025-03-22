namespace Practice.LongRunningProcessSimulation;

public class LongRunningProcessFaker
{
    public async Task<string> LongRunningProcess()
    {
        await Task.Delay(TimeSpan.FromSeconds(2)); //assumption a workflow

        throw new InvalidOperationException("Unknow error");

        Console.WriteLine($"Long running process done.");

        return "Long running process successful!";
    }
}
