using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KM.Common
{
    [Serializable]
    public class Encryption
    {
        private const string PasswordCharset = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
        private const string PasswordNonAlphaCharset = "!@#$%^&*()_-+=[{]};:<>|./?";
        private const int DefaultPasswordLength = 64;
        private const int DefaultNumberOfNonAlphaNumericChars = 12;

        public static string Encrypt(string plainText, KM.Common.Entity.Encryption ec)
        {

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(ec.InitVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(ec.SaltValue);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes password = new PasswordDeriveBytes(ec.PassPhrase, saltValueBytes, ec.HashAlgorithm, ec.PasswordIterations);

            // Use the password to generate pseudo-random bytes for the encryption key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(ec.KeySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining(CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization vector. Key size will be defined based on the number of the key bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }

        public static string Decrypt(string cipherText, KM.Common.Entity.Encryption ec)
        {
            if (!string.IsNullOrEmpty(cipherText))
            {
                // Convert strings defining encryption key characteristics into byte arrays. Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8 encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(ec.InitVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(ec.SaltValue);

                // Convert our ciphertext into a byte array.
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                // First, we must create a password, from which the key will be derived. This password will be generated from the specified 
                // passphrase and salt value. The password will be created using the specified hash algorithm. Password creation can be done in several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(ec.PassPhrase, saltValueBytes, ec.HashAlgorithm, ec.PasswordIterations);

                // Use the password to generate pseudo-random bytes for the encryption key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(ec.KeySize / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining(CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization vector. Key size will be defined based on the number of the key bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data will be, allocate the buffer long enough to hold ciphertext; plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                           0,
                                                           plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string. Let us assume that the original plaintext string was UTF8-encoded.
                string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                // Return decrypted string.   
                return plainText;
            }
            else
                return string.Empty;
        }

        public static string Base64Encrypt(string plainText, KM.Common.Entity.Encryption ec)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(Encrypt(plainText, ec));

            return Convert.ToBase64String(bytes); 
        }

        public static string Base64Decrypt(string plainText, KM.Common.Entity.Encryption ec)
        {
            byte[] bytes = Convert.FromBase64String(plainText);
            
            return Decrypt(Encoding.ASCII.GetString(bytes), ec);

        }

        public string GetRandomSalt()
        {
            return GeneratePassword(DefaultPasswordLength, DefaultNumberOfNonAlphaNumericChars);
        }

        public string GeneratePassword(int length, int numberOfNonAlphaNumericChars)
        {
            Guard.For(
                () => length <= 0,
                () => new ArgumentOutOfRangeException(nameof(length)));

            Guard.For(
                () => numberOfNonAlphaNumericChars < 0,
                () => new ArgumentOutOfRangeException(nameof(numberOfNonAlphaNumericChars)));

            Guard.For(
                () => numberOfNonAlphaNumericChars > length,
                () => new ArgumentOutOfRangeException(nameof(numberOfNonAlphaNumericChars)));

            var index = 0;
            var random = new Random();
            var pos = new int[length];
            while (index < length - 1)
            {
                var j = 0;
                var flag = false;
                var temp = random.Next(0, length);
                for (j = 0; j < length; j++)
                {
                    if (temp == pos[j])
                    {
                        flag = true;
                        j = length;
                    }
                }

                if (!flag)
                {
                    pos[index] = temp;
                    index++;
                }
            }

            var password = new char[length];
            for (var i = 0; i < length - numberOfNonAlphaNumericChars; i++)
            {
                var randomIndex = random.Next(0, PasswordCharset.Length);
                password[i] = PasswordCharset[randomIndex];
            }
            
            for (var i = length - numberOfNonAlphaNumericChars; i < length; i++)
            {
                var randomIndex = random.Next(0, PasswordNonAlphaCharset.Length);
                password[i] = PasswordNonAlphaCharset[randomIndex];
            }
            
            var sorted = new char[length];
            for (var i = 0; i < length; i++)
            {
                var passwordIndex = pos[i];
                sorted[i] = password[passwordIndex];
            }

            return new string(sorted);
        }
    }
}
