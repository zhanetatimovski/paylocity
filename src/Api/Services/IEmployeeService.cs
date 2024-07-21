using Api.Dtos.Employee;

namespace Api.Services;

public interface IEmployeeService
{
	Task<GetEmployeeDto?> Get(int id, CancellationToken cancellationToken);
	Task<IReadOnlyCollection<GetEmployeeDto>> GetAll(CancellationToken cancellationToken);
}
