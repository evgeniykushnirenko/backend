using MinimalBudgetAssistant.Web.Domain.CartAggregate;
using MinimalBudgetAssistant.Web.Domain.GuestUserAggregate;
using MinimalBudgetAssistant.Web.Domain.OrderAggregate;
using MinimalBudgetAssistant.Web.Domain.ProductAggregate;
using Vogen;

namespace MinimalBudgetAssistant.Web.Infrastructure.Data.Config;

[EfCoreConverter<ProductId>]
[EfCoreConverter<CartId>]
[EfCoreConverter<CartItemId>]
[EfCoreConverter<GuestUserId>]
[EfCoreConverter<OrderId>]
[EfCoreConverter<OrderItemId>]
[EfCoreConverter<Quantity>]
[EfCoreConverter<Price>]
internal partial class VogenEfCoreConverters;
