using BudgetAssistant.Core.ContributorAggregate;

namespace BudgetAssistant.UseCases.Contributors.Update;

public record UpdateContributorCommand(ContributorId ContributorId, ContributorName NewName) : ICommand<Result<ContributorDto>>;
