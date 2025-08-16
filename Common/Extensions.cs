namespace Bring2mind.Backup.FileWatcher.Common
{
  public static class Extensions
  {
    public static bool CanRead(this FileInfo fileInfo)
    {
      if (fileInfo == null)
      {
        throw new ArgumentNullException(nameof(fileInfo));
      }
      try
      {
        using (var stream = fileInfo.OpenRead())
        {
          return true;
        }
      }
      catch (UnauthorizedAccessException)
      {
        return false;
      }
      catch (IOException)
      {
        return false;
      }
      catch (Exception)
      {
        // Catch any other exceptions that may occur
        return false;
      }
    }
  }
}
