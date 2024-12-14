using System.ComponentModel.DataAnnotations.Schema;

namespace ProductFeedback.Model;

public class Vote
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }
    
    public int FeedbackId { get; set; }
    public Feedback Feedback { get; set; }
}