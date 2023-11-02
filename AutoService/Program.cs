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


builder.Services.AddScoped<BusService>();
builder.Services.AddScoped<BusRoutesService>();
builder.Services.AddScoped<PersonnelsService>();
builder.Services.AddScoped<TicketsService>();
builder.Services.AddScoped<TripsService>();
builder.Services.ScheduleJob();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
