using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    // External Email Model with Format Type Code and Subscribe Type Code option lists
    public class EmailWrapper : BaseWrapper
    {
        public EmailWrapper(int EmailID, int GroupID)
        {
            email = new Email(ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(EmailID), GroupID);
            InitLists();
        }
        public EmailWrapper(ecn.communicator.mvc.Models.Email e, int GroupID)
        {
            email = e;
            InitLists();
        }

        public EmailWrapper(ECN_Framework_Entities.Communicator.Email e, int GroupID)
        {
            email = new Email(e, GroupID);
            InitLists();
        }
        public ecn.communicator.mvc.Models.Email email { get; set; }
        public string UDFURL { get; set; }
        public List<ECN_Framework_Entities.Accounts.Code> FormatTypeCodes { get; set; }
        public List<Tuple<string, string>> SubscribeTypeCodes { get; set; }
        public List<ECN_Framework_Entities.Activity.View.BlastActivity> BlastActivity { get; set; }
        private void InitLists()
        {
            BlastActivity = new List<ECN_Framework_Entities.Activity.View.BlastActivity>();
            SubscribeTypeCodes = new List<Tuple<string, string>>();
            List<ECN_Framework_Entities.Accounts.Code> codeList = ECN_Framework_BusinessLayer.Accounts.Code.GetAll();
            FormatTypeCodes = (from src in codeList
                               where src.CodeType == "FormatType"
                               select src).ToList();
        }
    }
}