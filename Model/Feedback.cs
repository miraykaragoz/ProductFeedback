using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductFeedback.Model;

public class Feedback
{
    public int Id { get; set; }
    
    public int CategoryId { get; set; }

    public string UserId { get; set; } 
    [ForeignKey("UserId")] 
    [JsonIgnore]
    public ApplicationUser User { get; set; }

    [JsonIgnore] 
    public Category Category { get; set; }

    [JsonIgnore] 
    public List<Comment> Comments { get; set; }

    [NotMapped] 
    public int CommentsCount { get; set; }

    public int VoteCount { get; set; } = 0;
    public string Title { get; set; }
    public string Detail { get; set; }

    public Status Status { get; set; } = Status.Planned;
}

public enum Status
{
    Planned,
    InProgress,
    Live,
}