using Accounts.Domain.Messages;
using Accounts.Domain.Models;

using Herald.MessageQueue;
using Herald.MessageQueue.HealthCheck.RabbitMq;
using Herald.MessageQueue.RabbitMq;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using RabbitMQ.Client;

using System;

namespace Accounts.Api
{
    public static class Teste
    {
        public static IMessageQueueBuilder AddMessageQueueRabbitMq(this IServiceCollection services, Action<MessageQueueOptions> options)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            services.Configure(options);
            MessageQueueOptions messageQueueOptions = new MessageQueueOptions();
            options?.Invoke(messageQueueOptions);
            services.TryAddSingleton(messageQueueOptions);
            services.TryAddSingleton(delegate (IServiceProvider serviceProvider)
            {
                MessageQueueOptions requiredService = serviceProvider.GetRequiredService<MessageQueueOptions>();
                return new ConnectionFactory
                {
                    HostName = requiredService.Host,
                    Port = int.Parse(requiredService.Port),
                    UserName = requiredService.Username,
                    Password = requiredService.Password,
                    DispatchConsumersAsync = true
                }.CreateConnection();
            });
            services.TryAddSingleton(delegate (IServiceProvider serviceProvider)
            {
                serviceProvider.GetRequiredService<MessageQueueOptions>();
                IModel model = serviceProvider.GetRequiredService<IConnection>().CreateModel();
                model.ConfirmSelect();
                model.WaitForConfirmsOrDie();
                model.BasicQos(0u, 1, global: false);
                return model;
            });
            services.TryAddSingleton<IMessageQueue, MessageQueueRabbitMq>();
            return new MessageQueueBuilder(services);
        }
    }

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

            services.AddMessageQueueRabbitMq(setup =>
            {
                setup.Host = "localhost";
                setup.Port = "5672";
                setup.ExchangeName = nameof(CreateAccountMessage);
                setup.Username = "myUserName";
                setup.Password = "myPassword";
            });

            services.AddHealthChecks()
                    .AddRabbitMqCheck<CreateAccountMessage>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

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
