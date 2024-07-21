using Api.Models;

namespace Api.Repositories;

public interface IDependentRepository
{
	Task<Dependent?> Get(int id, CancellationToken cancellationToken);
	Task<IReadOnlyCollection<Dependent>> GetAll(CancellationToken cancellationToken);
}
