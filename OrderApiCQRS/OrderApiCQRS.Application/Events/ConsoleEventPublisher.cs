using OrderApiCQRS.Application.Events.Interfaces;

namespace OrderApiCQRS.Application.Events
{
    public class ConsoleEventPublisher : IEventPublisher
    {
        public Task PublishAsyunc<TEvent>(TEvent evt)
        {
            Console.WriteLine($"--> Event published {evt}");
            return Task.CompletedTask;
        }
    }
}
