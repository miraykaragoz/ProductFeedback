namespace ProductFeedback.Model;

public class Register
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public IFormFile Image { get; set; } 
}

public class Login
{
    public string Email { get; set; }
    public string Password { get; set; }
}