using Kitabeat.Api.Endpoints;
using Kitabeat.Api.Sentiment;
using Kitabeat.Application.Books;
using Kitabeat.Application.BookEmotions;
using Kitabeat.Application.Sentiment;
using Kitabeat.Domain.Books;
using Kitabeat.Domain.BookEmotions;
using Kitabeat.Infrastructure.Books;
using Kitabeat.Infrastructure.BookEmotions;
using Kitabeat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ========================
// Services Configuration
// ========================

// Database
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Host=localhost;Port=5432;Database=kitabeat;Username=kitabeat;Password=kitabeat_pwd";

builder.Services.AddDbContext<KitabeatDbContext>(options =>
    options.UseNpgsql(connectionString));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Repositories
builder.Services.AddScoped<IBookRepository, EfBookRepository>();
builder.Services.AddScoped<IBookEmotionRepository, EfBookEmotionRepository>();

// Application Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookSeedService, BookSeedService>();
builder.Services.AddScoped<IBookEmotionService, BookEmotionService>();
builder.Services.AddScoped<IBookEmotionAnalyzerService, BookEmotionAnalyzerService>();

// HTTP Clients
builder.Services.AddHttpClient<ISentimentClient, SentimentClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8001");
});

builder.Services.AddControllers();

// ========================
// App Pipeline
// ========================

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();

// Controllers
app.MapControllers();

// Minimal API Endpoints
app.MapApiEndpoints();

app.MapGet("/ping", () => "pong");

app.Run();
