using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FrameworkSubGen.BusinessLogic
{
    public class ValueOption
    {
        public bool SaveBulkXml(List<Entity.ValueOption> list)
        {
            CleanForXml(list);
            foreach (Entity.ValueOption x in list)
                FormatData(x);

            bool done = false;
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            int checkCount = 1;

            //batch this in 500 records
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.ValueOption x in list)
            {
                string msg = "Checking ValueOption: " + checkCount.ToString() + " of " + total.ToString();
                Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);

                checkCount++;

                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.ValueOption>(x));
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
                            DataAccess.ValueOption.SaveBulkXml("<XML>" + sbXML.ToString() + "</XML>");
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
        public void FormatData(Entity.ValueOption x)
        {
            try
            {
                #region truncate strings
                if (x.value != null && x.value.Length > 255)
                    x.value = x.value.Substring(0, 255);
                if (x.display_as != null && x.display_as.Length > 255)
                    x.display_as = x.display_as.Substring(0, 255);
                if (x.KMProductCode != null && x.KMProductCode.Length > 50)
                    x.KMProductCode = x.KMProductCode.Substring(0, 50);
                #endregion
            }
            catch (Exception ex)
            {
                API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
        }
        public List<Entity.ValueOption> Select(int field_id)
        {
            List<Entity.ValueOption> retItem = null;
            retItem = DataAccess.ValueOption.Select(field_id);
            return retItem;
        }
        public List<Entity.ValueOption> CleanForXml(List<Entity.ValueOption> list)
        {
            foreach (var x in list)
            {
                if (!string.IsNullOrEmpty(x.value))
                    x.value = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.value);
                if (!string.IsNullOrEmpty(x.display_as))
                    x.display_as = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.display_as);
                if (!string.IsNullOrEmpty(x.KMProductCode))
                    x.KMProductCode = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.KMProductCode);
            }
            return list;
        }
    }
}
