using Api.Configuration;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.Extensions.Options;

namespace Api.Services;

public class PaymentCalculator : IPaymentCalculator
{
	private readonly BenefitsConfiguration _benefitsConfiguration;

	public PaymentCalculator(IOptions<BenefitsConfiguration> benefitsOptions)
	{
		_benefitsConfiguration = benefitsOptions?.Value
			?? throw new ArgumentNullException(nameof(benefitsOptions));
	}

	public AnnualSalaryDto CalculateAnnualSalary(Employee employee)
	{
		var benefits = new List<BenefitDto>();

		var baseBenefit = CalculateBaseBenefit();
		if (baseBenefit is not null)
		{
			benefits.Add(baseBenefit);
		}

		var dependentBenefit = CalculateDependentBenefit(employee.Dependents.Count);
		if (dependentBenefit is not null)
		{
			benefits.Add(dependentBenefit);
		}

		var annualSalaryBenefit = CalculateSalaryAnnualBenefit(employee.Salary);
		if (annualSalaryBenefit is not null)
		{
			benefits.Add(annualSalaryBenefit);
		}

		var olderDependentBenefit = CalculateOlderDependentBenefit(employee.Dependents);
		if (olderDependentBenefit is not null)
		{
			benefits.Add(olderDependentBenefit);
		}

		var annualSalary = employee.Salary * _benefitsConfiguration.PaychecksPerYear;
		annualSalary += benefits.Sum(x => x.Amount);

		return new AnnualSalaryDto
		{
			Salary = annualSalary,
			Benefits = benefits
		};
	}

	public BenefitDto? CalculateBaseBenefit()
	{
		return new BenefitDto
		{
			Amount = _benefitsConfiguration.BaseBenefitPerMonth * 12,
			Description = "Base benefit"
		};
	}

	public BenefitDto? CalculateDependentBenefit(int numberOfDependents)
	{
		if (numberOfDependents == 0)
		{
			return null;
		}

		return new BenefitDto
		{
			Amount = numberOfDependents * _benefitsConfiguration.DependentBenefitPerMonth * 12,
			Description = $"Dependent benefit for {numberOfDependents} dependent(s)"
		};
	}

	public BenefitDto? CalculateSalaryAnnualBenefit(decimal salary)
	{
		if (salary < 0)
		{
			throw new ArgumentException("Salary cannot be a negative number", nameof(salary));
		}

		var baseAnnualSalary = salary * _benefitsConfiguration.PaychecksPerYear;
		if (baseAnnualSalary <= _benefitsConfiguration.SalaryAnnualBenefitThreshold)
		{
			return null;
		}

		var multiplier = _benefitsConfiguration.SalaryAnnualBenefitPercentage / 100;

		return new BenefitDto
		{
			Amount = baseAnnualSalary * multiplier,
			Description = $"Annual {_benefitsConfiguration.SalaryAnnualBenefitPercentage}% benefit for annual salaries over {_benefitsConfiguration.SalaryAnnualBenefitThreshold}"
		};
	}

	public BenefitDto? CalculateOlderDependentBenefit(ICollection<Dependent> dependents)
	{
		var olderDependentsCount = dependents.Count(x => x.Age > _benefitsConfiguration.AgeBenefitThreshold);
		if (olderDependentsCount == 0)
		{
			return null;
		}

		return new BenefitDto
		{
			Amount = olderDependentsCount * _benefitsConfiguration.AgeBenefitPerMonth * 12,
			Description = $"Older dependent benefit for {olderDependentsCount} dependents over {_benefitsConfiguration.AgeBenefitThreshold} years of age"
		};
	}
}
