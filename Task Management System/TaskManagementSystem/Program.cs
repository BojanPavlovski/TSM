using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using TaskManagementSystem.DataAccess;
using TaskManagementSystem.DataAccess.Implementations;
using TaskManagementSystem.DataAccess.Interfaces;
using TaskManagementSystem.Domain.Domain;
using TaskManagementSystem.Services.Implementations;
using TaskManagementSystem.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency Injection
builder.Services.AddScoped<ITaskRepository, TaskModelRepository>();
builder.Services.AddScoped<ITaskModelService, TaskModelService>();
builder.Services.AddDbContext<TaskManagementSystemDbContext>(options =>
    options.UseSqlServer("Server=.\\SQLExpress;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
