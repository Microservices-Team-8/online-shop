using Microsoft.EntityFrameworkCore;
using OnlineShop.Users.Api.EF;
using OnlineShop.Users.Api.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddDbContext<UsersDbContext>(options =>
	options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions<ServiceUrls>()
	.Bind(configuration.GetSection(ServiceUrls.SectionName));

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