// Active Calendar v2.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace ActiveUp.WebControls
{
	#region class Cryptography

	/// <summary>
	/// Encryption and decryption of data.
	/// </summary>
	internal class Cryptography
	{

		#region Functions

		/// <summary>
		/// Crypts a string.
		/// </summary>
		/// <param name="pToEncrypt">String to crypt.</param>
		/// <returns>Crypted string.</returns>
		public static string Encrypt(string pToEncrypt) 
		{
			return Encrypt(pToEncrypt, "4Jkw9N3f");
		}

		/// <summary>
		/// Crypts a string.
		/// </summary>
		/// <param name="pToEncrypt">String to crypt.</param>
		/// <param name="sKey">Key used in the encryption.</param>
		/// <returns>Crypted string.</returns>
		public static string Encrypt(string pToEncrypt, string sKey) 
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();

			//Put the string into a byte array
			byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);

			//Create the crypto objects, with the key, as passed in
			des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
			des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(),
				CryptoStreamMode.Write);
			//Write the byte array into the crypto stream
			//(It will end up in the memory stream)
			cs.Write(inputByteArray, 0, inputByteArray.Length);
			cs.FlushFinalBlock();

			//Get the data back from the memory stream, and into a string
			StringBuilder ret = new StringBuilder();
			foreach(byte b in ms.ToArray())
			{
				//Format as hex
				ret.AppendFormat("{0:X2}", b);
			}
			return ret.ToString();
		}

		/// <summary>
		/// Decrypts a string.
		/// </summary>
		/// <param name="pToDecrypt">String to decrypt.</param>
		/// <returns>Decrypted string.</returns>
		public static string Decrypt(string pToDecrypt) 
		{
			return Decrypt(pToDecrypt, "4Jkw9N3f");
		}

		/// <summary>
		/// Decrypts a string.
		/// </summary>
		/// <param name="pToDecrypt">String to decrypt.</param>
		/// <param name="sKey">Key used in the decryption.</param>
		/// <returns>Decrypted string.</returns>
		public static string Decrypt(string pToDecrypt, string sKey) 
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();

			//Put the input string into the byte array
			byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
			for(int x = 0; x < pToDecrypt.Length / 2; x++)
			{
				int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
				inputByteArray[x] = (byte)i;
			}

			//Create the crypto objects
			des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
			des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(),
				CryptoStreamMode.Write);
			//Flush the data through the crypto stream into the memory stream
			cs.Write(inputByteArray, 0, inputByteArray.Length);
			cs.FlushFinalBlock();

			//Get the decrypted data back from the memory stream
			StringBuilder ret = new StringBuilder();
			foreach(byte b in ms.ToArray())
			{
				ret.Append((char)b);
			}
			return ret.ToString();
		}

		#endregion
	}

	#endregion
}
