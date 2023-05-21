using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Baskets.Api.Controllers;
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
builder.Services.AddHttpClient<BasketsController>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/api/baskets/test-circuit-breaker",
	async (HttpContext httpContext, [FromServices] IConfiguration config) =>
	{
		var url = config.GetValue<string>("ServiceUrls:StoreService");
		var httpClient = new HttpClient();

		var response =  await httpClient.GetAsync(url);
		var json = await response.Content.ReadAsStringAsync();

		return Results.Ok(json);
	});

app.Run();
