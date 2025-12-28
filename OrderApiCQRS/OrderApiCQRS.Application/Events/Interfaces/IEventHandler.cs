namespace OrderApiCQRS.Application.Events.Interfaces
{
    public interface IEventHandler<TEvent>
    {
        Task HandleAsync(TEvent evt);
    }
}
