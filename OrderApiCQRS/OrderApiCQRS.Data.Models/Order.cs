using System.ComponentModel.DataAnnotations;

namespace OrderApiCQRS.Data.Models
{
    public class Order
    {
        public Order()
        {
            this.CreatedAt = DateTime.UtcNow;
        }

        [Key]
        public int Id { get; set; }

        public required string CustomerFirstName { get; set; }

        public required string CustomerLastName { get; set; }

        public required string Status { get; set; } 

        public DateTime CreatedAt { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
