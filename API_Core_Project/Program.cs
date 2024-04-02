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

builder.Services.AddScoped<DoctorOperationRepository>();
builder.Services.AddScoped<PatientOperationsRepository>();

builder.Services.AddScoped<IDataRepositoy<PatientModel, int>, PatientRepository>();
builder.Services.AddScoped<IDataRepositoy<DoctorModel, int>, DoctorRepository>();
builder.Services.AddScoped<IDataRepositoy<AppoinmentModel, int>, AppoinmentRepository>();
builder.Services.AddScoped<IDataRepositoy<BillModel, int>, BillRepository>();
builder.Services.AddScoped<IDataRepositoy<ReportModel, int>, ReportRepository>();
builder.Services.AddScoped<IDataRepositoy<VisitModel, int>, VisitRepository>();
builder.Services.AddScoped<IDataRepositoy<DoctorImconeModel, int>, DoctorIncomeRepository>();
builder.Services.AddScoped<IDataRepositoy<PrescriptionModel, int>, PrescriptionRepository>();

//Authentication with policies
builder.Services.AddAuthorization(options =>
{

    var policyRoleSection = builder.Configuration.GetSection("PolicyRole");

    var getPolicyRoles = policyRoleSection.GetValue<string>("PatientPolicy")?.Split(',');
    if (getPolicyRoles != null && getPolicyRoles.Length > 0)
    {
        options.AddPolicy("PatientPolicy", policy =>
        {
            policy.RequireRole(getPolicyRoles);
        });
    }

    var postPutPolicyRoles = policyRoleSection.GetValue<string>("DoctorPolicy")?.Split(',');
    if (postPutPolicyRoles != null && postPutPolicyRoles.Length > 0)
    {
        options.AddPolicy("DoctorPolicy", policy =>
        {
            policy.RequireRole(postPutPolicyRoles);
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("cors");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
