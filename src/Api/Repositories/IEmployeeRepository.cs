using Api.Models;

namespace Api.Repositories;

public interface IEmployeeRepository
{
	Task<Employee?> GetWithDependents(int id, CancellationToken cancellationToken);
	Task<IReadOnlyCollection<Employee>> GetAllWithDependents(CancellationToken cancellationToken);
}
