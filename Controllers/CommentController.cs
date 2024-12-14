using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductFeedback.Data;
using ProductFeedback.DTO.Comment;
using ProductFeedback.Model;

namespace ProductFeedback.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CommentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CommentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("/GetCommentsByFeedback/{id}")]
    public IActionResult GetCommentsByFeedbackId(int id)
    {
        return Ok(_context.Comments.Where(x => x.FeedbackId == id).ToList());
    }

    [Authorize]
    [HttpPost("/CreateComment")]
    public IActionResult CreateComment([FromBody] CommentDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest( new { message = "Form eksik veya hatalı." });
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var data = new Comment()
        {
            Id = model.Id,
            Content = model.Content,
            UserId = userId,
            FeedbackId = model.FeedbackId,
        };

        _context.Comments.Add(data);
        _context.SaveChanges();
        
        return Ok(model);
    }

    [Authorize]
    [HttpPost("/AddReplyToComment/{commentId}")]
    public IActionResult AddReplyToComment(int commentId, [FromBody] CommentDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var data = new Comment()
        {
            Id = model.Id,
            Content = model.Content,
            UserId = userId,
            ReplyId = commentId,
            FeedbackId = model.FeedbackId,
        };

        _context.Comments.Add(data);
        _context.SaveChanges();
        
        return Ok( new { message = "Yoruma yanıt eklendi."});
    }
}