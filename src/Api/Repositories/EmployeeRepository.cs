using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
	private readonly PaylocityContext _context;

	public EmployeeRepository(PaylocityContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<Employee?> GetWithDependents(int id, CancellationToken cancellationToken)
	{
		return await _context.Employees
			.AsNoTracking()
			.Where(x => x.Id == id)
			.Include(x => x.Dependents).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<IReadOnlyCollection<Employee>> GetAllWithDependents(CancellationToken cancellationToken)
	{
		return await _context.Employees
			.AsNoTracking()
			.Include(x => x.Dependents)
			.ToListAsync(cancellationToken);
	}
}
