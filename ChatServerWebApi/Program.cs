using ChatApp.Infrastructure;
using ChatServerWebApi.Hubs; 
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSignalR();

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

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS policy
// (used only when backend and frontend have different ports. If not, it will be ignored)
app.UseCors(policy => policy
    .WithOrigins(        
        "http://localhost:5173"
    )
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

app.UseAuthorization();

// For reactchat
app.UseDefaultFiles(); 
app.UseStaticFiles();

app.MapControllers();
app.MapHub<ChatHub>("/chathub");

app.MapFallbackToFile("index.html");

app.Run();