using MassTransit;

namespace Services.MessagePublisher;

public class EventHubPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventHubPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task SendMessage(string messageText)
    {

        await _publishEndpoint.Publish(new MyMessage(messageText));
    }
}

public record MyMessage(string Text);