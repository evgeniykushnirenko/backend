using BudgetAssistant.Core.ContributorAggregate;

namespace BudgetAssistant.UseCases.Contributors.Delete;

public record DeleteContributorCommand(ContributorId ContributorId) : ICommand<Result>;
