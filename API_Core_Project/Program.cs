using API_Core_Project.Customization.Security;
using API_Core_Project.Models;
using API_Core_Project.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<ClinicDbContext>(options =>
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("Clinic"));

});


builder.Services.AddDbContext<SecurityDbContext>(options =>
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("SecurityIntra"));

});

// Register the Identity Object Model in DI Container
// Register UserManager<IdentityUser>, RoleManager<IdentityRole>, and SignInManager<IdentityUser> classed
// in DI Container
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<SecurityDbContext>();

// Register the SecurityManagement class in DI Container

builder.Services.AddScoped<SecurityManagment>();

builder.Services.AddScoped<IDataRepositoy<PatientModel, int>, PatientRepository>();
builder.Services.AddScoped<IDataRepositoy<DoctorModel, int>, DoctorRepository>();


//Authentication with policies
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
