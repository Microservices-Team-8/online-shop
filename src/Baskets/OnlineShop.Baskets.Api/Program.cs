using Microsoft.EntityFrameworkCore;
using OnlineShop.Baskets.Api.Entities;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();

builder.Services.AddDbContext<BasketsDbContext>(options =>
{
	options.UseNpgsql("");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
