using Api.Dtos.Employee;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
	private readonly IEmployeeService _employeeService;

	public EmployeesController(IEmployeeService employeeService)
	{
		_employeeService = employeeService;
	}

	[SwaggerOperation(Summary = "Get employee by id")]
	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
	{
		var employee = await _employeeService.Get(id, cancellationToken);
		if (employee is null)
		{
			return NotFound();
		}

		var payload = new ApiResponse<GetEmployeeDto> { Data = employee };

		return Ok(payload);
	}

	[SwaggerOperation(Summary = "Get all employees")]
	[HttpGet]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var employees = await _employeeService.GetAll(cancellationToken);

		var payload = new ApiResponse<IReadOnlyCollection<GetEmployeeDto>>
		{
			Data = employees,
			Success = true
		};

		return Ok(payload);
	}
}
