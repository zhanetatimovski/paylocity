using Api.Dtos;
using Api.Dtos.Dependent;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
	private readonly IDependentService _dependentService;

	public DependentsController(IDependentService dependentService)
	{
		_dependentService = dependentService;
	}

	[SwaggerOperation(Summary = "Get dependent by id")]
	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
	{
		var dependent = await _dependentService.Get(id, cancellationToken);
		if (dependent is null)
		{
			return NotFound();
		}

		var payload = new ApiResponse<GetDependentDto> { Data = dependent };

		return Ok(payload);
	}

	[SwaggerOperation(Summary = "Get all dependents")]
	[HttpGet]
	public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll(CancellationToken cancellationToken)
	{
		var dependents = await _dependentService.GetAll(cancellationToken);

		var payload = new ApiResponse<IReadOnlyCollection<GetDependentDto>>
		{
			Data = dependents,
			Success = true
		};

		return Ok(payload);
	}
}
