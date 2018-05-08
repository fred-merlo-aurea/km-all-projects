using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace ecn.common.classes
{
    public class BlastContent
    {
        public static readonly string CommandGetColumns =
            " select lower(c.name) as shortname from sysobjects o join syscolumns c on o.id = c.id where o.name = 'Emails' " +
            " union " +
            " select shortname as shortname from groupdatafields where groupID in ({0}) and IsDeleted = 0 ";

        public static readonly string CommandGetContent =
            " select ContentSource as Content from Layout " +
            " join Content on  Content.ContentID = Layout.ContentSlot1 or Content.ContentID = Layout.ContentSlot2 or  Content.ContentID = Layout.ContentSlot3 or " +
            " Content.ContentID = Layout.ContentSlot4 or Content.ContentID = Layout.ContentSlot5 or Content.ContentID = Layout.ContentSlot6 or " +
            " Content.ContentID = Layout.ContentSlot7 or Content.ContentID = Layout.ContentSlot8 or Content.ContentID = Layout.ContentSlot9  " +
            " where layout.layoutID in ({0}) and Layout.IsDeleted = 0 and Content.IsDeleted = 0 union all " +
            " select ContentText as Content from Layout " +
            " join Content on  Content.ContentID = Layout.ContentSlot1 or Content.ContentID = Layout.ContentSlot2 or  Content.ContentID = Layout.ContentSlot3 or " +
            " Content.ContentID = Layout.ContentSlot4 or Content.ContentID = Layout.ContentSlot5 or Content.ContentID = Layout.ContentSlot6 or " +
            " Content.ContentID = Layout.ContentSlot7 or Content.ContentID = Layout.ContentSlot8 or Content.ContentID = Layout.ContentSlot9 " +
            " where Layout.layoutID in ({0}) and Layout.IsDeleted = 0 and Content.IsDeleted = 0 union all " +
            " select ContentMobile as Content from Layout " +
            " join Content on  Content.ContentID = Layout.ContentSlot1 or Content.ContentID = Layout.ContentSlot2 or  Content.ContentID = Layout.ContentSlot3 or " +
            " Content.ContentID = Layout.ContentSlot4 or Content.ContentID = Layout.ContentSlot5 or Content.ContentID = Layout.ContentSlot6 or " +
            " Content.ContentID = Layout.ContentSlot7 or Content.ContentID = Layout.ContentSlot8 or Content.ContentID = Layout.ContentSlot9 " +
            " where Layout.layoutID in ({0}) and Layout.IsDeleted = 0 and Content.IsDeleted = 0";

        public static readonly string[] NonColumns =
        {
            "blastid",
            "groupid",
            "groupname",
            "emailtofriend",
            "conversiontrkcde",
            "unsubscribelink",
            "lastchanged",
            "createdon",
            "publicview",
            "company_address",
            "surveytitle",
            "surveylink",
            "currdate",
            "customer_name",
            "customer_address",
            "customer_webaddress",
            "customer_udf1",
            "customer_udf2",
            "customer_udf3",
            "customer_udf4",
            "customer_udf5",
        };

        public static bool ValidateBlastContent(string groupIDs, string layoutID, string subject, ref string errorMessage)
        {
            bool valid = false;
            errorMessage = string.Empty;

            try
            {
                valid = CheckEachGroupSnippet(groupIDs, layoutID, subject, ref errorMessage);
            }
            catch (Exception ex)
            {
                valid = false;
                errorMessage = ex.ToString();
            }

            return valid;
        }

        public static bool ValidateBlastContent(string groupIDs, string layoutID, string subject, string dynamicFromName, string dynamicFromEmail, string dynamicReplyTo, ref string errorMessage)
        {
            bool valid = false;
            errorMessage = string.Empty;

            try
            {
                valid = CheckEachGroupSnippet(groupIDs, layoutID, subject + " " + dynamicFromName + " " + dynamicFromEmail + " " + dynamicReplyTo, ref errorMessage);
            }
            catch (Exception ex)
            {
                valid = false;
                errorMessage = ex.ToString();
            }

            return valid;
        }

        private static bool CheckEachGroupSnippet(string groupIds, string layoutId, string subject, ref string errorMessage)
        {
            bool isSuccess;

            try
            {
                // get column names in table Emails
                var getColumnsCommand = string.Format(CommandGetColumns, groupIds);
                var columnsList = DataFunctions.GetDataTable(getColumnsCommand, DataFunctions.con_communicator)
                    .Rows.Cast<DataRow>()
                    .Select(row => row["shortname"].ToString().ToLower())
                    .ToList();

                // get contents by layoutId
                var getContentCommand = string.Format(CommandGetContent, layoutId);
                var contentList = DataFunctions.GetDataTable(getContentCommand, DataFunctions.con_communicator)
                    .Rows.Cast<DataRow>()
                    .Select(row => row["Content"].ToString().ToLower())
                    .ToList();

                contentList.Add(subject.Trim().ToLower());

                // get fields in content
                var fieldsToMatch = GetFieldsToMatch(contentList, ref errorMessage, out isSuccess);
                if (isSuccess == false)
                {
                    return false;
                }

                var listNoExist = new List<string>();
                foreach (var field in fieldsToMatch)
                {
                    if (!columnsList.Contains(field))
                    {
                        listNoExist.Add(field);
                    }
                }

                RemoveNonColumns(listNoExist);

                if (listNoExist.Count > 0)
                {
                    var errorBuider = new StringBuilder();
                    errorBuider.Append("CodeSnippet(s) used in the content do not exist in the group. Please make sure the group includes a User Defined Field (UDF) for the CodeSnippet(s) below or remove the CodeSnippet(s) from the content and/or subject line.");

                    foreach (var field in listNoExist)
                    {
                        errorBuider.AppendLine($"<br /> %%{field}%%");
                    }

                    errorMessage = errorBuider.ToString();
                    isSuccess = false;
                }
            }
            catch (SqlException ex)
            {
                isSuccess = false;
                errorMessage = "ERROR:<br>" + ex.Message;
            }
            return isSuccess;
        }

        private static List<string> GetFieldsToMatch(List<string> contentList, ref string errorMessage, out bool success)
        {
            var fieldsToMatch = new List<string>();
            foreach (var content in contentList)
            {
                if (ValidateContentFormat(content, ref errorMessage) == false)
                {
                    success = false;
                    return fieldsToMatch;
                }

                //%% and ##
                var fieldsRegex = new Regex("%%.+?%%", RegexOptions.IgnoreCase);
                var fieldMatches = fieldsRegex.Matches(content);

                foreach (Match match in fieldMatches)
                {
                    if (!string.IsNullOrEmpty(match.Value))
                    {
                        var field = match.Value.Replace("%%", string.Empty);
                        if (!fieldsToMatch.Contains(field))
                        {
                            fieldsToMatch.Add(field);
                        }
                    }
                }
            }

            success = true;
            return fieldsToMatch;
        }

        private static bool ValidateContentFormat(string content, ref string errorMessage)
        {
            // Bad snippets - catches odd number of double % and catches non-alpha, non-numeric between the sets of double %
            var regMatch = new Regex("%%", RegexOptions.IgnoreCase);
            var matchList = regMatch.Matches(content);
            if (matchList.Count > 0)
            {
                if ((matchList.Count % 2) != 0)
                {
                    errorMessage = "ERROR:<br>Incorrectly formed code snippet";
                    return false;
                }
                else
                {
                    var regMatchGood = new Regex("%%[a-zA-Z0-9_]+?%%", RegexOptions.IgnoreCase);
                    var matchListGood = regMatchGood.Matches(content);
                    if ((matchList.Count / 2) > matchListGood.Count)
                    {
                        errorMessage = "ERROR:<br>Incorrectly formed code snippet";
                        return false;
                    }
                }
            }

            return true;
        }

        private static void RemoveNonColumns(List<string> listNoExist)
        {
            foreach (var column in NonColumns)
            {
                listNoExist.Remove(column);
            }
        }
    }
}
