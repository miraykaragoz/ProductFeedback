using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductFeedback.Data;
using ProductFeedback.DTO.Ticket;
using ProductFeedback.Model;

namespace ProductFeedback.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class FeedbackController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FeedbackController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("/GetFeedbacks")]
    public IActionResult GetFeedbacks()
    {
        return Ok(_context.Feedbacks.Include(x => x.Comments).OrderBy(x => x.VoteCount).ToList());
    }

    [Authorize]
    [HttpPost("/CreateFeedback")]
    public IActionResult CreateFeedback([FromBody] FeedbackDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Form eksik veya hatalı." });
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var data = new Feedback()
        {
            Id = model.Id,
            Title = model.Title,
            Detail = model.Detail,
            UserId = userId,
            CategoryId = model.CategoryId,
        };

        _context.Feedbacks.Add(data);
        _context.SaveChanges();

        return Ok("Feedback eklendi.");
    }

    [Authorize]
    [HttpPost("/UpdateFeedback")]
    public IActionResult UpdateFeedback([FromBody] FeedbackDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Form eksik veya hatalı." });
        }

        var feedback = _context.Feedbacks.FirstOrDefault(x => x.Id == model.Id);

        if (feedback == null)
        {
            return BadRequest(new { message = "Feedback bulunamadı." });
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == feedback.UserId)
        {
            feedback.Id = model.Id;
            feedback.Title = model.Title;
            feedback.Detail = model.Detail;
            feedback.Status = model.StatusId;
            feedback.UserId = userId;
            feedback.CategoryId = model.CategoryId;
            _context.Feedbacks.Update(feedback);
            _context.SaveChanges();
        }
        else
        {
            return BadRequest( new { message = "Bu feedback'i güncellemek için yetkiniz bulunamadı." });
        }

        return Ok("Feedback güncellendi.");
    }

    [HttpGet("/GetFeedbackDetail/{id}")]
    public IActionResult GetFeedbackDetail(int id)
    {
        var feedback = _context.Feedbacks
            .Include(x => x.Comments)
            .Include(x => x.Category)
            .FirstOrDefault(x => x.Id == id);

        if (feedback == null)
        {
            return NotFound(new { message = "Feedback bulunamadı." });
        }
        
        var data = new
        {
            feedback,
            CategoryName = feedback.Category.Name
        };

        return Ok(data);
    }
}