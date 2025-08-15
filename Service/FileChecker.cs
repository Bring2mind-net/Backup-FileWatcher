namespace Bring2mind.Backup.FileWatcher.Service
{
  using Bring2mind.Backup.FileWatcher.Common;
  using Bring2mind.Backup.FileWatcher.Configurations;
  using Microsoft.Extensions.Logging;

  public class FileChecker
  {
    private readonly ILogger _logger;

    public FileChecker(ILogger<FileChecker> logger)
    {
      _logger = logger;
    }

    public void Run(Configuration config)
    {
      _logger.LogInformation("Running FileChecker. Looking for {0}.", config.WatchPath);
      try
      {
        if (config.IsValid())
        {
          var d = new DirectoryInfo(config.WatchPath);
          if (d.Exists)
          {
            _logger.LogInformation("Examining {0}.", d.FullName);
            var filesToDelete = new List<string>();
            foreach (var backupFile in d.GetFiles("*.bak"))
            {
              var backupFileRoot = Path.Combine(d.FullName, backupFile.Name.Substring(0, backupFile.Name.Length - 4));
              var backupCompFile = $"{backupFileRoot}.comp.resources";
              if (!File.Exists(backupCompFile))
              {
                _logger.LogInformation("Backup file {BackupFile} does not have a corresponding compressed resources file.", backupFile.Name);
                CompressionManager.ZipFile(backupFile.FullName, backupCompFile);
              }
              else
              {
                _logger.LogInformation("Backup file {BackupFile} and compressed resources file {BackupCompFile} are present.", backupFile.Name, backupCompFile);
              }
              var encFile = $"{backupFileRoot}.comp.enc.resources";
              if (!File.Exists(encFile))
              {
                _logger.LogInformation("Backup file {BackupFile} does not have a corresponding encrypted resources file.", backupFile.Name);
                EncryptionManager.EncryptFile(backupCompFile, encFile, config.EncryptionKey, config.HostGuid);
                filesToDelete.Add(backupCompFile);
              }
              else
              {
                _logger.LogInformation("Backup file {BackupFile} and encrypted resources file {EncFile} are present.", backupFile.Name, encFile);
              }
              filesToDelete.Add(backupFile.FullName);
            }

            // clean up
            if (filesToDelete.Count > 0)
            {
              _logger.LogInformation("Deleting backup files: {Files}", string.Join(", ", filesToDelete));
              foreach (var file in filesToDelete)
              {
                File.Delete(file);
              }
            }
            filesToDelete.Clear();

            var uploadManager = new UploadManager(config);
            foreach (var backupFile in d.GetFiles("*.comp.enc.resources"))
            {
              try
              {
                _logger.LogInformation("Uploading file {BackupFile} to the server.", backupFile.Name);
                uploadManager.UploadFile(backupFile.FullName);
                filesToDelete.Add(backupFile.FullName);
              }
              catch (Exception ex)
              {
                _logger.LogError(ex, "An error occurred while uploading file {BackupFile}.", backupFile.Name);
              }
            }

            // clean up
            if (filesToDelete.Count > 0)
            {
              _logger.LogInformation("Deleting backup files: {Files}", string.Join(", ", filesToDelete));
              foreach (var file in filesToDelete)
              {
                File.Delete(file);
              }
            }
            filesToDelete.Clear();
          }
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred while checking files.");
      }
    }
  }
}
