using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Xunit;

namespace ApiTests.IntegrationTests;

public class EmployeeIntegrationTests : IntegrationTest
{
	[Fact]
	public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
	{
		var response = await HttpClient.GetAsync("/api/v1/employees");
		var employees = new List<GetEmployeeDto>
		{
			new()
			{
				Id = 1,
				FirstName = "LeBron",
				LastName = "James",
				Salary = 75420.9900m,
				DateOfBirth = new DateTime(1984, 12, 30),
				AnnualSalary = new AnnualSalaryDto { Salary = 1960945.7400m, Benefits = [] }
			},
			new()
			{
				Id = 2,
				FirstName = "Ja",
				LastName = "Morant",
				Salary = 92365.2200m,
				DateOfBirth = new DateTime(1999, 8, 10),
				Dependents =
				[
					new()
					{
						Id = 1,
						FirstName = "Spouse",
						LastName = "Morant",
						Relationship = Relationship.Spouse,
						DateOfBirth = new DateTime(1998, 3, 3)
					},
					new()
					{
						Id = 2,
						FirstName = "Child1",
						LastName = "Morant",
						Relationship = Relationship.Child,
						DateOfBirth = new DateTime(2020, 6, 23)
					},
					new()
					{
						Id = 3,
						FirstName = "Child2",
						LastName = "Morant",
						Relationship = Relationship.Child,
						DateOfBirth = new DateTime(2021, 5, 18)
					}
				],
				AnnualSalary = new AnnualSalaryDto { Salary = 2401495.7200m, Benefits = [] }
			},
			new()
			{
				Id = 3,
				FirstName = "Michael",
				LastName = "Jordan",
				Salary = 143211.1200m,
				DateOfBirth = new DateTime(1963, 2, 17),
				Dependents =
				[
					new()
					{
						Id = 4,
						FirstName = "DP",
						LastName = "Jordan",
						Relationship = Relationship.DomesticPartner,
						DateOfBirth = new DateTime(1974, 1, 2)
					}
				],
				AnnualSalary = new AnnualSalaryDto { Salary = 3723489.1200m, Benefits = [] }
			}
		};
		await response.ShouldReturn(HttpStatusCode.OK, employees);
	}

	[Fact]
	public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
	{
		var response = await HttpClient.GetAsync("/api/v1/employees/1");
		var employee = new GetEmployeeDto
		{
			Id = 1,
			FirstName = "LeBron",
			LastName = "James",
			Salary = 75420.9900m,
			DateOfBirth = new DateTime(1984, 12, 30),
			AnnualSalary = new AnnualSalaryDto { Salary = 1960945.7400m, Benefits = [] }
		};
		await response.ShouldReturn(HttpStatusCode.OK, employee);
	}

	[Fact]
	public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
	{
		var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
		await response.ShouldReturn(HttpStatusCode.NotFound);
	}
}

