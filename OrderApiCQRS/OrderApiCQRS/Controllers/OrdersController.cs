using Microsoft.AspNetCore.Mvc;
using OrderApiCQRS.Application.Features.Products.Commands;
using OrderApiCQRS.Application.Features.Products.Queries;
using OrderApiCQRS.DtoModels.Order;
using OrderApiCQRS.Application.Features.Interfaces;
using FluentValidation;

namespace OrderApiCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ICommandHandler<CreateOrderCommand, ViewOrderDto> createOrderCommandHandler;
        private readonly IQueryHandler<GetOrderByIdQuery, ViewOrderDto> getOrderByIdQueryHandler;

        public OrdersController(ICommandHandler<CreateOrderCommand, ViewOrderDto> createOrderCommandHandler,
            IQueryHandler<GetOrderByIdQuery, ViewOrderDto> getOrderByIdQueryHandler)
        {
            this.createOrderCommandHandler = createOrderCommandHandler;
            this.getOrderByIdQueryHandler = getOrderByIdQueryHandler;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand createOrderCommand)
        {

            try
            {
                ViewOrderDto? viewOrderDto = await this.createOrderCommandHandler.HandleAsync(createOrderCommand);

                if (viewOrderDto == null)
                {
                    return this.BadRequest();
                }

                return this.CreatedAtAction(nameof(this.GetById), new {Id = viewOrderDto.Id}, viewOrderDto);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

                return this.BadRequest(errors);
            }
        }
    }
}
