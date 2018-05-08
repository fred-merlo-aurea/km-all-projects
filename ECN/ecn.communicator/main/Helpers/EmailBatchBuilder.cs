using System.Collections;
using System.Collections.Generic;
using System.Text;
using ECN_Framework_Entities.Salesforce;
using KM.Common;

namespace ecn.communicator.main.Helpers
{
    public class EmailBatchBuilder
    {
        private readonly List<SalesForceBase> _items;
        private readonly Hashtable _hgdfFields;
        private bool _excludeExtendedFields = true;

        public EmailBatchBuilder(Hashtable hgdfFields, params IEnumerable<SalesForceBase>[] sources)
        {
            _hgdfFields = hgdfFields;
            _items = new List<SalesForceBase>();
            foreach (var salesForceBase in sources)
            {
                _items.AddRange(salesForceBase);
            }
        }

        public EmailBatchBuilder ExcludeExtendedFields(bool excludeExtendedFields)
        {
            _excludeExtendedFields = excludeExtendedFields;
            return this;
        }

        public IEnumerable<EmailBatch> Build()
        {
            var batchCount = 0;
            var xmlprofileUdf = new StringBuilder();
            var xmlUdf = new StringBuilder();
            foreach (var salesForceBase in _items)
            {
                BuildItemDescription(salesForceBase as SF_Lead, xmlUdf, xmlprofileUdf);
                BuildItemDescription(salesForceBase as SF_Contact, xmlUdf, xmlprofileUdf);

                if (batchCount != 0 && batchCount % 1000 == 0 || batchCount == _items.Count - 1)
                {
                    var emailBatch = new EmailBatch()
                    {
                        XmlProfileUdf = xmlprofileUdf.ToString(),
                        XmlUdf = xmlUdf.ToString()
                    };
                    xmlprofileUdf = new StringBuilder();
                    xmlUdf = new StringBuilder();
                    yield return emailBatch;
                }

                batchCount++;
            }
        }

        private bool ShouldEncode(string itemState)
        {
            return itemState.Trim().Length > 2;
        }

        private void BuildItemDescription(SF_Lead item, StringBuilder xmlUdf, StringBuilder xmlprofileUdf)
        {
            if (item == null)
            {
                return;
            }

            try
            {
                if (ShouldEncode(item.State))
                {
                    item.State = StringFunctions.EscapeXmlString(SF_Utilities.GetStateAbbr(item.State.Trim()));
                }

                xmlprofileUdf.Append("<Emails>");
                xmlprofileUdf.Append($"<emailaddress>{item.Email.Trim()}</emailaddress>");
                xmlprofileUdf.Append($"<firstname>{StringFunctions.EscapeXmlString(item.FirstName.Trim())}</firstname>");
                xmlprofileUdf.Append($"<lastname>{StringFunctions.EscapeXmlString(item.LastName.Trim())}</lastname>");
                xmlprofileUdf.Append($"<fullname>{StringFunctions.EscapeXmlString(item.Name.Trim())}</fullname>");
                xmlprofileUdf.Append($"<city>{StringFunctions.EscapeXmlString(item.City.Trim())}</city>");
                xmlprofileUdf.Append($"<state>{StringFunctions.EscapeXmlString(item.State.Trim())}</state>");
                xmlprofileUdf.Append($"<zip>{StringFunctions.EscapeXmlString(item.PostalCode.Trim())}</zip>");
                xmlprofileUdf.Append($"<address>{StringFunctions.EscapeXmlString(item.Street.Trim())}</address>");
                xmlprofileUdf.Append($"<voice>{StringFunctions.EscapeXmlString(item.Phone.Trim())}</voice>");
                xmlprofileUdf.Append($"<mobile>{StringFunctions.EscapeXmlString(item.MobilePhone.Trim())}</mobile>");
                if (_excludeExtendedFields)
                {
                    xmlprofileUdf.Append($"<country>{StringFunctions.EscapeXmlString(item.Country.Trim())}</country>");
                    xmlprofileUdf.Append($"<fax>{StringFunctions.EscapeXmlString(item.Fax.Trim())}</fax>");
                    xmlprofileUdf.Append($"<title>{StringFunctions.EscapeXmlString(item.Title.Trim())}</title>");
                    xmlprofileUdf.Append($"<website>{StringFunctions.EscapeXmlString(item.Website.Trim())}</website>");
                }

                xmlprofileUdf.Append("</Emails>");
                AddHgdfFields(item.Email, item.Id, xmlUdf, "Lead");
            }
            catch
            {
                xmlprofileUdf.Append("</Emails>");
            }
        }

        private void BuildItemDescription(SF_Contact item, StringBuilder xmlUdf, StringBuilder xmlprofileUdf)
        {
            if (item == null)
            {
                return;
            }

            try
            {
                if (ShouldEncode(item.MailingState))
                {
                    item.MailingState = StringFunctions.EscapeXmlString(SF_Utilities.GetStateAbbr(item.MailingState));
                }

                xmlprofileUdf.Append("<Emails>");
                xmlprofileUdf.Append($"<emailaddress>{item.Email.Trim()}</emailaddress>");
                xmlprofileUdf.Append($"<firstname>{StringFunctions.EscapeXmlString(item.FirstName.Trim())}</firstname>");
                xmlprofileUdf.Append($"<lastname>{StringFunctions.EscapeXmlString(item.LastName.Trim())}</lastname>");
                xmlprofileUdf.Append($"<fullname>{StringFunctions.EscapeXmlString(item.Name.Trim())}</fullname>");
                xmlprofileUdf.Append($"<city>{StringFunctions.EscapeXmlString(item.MailingCity.Trim())}</city>");
                xmlprofileUdf.Append($"<state>{StringFunctions.EscapeXmlString(item.MailingState.Trim())}</state>");
                xmlprofileUdf.Append($"<zip>{StringFunctions.EscapeXmlString(item.MailingPostalCode.Trim())}</zip>");
                xmlprofileUdf.Append($"<address>{StringFunctions.EscapeXmlString(item.MailingStreet.Trim())}</address>");
                xmlprofileUdf.Append($"<voice>{StringFunctions.EscapeXmlString(item.Phone.Trim())}</voice>");
                xmlprofileUdf.Append($"<mobile>{StringFunctions.EscapeXmlString(item.MobilePhone.Trim())}</mobile>");
                if (_excludeExtendedFields)
                {
                    xmlprofileUdf.Append($"<country>{StringFunctions.EscapeXmlString(item.MailingCountry.Trim())}</country>");
                    xmlprofileUdf.Append($"<title>{StringFunctions.EscapeXmlString(item.Title.Trim())}</title>");
                    xmlprofileUdf.Append($"<fax>{StringFunctions.EscapeXmlString(item.Fax.Trim())}</fax>");
                    xmlprofileUdf.Append($"<birthdate>{StringFunctions.EscapeXmlString(item.BirthDate.ToShortDateString())}</birthdate>");
                }

                xmlprofileUdf.Append("</Emails>");
                AddHgdfFields(item.Email, item.Id, xmlUdf, "Contact");
            }
            catch
            {
                xmlprofileUdf.Append("</Emails>");
            }

        }

        private void AddHgdfFields(string email, string id, StringBuilder xmlUdf, string entityType)
        {
            if (_hgdfFields.Count <= 0)
            {
                return;
            }

            //SF ID UDF
            xmlUdf.Append("<row>");
            xmlUdf.Append($"<ea>{email}</ea>");
            xmlUdf.Append($"<udf id=\"{_hgdfFields["user_sfid"]}\">");
            xmlUdf.Append($"<v><![CDATA[{id}]]></v>");
            xmlUdf.Append("</udf>");
            xmlUdf.Append("</row>");

            //SF Type UDF
            xmlUdf.Append("<row>");
            xmlUdf.Append($"<ea>{email}</ea>");
            xmlUdf.Append($"<udf id=\"{_hgdfFields["user_sftype"]}\">");
            xmlUdf.Append($"<v><![CDATA[{entityType}]]></v>");
            xmlUdf.Append("</udf>");
            xmlUdf.Append("</row>");
        }
    }
}