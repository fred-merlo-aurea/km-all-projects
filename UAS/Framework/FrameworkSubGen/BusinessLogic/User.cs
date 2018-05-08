using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FrameworkSubGen.BusinessLogic
{
    public class User
    {
        public bool SaveBulkXml(List<Entity.User> list, int accountId)
        {
            foreach (Entity.User x in list)
            {
                FormatData(x);
                x.account_id = accountId;
            }

            bool done = false;
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            int checkCount = 1;

            //batch this in 500 records
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.User x in list)
            {
                string msg = "Checking User: " + checkCount.ToString() + " of " + total.ToString();
                Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);

                checkCount++;

                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.User>(x));
                sbXML.AppendLine(xmlObject);

                counter++;
                processedCount++;
                done = false;
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            DataAccess.User.SaveBulkXml("<XML>" + sbXML.ToString() + "</XML>");
                            scope.Complete();
                            done = true;
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            done = false;
                            API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                        }
                    }
                    sbXML = new StringBuilder();
                    counter = 0;
                }
            }
            return done;
        }
        public void FormatData(Entity.User x)
        {
            try
            {
                #region truncate strings
                if (x.email != null && x.email.Length > 100)
                    x.email = x.email.Substring(0, 100);
                if (x.password != null && x.password.Length > 25)
                    x.password = x.password.Substring(0, 25);
                if (x.password_md5 != null && x.password_md5.Length > 32)
                    x.password_md5 = x.password_md5.Substring(0, 32);
                if (x.first_name != null && x.first_name.Length > 50)
                    x.first_name = x.first_name.Substring(0, 50);
                if (x.last_name != null && x.last_name.Length > 50)
                    x.last_name = x.last_name.Substring(0, 50);
                #endregion
            }
            catch (Exception ex)
            {
                API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
        }
    }
}
