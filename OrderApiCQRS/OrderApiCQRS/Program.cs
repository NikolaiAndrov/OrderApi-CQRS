using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Application.Features.Orders.Commands;
using OrderApiCQRS.Application.Features.Orders.Commands.Validators;
using OrderApiCQRS.Application.Features.Orders.Queries;
using OrderApiCQRS.Application.Features.Orders.Queries.Validators;
using OrderApiCQRS.Application.Features.Products.CommandHandlers;
using OrderApiCQRS.Application.Features.Products.Commands;
using OrderApiCQRS.Data;
using OrderApiCQRS.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding databases
builder.Services.AddDbContext<ReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReadConnection"));
});

builder.Services.AddDbContext<WriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WriteConnection"));
});

// Adding validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();

// Adding global exception handler middleware
builder.Services.AddScoped<GlobalExeptionHandlingMiddleware>();

// Adding AddMediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommandHandler).Assembly));

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
