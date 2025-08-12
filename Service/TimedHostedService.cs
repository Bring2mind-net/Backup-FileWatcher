namespace Bring2mind.Backup.FileWatcher.Service
{
  using Bring2mind.Backup.FileWatcher.Configurations;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  public class TimedHostedService : IHostedService, IDisposable
  {
    private readonly ILogger _logger;
    private Timer _timer;
    private Configuration _configuration;
    private FileChecker _fileChecker;

    public TimedHostedService(ILogger<TimedHostedService> logger, FileChecker fileChecker)
    {
      _logger = logger;
      _configuration = ConfigurationManager.GetConfiguration();
      _fileChecker = fileChecker;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Timed Background Service is starting.");
      _timer = new Timer(DoWork, null, TimeSpan.Zero,
          TimeSpan.FromMinutes(10));
      return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
      _logger.LogInformation("Timed Background Service is working.");
      _fileChecker.Run(_configuration);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Timed Background Service is stopping.");
      _timer?.Change(Timeout.Infinite, 0);
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }
  }
}
