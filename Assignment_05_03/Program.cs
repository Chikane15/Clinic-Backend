using Assignment_05_03.Customization.CustomMiddleWare;
using Assignment_05_03.Customization.Security;
using Assignment_05_03.DefaultRoles;
using Assignment_05_03.Models;
using Assignment_05_03.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<EshoppingDbContext>(options =>
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("EShopping"));
   
});

builder.Services.AddDbContext<LoggerDbContext>(options =>
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("AppLog"));

});

builder.Services.AddDbContext<AppSecurityDbContext>(options =>
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("SecurityIntra"));

});

// Register the Identity Object Model in DI Container
// Register UserManager<IdentityUser>, RoleManager<IdentityRole>, and SignInManager<IdentityUser> classed
// in DI Container
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppSecurityDbContext>();

// Register the SecurityManagement class in DI Container

builder.Services.AddScoped<SecurityManagment>();

builder.Services.AddScoped<IDataRespository<Category, int>, CategoryDataRepository>();
builder.Services.AddScoped<IDataRespository<Product, int>, ProductDataRespository>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });



// Cnnfigure the Authentication Service
// by verifying the received token

// Read the Secret Key from the appsettings.json so that it will be used for integrity check
byte[] secretKey = Convert.FromBase64String(builder.Configuration["JWTCoreSettings:SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Header Requirements
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Use the JWT Beare as a default scheme for the API so that the client MUST send the Bearer token in HTTP Header for Each Request
})
    // and validate the token for completing Authentication and Authorization
    .AddJwtBearer(token =>
    {
        // Check if the Https Metadata information is needed
        token.RequireHttpsMetadata = false;
        token.SaveToken = true; // Token will be meintained by the server
        token.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,  // Signeture based verification
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//Hardcoded in DI
/*builder.Services.AddAuthorization(options =>
{
  
    options.AddPolicy("GetPolicy", policy =>
    {
        policy.RequireRole("Manager", "Clerk", "Operator");
    });
    options.AddPolicy("PostPutPolicy", policy =>
    {
        policy.RequireRole("Manager", "Clerk");
    });
    options.AddPolicy("DeletePolicy", policy =>
    {
        policy.RequireRole("Manager");
    });
});*/

//Getting the policies from appsettings.js
builder.Services.AddAuthorization(options =>
{
    
    var policyRoleSection = builder.Configuration.GetSection("PolicyRole");

    var getPolicyRoles = policyRoleSection.GetValue<string>("GetPolicy")?.Split(',');
    if (getPolicyRoles != null && getPolicyRoles.Length > 0)
    {
        options.AddPolicy("GetPolicy", policy =>
        {
            policy.RequireRole(getPolicyRoles);
        });
    }

    var postPutPolicyRoles = policyRoleSection.GetValue<string>("PostPutPolicy")?.Split(',');
    if (postPutPolicyRoles != null && postPutPolicyRoles.Length > 0)
    {
        options.AddPolicy("PostPutPolicy", policy =>
        {
            policy.RequireRole(postPutPolicyRoles);
        });
    }

    var deletePolicyRoles = policyRoleSection.GetValue<string>("DeletePolicy")?.Split(',');
    if (deletePolicyRoles != null && deletePolicyRoles.Length > 0)
    {
        options.AddPolicy("DeletePolicy", policy =>
        {
            policy.RequireRole(deletePolicyRoles);
        });
    }

    var AdminPolicyRoles = policyRoleSection.GetValue<string>("AdminPolicy")?.Split(',');
    if (AdminPolicyRoles != null && AdminPolicyRoles.Length > 0)
    {
        options.AddPolicy("AdminPolicy", policy =>
        {
            policy.RequireRole(AdminPolicyRoles);
        });
    }
});

//For Default admin creation
IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
await Default_User_Roles.CreateDefaultAdmin_Role(serviceProvider);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("cors");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
//app.UseCustomErrorLogger();
//app.UseCustomLogger();


app.MapControllers();

app.Run();
