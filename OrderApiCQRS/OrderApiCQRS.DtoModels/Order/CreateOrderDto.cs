using System.ComponentModel.DataAnnotations;

namespace OrderApiCQRS.DtoModels.Order
{
    public class CreateOrderDto
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 2)]
        public string CustomerFirstName { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 2)]
        public string CustomerLastName { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        [StringLength(10, MinimumLength = 2)]
        public string Status { get; set; } = null!;

        public decimal TotalAmount { get; set; }
    }
}
