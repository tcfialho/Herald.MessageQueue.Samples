using Accounts.Domain.Messages;

using Herald.MessageQueue;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Accounts.Worker.Tasks
{
    public class CreateAccountTask : BackgroundService
    {
        private readonly IMessageQueue _messageQueue;
        private readonly ILogger<CreateAccountTask> _logger;

        public CreateAccountTask(IMessageQueue messageQueue, ILogger<CreateAccountTask> logger)
        {
            _messageQueue = messageQueue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var message in _messageQueue.Receive<CreateAccountMessage>(stoppingToken))
                {
                    _logger.LogInformation($"Consuming message: {DateTime.Now} - {message.Account.Id} - {message.Account.Name}");
                    await _messageQueue.Received(message);
                }
            }
            await Task.CompletedTask;
        }
    }
}