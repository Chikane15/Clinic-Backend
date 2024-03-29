namespace Assignment_05_03.Models
{
    public class AppUser
    {
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassowrd { get; set; }
    }


    public class LoginUser
    {
        public string? Email { get; set; }

        public string? Password { get; set; }

    }

    
    // Creation of a new Role
   
    public class RoleInfo
    {
        public string? Name { get; set; }
    }
    
    // For Assigning Role To User

    public class UserRole
    {
        public string? Email { get; set; }
        public string? RoleName { get; set; }
    }
}
