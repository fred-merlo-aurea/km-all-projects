using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using StringFunctions = ECN_Framework_Common.Functions.StringFunctions;

namespace ECN_Framework_DataLayer.Communicator.Helpers
{
    internal class CommunicatorMethodsHelper
    {
        private const string ExceptionProcedureNameNullOrWhiteSpace = "Procedure name is null or white space.";
        private const string SecondarySourceParam = "@source";
        private const string CompositeKeyName = "CompositeKey";
        private const string FilenameName = "Filename";
        private const string FormatTypeCodeName = "FormatTypeCode";
        private const string OverwriteWithNullName = "OverwriteWithNull";
        private const string OverwriteWithNullParam = "overwritewithNULL";
        private const string SubscribeTypeCodeName = "SubscribeTypeCode";
        private const string XmlProfileName = "XmlProfile";
        private const string XmlProfileParam = "xmlProfile";
        private const string XmlUdfName = "XmlUdf";
        private const string XmlUdfParam = "xmlUDF";
        private const string IdProperty = "ID";
        private const string IdRegex = "Id$";
        private const string RegexJoinSymbol = "|";

        public static int ExecuteScalarBlastSingle(int blastId, int emailId, int layoutPlanId, int customerId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                BlastId = blastId,
                EmailId = emailId,
                LayoutPlanId = layoutPlanId,
                CustomerId = customerId
            };
            return ExecuteScalar(readAndFillParams, procedureName);
        }

        public static int ExecuteScalarConversionLinks(int linkId, string linkName, int layoutId, int customerId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                LinkId = linkId,
                LinkName = linkName,
                LayoutId = layoutId,
                CustomerId = customerId
            };
            return ExecuteScalar(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryBlastSingle(int emailId, int layoutPlanId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                EmailId = emailId,
                LayoutPlanId = layoutPlanId,
                UpdatedUserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryLayoutPlans(int layoutPlanId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                LayoutPlanId = layoutPlanId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryLayoutPlans(int layoutPlanId, int userId, int layoutId, int customerId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                LayoutPlanId = layoutPlanId,
                UserId = userId,
                CustomerId = customerId,
                LayoutId = layoutId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryCampaignItem(int campaignId, int? campaignItemId, int customerId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                CampaignId = campaignId,
                CampaignItemId = campaignItemId,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryCampaignItemBlast(
            int? campaignItemId,
            int? campaignItemBlastId, 
            int? blastId, 
            int? customerId,
            int? userId, 
            string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                CampaignItemId = campaignItemId,
                CampaignItemBlastId = campaignItemBlastId,
                BlastId = blastId,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryContent(int contentId, int? filterId, int customerId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                ContentId = contentId,
                FilterId = filterId,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryEmailGroups(int groupId, int? emailId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                GroupId = groupId,
                EmailId = emailId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryFilter(int filterId, int? fdId, int customerId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                FilterId = filterId,
                FdId = fdId,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryFilter(int filterId, int customerId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                FilterId = filterId,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryFilterGroup(int filterId, int filterGroupId, int customerId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                FilterId = filterId,
                FilterGroupId = filterGroupId,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryFilterCondition(int filterGroupId, int customerId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                FilterGroupId = filterGroupId,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryFilterCondition(int filterGroupId, int filterConditionID, int customerId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                FilterGroupId = filterGroupId,
                FilterConditionId = filterConditionID,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryGroupDataFieldsSingle(int groupId, int groupDataFieldsId, int customerId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                GroupId = groupId,
                GroupDataFieldsId = groupDataFieldsId,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryGroupDataFields(int groupId, int customerId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                GroupId = groupId,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryLinks(int? layoutId, int? linkId, int customerId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                LayoutId = layoutId,
                LinkId = linkId,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static void ExecuteNonQueryEmailDataValues(int groupId, int? groupDataFieldsId, int customerId, int userId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                GroupId = groupId,
                GroupDataFieldsId = groupDataFieldsId,
                CustomerId = customerId,
                UserId = userId
            };
            ExecuteNonQuery(readAndFillParams, procedureName);
        }

        public static SqlCommand GetCampaignItemBlast(int? campaignItemBlastId, int? campaignItemId, int? blastId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                CampaignItemBlastId = campaignItemBlastId,
                CampaignItemId = campaignItemId,
                BlastId = blastId
            };
            return SqlParameterHelper<FillCommunicatorArgs>.CreateCommand(readAndFillParams, procedureName);
        }

        public static SqlCommand GetLinkAlias(int? contentId, int? linkOwnerId, string link, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                ContentId = contentId,
                OwnerId = linkOwnerId,
                Link = !string.IsNullOrWhiteSpace(link) ? link : null
            };
            return SqlParameterHelper<FillCommunicatorArgs>.CreateCommand(readAndFillParams, procedureName);
        }

        public static SqlCommand GetLinkOwner(int? linkOwnerIndexId, int? customerId, string procedureName)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                LinkOwnerIndexId = linkOwnerIndexId,
                CustomerId = customerId
            };
            return SqlParameterHelper<FillCommunicatorArgs>.CreateCommand(readAndFillParams, procedureName);
        }

        public static int ExecuteScalar(FillCommunicatorArgs fillCommunicatorArgs, string procedureName)
        {
            if (fillCommunicatorArgs == null)
            {
                throw new ArgumentNullException(nameof(fillCommunicatorArgs));
            }

            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException(ExceptionProcedureNameNullOrWhiteSpace);
            }

            var command = SqlParameterHelper<FillCommunicatorArgs>.CreateCommand(fillCommunicatorArgs, procedureName);
            var resultExecuteScalar = DataFunctions.ExecuteScalar(command, DataFunctions.ConnectionString.Communicator.ToString());
            var resultParse = 0;
            int.TryParse(resultExecuteScalar.ToString(), out resultParse);

            return resultParse;
        }

        public static void ExecuteNonQuery(FillCommunicatorArgs fillCommunicatorArgs, string procedureName)
        {
            if (fillCommunicatorArgs == null)
            {
                throw new ArgumentNullException(nameof(fillCommunicatorArgs));
            }

            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException(ExceptionProcedureNameNullOrWhiteSpace);
            }

            var command = SqlParameterHelper<FillCommunicatorArgs>.CreateCommand(fillCommunicatorArgs, procedureName);
            DataFunctions.ExecuteNonQuery(command, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetDataTableEmailGroups(FillEmailGroupsArgs fillEmailGroupsArgs, string procedureName)
        {
            if (fillEmailGroupsArgs == null)
            {
                throw new ArgumentNullException(nameof(fillEmailGroupsArgs));
            }

            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException(ExceptionProcedureNameNullOrWhiteSpace);
            }
            
            var command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = procedureName;

            var replacements = GetDefaultReplacements();
            var regex = new Regex(string.Join(RegexJoinSymbol, replacements.Keys));

            foreach (var prop in typeof(FillEmailGroupsArgs).GetProperties())
            {
                var propertyName = Regex.Replace(prop.Name, IdRegex, IdProperty);
                propertyName = regex.Replace(prop.Name, element => replacements[element.Value]);
                var propertyValue = prop.GetValue(fillEmailGroupsArgs);

                if (!string.IsNullOrWhiteSpace(propertyValue?.ToString()))
                {
                    command.Parameters.AddWithValue($"@{propertyName}", propertyValue);
                }
            }

            if(StringFunctions.HasValue(fillEmailGroupsArgs.SourceNotRequired))
            {
                command.Parameters.Add(new SqlParameter(SecondarySourceParam, fillEmailGroupsArgs.SourceNotRequired));
            }
            return DataFunctions.GetDataTable(command, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static Dictionary<string, string> GetDefaultReplacements()
        {
            return new Dictionary<string, string>
            {
                {XmlUdfParam, XmlUdfName},
                {XmlProfileParam, XmlProfileName},
                {FormatTypeCodeName.ToLower(), FormatTypeCodeName},
                {SubscribeTypeCodeName.ToLower(), SubscribeTypeCodeName},
                {FilenameName.ToLower(), FilenameName},
                {OverwriteWithNullParam, OverwriteWithNullName},
                {CompositeKeyName.ToLower(), CompositeKeyName}
            };
        }
    }
}
