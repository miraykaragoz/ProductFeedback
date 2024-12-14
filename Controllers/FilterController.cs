using Microsoft.AspNetCore.Mvc;
using ProductFeedback.Data;

namespace ProductFeedback.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class FilterController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FilterController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("/FilterFeedback/{id}")]
    public IActionResult FilterFeedback(int id)
    {
        if (id == 1)
        {
            return Ok(_context.Feedbacks
                .OrderByDescending(t => t.VoteCount)
                .ToList());
        }
        else if (id == 2)
        {
            return Ok(_context.Feedbacks
                .OrderBy(t => t.VoteCount)
                .ToList());
        }
        else if (id == 3)
        {
            return Ok(_context.Feedbacks
                .OrderByDescending(t => t.Comments.Count) 
                .ToList());
        }
        else if (id == 4)
        {
            return Ok(_context.Feedbacks
                .OrderBy(t => t.Comments.Count)
                .ToList());
        }
        else
        {
            return NotFound( new { message = "Ticket bulunamadı." });
        }
    }
}