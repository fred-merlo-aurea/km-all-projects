using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

namespace KM.Common.Functions
{
    public class DESEncryption
    {
        public static string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] key = { };
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 }; // it can be any byte value
            var inputByteArray = new byte[stringToDecrypt.Length];


            key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(stringToDecrypt);

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(key, IV), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cryptoStream.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    return encoding.GetString(memoryStream.ToArray());
                }
            }
        }

        public static string Encrypt(string stringToEncrypt, string sEncryptionKey)
        {
            byte[] key = { };
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 }; // it can be any byte value
            byte[] inputByteArray; //Convert.ToByte(stringToEncrypt.Length)

            key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(key, IV), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cryptoStream.FlushFinalBlock();
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
          
        }
    }
}
