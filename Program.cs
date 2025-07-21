// --- 5. Program.cs ---
// This file configures the ASP.NET Core application, including services and middleware.
// Replace the content of `Program.cs` in the root of your ArifTestApi project.
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Controllers for API endpoints
builder.Services.AddControllers();

// Configure CORS (Cross-Origin Resource Sharing)
// This is crucial for allowing your frontend HTML file (served from a browser)
// to make requests to your backend API (running on a different port/origin).
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin() // Allows requests from any origin (e.g., your local HTML file)
                   .AllowAnyMethod() // Allows all HTTP methods (GET, POST, etc.)
                   .AllowAnyHeader(); // Allows all HTTP headers
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS policy. Make sure this is called before UseAuthorization and UseEndpoints.
app.UseCors("AllowAllOrigins");

app.UseForwardedHeaders();
app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS (optional, but good practice)

app.UseAuthorization();

// Maps controller actions to incoming requests
app.MapControllers();

app.Run();
