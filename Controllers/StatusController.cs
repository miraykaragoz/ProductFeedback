using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductFeedback.Data;
using ProductFeedback.Model;

namespace ProductFeedback.Controllers;

public class StatusController : Controller
{
    private readonly ApplicationDbContext _context;

    public StatusController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("/GetStatus")]
    public IActionResult GetStatus()
    {
        var plannedFeedbacks = _context.Feedbacks
            .Where(x => x.Status == Status.Planned)
            .ToList();

        var inProgressFeedbacks = _context.Feedbacks
            .Where(x => x.Status == Status.InProgress)
            .ToList();

        var liveFeedbacks = _context.Feedbacks
            .Where(x => x.Status == Status.Live)
            .ToList();

        var result = new 
        {
            Planned = plannedFeedbacks,
            InProgress = inProgressFeedbacks,
            Live = liveFeedbacks
        };

        return Ok(result);
    }
}