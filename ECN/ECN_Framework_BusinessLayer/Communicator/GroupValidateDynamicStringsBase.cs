using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public abstract class GroupValidateDynamicStringsBase
    {
        private const Enums.Entity Entity = Enums.Entity.Group;
        private const Enums.Method Method = Enums.Method.Validate;
        private List<ECNError> _errorList = new List<ECNError>();

        protected List<string> _listCS = new List<string>();
        protected List<string> _listNoExist = new List<string>();
        protected string _errorMessage;

        protected abstract void AppendAdditionalItems();

        protected abstract void RemoveAdditionalItems();

        public void ValidateDynamicStrings(IList<string> listLY, int groupId, User user)
        {
            var dtEmail = Email.GetColumnNames();
            var groupDataFields = GroupDataFields.GetByGroupID_NoAccessCheck(groupId);

            foreach (DataRow dataRow in dtEmail.Rows)
            {
                _listCS.Add(dataRow["columnName"].ToString().ToLower());
            }

            foreach (ECN_Framework_Entities.Communicator.GroupDataFields communicatorGroupDataFields in groupDataFields)
            {
                _listCS.Add(communicatorGroupDataFields.ShortName.ToLower());
            }

            AppendAdditionalItems();

            var subLY = ValidateList(listLY);

            _listNoExist = new List<string>();
            foreach (var s in subLY)
            {
                if (!_listCS.Contains(s))
                {
                    _listNoExist.Add(s);
                }
            }

            RemoveItems();
            RemoveAdditionalItems();

            if (_listNoExist.Count > 0)
            {
                var errormsg = new StringBuilder();
                errormsg.Append(_errorMessage);

                foreach (var s in _listNoExist)
                {
                    errormsg.AppendLine($"<br /> %%{s}%%");
                }

                _errorList.Add(new ECNError(Entity, Method, errormsg.ToString()));
            }

            if (_errorList.Count > 0)
            {
                throw new ECNException(_errorList);
            }
        }

        private IList<string> ValidateList(IList<string> listLY)
        {
            var subLY = new List<string>();
            foreach (string s in listLY)
            {
                var regMatch = new Regex("%%", RegexOptions.IgnoreCase);
                var matchList = regMatch.Matches(s);
                if (matchList.Count > 0)
                {
                    if ((matchList.Count % 2) != 0)
                    {
                        _errorList.Add(new ECNError(Entity, Method, "Incorrectly formed code snippet"));
                    }
                    else
                    {
                        var regMatchGood = new Regex("%%[a-zA-Z0-9_]+?%%", RegexOptions.IgnoreCase);
                        var matchListGood = regMatchGood.Matches(s);
                        if ((matchList.Count / 2) > matchListGood.Count)
                        {
                            _errorList.Add(new ECNError(Entity, Method, "Incorrectly formed code snippet"));
                        }
                    }
                }

                //%% and ##
                var reg1 = new Regex("%%.+?%%", RegexOptions.IgnoreCase);
                var matchList1 = reg1.Matches(s);

                foreach (Match m in matchList1)
                {
                    if (!string.IsNullOrWhiteSpace(m.Value.ToString()))
                    {
                        if (!subLY.Contains(m.Value.ToString().ToLower().Replace("%%", string.Empty)))
                        {
                            subLY.Add(m.Value.ToString().ToLower().Replace("%%", string.Empty));
                        }
                    }
                }
            }

            return subLY;
        }

        private void RemoveItems()
        {
            _listNoExist.Remove("blastid");
            _listNoExist.Remove("groupid");
            _listNoExist.Remove("groupname");
            _listNoExist.Remove("emailtofriend");
            _listNoExist.Remove("conversiontrkcde");
            _listNoExist.Remove("unsubscribelink");
            _listNoExist.Remove("lastchanged");
            _listNoExist.Remove("createdon");
            _listNoExist.Remove("publicview");
            _listNoExist.Remove("company_address");
            _listNoExist.Remove("surveytitle");
            _listNoExist.Remove("surveylink");
            _listNoExist.Remove("currdate");
            _listNoExist.Remove("reportabuselink");
            _listNoExist.Remove("customer_name");
            _listNoExist.Remove("customer_address");
            _listNoExist.Remove("customer_webaddress");
            _listNoExist.Remove("customer_udf1");
            _listNoExist.Remove("customer_udf2");
            _listNoExist.Remove("customer_udf3");
            _listNoExist.Remove("customer_udf4");
            _listNoExist.Remove("customer_udf5");
        }
    }
}
