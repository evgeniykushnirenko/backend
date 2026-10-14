using MinimalBudgetAssistant.Web.Domain.ProductAggregate;

namespace MinimalBudgetAssistant.Web.ProductFeatures;
public record ProductDto(ProductId Id, string Name, decimal UnitPrice);
