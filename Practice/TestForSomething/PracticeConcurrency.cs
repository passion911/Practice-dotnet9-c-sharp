namespace Practice.TestForSomething;

public class PracticeConcurrency
{
    public async Task ProcessDataAsync()
    {
        await Task.Run(() =>
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(i);
            }
        });
    }
}
