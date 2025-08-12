namespace Bring2mind.Backup.FileWatcher.Configurations
{
  using Bring2mind.Backup.FileWatcher.Common;
  using System.Collections.Generic;
  using System.IO;

  public class ConfigurationManager
  {
    public static Configuration GetConfiguration()
    {
      var res = new List<Configuration>();
      var configFile = "./data/config.json";
      if (!File.Exists(configFile))
      {
        var c = new Configuration();
        Globals.SaveObject(configFile, c);
        return c;
      }
      else
      {
        return Globals.GetObject<Configuration>(configFile, null);
      }
    }
  }
}
