using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Activity
{
    [Serializable]
    public class EmailClients
    {
        public static List<ECN_Framework_Entities.Activity.EmailClients> Get()
        {
            return ECN_Framework_DataLayer.Activity.EmailClients.Get();
        }
    }
}
