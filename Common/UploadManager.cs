using Bring2mind.Backup.FileWatcher.Configurations;
using System.Net.Http.Headers;

namespace Bring2mind.Backup.FileWatcher.Common
{
  public class UploadManager
  {
    private readonly Configuration _configuration;

    public UploadManager(Configuration configuration)
    {
      _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public void UploadFile(string filePath)
    {
      if (string.IsNullOrEmpty(filePath))
      {
        throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
      }
      if (!_configuration.IsValid())
      {
        throw new InvalidOperationException("Configuration is not valid. Please check the settings.");
      }
      // Here you would implement the logic to upload the file to the server.
      // This is a placeholder for the actual upload logic.
      Console.WriteLine($"Uploading file: {filePath} to {_configuration.SiteUrl} with API key {_configuration.ApiKey}");
      var url = $"{_configuration.SiteUrl}/api/Bring2mind/Backup/File/Upload";
      // use POST with form data to upload the file to this url
      using (var client = new HttpClient())
      {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration.ApiKey}");
        using (var form = new MultipartFormDataContent())
        {
          var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
          fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
          form.Add(fileContent, "file", Path.GetFileName(filePath));
          var response = client.PostAsync(url, form).Result;
          if (!response.IsSuccessStatusCode)
          {
            throw new Exception($"Failed to upload file: {response.ReasonPhrase}");
          }
        }
      }
      Console.WriteLine($"File {filePath} uploaded successfully.");
    }
  }
}
