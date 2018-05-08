using System;
using System.Configuration;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="LocalizationManager"/> object.
	/// </summary>
	[Serializable]
	public class LocalizationManager
	{
		private const bool traceEnabled = true;

		/// <summary>
		/// Defined languages.
		/// </summary>
		public static string DefinedLanguages = string.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizationManager"/> class.
		/// </summary>
		public LocalizationManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Gets the active language number from code.
		/// </summary>
		/// <returns></returns>
		public static string GetActiveLanguageNumberFromCode()
		{
			return GetLanguageCodeFromNumber(GetActiveLanguageNumber());
		}

		/// <summary>
		/// Gets the active language number.
		/// </summary>
		/// <returns></returns>
		public static int GetActiveLanguageNumber()
		{
			int languageNumber = 0;

			// Check if the language is defined on session.
			if (System.Web.HttpContext.Current.Session["ActiveLanguage"] == null)
			{
				string cookie = GetCookie("LanguageManager");
				if (cookie != null)
					languageNumber = GetLanguage(cookie);

				// Check if the language is defined on a cookie.
				if (languageNumber > 0)
				{
					return SetActiveLanguageNumber(languageNumber);
				}
				else
				{
					// Check the browser setting.
					try
					{
						return SetActiveLanguageNumberFromCode(ParseLanguageCode());
					}
					catch
					{
						return SetActiveLanguageNumber(1);
					}
				}
			}
			else
			{
				// Set the language defined on session.
				try
				{
					return Convert.ToInt32(System.Web.HttpContext.Current.Session["ActiveLanguage"]);
				}
				catch
				{
					return 0;
				}
			}
			
		}

		/// <summary>
		/// Gets the active language code.
		/// </summary>
		/// <returns></returns>
		public static string GetActiveLanguageCode()
		{
			string languages = string.Empty;
			if (System.Web.HttpContext.Current.Session["AvailableLanguages"] != null)
				languages = System.Web.HttpContext.Current.Session["AvailableLanguages"].ToString();
			else if (DefinedLanguages != string.Empty)
				languages = DefinedLanguages;
	
			return GetActiveLanguageCode(languages);
		}

		/// <summary>
		/// Gets the active language code.
		/// </summary>
		/// <param name="languages">The languages.</param>
		/// <returns></returns>
		public static string GetActiveLanguageCode(string languages)
		{
			string languageCode = string.Empty;

			// Check if the language is defined on session.
			if (System.Web.HttpContext.Current.Session["ActiveLanguage"] == null)
			{
				string cookie = GetCookie("LanguageManager");
			
				if (cookie != null)
					languageCode = cookie;

				// Check if the language is defined on a cookie.
				if (languageCode != null && languageCode != string.Empty)
				{
					return SetActiveLanguageCode(languageCode);
				}
				else
				{
					// Check the browser setting.
					try
					{
						return SetActiveLanguageCode(ParseLanguageCode());
					}
					catch
					{
						return SetActiveLanguageCode("");
					}
				}
			}
			else
			{
				// Set the language defined on session.
				try
				{
					return SafelyDefined(System.Web.HttpContext.Current.Session["ActiveLanguage"].ToString(), languages);
				}
				catch
				{
					return SafelyDefined(string.Empty, languages);
				}
			}
			
		}

		private static string SafelyDefined(string code, string languages)
		{
			if (languages != string.Empty)
			{
				foreach(string definedCode in languages.Split(','))
					if (definedCode.ToUpper() == code.ToUpper())
						return code;

				return languages.Split(',')[0];
			}
			else
				return code;
		}

		/// <summary>
		/// Gets the cookie.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static string GetCookie(string key)
		{
			try
			{
				System.Web.HttpCookie cookie;
				cookie = System.Web.HttpContext.Current.Request.Cookies[key];

				return cookie.Value;
			}
			catch(Exception ex)
			{
				DebugTrace(ex.ToString());
			}

			return null;
		}

		/// <summary>
		/// Sets the cookie.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="val">The value.</param>
		public static void SetCookie(string key, string val)
		{
			try
			{
				System.Web.HttpContext.Current.Response.Cookies[key].Value = val;
				System.Web.HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddYears(10);

				/*System.Web.HttpCookie cookie;
				cookie = new System.Web.HttpCookie(key);
				cookie.Value = val;*/
			}
			catch(Exception ex)
			{
				DebugTrace(ex.ToString());
			}
		}

		/// <summary>
		/// Deletes the cookie.
		/// </summary>
		/// <param name="key">The key.</param>
		public static void DeleteCookie(string key)
		{
			try
			{
				System.Web.HttpContext.Current.Request.Cookies.Remove(key);
			}
			catch(Exception ex)
			{
				DebugTrace(ex.ToString());
			}
		}

		/// <summary>
		/// Sets the active language code.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="languages">The languages.</param>
		/// <returns></returns>
		public static string SetActiveLanguageCode(string code, string languages)
		{
			string languageCode = SafelyDefined(code, languages);
			// Set language selection on session.
			System.Web.HttpContext.Current.Session["ActiveLanguage"] = languageCode;

			// Set language selection on cookie.
			SetCookie("LanguageManager", languageCode);

			return languageCode;
		}

		/// <summary>
		/// Sets the active language code.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns></returns>
		public static string SetActiveLanguageCode(string code)
		{
			return SetActiveLanguageCode(code, string.Empty);
		}

		/// <summary>
		/// Sets the active language number.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <returns></returns>
		public static int SetActiveLanguageNumber(int number)
		{
			// Set language selection on session.
			System.Web.HttpContext.Current.Session["ActiveLanguage"] = number;

			// Set language selection on cookie.
			SetCookie("LanguageManager", GetLanguageCodeFromNumber(number));

			return number;
		}

		/// <summary>
		/// Sets the active language number from code.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns></returns>
		public static int SetActiveLanguageNumberFromCode(string code)
		{
			int languageNumber = GetLanguage(code);

			if (languageNumber > 0)
				SetActiveLanguageNumber(languageNumber);
			else
			{
				SetActiveLanguageNumber(1);
				languageNumber = 1;
			}

			return languageNumber;
		}

		/// <summary>
		/// Gets the language.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns></returns>
		public static int GetLanguage(string code)
		{
			try
			{
				string[] codes = GetLanguageCodes();
				int index = 0;

				for(index=0;index<codes.Length;index++)
				{
					if (codes[index].ToUpper() == code.ToUpper())
						return index+1;
				}
			}
			catch(Exception ex)
			{
				DebugTrace(ex.ToString());
			}

			return 0;
		}

		/// <summary>
		/// Gets the language code from number.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <returns></returns>
		public static string GetLanguageCodeFromNumber(int number)
		{
			string languageCode = string.Empty;

			try
			{
				if (DefinedLanguages != string.Empty)
					languageCode = DefinedLanguages.Split(',')[number-1];
				else
					languageCode = ConfigurationSettings.AppSettings["Language" + number.ToString()];

				return languageCode;
			}
			catch(Exception ex)
			{
				DebugTrace(ex.ToString());
				return string.Empty;
			}
		}

		/// <summary>
		/// Gets the language codes.
		/// </summary>
		/// <returns></returns>
		public static string[] GetLanguageCodes()
		{
			bool failed = false;
			int count = 0;
			string languages = string.Empty;

			while (!failed)
			{
				count++;

				try
				{
					string code = ConfigurationSettings.AppSettings["Language" + count.ToString()];
					if (code != string.Empty && code != null)
						languages += "," + ConfigurationSettings.AppSettings["Language" + count.ToString()];
					else
						failed = true;
				}
				catch
				{
					failed = true;
				}
			}

			return languages.Trim(',').Split(',');
		}

		/// <summary>
		/// Parses the country code.
		/// </summary>
		/// <param name="acceptLanguage">The accept language.</param>
		/// <returns></returns>
		public static string ParseCountryCode(string acceptLanguage)
		{
			try
			{
				if (acceptLanguage.Length > 4)
					return acceptLanguage.Substring(3, 2);
				else if (acceptLanguage.Length > 1)
					return acceptLanguage.Substring(0, 2);
				else
					return null;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Parses the country code.
		/// </summary>
		/// <returns></returns>
		public static string ParseCountryCode()
		{
			return ParseCountryCode(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"]);
		}

		/// <summary>
		/// Parses the language code.
		/// </summary>
		/// <param name="acceptLanguage">The accepted language.</param>
		/// <returns></returns>
		public static string ParseLanguageCode(string acceptLanguage)
		{
			if (acceptLanguage.Length > 1)
				return acceptLanguage.Substring(0, 2);
			else
				return null;
		}

		/// <summary>
		/// Parses the language code.
		/// </summary>
		/// <returns></returns>
		public static string ParseLanguageCode()
		{
			return ParseLanguageCode(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"]);
		}

		private static void DebugTrace(string trace)
		{
			if (System.Web.HttpContext.Current != null && traceEnabled)
				System.Web.HttpContext.Current.Trace.Write(trace);
		}
	}
}
