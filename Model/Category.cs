using System.Text.Json.Serialization;

namespace ProductFeedback.Model;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    [JsonIgnore]
    public string Slug { get; set; } = string.Empty;
    
    [JsonIgnore]
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}