namespace Assignment_05_03.Models
{
    public class SecurityResponse
    {
        public string? Message { get; set; }

        public string? Token { get; set; }

        public bool IsLoggedIn { get; set; } = false;

        public  string? roles { get; set; }
    }
}
