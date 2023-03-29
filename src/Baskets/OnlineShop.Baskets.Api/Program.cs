using Microsoft.EntityFrameworkCore;
using OnlineShop.Baskets.Api.Entities;

var builder = WebApplication.CreateBuilder();

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddDbContext<BasketsDbContext>(options =>
{
	options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
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