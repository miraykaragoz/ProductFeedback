using System.Text.Json.Serialization;

namespace ProductFeedback.DTO.Comment;

public class CommentDto
{
    public int Id { get; set; }
    public int FeedbackId { get; set; }
    public string Content { get; set; }
}