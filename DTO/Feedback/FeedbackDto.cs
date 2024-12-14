using System.ComponentModel.DataAnnotations;
using ProductFeedback.Model;

namespace ProductFeedback.DTO.Ticket;

public class FeedbackDto
{
    public int  Id { get; set; }
    public Status StatusId { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public string Detail { get; set; }
}