using Hogwarts.Context;
using Hogwarts.View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hogwarts
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<WizardContext>();
                    services.AddTransient<StartUp>();
                    services.AddTransient<Add>();
                })
                .Build();

            using var serviceScope = host.Services.CreateScope();
            var startForm = serviceScope.ServiceProvider.GetRequiredService<StartUp>();

            Application.Run(startForm);
        }
    }
}
