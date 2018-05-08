using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using ECN_Framework_Common.Objects;
using System.Text.RegularExpressions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class EmailDirect
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.EmailDirect;

        public static int Save(ECN_Framework_Entities.Communicator.EmailDirect ed)
        {
            if (string.IsNullOrEmpty(ed.FromEmailAddress))
                ed.FromEmailAddress = "emaildirect@ecn5.com";
            Validate(ed);

            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.EmailDirect.Save(ed);
                scope.Complete();
            }
            return retID;
        }

        public static ECN_Framework_Entities.Communicator.EmailDirect GetNextToSend()
        {
            ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ed = ECN_Framework_DataLayer.Communicator.EmailDirect.GetNextToSend();
                scope.Complete();
            }
            return ed;
        }

        public static void UpdateStatus(int EmailDirectID, ECN_Framework_Common.Objects.EmailDirect.Enums.Status Status)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailDirect.UpdateStatus(EmailDirectID, Status);
                scope.Complete();
            }
        }

        public static ECN_Framework_Entities.Communicator.EmailDirect GetByEmailDirectID(int EmailDirectID)
        {
            ECN_Framework_Entities.Communicator.EmailDirect ed = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ed = ECN_Framework_DataLayer.Communicator.EmailDirect.GetByEmailDirectID(EmailDirectID);
                scope.Complete();
            }
            return ed;
        }

        public static List<ECN_Framework_Entities.Communicator.EmailDirect> GetByCustomerID(int CustomerID)
        {
            List<ECN_Framework_Entities.Communicator.EmailDirect> retList = new List<ECN_Framework_Entities.Communicator.EmailDirect>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.EmailDirect.GetByCustomerID(CustomerID);
                scope.Complete();
            }
            return retList;
        }

        private static void Validate(ECN_Framework_Entities.Communicator.EmailDirect emailDirect)
        {
            var validationErrors = new List<string>();
            
            ValidateParameters(emailDirect, validationErrors);

            if (string.IsNullOrWhiteSpace(emailDirect.Content))
            {
                AddMissingError(validationErrors, "Content");
            }
            else
            {
                ValidateHtmlContent(emailDirect, validationErrors);
            }

            if (validationErrors.Count > 0)
            {
                ThrowValidationErrors(validationErrors);
            }

            CorrectHtmlContent(emailDirect);
        }

        private static void ValidateParameters(ECN_Framework_Entities.Communicator.EmailDirect emailDirect, List<string> validationErrors)
        {
            if (string.IsNullOrWhiteSpace(emailDirect.EmailAddress))
            {
                AddMissingError(validationErrors, "EmailAddress");
            }
            else
            {
                var emails = emailDirect.EmailAddress.Split(',');
                foreach (var s in emails)
                {
                    if (!Email.IsValidEmailAddress(s))
                    {
                        validationErrors.Add("EmailAddress must be a valid email address");
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(emailDirect.Source))
            {
                AddMissingError(validationErrors, "Source");
            }

            if (string.IsNullOrWhiteSpace(emailDirect.ReplyEmailAddress))
            {
                AddMissingError(validationErrors, "ReplyEmailAddress");
            }
            else
            {
                if (!Email.IsValidEmailAddress(emailDirect.ReplyEmailAddress))
                {
                    validationErrors.Add("ReplyEmailAddress must be a valid email address");
                }
            }

            if (string.IsNullOrEmpty(emailDirect.FromEmailAddress))
            {
                AddMissingError(validationErrors, "FromEmailAddress");
            }
            else
            {
                if (!Email.IsValidEmailAddress(emailDirect.FromEmailAddress))
                {
                    validationErrors.Add("FromEmailAddress must be a valid email address");
                }
            }

            if (string.IsNullOrWhiteSpace(emailDirect.Process))
            {
                AddMissingError(validationErrors, "Process");
            }

            if (string.IsNullOrWhiteSpace(emailDirect.EmailSubject))
            {
                AddMissingError(validationErrors, "EmailSubject");
            }

            if (string.IsNullOrWhiteSpace(emailDirect.FromName))
            {
                AddMissingError(validationErrors, "FromName");
            }
        }

        private static void ValidateHtmlContent(ECN_Framework_Entities.Communicator.EmailDirect emailDirect, List<string> validationErrors)
        {
            //HTML Content Validation
            var htmlStartRegex = new Regex("<html.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var htmlEndRegex = new Regex("</html.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var headStartRegex = new Regex("<head.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var headEndRegex = new Regex("</head.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var bodyStartRegex = new Regex("<body.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var bodyEndRegex = new Regex("</body.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (htmlStartRegex.IsMatch(emailDirect.Content))
            {
                if (!htmlEndRegex.IsMatch(emailDirect.Content))
                {
                    validationErrors.Add("Incorrect closing HTML tag.");
                }
            }
            else
            {
                if (htmlEndRegex.IsMatch(emailDirect.Content))
                {
                    validationErrors.Add("Incorrect opening HTML tag.");
                }
            }

            if (headStartRegex.IsMatch(emailDirect.Content))
            {
                if (!headEndRegex.IsMatch(emailDirect.Content))
                {
                    validationErrors.Add("Incorrect closing HEAD tag.");
                }
            }
            else
            {
                if (headEndRegex.IsMatch(emailDirect.Content))
                {
                    validationErrors.Add("Incorrect opening HEAD tag.");
                }
            }

            if (bodyStartRegex.IsMatch(emailDirect.Content))
            {
                if (!bodyEndRegex.IsMatch(emailDirect.Content))
                {
                    validationErrors.Add("Incorrect closing BODY tag.");
                }
            }
            else
            {
                if (bodyEndRegex.IsMatch(emailDirect.Content))
                {
                    validationErrors.Add("Incorrect opening BODY tag.");
                }
            }
        }

        private static void CorrectHtmlContent(ECN_Framework_Entities.Communicator.EmailDirect emailDirect)
        {
            // Wraps the content necessary to be sent
            if (emailDirect.Content.StartsWith("<!doctype", StringComparison.OrdinalIgnoreCase))
            {
                // remove doctype
                var endOfDocTypeIndex = emailDirect.Content.IndexOf(">", StringComparison.OrdinalIgnoreCase);
                emailDirect.Content = emailDirect.Content.Substring(endOfDocTypeIndex + 1);
            }

            // add missing tags
            if (!emailDirect.Content.StartsWith("<html", StringComparison.OrdinalIgnoreCase))
            {
                if (emailDirect.Content.IndexOf("<body", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    // if no html, or body tags: add them
                    emailDirect.Content = "<html><body>" + emailDirect.Content + "</body></html>";
                }
                else
                {
                    // if no html add it
                    emailDirect.Content = "<html>" + emailDirect.Content + "</html>";
                }
            }
            else
            {
                // if no body. Has HTML tag: adds missing
                if (emailDirect.Content.IndexOf("<body", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    var firstEndTag = emailDirect.Content.IndexOf(">", StringComparison.OrdinalIgnoreCase);
                    var lastBeginTag = emailDirect.Content.LastIndexOf("<", StringComparison.OrdinalIgnoreCase);
                    emailDirect.Content = emailDirect.Content.Substring(0, lastBeginTag) + "</body>" + emailDirect.Content.Substring(lastBeginTag);
                    emailDirect.Content = emailDirect.Content.Substring(0, firstEndTag + 1) + "<body>" + emailDirect.Content.Substring(firstEndTag + 1);
                }
            }

            // add proper doctype
            emailDirect.Content = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transistional//EN\">" + emailDirect.Content;
        }

        private static void ThrowValidationErrors(List<string> validationErrors)
        {
            throw new ECNException(
                validationErrors
                    .Select(message => new ECNError(Enums.Entity.EmailDirect, Enums.Method.Validate, message))
                    .ToList());
        }

        private static void AddMissingError(List<string> validationErrors, string field)
        {
            validationErrors.Add($"{field} is required");
        }
    }
}
