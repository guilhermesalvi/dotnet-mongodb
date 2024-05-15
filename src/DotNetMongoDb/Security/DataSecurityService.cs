using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;

namespace DotNetMongoDb.Security;

public class DataSecurityService(string encryptionKey)
{
    public BsonBinaryData Encrypt(string plainText)
    {
        var key = Convert.FromBase64String(encryptionKey);
        var iv = new byte[12];
        var tag = new byte[16];

        RandomNumberGenerator.Fill(iv);

        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipher = new byte[plainBytes.Length];

        using var aesGcm = new AesGcm(key, 16);
        aesGcm.Encrypt(iv, plainBytes, cipher, tag);

        var buffer = new byte[iv.Length + tag.Length + cipher.Length];
        Buffer.BlockCopy(iv, 0, buffer, 0, iv.Length);
        Buffer.BlockCopy(tag, 0, buffer, iv.Length, tag.Length);
        Buffer.BlockCopy(cipher, 0, buffer, iv.Length + tag.Length, cipher.Length);

        return new BsonBinaryData(buffer, BsonBinarySubType.Encrypted);
    }

    public string Decrypt(BsonBinaryData encryptedData)
    {
        var iv = encryptedData.Bytes[..12];
        var tag = encryptedData.Bytes[12..28];
        var cipher = encryptedData.Bytes[28..];

        var decryptedBytes = new byte[cipher.Length];

        using var aesGcm = new AesGcm(Convert.FromBase64String(encryptionKey), 16);
        aesGcm.Decrypt(iv, cipher, tag, decryptedBytes);

        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
