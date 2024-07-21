using Api.Dtos.Dependent;

namespace Api.Services;

public interface IDependentService
{
	Task<GetDependentDto?> Get(int id, CancellationToken cancellationToken);
	Task<IReadOnlyCollection<GetDependentDto>> GetAll(CancellationToken cancellationToken);
}
