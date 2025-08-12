namespace Bring2mind.Backup.FileWatcher
{
  using Bring2mind.Backup.FileWatcher.Service;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using System.Threading.Tasks;

  public class Program
  {
    public static async Task Main(string[] args)
    {
      Console.WriteLine("Starting FileWatcher...");
      var hostBuilder = new HostBuilder()
      // Add configuration, logging, ...
      .ConfigureServices((hostContext, services) =>
      {
        services.AddHostedService<TimedHostedService>();
        services.AddTransient<FileChecker>();
        services.AddLogging(builder => builder.AddConsole());
      });

      await hostBuilder.RunConsoleAsync();
    }
  }
}
