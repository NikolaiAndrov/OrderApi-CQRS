namespace OrderApiCQRS.Application.Events.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsyunc<TEvent>(TEvent evt);
    }
}