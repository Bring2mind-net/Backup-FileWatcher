namespace Bring2mind.Backup.FileWatcher.Common
{
  using System.Security.Cryptography;
  using System.Text;

  public class EncryptionManager
  {
    /// <summary>
    /// In part from http://johnrush.github.io/File-Encryption-Tutorial/
    /// </summary>
    public static void EncryptFile(string inFile, string outFile, string encryptionKey, string salt)
    {
      var iterations = 1000;
      byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
      var derivedBytes = new Rfc2898DeriveBytes(encryptionKey, saltBytes, iterations);
      byte[] derivedKey = derivedBytes.GetBytes(32); // 256 bits
      byte[] derivedInitVector = derivedBytes.GetBytes(16); // 128 bits

      using (var sourceStream = File.OpenRead(inFile))
      using (var destinationStream = File.Create(outFile))
      using (var aesProvider = new AesCryptoServiceProvider()
      {
        KeySize = 256,
        Padding = PaddingMode.ISO10126,
        Mode = CipherMode.CBC
      })
      using (var cryptoTransform = aesProvider.CreateEncryptor(derivedKey, derivedInitVector))
      using (var cryptoStream = new CryptoStream(destinationStream, cryptoTransform, CryptoStreamMode.Write))
      {
        sourceStream.CopyTo(cryptoStream);
      }
    }

  }
}
