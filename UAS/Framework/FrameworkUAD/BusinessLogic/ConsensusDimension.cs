using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ServiceStack.Text;

namespace FrameworkUAD.BusinessLogic
{
    public class ConsensusDimension
    {
        public bool SaveXML(List<Object.ConsensusDimension> list, int masterGroupID, KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
            int BatchSize = 250;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            StringBuilder sbXML = new StringBuilder();
            foreach (Object.ConsensusDimension x in list)
            {
                try
                {
                    x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Phone);

                    if (x.FirstName != null && x.FirstName.Length > 100)
                        x.FirstName = x.FirstName.Substring(0, 100);
                    if (x.LastName != null && x.LastName.Length > 100)
                        x.LastName = x.LastName.Substring(0, 100);
                    if (x.Company != null && x.Company.Length > 100)
                        x.Company = x.Company.Substring(0, 100);
                    if (x.Address1 != null && x.Address1.Length > 255)
                        x.Address1 = x.Address1.Substring(0, 255);
                    if (x.Address2 != null && x.Address2.Length > 255)
                        x.Address2 = x.Address2.Substring(0, 255);
                    if (x.City != null && x.City.Length > 50)
                        x.City = x.City.Substring(0, 50);
                    if (x.State != null && x.State.Length > 50)
                        x.State = x.State.Substring(0, 50);
                    if (x.Zipcode != null && x.Zipcode.Length > 50)
                        x.Zipcode = x.Zipcode.Substring(0, 50);
                    if (x.Country != null && x.Country.Length > 100)
                        x.Country = x.Country.Substring(0, 100);
                    if (x.Email != null && x.Email.Length > 100)
                        x.Email = x.Email.Substring(0, 100);

                    string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Object.ConsensusDimension>(x));
                    sbXML.AppendLine(xmlObject);

                    counter++;
                    processedCount++;
                    done = false;
                    if (processedCount == total || counter == BatchSize)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            DataAccess.ConsensusDimension.SaveXML("<XML>" + sbXML.ToString() + "</XML>", masterGroupID, client);
                            scope.Complete();
                            done = true;
                        }
                        sbXML = new StringBuilder();
                        counter = 0;
                    }
                }
                catch (Exception ex)
                {
                    counter++;
                    processedCount++;
                    string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                    fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message,"ConsensusDimension"));
                }
            }
            return done;
        }
        
    }
}
