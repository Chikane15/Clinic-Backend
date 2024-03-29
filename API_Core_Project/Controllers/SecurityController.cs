using API_Core_Project.Customization.Security;
using API_Core_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Core_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        SecurityManagment security;
        SecurityResponse securityResponse;

        public SecurityController(SecurityManagment security)
        {

            this.security = security;
            securityResponse = new SecurityResponse();
        }

        [HttpPost("Register")]
        [ActionName("register")]

        public async Task<IActionResult> RegisterUserAsync(AppUser user)
        {
            try
            {
                var result = await security.RegisterUserAsync(user);
                if (result)
                {
                    securityResponse.Message = $"User {user.Email} is created sucecssfully";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(securityResponse);
        }

        [HttpPost]
        [ActionName("authorization")]

        public async Task<IActionResult> AuthenticateUserAsync(LoginUser user)
        {
            try
            {
                securityResponse = await security.AuthenticateUserAsync(user);


                if (securityResponse.IsLoggedIn)
                {
                    securityResponse.Message = $"User {user.Email} is authenticated sucecssfully";

                }
            }
            catch (Exception ex)
            {
                // The Exception Midleware will Listen it
                throw ex;
            }
            return Ok(securityResponse);
        }

        [HttpPost]
        [ActionName("newrole")]
        // [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateRole(RoleInfo role)
        {
            try
            {
                var result = await security.CreateRoleAsync(role);
                if (result)
                {
                    securityResponse.Message = $"Role {role.Name} is created sucecssfully";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(securityResponse);
        }

        [HttpPost]
        [ActionName("approveuser")]
        //[Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ApproveUser(UserRole userRole)
        {
            try
            {
                var result = await security.AssignRoleToUser(userRole);
                if (result)
                {
                    securityResponse.Message = $"Role {userRole.RoleName} is assigned to User {userRole.Email} successfully";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(securityResponse);
        }

        [HttpPost("Logout")]

        public async Task<IActionResult> Logout()
        {
            var isLogOut = await security.LogoutAsync();
            return Ok("Logged out Successfully");
        }
    }
}
