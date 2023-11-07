using AutoService.Data;
using AutoService.ServiceInterfaces;
using AutoService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AutoContext>(
    options=>
    {
        options.UseMySql(builder.Configuration.GetConnectionString("AutoDB"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));
    }
);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IBusService, BusService>();
builder.Services.AddScoped<IBusRoutesService, BusRoutesService>();
builder.Services.AddScoped<IPersonnelsService, PersonnelsService>();
builder.Services.AddScoped<ITicketsService, TicketsService>();
builder.Services.AddScoped<ITripsService, TripsService>();
builder.Services.ScheduleJob();

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
app.UseAuthorization();

app.MapControllers();

app.Run();
