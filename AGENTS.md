# Backend AGENTS.md

Delta for `code/backend/`. Root `AGENTS.md` and `docs/PROJECT_CONTEXT.md` still apply.
Closest file wins for conflicts.

## Commands (run from `code/backend`)

```sh
dotnet build
dotnet test
dotnet test --filter FullyQualifiedName~Contributor

dotnet run --project src/BudgetAssistant.AspireHost
dotnet run --project src/BudgetAssistant.Web --launch-profile https

# Migrations (prefer from repo; paths relative to this directory)
dotnet ef migrations add <Name> -c AppDbContext \
  -p src/BudgetAssistant.Infrastructure/BudgetAssistant.Infrastructure.csproj \
  -s src/BudgetAssistant.Web/BudgetAssistant.Web.csproj \
  -o Data/Migrations

dotnet ef database update -c AppDbContext \
  -p src/BudgetAssistant.Infrastructure/BudgetAssistant.Infrastructure.csproj \
  -s src/BudgetAssistant.Web/BudgetAssistant.Web.csproj
```

- SDK pinned in `global.json` (.NET 10). Warnings are errors (`Directory.Build.props`).
- Package versions are central: edit `Directory.Packages.props`, not random Version= in csproj.
- Aspire applies migrations on startup in Development; Web-only on macOS/Linux often uses SQLite.

## Layer checklist

When adding a budget-domain feature, touch layers in this order:

1. **Core** — aggregate / entity / value objects (`Vogen`), domain events, interfaces (`IRepository<>`, etc.)
2. **UseCases** — folder per feature action: `FeatureName/{Create,Get,List,Update,Delete}/` with Command|Query + Handler; return `Ardalis.Result`
3. **Infrastructure** — EF `IEntityTypeConfiguration<>`, repositories/query services, external clients (rates, Redis later)
4. **Web** — FastEndpoints under `BudgetAssistant.Web/<Feature>/`; validate with FluentValidation; `IMediator.Send`; map via existing result extensions
5. **Tests** — unit for domain/handlers; integration for EF; functional for HTTP

List queries may use a read-side `I*QueryService` implemented in Infrastructure (see Contributors List).

## Patterns to copy

- Aggregate: `src/BudgetAssistant.Core/ContributorAggregate/`
- Use case: `src/BudgetAssistant.UseCases/Contributors/Create/`
- Endpoint: `src/BudgetAssistant.Web/Contributors/Create.cs` (and sibling request/validator files for larger endpoints)
- EF config: `src/BudgetAssistant.Infrastructure/Data/Config/`

Use **`Mediator`** (`ICommandHandler` / `IQueryHandler` / `IMediator`) — not MediatR.
Use **FastEndpoints** — not MVC controllers.

## Money and rates (when implementing)

- `decimal` everywhere; configure SQL `DECIMAL` precision in EF configs.
- Persist original amount, original currency, converted amount, rate, rate timestamp, and rate source.
- Exchange-rate API + Redis cache: abstractions in UseCases/Core; implementations in Infrastructure only.
- Never invent rates; stale cache fallback must be visible in the result/API.

## Do not

- Put product features in `MinimalClean/` (template only).
- Reference Infrastructure from Core or UseCases.
- Hand-edit `Data/Migrations/*` or `AppDbContextModelSnapshot.cs` to fix schema — add a migration.
- Disable `TreatWarningsAsErrors` or sprinkle `#pragma warning` without cause.
- Commit `appsettings.*.json` secrets; use env vars / user secrets / `.env` at repo root for Compose.


## Done when

- `dotnet build` succeeds
- Affected tests pass (`dotnet test` with a filter is fine while iterating; full `dotnet test` before finishing larger changes)
- New public endpoints documented via FastEndpoints Summary/Tags (Scalar)
- Migration added if the model changed
