using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductFeedback.Data;
using ProductFeedback.DTO.Category;
using ProductFeedback.Model;

namespace ProductFeedback.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("/Categories")]
    public IActionResult GetCategories()
    {
        return Ok(_context.Categories.ToList());
    }

    [HttpGet("/GetFeedbackByCategory/{slug}")]
    public IActionResult GetFeedbackByCategory(string slug)
    {
        if (slug == "all")
        {
            var allFeedbacks = _context.Feedbacks
                .Select(f => new
                {
                    Feedback = f,
                    CommentCount = f.Comments.Count,
                    CategoryName = f.Category.Name
                })
                .ToList();
            
            return Ok(allFeedbacks);
        }
        else if (_context.Categories.Any(c => c.Slug == slug))
        {
            var categoryFeedbacks = _context.Feedbacks
                .Include(c => c.Category)
                .Where(x => x.Category.Slug == slug)
                .Select(f => new
                {
                    Feedback = f,
                    CommentCount = f.Comments.Count
                })
                .ToList();

            if (categoryFeedbacks.Count == 0)
            {
                return NotFound(new { message = "Bu kategoriye ait feedback bulunamadı." });
            }

            return Ok(categoryFeedbacks);
        }
        else
        {
            return NotFound(new { message = "Böyle bir kategori bulunamadı." });
        }
    }

    [HttpPost("/CreateCategory")]
    public IActionResult CreateCategory(Category model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Form eksik veya hatalı." });
        }

        model.Slug = Slugify(model.Name);
        _context.Categories.Add(model);
        _context.SaveChanges();

        return Ok("Kategori eklendi.");
    }

    [HttpPost("/UpdateCategory")]
    public IActionResult UpdateCategory([FromBody] CategoryDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Form eksik veya hatalı." });
        }

        var category = _context.Categories.Find(model.Id);

        if (category is null)
        {
            return NotFound(new { message = "Kategori bulunamadı." });
        }

        category.Name = model.Name;
        category.Slug = Slugify(model.Name);
        _context.Categories.Update(category);
        _context.SaveChanges();

        return Ok("Kategori güncellendi.");
    }

    [HttpDelete("/DeleteCategory/{id}")]
    public IActionResult DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);

        if (category is null)
        {
            return NotFound(new { message = "Kategori bulunamadı." });
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();

        return Ok("Kategori silindi.");
    }
    
    private string Slugify(string title)
    {
        return title.ToLower().Replace(" ", "-");
    }
}