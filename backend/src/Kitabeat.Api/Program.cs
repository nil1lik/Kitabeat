using Kitabeat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Kitabeat.Application.Books;
using Kitabeat.Domain.Books;
using Kitabeat.Infrastructure.Books;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Connection string
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Host=localhost;Port=5432;Database=kitabeat;Username=kitabeat;Password=kitabeat_pwd";

// DbContext
builder.Services.AddDbContext<KitabeatDbContext>(options =>
    options.UseNpgsql(connectionString));

// CORS – tek policy yeter kanka
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Angular
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Repository & Service
builder.Services.AddScoped<IBookRepository, EfBookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

// Basit ping
app.MapGet("/ping", () => "pong");

app.MapPost("/api/books/seed", async (
    IBookRepository bookRepository,
    IWebHostEnvironment env,
    CancellationToken cancellationToken) =>
{
    if (await bookRepository.AnyAsync(cancellationToken))
        return Results.BadRequest("Books table already has data.");

    var filePath = Path.Combine(env.ContentRootPath, "Data", "book-seed.json");

    if (!File.Exists(filePath))
        return Results.NotFound("Seed file not found.");

    var json = await File.ReadAllTextAsync(filePath, cancellationToken);

    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    // 🔥 JSON → DTO yüklüyoruz
    var seedDtos = JsonSerializer.Deserialize<List<BookSeedDto>>(json, options);

    if (seedDtos is null || seedDtos.Count == 0)
        return Results.BadRequest("No books found in seed file.");

    // 🔥 DTO → Entity constructor’ı ile mapliyoruz (En kritik nokta)
    var books = seedDtos
        .Select(dto => new Book(dto.Title, dto.Author, dto.Description))
        .ToList();

    await bookRepository.AddRangeAsync(books, cancellationToken);

    return Results.Ok(new
    {
        Message = "Books seeded successfully.",
        Count = books.Count
    });
});



app.Run();
 