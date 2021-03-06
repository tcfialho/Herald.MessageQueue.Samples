using Accounts.Domain.Messages;
using Accounts.Domain.Models;

using Herald.MessageQueue.HealthCheck.Kafka;
using Herald.MessageQueue.Kafka;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Accounts.Api
{
    public class Startup
    {
        private readonly OpenApiInfo _swaggerInfo = new OpenApiInfo() { Title = nameof(Accounts.Api), Version = "v1.0.0" };

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson();

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(_swaggerInfo.Version, _swaggerInfo);
            });

            services.AddMediatR(typeof(Account).Assembly);

            services.AddHealthChecks().AddKafkaCheck<CreateAccountMessage>();
            services.AddMessageQueueKafka(setup => Configuration.GetSection("MessageQueueOptions").Bind(setup));

            services.AddLogging(loggin => loggin.AddDebug()
                        .AddConsole()
                        .SetMinimumLevel(LogLevel.Information)
                        .AddConfiguration(Configuration.GetSection("Logging")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint($"/swagger/{_swaggerInfo.Version}/swagger.json", _swaggerInfo.Version);
            });

            app.UseHealthChecks("/hc");
        }
    }
}
