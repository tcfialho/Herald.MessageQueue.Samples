
using Accounts.Domain.Messages;
using Accounts.Domain.Models;

using Herald.MessageQueue.HealthCheck.RabbitMq;
using Herald.MessageQueue.RabbitMq;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounts.Worker
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Account).Assembly);

            services.AddHealthChecks().AddRabbitMqCheck<CreateAccountMessage>();
            services.AddMessageQueueRabbitMq(setup => Configuration.GetSection("MessageQueueOptions").Bind(setup));

            services.AddLogging(loggin => loggin.AddDebug()
                                    .AddConsole()
                                    .SetMinimumLevel(LogLevel.Information)
                                    .AddConfiguration(Configuration.GetSection("Logging")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/hc");
        }
    }
}
