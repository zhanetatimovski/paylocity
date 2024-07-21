using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class PaylocityContext : DbContext
{
	public PaylocityContext(DbContextOptions<PaylocityContext> options)
		   : base(options)
	{
	}

	public DbSet<Dependent> Dependents { get; set; }
	public DbSet<Employee> Employees { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Dependent>(d =>
		{
			d.Property(d => d.FirstName).HasMaxLength(50);
			d.Property(d => d.LastName).HasMaxLength(50);
		});

		modelBuilder.Entity<Employee>(e =>
		{
			e.Property(e => e.FirstName).HasMaxLength(50);
			e.Property(e => e.LastName).HasMaxLength(50);
			e.Property(e => e.Salary).HasColumnType("money").HasPrecision(2);
		});
	}
}
