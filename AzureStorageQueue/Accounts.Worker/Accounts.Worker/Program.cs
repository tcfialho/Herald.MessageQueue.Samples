using Accounts.Worker.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Diagnostics.CodeAnalysis;

namespace Accounts.Worker
{
    public class Program
    {
        [ExcludeFromCodeCoverage]
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.UseStartup<Startup>();
                       })
                       .ConfigureServices(services =>
                       {
                           services.AddHostedService<CreateAccountTask>();
                       });
        }
    }
}