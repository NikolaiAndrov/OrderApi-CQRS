using Microsoft.AspNetCore.Mvc;
using OrderApiCQRS.Application.Features.Products.Queries;
using OrderApiCQRS.Application.Features.Products.QueryHandlers;
using OrderApiCQRS.Data;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public OrdersController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            ViewOrderDto? viewOrderDto = await GetOrderByIdQueryHandler.Handle(new GetOrderByIdQuery(Id), dbContext);

            if (viewOrderDto == null)
            {
                return this.NotFound();
            }

            return Ok(viewOrderDto);
        }
    }
}
