using AutoService.Data;
using AutoService.Services;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddHostedService<BookingCheckService>();


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
