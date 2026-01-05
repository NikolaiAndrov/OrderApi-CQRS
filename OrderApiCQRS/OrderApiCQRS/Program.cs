using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Application.Behaviors;
using OrderApiCQRS.Application.Features.Orders.Commands.Validators;
using OrderApiCQRS.Application.Features.Products.CommandHandlers;
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

// Adding global exception handler middleware
builder.Services.AddScoped<GlobalExeptionHandlingMiddleware>();

// Adding AddMediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommandHandler).Assembly));

// Adding validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();

// Adding/Automating validations
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>)
);

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
