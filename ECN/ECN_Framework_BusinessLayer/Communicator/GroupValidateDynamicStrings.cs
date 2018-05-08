using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class GroupValidateDynamicStrings : GroupValidateDynamicStringsBase
    {
        public GroupValidateDynamicStrings()
        {
            _errorMessage = "CodeSnippet(s) used in the content do not exist in the group. Please make sure the group includes a User Defined Field (UDF) for the CodeSnippet(s) below or remove the CodeSnippet(s) from dynamic personalization, content and/or subject line.";
        }

        protected override void AppendAdditionalItems()
        {
            
        }

        protected override void RemoveAdditionalItems()
        {
            
        }
    }
}
