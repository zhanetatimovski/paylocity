using System.ComponentModel.DataAnnotations;

namespace Api.Configuration;

public class BenefitsConfiguration
{
	public const string Section = "Benefits";

	[Required]
	public short PaychecksPerYear { get; set; }

	[Required]
	public decimal BaseBenefitPerMonth { get; set; }

	[Required]
	public decimal DependentBenefitPerMonth { get; set; }

	/// <summary>
	/// Employees that make more than this amount per year will incur an additional percentage
	/// (configured in <see cref="SalaryAnnualBenefitPercentage"/>) of their yearly salary in benefits costs.
	/// </summary>
	[Required]
	public decimal SalaryAnnualBenefitThreshold { get; set; }

	/// <summary>
	/// Additional benefit based on yearly salary.
	/// </summary>
	[Required]
	public decimal SalaryAnnualBenefitPercentage { get; set; }

	/// <summary>
	/// Age over which the employee will incur an additional benefit (configured in <see cref="AgeBenefitPerMonth"/>) per month.
	/// </summary>
	[Required]
	public short AgeBenefitThreshold { get; set; }

	[Required]
	public decimal AgeBenefitPerMonth { get; set; }
}
