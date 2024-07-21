using Api.Repositories;

namespace Api.Extensions;

public static class DatabaseExtensions
{
	/// <summary>
	/// Creates the tables if they haven't been created, and seeds.
	/// </summary>
	public static void InitializeDatabase(this IApplicationBuilder appBuilder)
	{
		using var scope = appBuilder.ApplicationServices.CreateScope();

		using var context = scope.ServiceProvider.GetRequiredService<PaylocityContext>();
		DatabaseSeeder.CreateAndSeed(context);
	}
}
