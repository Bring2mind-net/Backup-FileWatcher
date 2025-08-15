namespace Bring2mind.Backup.FileWatcher.Configurations
{
  using Newtonsoft.Json;

  [JsonObject]
  public class Configuration
  {
    [JsonProperty("watchPath")]
    public string WatchPath { get; set; } = "";

    [JsonProperty("siteUrl")]
    public string SiteUrl { get; set; } = "";

    [JsonProperty("apiKey")]
    public string ApiKey { get; set; } = "";

    [JsonProperty("encKey")]
    public string EncryptionKey { get; set; } = "";

    [JsonProperty("hostGuid")]
    public string HostGuid { get; set; } = "";

    public bool IsValid()
    {
      return !(string.IsNullOrEmpty(this.SiteUrl)
          || string.IsNullOrEmpty(this.WatchPath)
          || string.IsNullOrEmpty(this.ApiKey));
    }
  }
}
