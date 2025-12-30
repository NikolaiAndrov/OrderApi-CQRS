using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace OrderApiCQRS.Middlewares
{
    public class GlobalExeptionHandlingMiddleware : IMiddleware
    {
        IHostEnvironment hostEnvironment;

        public GlobalExeptionHandlingMiddleware(IHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(ValidationException ex)
            {
                int statusCode = this.GetStatusCode(ex);
                context.Response.StatusCode = statusCode;

                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                string json = JsonSerializer.Serialize(errors);

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                int statusCode = this.GetStatusCode(ex);
                context.Response.StatusCode = statusCode;
                string message = this.GetMessage(ex);

                ProblemDetails problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = message,
                    Detail = this.hostEnvironment.IsDevelopment() ? ex.Message : message
                };

                string json = JsonSerializer.Serialize(problemDetails);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }

        private int GetStatusCode(Exception ex)
        {
            return ex switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                DbUpdateConcurrencyException => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }

        private string GetMessage(Exception ex)
        {
            return ex switch
            {
                ArgumentException => "Incorrect Request!",
                KeyNotFoundException => "Not Found!",
                NotFoundException => "Not Found!",
                UnauthorizedAccessException => "Unauthorized!",
                DbUpdateConcurrencyException => "Concurrency conflict!",
                _ => "Internal ServerError!"
            };
        }
    }
}
