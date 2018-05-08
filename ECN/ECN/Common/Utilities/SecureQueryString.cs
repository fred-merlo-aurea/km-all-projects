using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web;



namespace ecn.common.classes.utilities
{
	/// <summary>
	/// Provides a secure means for transfering data within a query string.
	/// </summary>
	public class SecureQueryString : NameValueCollection
	{
		// The Initialization Vector for the DES encryption routine
		private readonly byte[] IV = new byte[8] {240, 3, 45, 29, 0, 76, 173, 59};

		private const string timeStampKey = "__TimeStamp__";		
		private const string InvalidQueryStringExceptionMessage = " ";
		
		// The key used for generating the encrypted string
		private const string cryptoKey = "ecn";		
		
		public SecureQueryString() : base() {}
		
		public SecureQueryString(string encryptedString) 
		{
			deserialize(decrypt(encryptedString));
			
			// Compare the Expiration Time with the current Time to ensure
			// that the queryString has not expired.
			if (DateTime.Compare(ExpireTime, DateTime.Now) < 0) 
			{
				throw new ExpiredQueryStringException();
			}
		}

		/// <summary>
		/// Returns the encrypted query string.
		/// </summary>
		public string EncryptedString 
		{
			get 
			{
				return HttpUtility.UrlEncode(encrypt(serialize()));
			}
		}

		private DateTime _expireTime = DateTime.MaxValue;
		/// <summary>
		/// The timestamp in which the EncryptedString should expire
		/// </summary>
		public DateTime ExpireTime 
		{
			get 
			{
				return _expireTime;
			}
			set 
			{
				_expireTime = value;
			}
		}

		/// <summary>
		/// Returns the EncryptedString property.
		/// </summary>
		public override string ToString()
		{
			return EncryptedString;
		}

		/// <summary>
		/// Encrypts a serialized query string 
		/// </summary>
		private string encrypt(string serializedQueryString) 
		{
			byte[] buffer = Encoding.ASCII.GetBytes(serializedQueryString);
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
			des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cryptoKey));
			des.IV = IV;
			return Convert.ToBase64String(
				des.CreateEncryptor().TransformFinalBlock(
				buffer,
				0,
				buffer.Length
				)
				);
		}

		/// <summary>
		/// Decrypts a serialized query string
		/// </summary>
		private string decrypt(string encryptedQueryString) 
		{
			try 
			{
				byte[] buffer = Convert.FromBase64String(encryptedQueryString);
				TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
				MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
				des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cryptoKey));
				des.IV = IV;
				return Encoding.ASCII.GetString(
					des.CreateDecryptor().TransformFinalBlock(
					buffer,
					0,
					buffer.Length
					)
					);
			} 
			catch (CryptographicException) 
			{
				throw new InvalidQueryStringException();
			}
			catch (FormatException) 
			{
				throw new InvalidQueryStringException();
			}
		}

		/// <summary>
		/// Deserializes a decrypted query string and stores it
		/// as name/value pairs.
		/// </summary>
		private void deserialize(string decryptedQueryString) 
		{
			string[] nameValuePairs = decryptedQueryString.Split('&');
			for (int i=0; i<nameValuePairs.Length; i++) 
			{
				string[] nameValue = nameValuePairs[i].Split('=');
				if (nameValue.Length == 2) 
				{
					base.Add(nameValue[0], nameValue[1]);
				}
			}
			// Ensure that timeStampKey exists and update the expiration time.
			if (base[timeStampKey] != null) 
				_expireTime = DateTime.Parse(base[timeStampKey]);
		}

		/// <summary>
		/// Serializes the underlying NameValueCollection as a QueryString
		/// </summary>
		private string serialize()
		{
			

			StringBuilder sb = new StringBuilder();
			foreach (string key in base.AllKeys) 
			{
				if(String.Compare(key,timeStampKey) != 0 )
				{
					sb.Append(key);
					sb.Append('=');
					sb.Append(base[key]);
					sb.Append('&');
				}
			}

			// Append timestamp
			sb.Append(timeStampKey);
			sb.Append('=');
			sb.Append(_expireTime);

			return sb.ToString();
		}


		
		//Encrypt each Key
		public string EncryptThis(string ToEncrypt) 
		{ 
			byte[] buffer = Encoding.ASCII.GetBytes(ToEncrypt); 
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider(); 
			MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider(); 
			des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cryptoKey)); 
			des.IV = IV; 
			return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer,0,buffer.Length)); 
		} 

		//Decrypt each Key
		public string DecryptThis(string ToDecrypt) 
		{
			try 
			{
				byte[] buffer = Convert.FromBase64String(ToDecrypt); 
				TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider(); 
				MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider(); 
				des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cryptoKey)); 
				des.IV = IV; 
				return Encoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buffer,0,buffer.Length)); 
			} 
			catch (CryptographicException) 
			{
				throw new InvalidQueryStringException("Invalid Query String");
			}
			catch (FormatException) 
			{
				throw new InvalidQueryStringException("Invalid Query String");
			}
		}


//		private string  RSAEncrypt(string ToEncrypt)
//		{
//			byte[] CypherTextBArray;
//			byte[] PlainTextBArray;
//			string xmlPublicKey="";
//
//			RSACryptoServiceProvider rsa=new  RSACryptoServiceProvider();
//		
//			//get the public key so you can encrypt the message:
//			rsa.FromXmlString(xmlPublicKey);
//
//			//get the message
//
//	        //transform message string into a byte array:
//			  
//		    PlainTextBArray =(new UnicodeEncoding()).GetBytes(ToEncrypt);
//
//			// Encrypt 
//			CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
//
//			return Convert.ToBase64String(CypherTextBArray);
//
//		}
//
//		private string  RSADecrypt(string ToEncrypt)
//		{
//			byte[] RestoredPlainText;
//			byte[] CypherTextBArray=Convert.ToByte(ToEncrypt);
//			string xmlPublicKey="";
//			RSACryptoServiceProvider rsa=new RSACryptoServiceProvider();
//			rsa.FromXmlString(xmlPublicKey);
//			RestoredPlainText= rsa.Decrypt(CypherTextBArray, false);
//			return Convert.ToBase64String(RestoredPlainText);
//
//		}



  }

}
