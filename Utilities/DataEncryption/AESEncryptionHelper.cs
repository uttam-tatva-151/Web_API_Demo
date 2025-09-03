using System.Security.Cryptography;
using System.Text;

namespace Common.DataEncryption;

public class AESEncryptionHelper
{
    public static string EncryptString(string plainText, string _key, string _iv)
    {
        using Aes aes = Aes.Create();
        aes.Key =  Convert.FromBase64String(_key);
        aes.IV = Convert.FromBase64String(_iv);
        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
        using StreamWriter sw = new(cs, Encoding.UTF8);
        sw.Write(plainText);
        sw.Flush();
        cs.FlushFinalBlock();
        return Convert.ToBase64String(ms.ToArray());
    }

    public static string DecryptString(string cipherText, string _key, string _iv)
    {
        byte[] buffer = Convert.FromBase64String(cipherText);
        using Aes aes = Aes.Create();
        aes.Key =  Convert.FromBase64String(_key);
        aes.IV = Convert.FromBase64String(_iv);
        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using MemoryStream ms = new(buffer);
        using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
        using StreamReader sr = new(cs, Encoding.UTF8);
        return sr.ReadToEnd();
    }
}
