using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Application.Events;
using OrderApiCQRS.Application.Events.Interfaces;
using OrderApiCQRS.Application.Features.Interfaces;
using OrderApiCQRS.Application.Features.Orders.Commands.Validators;
using OrderApiCQRS.Application.Features.Orders.Queries;
using OrderApiCQRS.Application.Features.Orders.Queries.Validators;
using OrderApiCQRS.Application.Features.Orders.QueryHandlers;
using OrderApiCQRS.Application.Features.Products.CommandHandlers;
using OrderApiCQRS.Application.Features.Products.Commands;
using OrderApiCQRS.Application.Features.Products.Queries;
using OrderApiCQRS.Application.Features.Products.QueryHandlers;
using OrderApiCQRS.Data;
using OrderApiCQRS.DtoModels.Order;
using OrderApiCQRS.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReadConnection"));
});

builder.Services.AddDbContext<WriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WriteConnection"));
});

// Adding command and queries
builder.Services.AddScoped<ICommandHandler<CreateOrderCommand, ViewOrderDto>, CreateOrderCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetOrderByIdQuery, ViewOrderDto>, GetOrderByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllOrdersQuery, ICollection<ViewOrderDto>>, GetAllOrdersQueryHandler>();

// Adding validators
builder.Services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
builder.Services.AddScoped<IValidator<GetAllOrdersQuery>, GetAllOrdersQueryValidator>();

// Adding global exception handler middleware
builder.Services.AddScoped<GlobalExeptionHandlingMiddleware>();

// Adding event publisher
builder.Services.AddSingleton<IEventPublisher, ConsoleEventPublisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handling middleware
app.UseMiddleware<GlobalExeptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
