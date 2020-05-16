
using Accounts.Domain.Messages;
using Accounts.Domain.Models;
using Accounts.Worker.Tasks;

using Herald.MessageQueue.HealthCheck.Sqs;
using Herald.MessageQueue.Sqs;

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

            services.AddHealthChecks().AddSqsCheck<CreateAccountMessage>();
            services.AddMessageQueueSqs(setup =>
            {
                setup.Host = "http://127.0.0.1";
                setup.Port = "4576";
                setup.GroupId = nameof(CreateAccountMessage);
                setup.RegionEndpoint = "us-east-1";
                setup.VisibilityTimeout = 1;
                setup.WaitTimeSeconds = 1;
                setup.EnableFifo = true;
            });

            services.AddMediatR(typeof(Account).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/hc");
        }
    }
}
