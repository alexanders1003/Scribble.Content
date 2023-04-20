using Calabonga.AspNetCore.AppDefinitions;
using MassTransit;

namespace Scribble.Content.Web.Definitions.MessageBroker;

public class MessageBrokerDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        var section = builder.Configuration.GetSection("MessageBrokerHostOptions");

        if (section == null)
            throw new ArgumentNullException(nameof(section),
                "'MessageBrokerOptions' section is undefined in the appsettings.json file");

        var brokerOptions = section.Get<MessageBrokerConfiguration>();

        if (brokerOptions == null)
            throw new ArgumentNullException(nameof(brokerOptions),
                $"Cannot bind the options from appsettings.json file to '{nameof(MessageBrokerConfiguration)}' class");

        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();

            configurator.UsingRabbitMq((context, config) =>
            {
                config.Host(brokerOptions.Host, brokerOptions.VirtualHost, hostConfigurator =>
                {
                    hostConfigurator.Username(brokerOptions.Username);
                    hostConfigurator.Password(brokerOptions.Password);
                });

                config.ConfigureEndpoints(context);
            });
        });

        services.AddOptions<MassTransitHostOptions>()
            .Configure(options =>
            {
                options.WaitUntilStarted = true;
                options.StartTimeout = TimeSpan.FromSeconds(10);
                options.StopTimeout = TimeSpan.FromSeconds(30);
            });
    }
}