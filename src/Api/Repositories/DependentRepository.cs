using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class DependentRepository : IDependentRepository
{
	private readonly PaylocityContext _context;

	public DependentRepository(PaylocityContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<Dependent?> Get(int id, CancellationToken cancellationToken)
	{
		return await _context.Dependents
			.AsNoTracking()
			.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<IReadOnlyCollection<Dependent>> GetAll(CancellationToken cancellationToken)
	{
		return await _context.Dependents.AsNoTracking().ToListAsync(cancellationToken);
	}
}
