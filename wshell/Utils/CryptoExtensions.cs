using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace wshell.Utils
{
    public static class CryptoExtensions
    {
        internal static byte[] EncryptAES(this byte[] plainBytes, byte[] secretShared)
        {
            Aes encryptor = Aes.Create();
            encryptor.Mode = CipherMode.ECB;
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(secretShared);
            encryptor.Key = key;
            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return cipherBytes;
        }
        internal static byte[] DecryptAES(this byte[] cipherBytes, byte[] secretShared)
        {
            Aes encryptor = Aes.Create();
            encryptor.Mode = CipherMode.ECB;
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(secretShared);
            encryptor.Key = key;
            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            byte[] plainBytes = Array.Empty<byte>();

            try
            {
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                cryptoStream.FlushFinalBlock();
                plainBytes = memoryStream.ToArray();
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }
            return plainBytes;
        }
    }
}
