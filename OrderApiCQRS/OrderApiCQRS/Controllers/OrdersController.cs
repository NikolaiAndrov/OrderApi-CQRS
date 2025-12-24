using Microsoft.AspNetCore.Mvc;
using OrderApiCQRS.Application.Features.Products.Commands;
using OrderApiCQRS.Application.Features.Products.Queries;
using OrderApiCQRS.DtoModels.Order;
using OrderApiCQRS.Application.Features.Interfaces;
using OrderApiCQRS.Application.Features.Orders.Queries;

namespace OrderApiCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ICommandHandler<CreateOrderCommand, ViewOrderDto> createOrderCommandHandler;
        private readonly IQueryHandler<GetOrderByIdQuery, ViewOrderDto> getOrderByIdQueryHandler;
        private readonly IQueryHandler<GetAllOrdersQuery, ICollection<ViewOrderDto>> getAllOrdersQueryHandler;

        public OrdersController(ICommandHandler<CreateOrderCommand, ViewOrderDto> createOrderCommandHandler,
            IQueryHandler<GetOrderByIdQuery, ViewOrderDto> getOrderByIdQueryHandler,
             IQueryHandler<GetAllOrdersQuery, ICollection<ViewOrderDto>> getAllOrdersQueryHandler)
        {
            this.createOrderCommandHandler = createOrderCommandHandler;
            this.getOrderByIdQueryHandler = getOrderByIdQueryHandler;
            this.getAllOrdersQueryHandler = getAllOrdersQueryHandler;
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            ViewOrderDto? viewOrderDto = await this.getOrderByIdQueryHandler.HandleAsync(new GetOrderByIdQuery(Id));

            if (viewOrderDto == null)
            {
                return this.NotFound();
            }

            return Ok(viewOrderDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllOrdersQuery getAllOrdersQuery)
        {
            ICollection<ViewOrderDto>? viewOrderDtos = await this.getAllOrdersQueryHandler.HandleAsync(getAllOrdersQuery);

            return this.Ok(viewOrderDtos);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand createOrderCommand)
        {
            ViewOrderDto? viewOrderDto = await this.createOrderCommandHandler.HandleAsync(createOrderCommand);

            if (viewOrderDto == null)
            {
                return this.BadRequest();
            }

            return this.CreatedAtAction(nameof(this.GetById), new { Id = viewOrderDto.Id }, viewOrderDto);
        }
    }
}
