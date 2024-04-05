namespace API_Core_Project.Models
{
    public class SecurityResponse
    {
        public string? Message { get; set; }

        public string? Token { get; set; }

        public bool IsLoggedIn { get; set; } = false;

        public string? roles { get; set; }

        public AppUser AppUser { get; set; }

        public UserRole UserRole { get; set; }

        public LoginUser LoginUser { get; set; }

        public RoleInfo RoleInfo { get; set; }
    }
}
