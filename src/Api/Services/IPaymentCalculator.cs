using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services;

public interface IPaymentCalculator
{
	/// <summary>
	/// Calculates annual salary based on the employee salary, their dependents, and configuration.
	/// </summary>
	AnnualSalaryDto CalculateAnnualSalary(Employee employee);

	BenefitDto? CalculateBaseBenefit();
	BenefitDto? CalculateDependentBenefit(int numberOfDependents);
	BenefitDto? CalculateSalaryAnnualBenefit(decimal salary);
	BenefitDto? CalculateOlderDependentBenefit(ICollection<Dependent> dependents);
}
