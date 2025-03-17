namespace Application.Services.Email;

public class EmailService : IEmailService
{
    public async Task SendMailAsync()
    {
        Console.WriteLine("Sending notifications to consumer...");
    }
}
