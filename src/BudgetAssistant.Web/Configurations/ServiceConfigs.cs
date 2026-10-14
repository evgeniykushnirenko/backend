using BudgetAssistant.Core.Interfaces;
using BudgetAssistant.Infrastructure;
using BudgetAssistant.Infrastructure.Email;

namespace BudgetAssistant.Web.Configurations;

public static class ServiceConfigs
{
  public static IServiceCollection AddServiceConfigs(this IServiceCollection services, Microsoft.Extensions.Logging.ILogger logger, WebApplicationBuilder builder)
  {
    services.AddInfrastructureServices(builder.Configuration, logger)
            .AddMediatorSourceGen(logger);

    var useFakeEmail = builder.Configuration.GetValue<bool>("Email:UseFake");
    if (useFakeEmail)
    {
      services.AddScoped<IEmailSender, FakeEmailSender>();
    }
    else if (builder.Environment.IsDevelopment())
    {
      // Use a local test email server - configured in Aspire
      // See: https://ardalis.com/configuring-a-local-test-email-server/
      services.AddScoped<IEmailSender, MimeKitEmailSender>();
    }
    else
    {
      services.AddScoped<IEmailSender, MimeKitEmailSender>();
    }

    logger.LogInformation("{Project} services registered", "Mediator Source Generator and Email Sender");

    return services;
  }


}
