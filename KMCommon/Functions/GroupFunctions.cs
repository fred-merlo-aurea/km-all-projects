using System;
using System.Data;
using System.Data.SqlClient;

namespace KM.Common.Functions
{
    public class GroupFunctions
    {
        private const string InsertUdfCommandText = "INSERT INTO GroupDatafields (ShortName, longname, GroupID,IsPublic) VALUES (@ShortName, @LongName, @GroupID, @IsPublic); select @@identity";
        private const string InsertUdfParameterShortName = "@ShortName";
        private const string InsertUdfParameterLongName = "@LongName";
        private const string InsertUdfParameterGroupId = "@GroupID";
        private const string InsertUdfParameterIsPublic = "@IsPublic";
        private const string Underscore = "_";
        private const string Yes = "Y";
        private const string Space = " ";
        private const string UdfExistsCommandText = "SELECT groupdatafieldsID FROM GroupDatafields WHERE isDeleted=0 and  GroupID = {0} AND replace(ShortName, ' ', '_') = '{1}'";
        private const string InsertGroupNoFolderCommandText =
            "INSERT INTO Groups (GroupName, GroupDescription, CustomerID, OwnerTypeCode, PublicFolder ) values ('{0}', '{0}', {1}, 'customer' , 0); select @@identity";
        private const string InsertGroupWithFolderCommandText =
            "INSERT INTO Groups (GroupName, GroupDescription, CustomerID, FolderID, OwnerTypeCode, PublicFolder ) values ('{0}', '{0}', {1}, {2}, 'customer' , 0); select @@identity";

        public static int InsertGroup(string groupName, int customerId, int folderId, string connectionString)
        {
            var commandtext = folderId == 0
                              ? string.Format(InsertGroupNoFolderCommandText, groupName, customerId)
                              : string.Format(InsertGroupWithFolderCommandText, groupName, customerId, folderId);

            object result;
            using (var sqlCommand = new SqlCommand(commandtext) { CommandType = CommandType.Text })
            {
                sqlCommand.Connection = new SqlConnection(connectionString);
                result = DataFunctions.ExecuteScalar(sqlCommand, false);
            }

            return Convert.ToInt32(result);
        }

        public static int UdfExists(int groupId, string name, string connectionString)
        {
            var cleanName = DataFunctions
                .CleanString(name)
                .Replace(Space, Underscore);

            var commandtext = string.Format(
                UdfExistsCommandText,
                groupId,
                cleanName);

            object result;
            using (var sqlCommand = new SqlCommand(commandtext) { CommandType = CommandType.Text })
            {
                sqlCommand.Connection = new SqlConnection(connectionString);
                result = DataFunctions.ExecuteScalar(sqlCommand, false);
            }

            return Convert.ToInt32(result);
        }

        public static int InsertUdf(int groupId, string name, string connectionString)
        {
            object result;
            using (var sqlCommand = new SqlCommand(InsertUdfCommandText) { CommandType = CommandType.Text })
            {
                sqlCommand.Parameters.AddWithValue(InsertUdfParameterShortName, name.Replace(Space, Underscore));
                sqlCommand.Parameters.AddWithValue(InsertUdfParameterLongName, name);
                sqlCommand.Parameters.AddWithValue(InsertUdfParameterGroupId, groupId);
                sqlCommand.Parameters.AddWithValue(InsertUdfParameterIsPublic, Yes);
                sqlCommand.Connection = new SqlConnection(connectionString);
                result = DataFunctions.ExecuteScalar(sqlCommand, false);
            }

            return Convert.ToInt32(result);
        }
    }
}
