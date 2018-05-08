using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FrameworkSubGen.BusinessLogic
{
    public class Purchase
    {
        public bool SaveBulkXml(List<Entity.Purchase> list)
        {
            foreach (Entity.Purchase x in list)
                FormatData(x);

            bool done = false;
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            int checkCount = 1;

            //batch this in 500 records
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.Purchase x in list)
            {
                string msg = "Checking Purchase: " + checkCount.ToString() + " of " + total.ToString();
                Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);

                checkCount++;

                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.Purchase>(x));
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
                            DataAccess.Purchase.SaveBulkXml("<XML>" + sbXML.ToString() + "</XML>");
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
        public void FormatData(Entity.Purchase x)
        {
            try
            {
                #region truncate strings
                if (x.name != null && x.name.Length > 50)
                    x.name = x.name.Substring(0, 50);
                #endregion
            }
            catch (Exception ex)
            {
                API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
        }
    }
}
