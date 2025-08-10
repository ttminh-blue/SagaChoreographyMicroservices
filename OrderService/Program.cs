using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using OrderService.Events;
using OrderService.Events.Interfaces;
using OrderService.Models;
using OrderService.Models.Context;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<OrderDb>(opt => opt.UseInMemoryDatabase("Orders"), ServiceLifetime.Singleton);

builder.Services.AddSingleton<RabbitMqChannelProvider>();
builder.Services.AddHostedService<EventListener>();
builder.Services.AddSingleton<IPublisher, Publisher>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
