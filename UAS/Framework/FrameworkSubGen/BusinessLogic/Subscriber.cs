using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkSubGen.BusinessLogic
{
    public class Subscriber
    {
        public string GetXmlForUpdateClientUAD(List<Entity.Subscriber> list)
        {
            foreach (Entity.Subscriber x in list)
                FormatData(x);
            list = CleanForXml(list);

            string xml = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<List<Entity.Subscriber>>(list));
            return xml;
        }
        public List<Entity.Subscriber> CleanForXml(List<Entity.Subscriber> list)
        {
            foreach (var x in list)
            {
                if (!string.IsNullOrEmpty(x.email))
                    x.email = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.email);
                if (!string.IsNullOrEmpty(x.first_name))
                    x.first_name = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.first_name);
                if (!string.IsNullOrEmpty(x.last_name))
                    x.last_name = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.last_name);
                if (!string.IsNullOrEmpty(x.password))
                    x.password = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.password);
                if (!string.IsNullOrEmpty(x.password_md5))
                    x.password_md5 = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.password_md5);
                if (!string.IsNullOrEmpty(x.renewal_code))
                    x.renewal_code = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.renewal_code);
                if (!string.IsNullOrEmpty(x.source))
                    x.source = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.source);
            }
            return list;
        }

        public bool SaveBulkXml(IList<Entity.Subscriber> subscribers)
        {
            if (subscribers == null || !subscribers.Any())
            {
                return true;
            }

            foreach (var item in subscribers)
            {
                FormatData(item);
            }

            const int batchSize = 500;
            const string errorMessage = "SubGen.BusinessLogic.Subscriber.SaveBulkXml";
            var coreImporter = new CoreImport();
            var result = coreImporter.CoreSaveBulkXml(
                subscribers,
                (xml) =>
                {
                    DataAccess.Subscriber.SaveBulkXml(xml);
                },
                batchSize,
                errorMessage,
                false,
                true);

            return result;
        }

        public bool Save(Entity.Subscriber sub)
        {
            FormatData(sub);
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    DataAccess.Subscriber.Save(sub);
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
            return done;
        }
        public bool Save(List<Entity.Subscriber> list)
        {
            foreach (Entity.Subscriber x in list)
                FormatData(x);
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    DataAccess.Subscriber.Save(list);
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
            return done;
        }
        public void FormatData(Entity.Subscriber x)
        {
            try
            {
                #region truncate strings
                if (x.renewal_code != null && x.renewal_code.Length > 5)
                    x.renewal_code = x.renewal_code.Substring(0, 5);
                if (x.email != null && x.email.Length > 100)
                    x.email = x.email.Substring(0, 100);
                if (x.password != null && x.password.Length > 25)
                    x.password = x.password.Substring(0, 25);
                if (x.password_md5 != null && x.password_md5.Length > 32)
                    x.password_md5 = x.password_md5.Substring(0, 32);
                if (x.first_name != null && x.first_name.Length > 25)
                    x.first_name = x.first_name.Substring(0, 25);
                if (x.last_name != null && x.last_name.Length > 25)
                    x.last_name = x.last_name.Substring(0, 25);
                if (x.source != null && x.source.Length > 100)
                    x.source = x.source.Substring(0, 100);
                if (x.create_date == DateTime.Parse("0001-01-01T00:00:00") || x.create_date == DateTime.MinValue || x.create_date <= DateTime.Parse("1/1/1900"))
                    x.create_date = DateTime.Now;
                if (x.delete_date == DateTime.Parse("0001-01-01T00:00:00") || x.delete_date == DateTime.MinValue || x.delete_date <= DateTime.Parse("1/1/1900"))
                    x.delete_date = DateTime.Now;
                #endregion
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string error = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(error, "SubGen.BusinessLogic.Subscriber.FormatData", KMPlatform.BusinessLogic.Enums.Applications.SubGen_Integration);
            }
        }
        public List<Entity.Subscriber> FindSubscribers(string email = "", string firstName = "", string lastName = "")
        {
            List<Entity.Subscriber> x = null;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(email))
                parameters.Add("email", email);
            if (!string.IsNullOrEmpty(firstName))
                parameters.Add("first_name", firstName);
            if (!string.IsNullOrEmpty(lastName))
                parameters.Add("last_name", lastName);

            x = DataAccess.Subscriber.FindSubscribers(parameters);
            return x;
        }
    }
}
