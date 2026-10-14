using BudgetAssistant.Core.ContributorAggregate;

namespace BudgetAssistant.UseCases.Contributors;
public record ContributorDto(ContributorId Id, ContributorName Name, PhoneNumber PhoneNumber);
