using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repositories;

namespace Api.Services;

public class EmployeeService : IEmployeeService
{
	private readonly IPaymentCalculator _paymentCalculator;
	private readonly IEmployeeRepository _employeeRepository;

	public EmployeeService(
		IPaymentCalculator paymentCalculator,
		IEmployeeRepository employeeRepository)
	{
		_paymentCalculator = paymentCalculator ?? throw new ArgumentNullException(nameof(paymentCalculator));
		_employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
	}

	public async Task<GetEmployeeDto?> Get(int id, CancellationToken cancellationToken)
	{
		var employee = await _employeeRepository.GetWithDependents(id, cancellationToken);
		if (employee is null)
		{
			return null;
		}

		var dto = MapEmployeeWithAnnualSalary(employee);
		return dto;
	}

	public async Task<IReadOnlyCollection<GetEmployeeDto>> GetAll(CancellationToken cancellationToken)
	{
		var employees = await _employeeRepository.GetAllWithDependents(cancellationToken);

		var dtos = employees.Select(MapEmployeeWithAnnualSalary).ToList();
		return dtos;
	}

	private static GetDependentDto MapDependent(Dependent dependent)
		=> new()
		{
			Id = dependent.Id,
			FirstName = dependent.FirstName,
			LastName = dependent.LastName,
			DateOfBirth = dependent.DateOfBirth,
			Relationship = dependent.Relationship
		};

	private GetEmployeeDto MapEmployeeWithAnnualSalary(Employee employee)
		=> new()
		{
			Id = employee.Id,
			FirstName = employee.FirstName,
			LastName = employee.LastName,
			Salary = employee.Salary,
			DateOfBirth = employee.DateOfBirth,
			Dependents = employee.Dependents.Select(MapDependent).ToList(),
			AnnualSalary = _paymentCalculator.CalculateAnnualSalary(employee)
		};
}
