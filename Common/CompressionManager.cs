
namespace Bring2mind.Backup.FileWatcher.Common
{
  using System.IO.Compression;

  public class CompressionManager
  {
    public static void ZipFile(string inFile, string outFile)
    {
      using (var zipOut = new ZipArchive(File.Create(outFile), ZipArchiveMode.Create))
      {
        var fileToProcess = new FileInfo(inFile);
        var objZipEntry = zipOut.CreateEntry(Path.GetFileName(inFile));
        using (var fileIn = new FileStream(inFile, FileMode.Open, FileAccess.Read))
        {
          using (var outStream = objZipEntry.Open())
          {
            Globals.CopyBigStream(fileIn, outStream);
          }
        }
      }
    }
  }
}
