using Api.Dtos.Dependent;
using Api.Models;
using Api.Repositories;

namespace Api.Services;

public class DependentService : IDependentService
{
	private readonly IDependentRepository _dependentRepository;

	public DependentService(IDependentRepository dependentRepository)
	{
		_dependentRepository = dependentRepository ?? throw new ArgumentNullException(nameof(dependentRepository));
	}

	public async Task<GetDependentDto?> Get(int id, CancellationToken cancellationToken)
	{
		var dependent = await _dependentRepository.Get(id, cancellationToken);
		if (dependent is null)
		{
			return null;
		}

		var dto = MapDependent(dependent);
		return dto;
	}

	public async Task<IReadOnlyCollection<GetDependentDto>> GetAll(CancellationToken cancellationToken)
	{
		var dependents = await _dependentRepository.GetAll(cancellationToken);

		var dtos = dependents.Select(MapDependent).ToList();
		return dtos;
	}

	private static GetDependentDto MapDependent(Dependent dependent)
	{
		return new()
		{
			Id = dependent.Id,
			FirstName = dependent.FirstName,
			LastName = dependent.LastName,
			DateOfBirth = dependent.DateOfBirth,
			Relationship = dependent.Relationship
		};
	}
}
