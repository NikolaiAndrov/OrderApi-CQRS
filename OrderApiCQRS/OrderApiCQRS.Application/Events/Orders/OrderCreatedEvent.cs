using MediatR;

namespace OrderApiCQRS.Application.Events.Orders
{
    public record OrderCreatedEvent : INotification
    {
        public int Id { get; set; }
        
        public string CustomerFirstName { get; set; } = string.Empty;

        public string CustomerLastName { get; set; } = string.Empty;

        public string Status {  get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }
    }
}
