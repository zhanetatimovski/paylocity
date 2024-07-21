using Api.Models;

namespace Api.Dtos.Dependent;

public class GetDependentDto
{
	public int Id { get; init; }
	public string? FirstName { get; init; }
	public string? LastName { get; init; }
	public DateTime DateOfBirth { get; init; }
	public Relationship Relationship { get; init; }
}
