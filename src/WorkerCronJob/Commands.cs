using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerCronJob
{
    public static class Commands
    {
        public static Command RunCronJob(string[] args)
        {
            var command = new Command("cronjob", "run the application in cronjob mode")
            {
            };

            var argument = new Argument("name")
            {
                Description = "A name of the cron job",
                Arity = ArgumentArity.ZeroOrOne
            };

            command.AddArgument(argument);

            command.Handler = CommandHandler.Create<IConsole, string>((console, name) =>
            {
                console.Out.WriteLine(name);

                return RunAsync(args, name);
            });

            return command;
        }

        private static async Task<int> RunAsync(string[] args, string name)
        {
            using var host = Program.CreateHostBuilder(args).UseConsoleLifetime().Build();

            await host.StartAsync();

            using var scope = host.Services.CreateScope();
            var appLifeTime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();

            await Task.Delay(TimeSpan.FromSeconds(10));

            await host.StopAsync();
            return 0;
        }
    }
}
