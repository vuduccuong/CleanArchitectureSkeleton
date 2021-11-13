namespace Presentation.Dtos.AuthenticateDto
{
    public class LoginDto
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
