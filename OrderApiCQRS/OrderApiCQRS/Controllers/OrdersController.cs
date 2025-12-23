using Microsoft.AspNetCore.Mvc;
using OrderApiCQRS.Application.Features.Products.Commands;
using OrderApiCQRS.Application.Features.Products.Queries;
using OrderApiCQRS.Application.Features.Products.QueryHandlers;
using OrderApiCQRS.Application.Features.Products.CommandHandlers;
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

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            ViewOrderDto? viewOrderDto = await GetOrderByIdQueryHandler.Handle(new GetOrderByIdQuery(Id), dbContext);

            if (viewOrderDto == null)
            {
                return this.NotFound();
            }

            return Ok(viewOrderDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand createOrderCommand)
        {
            ViewOrderDto? viewOrderDto = await CreateOrderCommandHandler.Handle(createOrderCommand, dbContext);

            if (viewOrderDto == null)
            {
                return this.BadRequest();
            }

            return this.CreatedAtAction(nameof(this.GetById), new {Id = viewOrderDto.Id}, viewOrderDto);
        }
    }
}
