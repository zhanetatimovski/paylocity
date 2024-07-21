using Api.Models;

namespace Api.Repositories;

public static class DatabaseSeeder
{
	private static readonly List<Employee> Employees =
	[
		new()
		{
			FirstName = "LeBron",
			LastName = "James",
			Salary = 75420.99m,
			DateOfBirth = new DateTime(1984, 12, 30)
		},
		new()
		{
			FirstName = "Ja",
			LastName = "Morant",
			Salary = 92365.22m,
			DateOfBirth = new DateTime(1999, 8, 10),
			Dependents =
			[
				new()
				{
					FirstName = "Spouse",
					LastName = "Morant",
					Relationship = Relationship.Spouse,
					DateOfBirth = new DateTime(1998, 3, 3)
				},
				new()
				{
					FirstName = "Child1",
					LastName = "Morant",
					Relationship = Relationship.Child,
					DateOfBirth = new DateTime(2020, 6, 23)
				},
				new()
				{
					FirstName = "Child2",
					LastName = "Morant",
					Relationship = Relationship.Child,
					DateOfBirth = new DateTime(2021, 5, 18)
				}
			]
		},
		new()
		{
			FirstName = "Michael",
			LastName = "Jordan",
			Salary = 143211.12m,
			DateOfBirth = new DateTime(1963, 2, 17),
			Dependents =
			[
				new()
				{
					FirstName = "DP",
					LastName = "Jordan",
					Relationship = Relationship.DomesticPartner,
					DateOfBirth = new DateTime(1974, 1, 2)
				}
			]
		}
	];

	public static void CreateAndSeed(PaylocityContext context)
	{
		context.Database.EnsureCreated();

		if (!context.Employees.Any())
		{
			context.Employees.AddRange(Employees);
			context.SaveChanges();
		}
	}
}
