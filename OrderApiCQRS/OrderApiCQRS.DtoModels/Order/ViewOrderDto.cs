namespace OrderApiCQRS.DtoModels.Order
{
    public class ViewOrderDto
    {
        public int Id { get; set; }

        public string CustomerFulltName { get; set; } = null!;

        public string Status { get; set; } = null!;

        public decimal TotalAmount { get; set; }
    }
}
