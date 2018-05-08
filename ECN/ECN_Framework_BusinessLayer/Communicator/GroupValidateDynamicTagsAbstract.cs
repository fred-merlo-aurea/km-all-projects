using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public abstract class GroupValidateDynamicTagsAbstract
    {
        private const ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Group;

        private List<string> fieldsList;

        public abstract IList<ECN_Framework_Entities.Communicator.GroupDataFields> GetGroupDataFields(int groupId);

        public abstract DataTable GetDataTable();

        public abstract ECN_Framework_Entities.Communicator.Layout GetLayout(int layoutId);

        public void ValidateDynamicTags(int groupId, int layoutId, KMPlatform.Entity.User user)
        {
            var errorList = new List<ECNError>();

            var dtEmail = GetDataTable();
            var gdfList = GetGroupDataFields(groupId);

            fieldsList = new List<string>();

            foreach (DataRow dataRow in dtEmail.Rows)
            {
                fieldsList.Add(dataRow["columnName"].ToString().ToLower());
            }
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in gdfList)
            {
                fieldsList.Add(groupDataFields.ShortName.ToLower());
            }

            ECN_Framework_Entities.Communicator.Layout layout = GetLayout(layoutId);
            ValidateLayOutContent(layout.Slot1);
            ValidateLayOutContent(layout.Slot2);
            ValidateLayOutContent(layout.Slot3);
            ValidateLayOutContent(layout.Slot4);
            ValidateLayOutContent(layout.Slot5);
            ValidateLayOutContent(layout.Slot6);
            ValidateLayOutContent(layout.Slot7);
            ValidateLayOutContent(layout.Slot8);
            ValidateLayOutContent(layout.Slot9);
        }

        private void ValidateLayOutContent(ECN_Framework_Entities.Communicator.Content content)
        {
            var errorList = new List<ECNError>();
            if (content != null)
            {
                errorList = ValidateDynamicTags(content, fieldsList);
                if (errorList.Count > 0)
                {
                    throw new ECNException(errorList);
                }
            }
        }

        private List<ECNError> ValidateDynamicTags(ECN_Framework_Entities.Communicator.Content content, List<string> fieldsList)
        {
            var missingUDF = new List<string>();
            var errorList = new List<ECNError>();
            var Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            var dynamicTagList = content.DynamicTagList;
            if (dynamicTagList != null && dynamicTagList.Count > 0)
            {
                foreach (var dynamicTag in dynamicTagList)
                {
                    var dynamicTagRuleList = dynamicTag.DynamicTagRulesList;
                    if (dynamicTagRuleList != null && dynamicTagRuleList.Count > 0)
                    {
                        foreach (var dynamicTagRule in dynamicTagRuleList)
                        {
                            foreach (var RuleCondition in dynamicTagRule.Rule.RuleConditionsList)
                            {
                                if (!fieldsList.Contains(RuleCondition.Field.ToLower()))
                                {
                                    if (!missingUDF.Contains(RuleCondition.Field))
                                    {
                                        missingUDF.Add(RuleCondition.Field);
                                    }
                                }
                            }
                        }
                    }
                }
                if (missingUDF.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string udf in missingUDF.Distinct())
                    {
                        sb.Append($"{udf},");
                    }

                    errorList.Add(new ECNError(Entity, Method, $"Group does not contain necessary fields ({sb.Remove(sb.Length - 1, 1)}) for Dynamic Tags"));
                }
            }
            return errorList;
        }
    }
}
