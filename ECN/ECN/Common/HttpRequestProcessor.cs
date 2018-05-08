using System;
using System.Collections.Specialized;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Web;

namespace ecn.common.classes
{
	
	
	
	public class HttpRequestProcessor
	{
		public HttpRequestProcessor(string path) {
			_path = path;
			_keyValuePair = new NameValueCollection();
		}
		
		NameValueCollection _keyValuePair;

		string _path;
		public string Path {
			get { return _path;}
		}

		public string EncryptedHttpRequest {
			get { 
				StringBuilder request = new StringBuilder(Path);
				for(int i=0; i< _keyValuePair.Count; i++) {
					if (i==0) {
						request.Append("?");
					} else {
						request.Append("&");
					}

					request.Append(String.Format("{0}={1}", _keyValuePair.GetKey(i), EncryptValue(_keyValuePair[i])));
				}			
				return request.ToString();
			}
		}		

		public void Add(string key, int val) {
			_keyValuePair.Add(key, val.ToString());
		}

		static string key = "3#eDc1!qAz8*iK,";
		private string EncryptValue(string val) {
			TripleDESCryptoServiceProvider des=null;
			MD5CryptoServiceProvider       hashmd5;
			byte[]                         pwdhash, buff;
			try {
				hashmd5 = new MD5CryptoServiceProvider();
				pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
				hashmd5 = null;
				des = new TripleDESCryptoServiceProvider();
				des.Key = pwdhash;
				des.Mode = CipherMode.ECB; 				
				buff = ASCIIEncoding.ASCII.GetBytes(val);
				return Convert.ToBase64String( des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));					
			} finally {
				if (des!=null) {
					des.Clear();
					des = null;
				}
			}
		}		

		public static string Decrypt(string val) {
			if (val ==  null) {
				return "0";
			}

			val = val.Replace(" ", "+");
			TripleDESCryptoServiceProvider des=null;
			MD5CryptoServiceProvider       hashmd5;
			byte[]                         pwdhash, buff;
			try {
				hashmd5 = new MD5CryptoServiceProvider();
				pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
				hashmd5 = null;
				des = new TripleDESCryptoServiceProvider();
				des.Key = pwdhash;
				des.Mode = CipherMode.ECB; //CBC, CFB
				
				buff = Convert.FromBase64String(val);
				return ASCIIEncoding.ASCII.GetString( des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));				
			} 	finally {
				if (des!=null) {
					des.Clear();
					des = null;
				}
			}
		}		

		public static string GetRandomString(int len) {
			string candidates = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			StringBuilder ret = new StringBuilder();
			Random rand = new Random();
			for(int i=0; i<len;i++) {				
				int index = rand.Next(candidates.Length);
				ret.Append(candidates.Substring(index, 1));
			}
			return ret.ToString();
		}
	}
}
