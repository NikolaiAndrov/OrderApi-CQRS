using Microsoft.AspNetCore.Mvc;
using MediatR;
using OrderApiCQRS.Application.Features.Products.Commands;
using OrderApiCQRS.Application.Features.Products.Queries;
using OrderApiCQRS.DtoModels.Order;
using OrderApiCQRS.Application.Features.Orders.Queries;
using OrderApiCQRS.Application.Features.Orders.Commands;

namespace OrderApiCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            ViewOrderDto? viewOrderDto = await this.mediator.Send(new GetOrderByIdQuery(id));

            if (viewOrderDto == null)
            {
                return this.NotFound();
            }

            return Ok(viewOrderDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllOrdersQuery getAllOrdersQuery)
        {
            ICollection<ViewOrderDto>? viewOrderDtos = await this.mediator.Send(getAllOrdersQuery);

            return this.Ok(viewOrderDtos);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand createOrderCommand)
        {
            int newOrderId = await this.mediator.Send(createOrderCommand);
            ViewOrderDto? viewOrderDto = await this.mediator.Send(new GetOrderByIdQuery(newOrderId));

            if (viewOrderDto == null)
            {
                return this.BadRequest();
            }

            return this.CreatedAtAction(nameof(this.GetById), new { Id = viewOrderDto.Id }, viewOrderDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await this.mediator.Send(new DeleteOrderCommand(id));
            return this.NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateOrderCommand updateOrderCommand)
        {
            if (id != updateOrderCommand.Id)
            {
                return this.BadRequest();
            }

            int updatedOrderId = await this.mediator.Send(updateOrderCommand);
            ViewOrderDto? viewOrderDto = await this.mediator.Send(new GetOrderByIdQuery(updatedOrderId));

            if (viewOrderDto == null)
            {
                return this.BadRequest(); 
            }

            return this.Ok(viewOrderDto);
        }
    }
}
