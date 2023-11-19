using UserService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.ServiceInterfaces;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AutoUsersContext>(
    options =>
    {
        options.UseMySql(builder.Configuration.GetConnectionString("AutoUsersDB"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));
    }
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 5;
    
}).AddEntityFrameworkStores<AutoUsersContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IAutoUser, AutoUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddCors(o =>
{
    o.AddPolicy("MyOrigin",
        b => b
            .WithOrigins("https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("MyOrigin");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
