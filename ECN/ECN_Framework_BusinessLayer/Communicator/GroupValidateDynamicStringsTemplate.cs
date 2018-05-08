using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class GroupValidateDynamicStringsTemplate : GroupValidateDynamicStringsBase
    {
        public GroupValidateDynamicStringsTemplate()
        {
            _errorMessage = "CodeSnippet(s) used in the template do not exist in the group. Please make sure the group includes a User Defined Field (UDF) for the CodeSnippet(s) below or remove the CodeSnippet(s) from template.";
        }

        protected override void AppendAdditionalItems()
        {
            _listCS.Add("emailaddress");
            _listCS.Add("formattypecode");
            _listCS.Add("subscribetypecode");
            _listCS.Add("title");
            _listCS.Add("firstname");
            _listCS.Add("lastname");
            _listCS.Add("fullname");
            _listCS.Add("company");
            _listCS.Add("occupation");
            _listCS.Add("address");
            _listCS.Add("address2");
            _listCS.Add("city");
            _listCS.Add("state");
            _listCS.Add("zip");
            _listCS.Add("country");
            _listCS.Add("voice");
            _listCS.Add("mobile");
            _listCS.Add("fax");
            _listCS.Add("website");
            _listCS.Add("age");
            _listCS.Add("income");
            _listCS.Add("gender");
            _listCS.Add("user1");
            _listCS.Add("user2");
            _listCS.Add("user3");
            _listCS.Add("user4");
            _listCS.Add("user5");
            _listCS.Add("user6");
            _listCS.Add("birthdate");
            _listCS.Add("userevent1");
            _listCS.Add("userevent1date");
            _listCS.Add("userevent2");
            _listCS.Add("userevent2date");
            _listCS.Add("notes");
        }

        protected override void RemoveAdditionalItems()
        {
            _listNoExist.Remove("profilepreferences");
            _listNoExist.Remove("emailfromaddress");
            _listNoExist.Remove("slot1");
            _listNoExist.Remove("slot2");
            _listNoExist.Remove("slot3");
            _listNoExist.Remove("slot4");
            _listNoExist.Remove("slot5");
            _listNoExist.Remove("slot6");
            _listNoExist.Remove("slot7");
            _listNoExist.Remove("slot8");
            _listNoExist.Remove("slot9");
            _listNoExist.Remove("slot10");
        }
    }
}
