using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailPreview
{
    public class Accounts
    {
        /// <summary>
        /// User's Litmus username
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// User's Litmus Password
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Subdomain for Litmus user's account, eg https://mycompany.litmus.com/
        /// </summary>
        public string Subdomain { get; private set; }

        /// <summary>
        /// Base url for REST requests to the Litmus API for this account.
        /// </summary>
        public string LitmusBaseUrl
        {
            get { return string.Format("https://{0}.litmus.com/", Subdomain); }
        }

        /// <summary>
        /// Account constructor
        /// </summary>
        /// <param name="subdomain"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Accounts(string subdomain, string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(subdomain)) throw new ArgumentException("You must pass a valid user, password and subdomain to create an account.");
            Subdomain = subdomain;
            Username = username;
            Password = password;
        }
    }
}
