using BudgetAssistant.Core.ContributorAggregate;

namespace BudgetAssistant.UseCases.Contributors.Get;

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>;
