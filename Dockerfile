# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Use NuGet sources that work inside Linux containers (no Windows local feed)
COPY nuget.Docker.config ./nuget.config
COPY Directory.Build.props Directory.Packages.props global.json ./
COPY src/BudgetAssistant.Web/BudgetAssistant.Web.csproj src/BudgetAssistant.Web/
COPY src/BudgetAssistant.Infrastructure/BudgetAssistant.Infrastructure.csproj src/BudgetAssistant.Infrastructure/
COPY src/BudgetAssistant.UseCases/BudgetAssistant.UseCases.csproj src/BudgetAssistant.UseCases/
COPY src/BudgetAssistant.Core/BudgetAssistant.Core.csproj src/BudgetAssistant.Core/
COPY src/BudgetAssistant.ServiceDefaults/BudgetAssistant.ServiceDefaults.csproj src/BudgetAssistant.ServiceDefaults/

# Disable NuGet audit during image builds; TreatWarningsAsErrors turns NU19xx into hard failures
RUN dotnet restore src/BudgetAssistant.Web/BudgetAssistant.Web.csproj -p:NuGetAudit=false

COPY src/ ./src/
RUN dotnet publish src/BudgetAssistant.Web/BudgetAssistant.Web.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    -p:NuGetAudit=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

HEALTHCHECK --interval=10s --timeout=5s --start-period=40s --retries=5 \
  CMD curl -fsS http://localhost:8080/alive || exit 1

ENTRYPOINT ["dotnet", "BudgetAssistant.Web.dll"]
