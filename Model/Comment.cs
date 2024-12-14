using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductFeedback.Model;

public class Comment
{
    public int Id { get; set; }
    
    public int? ReplyId { get; set; }
    [ForeignKey("ReplyId")]
    public Comment Reply { get; set; }
    
    public int FeedbackId { get; set; }
    [JsonIgnore]
    public Feedback Feedback { get; set; }

    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }
    
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}