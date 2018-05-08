using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net.Mail;
using System.Data;
using System.Data.SqlClient;

namespace KMPS_JF_Objects.Objects
{
    public class Utilities
    {
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }


        public static bool ExternalHttpPost(string postURL)
        {
            if (!postURL.StartsWith("http://"))
                postURL = "http://" + postURL;
            postURL = postURL.Replace("#", "");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(postURL);
            webRequest.Method = "GET";
            HttpWebResponse WebResp = (HttpWebResponse)webRequest.GetResponse();
            if (WebResp.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public static string Encrypt(string plainText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {                      
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector); 
            byte[] saltValueBytes = Encoding.UTF8.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();    

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             initVectorBytes);     

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
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

        public static string Decrypt(string cipherText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.UTF8.GetBytes(saltValue);

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText.Replace(" ","+"));

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                          decryptor,
                                                          CryptoStreamMode.Read);

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                       0,
                                                       decryptedByteCount);

            // Return decrypted string.   
            return plainText;
        }

        public static void SendMail(string emailmsg)
        {
            string body = emailmsg;
            MailMessage mm = new MailMessage();
            mm.Body = body;
            mm.IsBodyHtml = true;

            mm.From = new MailAddress("info@knowledgemarketing.com");
            mm.Subject = "JFs Issues";
            mm.To.Add("sunil.theenathayalu@knowledgemarketing.com");

            SmtpClient sc = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);      
            sc.Send(mm);
        }

        public static void SendNotification(string FromEmail, string FromName, string ToEmail, string Subject, string Body)
        {
            try
            {
                if (FromEmail.Trim().Length > 0 && ToEmail.Trim().Length > 0 && Body.Trim().Length > 0)
                {
                    System.Net.Mail.MailMessage simpleMail = new System.Net.Mail.MailMessage();
                    simpleMail.From = new System.Net.Mail.MailAddress(FromEmail, FromName);
                    simpleMail.To.Add(ToEmail);
                    simpleMail.Subject = Subject;
                    simpleMail.Body = Body;
                    simpleMail.IsBodyHtml = true;
                    simpleMail.Priority = System.Net.Mail.MailPriority.Normal;

                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString());
                    smtpclient.Send(simpleMail);
                }
            }
            catch 
            { }
        }
    }
}
