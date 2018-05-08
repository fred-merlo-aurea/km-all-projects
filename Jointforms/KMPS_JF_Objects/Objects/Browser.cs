using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KMPS_JF_Objects.Objects
{
    public class ClientBrowser
    {
        public ClientBrowser(string platform, string browser, string version, string majorVersion, string minorVersion, string beta, string crawler, string aol, string win16, string win32, string tables, string cookies, string vbs, string js)
        {
            Platform = platform;
            Platform = browser;
            Platform = version;
            Platform = majorVersion;
            Platform = minorVersion;
            Platform = beta;
            Platform = crawler;
            Platform = aol;
            Platform = win16;
            Platform = win32;
            Platform = tables;
            Platform = cookies;
            Platform = vbs;
            Platform = js;

        }

        public string Platform { get; set; }
        public string Browser { get; set; }
        public string Version { get; set; }
        public string MajorVersion { get; set; }
        public string MinorVersion { get; set; }
        public string Beta { get; set; }
        public string Crawler { get; set; }
        public string AOL { get; set; }

        public string Win16 { get; set; }
        public string Win32 { get; set; }
        public string Tables { get; set; }

        public string Cookies { get; set; }
        public string VBScript { get; set; }
        public string JavaScript { get; set; }
        
    }
}
