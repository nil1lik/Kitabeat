using Kitabeat.Application.Books;
using Microsoft.AspNetCore.Mvc;

namespace Kitabeat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("seed")]
    public async Task<IActionResult> Seed(CancellationToken cancellationToken)
    {
        await _bookService.SeedIfEmptyAsync(cancellationToken);
        return Ok("Seed completed (or already seeded).");
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query is required.");

        var results = await _bookService.SearchAsync(query, 20, cancellationToken);
        return Ok(results);
    }
}
