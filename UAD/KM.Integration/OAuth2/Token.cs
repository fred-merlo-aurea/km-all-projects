using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KM.Integration.OAuth2
{
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }

        public readonly DateTime created_datetime = System.DateTime.Now;

        public bool isExpired
        {
            get
            {
                DateTime currenttime = System.DateTime.Now;
                return currenttime.Subtract(created_datetime).TotalSeconds > expires_in - 5;
            }
        }
    }
}
