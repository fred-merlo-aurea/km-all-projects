using System;
using System.IO;
using System.Net;

namespace MAF.NorthStarExport
{
    public class FtpFunctions : KM.Common.Functions.FtpFunctions
    {
        /* Construct Object */
        public FtpFunctions(string hostIP, string userName, string password) : base(hostIP, userName, password)
        {
        }
    }
}
