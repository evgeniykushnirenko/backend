using MinimalBudgetAssistant.Web.Domain.CartAggregate;

namespace MinimalBudgetAssistant.Web.CartFeatures;

public record CartDto(CartId Id, IReadOnlyList<CartItemDto> Items, decimal Total);

public record CartItemDto(int ProductId, int Quantity, decimal UnitPrice, decimal TotalPrice);
