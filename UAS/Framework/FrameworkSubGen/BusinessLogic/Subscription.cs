using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FrameworkSubGen.BusinessLogic
{
    public class Subscription
    {
        public bool SaveBulkXml(List<Entity.Subscription> list)
        {
            foreach (Entity.Subscription x in list)
                FormatData(x);

            bool done = false;
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            int checkCount = 1;

            //batch this in 500 records
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.Subscription x in list)
            {
                string msg = "Checking Subscription: " + checkCount.ToString() + " of " + total.ToString();
                Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);

                checkCount++;

                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.Subscription>(x));
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
                            DataAccess.Subscription.SaveBulkXml("<XML>" + sbXML.ToString() + "</XML>");
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
        public void FormatData(Entity.Subscription x)
        {
            try
            {
                #region truncate strings
                if (x.audit_classification != null && x.audit_classification.Length > 50)
                    x.audit_classification = x.audit_classification.Substring(0, 50);
                if (x.audit_request_type != null && x.audit_request_type.Length > 50)
                    x.audit_request_type = x.audit_request_type.Substring(0, 50);
                if (x.date_created == DateTime.Parse("0001-01-01T00:00:00") || x.date_created == DateTime.MinValue || x.date_created <= DateTime.Parse("1/1/1900"))
                    x.date_created = DateTime.Now;
                if (x.date_expired == DateTime.Parse("0001-01-01T00:00:00") || x.date_expired == DateTime.MinValue || x.date_expired <= DateTime.Parse("1/1/1900"))
                    x.date_expired = DateTime.Now;
                if (x.date_last_renewed == DateTime.Parse("0001-01-01T00:00:00") || x.date_last_renewed == DateTime.MinValue || x.date_last_renewed <= DateTime.Parse("1/1/1900"))
                    x.date_last_renewed = DateTime.Now;
                #endregion
            }
            catch (Exception ex)
            {
                API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
        }
        public Entity.Subscription Select(int subscriptionId)
        {
            Entity.Subscription x = null;
            x = DataAccess.Subscription.Select(subscriptionId);
            return x;
        }
    }
}
