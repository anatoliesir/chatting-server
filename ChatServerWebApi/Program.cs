using ChatApp.Infrastructure;
using ChatServerWebApi.Hubs; // Added this to recognize ChatHub
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add this line to register all Infrastructure services (DB, Repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Add this line to register MediatR from the Application project
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(ChatApp.Application.Users.Commands.RegisterUserCommand).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// CORS policy
app.UseCors(policy => policy
    .WithOrigins("https://localhost:7112", "http://localhost:5273")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub");

app.Run();