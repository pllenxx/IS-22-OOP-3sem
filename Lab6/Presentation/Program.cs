using Business.Extensions;
using DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddDataAccess(x => x.UseLazyLoadingProxies().UseSqlite("Data Source=database.db"));
builder.Services.AddControllers();
builder.Services.AddCooliesAuthentication().AddRoles();
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

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();