using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Users.Api.Models;

namespace OnlineShop.Users.Api.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(u => u.Id);

		builder.Property(u => u.Email)
			.HasMaxLength(100);
		builder.Property(u => u.PhoneNumber)
			.HasMaxLength(100);

		var users = new List<User>()
		{
			new User
			{
				Id = 1,
				FirstName = "John",
				LastName = "Doe",
				Email = "johndoe@example.com",
				PhoneNumber = "555-555-5555",
				OrderIds = new List<int> { 1, 2 }
			},
			new User
			{
				Id = 2,
				FirstName = "Jane",
				LastName = "Smith",
				Email = "janesmith@example.com",
				PhoneNumber = "555-555-5555",
				OrderIds = new List<int> { 3 }
			}
		};

		builder.HasData(users);
	}
}