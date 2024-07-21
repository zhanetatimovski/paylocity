namespace Api.Dtos.Employee;

public class AnnualSalaryDto
{
	public decimal Salary { get; set; }
	public required IReadOnlyCollection<BenefitDto> Benefits { get; set; }
}
