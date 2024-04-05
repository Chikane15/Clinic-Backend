using API_Core_Project.Models;
using API_Core_Project.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace API_Core_Project.Customization.Security
{
    public class SecurityManagment
    {
        UserManager<IdentityUser> userManager;//Used to Create and Manage Application Users
        SignInManager<IdentityUser> signInManager;//Used to Manage the User SignIn w.r.t. the application
        RoleManager<IdentityRole> roleManager;//Used to assign role
        IDataRepositoy<DoctorModel, int> docRepo;
        ClinicDbContext ctx;

        IConfiguration config;//To configure from appsetting.json

        public SecurityManagment(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration, IDataRepositoy<DoctorModel, int> docRepo, ClinicDbContext ctx)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.config = configuration;
            this.docRepo = docRepo;
            this.ctx = ctx;
        }

        public async Task<bool> RegisterUserAsync(AppUser user)
        {
            bool isUserCreated = false;
            try
            {
                if (!IsValidEmail(user.Email))
                {
                    throw new Exception($"Email format should be correct");
                }
                // 1. Check if the user already exist
                var isUserExist = await userManager.FindByEmailAsync(user.Email);
                if (isUserExist != null)
                {
                    throw new Exception($"User with Email {user.Email} is already exist");
                }

                if (user.Password != user.ConfirmPassowrd)
                {
                    throw new Exception("Password and Confirm Password do not match");
                }


                // 2. Otherwise create new user

                // 2.a. Store the Received Info in IdetityUser Object
                IdentityUser newUser = new IdentityUser()
                {
                    Email = user.Email,
                    UserName = user.Email
                };
                // 2.b. Create the user
                //var result = await userManager.CreateAsync(newUser, user.Password);
                var result= await userManager.CreateAsync(newUser,user.ConfirmPassowrd);
                if (result.Succeeded)
                {
                    isUserCreated = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUserCreated;
        }

        public async Task<SecurityResponse> AuthenticateUserAsync(LoginUser user)
        {
            SecurityResponse response = new SecurityResponse();
            //bool isUserCreated = false;
            try
            {

                //1.Check if user is created or not
                var userExist = await userManager.FindByEmailAsync(user.Email);
                if (userExist == null)
                    throw new Exception($"User with Email {user.Email} is not found");

                // 1.a. If the User Does not have any Role then do not 
                // authenticate it

                //To het the role
                var roleList = await userManager.GetRolesAsync(userExist);
                //If there is no role for assigned to user the throw exception
                if (roleList.Count == 0)
                {
                    throw new Exception($"User with Email {user.Email} is not assigned to any role, please assign role for authentication");
                }

                // 2. Authenticate the User
                var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                {

                    // 3. Generate the token
                    // 3.a. : Read the secret Key and expiry from appsettings.json
                    var secretKey = Convert.FromBase64String(config["JWTCoreSettings:SecretKey"]);
                    var expiry = Convert.ToInt32(config["JWTCoreSettings:ExpiryInMinuts"]);

                    // 3.b. Create an IdentityUser object, so that we will use its Id as a Cliam in the Token
                    IdentityUser idUser = new IdentityUser(user.Email);

                    // logic to get the user role
                    // get the user object based on Email
                    var user1 = await userManager.FindByEmailAsync(user.Email);
                    var role = await userManager.GetRolesAsync(user1);

                    string userId = "";

                    if (role.Contains("Doctor"))
                    {
                        var doctor = await ctx.Doctors.FirstOrDefaultAsync(d => d.Email == user.Email);
                        if (doctor == null)
                            throw new Exception($"Doctor with Email {user.Email} not found");

                        userId = doctor.DoctorID.ToString();
                    }
                    else if (role.Contains("Patient"))
                    {
                        var patient = await ctx.Patients.FirstOrDefaultAsync(p => p.Email == user.Email);
                        if (patient == null)
                            throw new Exception($"Patient with Email {user.Email} not found");

                        userId = patient.PatientID.ToString();
                    }
                    else if (role.Contains("Administrator"))
                    {
                        userId = "";
                    }

                    //
                    //Here to add for assignmnet
                    //


                    // 3.c. Create a Token Descriptor with Information as follows
                    // Issuer: Who Issues the token
                    // Audience: The accessor of the token
                    // Subject: The Claims, used for Identity Verification
                    // Expiry and IssueTime
                    // Signeture: the Token Integrity Check

                    var securityTokenDescription = new SecurityTokenDescriptor()
                    {
                        Issuer = null,
                        Audience = null,
                        Subject = new System.Security.Claims.ClaimsIdentity(new List<Claim>() {
                            new Claim("username", idUser.Id),
                            //Addition for assignment
                            new Claim("role",role[0]),
                            new Claim("userId", userId)


                        }),
                        Expires = DateTime.UtcNow.AddMinutes(expiry),
                        IssuedAt = DateTime.UtcNow,
                        NotBefore = DateTime.UtcNow,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                    };

                    // 3.d. Generate the JSON Token
                    // Header.Payload.Signeture
                    var jwtHandler = new JwtSecurityTokenHandler();


                    // 3.d.1. Use the Security Description(named "securityTokenDescription")
                    var jtwTokne = jwtHandler.CreateJwtSecurityToken(securityTokenDescription);

                    // for fetching the role from Claim
                    var roleName = jtwTokne.Claims.Take(2).Last().Value;
                    var id  = jtwTokne.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

                    // 3.d.2.Write the Token in the JSON Web Token Format as string 
                    response.Token = jwtHandler.WriteToken(jtwTokne);
                   

                    //for storing the role from claim into response.roles
                    response.roles = roleName;
                    response.userId = id;

                    response.IsLoggedIn = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        /* public string GetRoleFormToken(string token)
         {
             string roleName = "";
             var jwtHandler = new JwtSecurityTokenHandler();
             // read the token values
             var jwtSecurityToken = jwtHandler.ReadJwtToken(token);
             // read claims
             var claims = jwtSecurityToken.Claims;
             // read first two claim
             var roleClaim = claims.Take(2);
             // read the role
             var roleRecord = roleClaim.Last();
             // read the role name
             roleName = roleRecord.Value;
             return roleName;
         }*/

        public async Task<bool> CreateRoleAsync(RoleInfo role)
        {
            bool isCreated = false;
            try
            {
                // 1. First Check if the Role Already Exist
                var roleAvailable = await roleManager.FindByNameAsync(role.Name);
                if (roleAvailable != null)
                    throw new Exception($"Role {role.Name} is already exist");

                // 2. Else Create role
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = role.Name,
                    NormalizedName = role.Name.ToUpper()
                };
                var result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                    isCreated = true;
                else
                    throw new Exception($"Role {role.Name} creation failed");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isCreated;
        }

        public async Task<bool> AssignRoleToUser(UserRole userRole)//UserRole is Assign role
        {
            bool isRoleAssigned = false;
            try
            {
                // 1. Check if Role Exist
                var roleAvailable = await roleManager.FindByNameAsync(userRole.RoleName);
                if (roleAvailable == null)
                    throw new Exception($"Role {userRole.RoleName} is not exist");

                // 2. Check if User Exist
                var userAvailable = await userManager.FindByEmailAsync(userRole.Email);
                if (userAvailable == null)
                    throw new Exception($"User {userRole.Email} is not exist");

                // 3. Assign Role to the User
                var result = await userManager.AddToRoleAsync(userAvailable, userRole.RoleName);
                if (result.Succeeded)
                {
                    isRoleAssigned = true;
                }
                else
                {
                    throw new Exception($"Some error occurred while assigneing Role : {userRole.RoleName} to User : {userRole.Email}");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isRoleAssigned;
        }

        public async Task<bool> LogoutAsync()
        {
            await signInManager.SignOutAsync();
            return true;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;


            // Regex pattern for email validation
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);

        }

    }
}
