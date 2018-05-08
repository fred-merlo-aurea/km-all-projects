using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ecn.webservices.console
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Start();
        }
        void Start()
        {
            SaversAPI_LOCAL.SaversAPI saveAPI = new SaversAPI_LOCAL.SaversAPI();
            StringBuilder ZipXML = new StringBuilder();
            ZipXML.AppendLine("<ZipCodes>");
            ZipXML.AppendLine("<ZipCode>55447</ZipCode>");
            ZipXML.AppendLine("<ZipCode>55388</ZipCode>");
            ZipXML.AppendLine("</ZipCodes>");

            //values are for VVA - chairtyID 22 in vva database on 131
            //6A870C4E-89D9-4FDD-9A23-B0DD99A5C3E6
            //2676
            string result = saveAPI.CreateWeeklySolicitationFilter("6A870C4E-89D9-4FDD-9A23-B0DD99A5C3E6", 7, "1/12/2014", "1/18/2014", ZipXML.ToString());
        }
    }
}
