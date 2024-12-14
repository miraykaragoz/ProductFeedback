using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductFeedback.Data;
using ProductFeedback.Model;

namespace ProductFeedback.Controllers;

public class VoteController : Controller
{
    private readonly ApplicationDbContext _context;

    public VoteController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [Authorize]
    [HttpPost("/VoteFeedback/{feedbackId}")]
    public IActionResult VoteFeedback(int feedbackId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == feedbackId);

        if (feedback == null)
        {
            return NotFound("Feedback bulunamadı.");
        }

        var existingVote = _context.Votes.FirstOrDefault(v => v.FeedbackId == feedbackId && v.UserId == userId);

        if (existingVote != null)
        {
            _context.Votes.Remove(existingVote);
            feedback.VoteCount--;
            _context.Feedbacks.Update(feedback);
            _context.SaveChanges();

            return Ok("Oy kaldırıldı.");
        }

        var vote = new Vote
        {
            UserId = userId,
            FeedbackId = feedbackId,
        };

        _context.Votes.Add(vote);
        feedback.VoteCount++;
        _context.Feedbacks.Update(feedback);
        _context.SaveChanges();

        return Ok("Oy eklendi.");
    }
}