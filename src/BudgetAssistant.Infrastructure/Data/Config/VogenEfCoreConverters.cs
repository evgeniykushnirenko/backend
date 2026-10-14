using BudgetAssistant.Core.ContributorAggregate;
using Vogen;

namespace BudgetAssistant.Infrastructure.Data.Config;

[EfCoreConverter<ContributorId>]
[EfCoreConverter<ContributorName>]
internal partial class VogenEfCoreConverters;
