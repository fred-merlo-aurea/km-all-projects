using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using CommGroup = ECN_Framework_Entities.Communicator.Group;
using CommEmailGroup = ECN_Framework_Entities.Communicator.EmailGroup;
using DataEmailGroup = ECN_Framework_DataLayer.Communicator.EmailGroup;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public abstract class EmailGroupsImportBase
    {
        public abstract CommGroup GetMasterSuppressionGroup(int customerId, User user);

        public abstract CommEmailGroup GetByEmailAddressGroupID(string emailAddress, int groupID, User user);

        public void Import(string xml, int userId)
        {
            var error = new List<ECNError>();
            var xDoc = new XmlDocument();
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                { 
                    writer.Write(xml);
                    writer.Flush();
                    stream.Position = 0;
                    xDoc.Load(stream);
                }
            }

            var emailAddress = xDoc.DocumentElement.ChildNodes[0]?.Attributes["emailAddress"]?.Value?.ToString();
            var user = KMPlatform.BusinessLogic.User.GetByUserID(userId, false);
            var masterEmails = new List<string>();

            foreach (XmlNode node in xDoc.DocumentElement.ChildNodes[0].ChildNodes)
            {
                var customerId = Convert.ToInt32(node.Attributes["customer"].Value.ToString());
                var msGroup = GetMasterSuppressionGroup(customerId, user);

                try
                {
                    if (!string.IsNullOrWhiteSpace(emailAddress) && !masterEmails.Contains(emailAddress))
                    {
                        masterEmails.Add(emailAddress);
                        CommEmailGroup msEmailGroup = GetByEmailAddressGroupID(emailAddress, msGroup.GroupID, user);

                        if (msEmailGroup != null && msEmailGroup.CustomerID.HasValue)
                        {
                            error.Add(new ECNError(Enums.Entity.EmailGroup, Enums.Method.Validate, "Email address: " + emailAddress + " is Master Suppressed"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Invalid XML String : {0}", ex);
                    error.Add(new ECNError(Enums.Entity.EmailGroup, Enums.Method.Validate, "Invalid XML string"));
                }
            }

            if (error.Count > 0)
            {
                throw new ECNException(error, Enums.ExceptionLayer.Business);
            }

            using (var scope = new TransactionScope())
            {
                DataEmailGroup.ImportEmailsToGroups(xml, userId);
                scope.Complete();
            }
        }
    }
}
