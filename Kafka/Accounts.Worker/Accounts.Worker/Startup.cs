
using Accounts.Domain.Messages;
using Accounts.Domain.Models;
using Accounts.Worker.Tasks;

using Herald.MessageQueue.HealthCheck.Kafka;
using Herald.MessageQueue.Kafka;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddHostedService<CreateAccountTask>();

            services.AddHealthChecks().AddKafkaCheck<CreateAccountMessage>();
            services.AddMessageQueueKafka(setup =>
            {
                setup.Host = "localhost:9092";
                setup.GroupId = nameof(CreateAccountMessage);
            });

            services.AddMediatR(typeof(Account).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/hc");
        }
    }
}
