using Api.Configuration;
using Api.Models;
using Api.Services;
using Microsoft.Extensions.Options;

namespace ServiceTests;

public class PaymentCalculatorTests
{
	private PaymentCalculator? paymentCalculator;

	[Fact]
	public void CalculateAnnualSalary_ReturnsAnunualSalaryWithBenefits()
	{
		// Arrange
		var config = new BenefitsConfiguration
		{
			PaychecksPerYear = 26
		};
		paymentCalculator = new(Options.Create(config));

		var employee = new Employee
		{
			Salary = 4000,
			Dependents =
			[
				new Dependent
				{
					DateOfBirth = DateTime.Today.AddYears(-30),
				}
			]
		};

		const int expectedAnnualSalary = 104000;
		const int expectedNumberOfBenefits = 4;

		// Act
		var annualSalary = paymentCalculator.CalculateAnnualSalary(employee);

		// Assert
		Assert.Equal(expectedAnnualSalary, annualSalary.Salary);
		Assert.Equal(expectedNumberOfBenefits, annualSalary.Benefits.Count);
	}

	[Fact]
	public void CalculateBaseBenefit_ReturnsBaseBenefitAmountForAYear()
	{
		// Arrange
		var config = new BenefitsConfiguration
		{
			BaseBenefitPerMonth = 1000
		};
		paymentCalculator = new(Options.Create(config));

		var expected = config.BaseBenefitPerMonth * 12;

		// Act
		var benefit = paymentCalculator.CalculateBaseBenefit();

		// Assert
		Assert.NotNull(benefit);
		Assert.Equal(expected, benefit.Amount);
	}

	[Fact]
	public void CalculateDependentBenefit_WhenZeroDependents_ReturnsNull()
	{
		// Arrange
		var config = new BenefitsConfiguration();
		paymentCalculator = new(Options.Create(config));

		const int numberOfDependents = 0;

		// Act
		var benefit = paymentCalculator.CalculateDependentBenefit(numberOfDependents);

		// Assert
		Assert.Null(benefit);
	}

	[Fact]
	public void CalculateDependentBenefit_ReturnsDependentBenefitPerAYear()
	{
		// Arrange
		var config = new BenefitsConfiguration
		{
			DependentBenefitPerMonth = 600
		};
		paymentCalculator = new(Options.Create(config));

		const int numberOfDependents = 3;

		var expected = numberOfDependents * config.DependentBenefitPerMonth * 12;

		// Act
		var benefit = paymentCalculator.CalculateDependentBenefit(numberOfDependents);

		// Assert
		Assert.NotNull(benefit);
		Assert.Equal(expected, benefit.Amount);
	}

	[Fact]
	public void CalculateSalaryAnnualBenefit_WhenInvalidSalary_ReturnsArgumentException()
	{
		// Arrange
		var config = new BenefitsConfiguration();
		paymentCalculator = new(Options.Create(config));

		const decimal salary = -500;

		// Act & Assert
		Assert.Throws<ArgumentException>(() => paymentCalculator.CalculateSalaryAnnualBenefit(salary));
	}

	[Fact]
	public void CalculateSalaryAnnualBenefit_WhenSalaryBelowThreshold_ReturnsNull()
	{
		// Arrange
		var config = new BenefitsConfiguration
		{
			PaychecksPerYear = 26,
			SalaryAnnualBenefitThreshold = 80000
		};
		paymentCalculator = new(Options.Create(config));

		const decimal salary = 4000;

		// Act
		var benefit = paymentCalculator.CalculateSalaryAnnualBenefit(salary);

		// Assert
		Assert.Null(benefit);
	}


	[Fact]
	public void CalculateSalaryAnnualBenefit_ReturnsAnnualSalary()
	{
		// Arrange
		var config = new BenefitsConfiguration
		{
			PaychecksPerYear = 26,
			SalaryAnnualBenefitThreshold = 80000,
			SalaryAnnualBenefitPercentage = 2
		};
		paymentCalculator = new(Options.Create(config));

		const decimal salary = 90000;

		var baseAnnualSalary = salary * config.PaychecksPerYear;
		var multiplier = config.SalaryAnnualBenefitPercentage / 100;
		var expected = baseAnnualSalary * multiplier;

		// Act
		var benefit = paymentCalculator.CalculateSalaryAnnualBenefit(salary);

		// Assert
		Assert.NotNull(benefit);
		Assert.Equal(expected, benefit.Amount);
	}

	[Fact]
	public void CalculateOlderDependentBenefit_WhenZeroDependents_ReturnsNull()
	{
		// Arrange
		var config = new BenefitsConfiguration
		{
			AgeBenefitThreshold = 50,
		};
		paymentCalculator = new(Options.Create(config));

		// Act
		var benefit = paymentCalculator.CalculateOlderDependentBenefit([]);

		// Assert
		Assert.Null(benefit);
	}

	[Fact]
	public void CalculateOlderDependentBenefit_WhenNoOlderDependents_ReturnsNull()
	{
		// Arrange
		var config = new BenefitsConfiguration
		{
			AgeBenefitThreshold = 50,
		};
		paymentCalculator = new(Options.Create(config));

		var dependents = new List<Dependent>
		{
			new()
			{
				DateOfBirth = DateTime.Today.AddYears(-30)
			},
			new()
			{
				DateOfBirth = DateTime.Today.AddYears(-50)
			}
		};

		// Act
		var benefit = paymentCalculator.CalculateOlderDependentBenefit(dependents);

		// Assert
		Assert.Null(benefit);
	}

	[Fact]
	public void CalculateOlderDependentBenefit_ReturnsOlderDependentBenefitPerAYear()
	{
		// Arrange
		var config = new BenefitsConfiguration
		{
			AgeBenefitThreshold = 50,
			AgeBenefitPerMonth = 200
		};
		paymentCalculator = new(Options.Create(config));

		var dependents = new List<Dependent>
		{
			new()
			{
				DateOfBirth = DateTime.Today.AddYears(-30)
			},
			new()
			{
				DateOfBirth = DateTime.Today.AddYears(-60)
			},
			new()
			{
				DateOfBirth = DateTime.Today.AddYears(-50)
			}
		};

		var olderDependentsCount = dependents.Count(x => x.Age > config.AgeBenefitThreshold);
		var expected = olderDependentsCount * config.AgeBenefitPerMonth * 12;

		// Act
		var benefit = paymentCalculator.CalculateOlderDependentBenefit(dependents);

		// Assert
		Assert.NotNull(benefit);
		Assert.Equal(expected, benefit.Amount);
	}
}
