using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace ProductFeedback.Model;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Nickname { get; set; }
    [NotMapped]
    public string Fullname => $"{FirstName} {LastName}";
    [NotMapped]
    public IFormFile Image { get; set; }
    public string? ImagePath { get; set; }
    
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();    

    [JsonIgnore]
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    [JsonIgnore]
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}