﻿
using Accounts.Domain.Models;
using Accounts.Worker.Tasks;

using Herald.MessageQueue.RabbitMq;

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

            services.AddMessageQueueRabbitMq(setup =>
            {
                setup.Host = "localhost";
                setup.Port = "5672";
                setup.ExchangeName = "createaccountmessage";
                setup.Username = "myUserName";
                setup.Password = "myPassword";
            });

            services.AddMediatR(typeof(Account).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}