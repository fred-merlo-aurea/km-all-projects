using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class GroupDataFields
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.GroupUDFs;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.GroupDataFields;

        public static bool Exists(string shortName, int? groupDataFieldsID, int groupID, int customerID)
        {
            shortName = shortName.Replace("%", "");
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.GroupDataFields.Exists(shortName, groupDataFieldsID, groupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int groupDataFieldsID, int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.GroupDataFields.Exists(groupDataFieldsID, groupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.GroupDataFields.Exists(groupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static SortedList AddGroupDataFieldsToEmailTableColumns(ArrayList groupDataFields, SortedList _columns)
        {
            int i = _columns.Count;
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields field in groupDataFields)
            {
                _columns.Add(i++, string.Format("user_{0}", field.ShortName));
            }
            return _columns;
        }

        public static List<ECN_Framework_Entities.Communicator.GroupDataFields> GetByDataFieldSetID(int groupID, int datafieldSetID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = new List<ECN_Framework_Entities.Communicator.GroupDataFields>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                gdfList = ECN_Framework_DataLayer.Communicator.GroupDataFields.GetByDataFieldSetID(groupID, datafieldSetID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(gdfList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return gdfList;
        }

        public static List<ECN_Framework_Entities.Communicator.GroupDataFields> GetByGroupID(int groupID, KMPlatform.Entity.User user, bool getChildren = false)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = new List<ECN_Framework_Entities.Communicator.GroupDataFields>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                gdfList = ECN_Framework_DataLayer.Communicator.GroupDataFields.GetByGroupID(groupID);
                scope.Complete();
            }

            if(gdfList.Count > 0 && getChildren )
            {
                foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdfList)
                {
                    gdf.DefaultValue = ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.GetByGDFID(gdf.GroupDataFieldsID);
                }
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(gdfList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return gdfList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.GroupDataFields> GetByGroupID_NoAccessCheck(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = new List<ECN_Framework_Entities.Communicator.GroupDataFields>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                gdfList = ECN_Framework_DataLayer.Communicator.GroupDataFields.GetByGroupID(groupID);
                scope.Complete();
            }

            return gdfList;
        }

        public static List<ECN_Framework_Entities.Communicator.GroupDataFields> GetByGroupID_NoAccessCheck_UseAmbientTransaction(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = new List<ECN_Framework_Entities.Communicator.GroupDataFields>();
            using (TransactionScope scope = new TransactionScope())
            {
                gdfList = ECN_Framework_DataLayer.Communicator.GroupDataFields.GetByGroupID(groupID);
                scope.Complete();
            }

            return gdfList;
        }

        public static List<ECN_Framework_Entities.Communicator.GroupDataFields> GetByCustomerID(int CustomerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = new List<ECN_Framework_Entities.Communicator.GroupDataFields>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                gdfList = ECN_Framework_DataLayer.Communicator.GroupDataFields.GetByCustomerID(CustomerID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(gdfList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return gdfList;
        }

        public static ECN_Framework_Entities.Communicator.GroupDataFields GetByID(int groupDatafieldsID, int groupID, KMPlatform.Entity.User user)
        {
            return GetByID(groupDatafieldsID, user);
        }

        public static ECN_Framework_Entities.Communicator.GroupDataFields GetByID(int groupDatafieldsID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.GroupDataFields gdf = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                gdf = ECN_Framework_DataLayer.Communicator.GroupDataFields.GetByID(groupDatafieldsID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(gdf, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return gdf;
        }

        public static void Delete(int groupDataFieldsID, int groupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(groupDataFieldsID, groupID, user.CustomerID))
            {
                if (ECN_Framework_BusinessLayer.FormDesigner.Form.ActiveByGDF(groupDataFieldsID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Custom Field is used in Form(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                if (ECN_Framework_BusinessLayer.FormDesigner.Newsletter.ActiveByGDF(groupDataFieldsID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Custom Field is used in Form(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                ECN_Framework_Entities.Communicator.GroupDataFields gdf = GetByID(groupDataFieldsID, groupID, user);
                if (IsReservedWord(gdf.ShortName))
                {
                    errorList.Add(new ECNError(Entity, Method, gdf.ShortName + " is a reserved UDF. This UDF cannot be deleted."));
                    throw new ECNException(errorList);
                }
                
                List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList = ECN_Framework_BusinessLayer.Communicator.Blast.GetByGroupID(groupID, user, false);
                if (blastList.Count > 0)
                {
                    if (ECN_Framework_DataLayer.Communicator.GroupDataFields.ActiveByGDF(gdf.ShortName, groupID, user.CustomerID))
                    {
                        errorList.Add(new ECNError(Entity, Method, "User Defined Field is used in Group Filter(s). Deleting is not allowed."));
                        throw new ECNException(errorList);
                    }
                }

                if (UsedInFilter(groupDataFieldsID, groupID))
                {
                    errorList.Add(new ECNError(Entity, Method, "User Defined Field is used in Group Filter(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }


                    if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                        throw new ECN_Framework_Common.Objects.SecurityException();

                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_BusinessLayer.Communicator.EmailDataValues.Delete(groupID, groupDataFieldsID, user);

                    ECN_Framework_DataLayer.Communicator.GroupDataFields.Delete(groupID, groupDataFieldsID, user.CustomerID, user.UserID);
                    scope.Complete();
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "UDF does not exist"));
                throw new ECNException(errorList);
            }

        }

        public static bool UsedInFilter(int groupDataFieldsID, int groupID)
        {
            bool used = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                used = ECN_Framework_DataLayer.Communicator.GroupDataFields.UsedInFilter(groupDataFieldsID, groupID);
                scope.Complete();
            }

            return used;
        }

        public static void Delete(int groupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            Group.GetByGroupID(groupID, user);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_BusinessLayer.Communicator.EmailDataValues.Delete(groupID, user);

                ECN_Framework_DataLayer.Communicator.GroupDataFields.Delete(groupID, user.CustomerID, user.UserID);
                scope.Complete();
            }

        }

        public static int Save(ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (groupDataFields.GroupDataFieldsID > 0)
            {
                if (!Exists(groupDataFields.GroupDataFieldsID, groupDataFields.GroupID, groupDataFields.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "UDF does not exist"));
                    throw new ECNException(errorList);
                }
            }
            Validate(groupDataFields);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(groupDataFields, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                groupDataFields.GroupDataFieldsID = ECN_Framework_DataLayer.Communicator.GroupDataFields.Save(groupDataFields);
                scope.Complete();
            }
            return groupDataFields.GroupDataFieldsID;
        }

        public static int Save_NoAccessCheck(ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (groupDataFields.GroupDataFieldsID > 0)
            {
                if (!Exists(groupDataFields.GroupDataFieldsID, groupDataFields.GroupID, groupDataFields.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "UDF does not exist"));
                    throw new ECNException(errorList);
                }
            }
            Validate(groupDataFields);

            //if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
            //    throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(groupDataFields, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                groupDataFields.GroupDataFieldsID = ECN_Framework_DataLayer.Communicator.GroupDataFields.Save(groupDataFields);
                scope.Complete();
            }
            return groupDataFields.GroupDataFieldsID;
        }


        public static bool IsReservedWord(string shortName)
        {
            DataTable email = ECN_Framework_BusinessLayer.Communicator.Email.GetColumnNames();
            DataTable emailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetColumnNames();
            foreach (DataRow row in email.Rows)
            {
                if (row["columnName"].ToString().ToUpper() == shortName.Trim().ToUpper())
                    return true;
            }
            foreach (DataRow row in emailGroup.Rows)
            {
                if (row["columnName"].ToString().ToUpper() == shortName.Trim().ToUpper())
                    return true;
            }
            if (shortName.Trim().ToUpper() == "TMP_EMAILID")
                return true;
            if (shortName.Trim().ToUpper() == "SHORTNAME")
                return true;
            if (shortName.Trim().ToUpper() == "BLASTID")
                return true;

            return false;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (groupDataFields.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                if (groupDataFields.ShortName.Trim() != string.Empty)
                {
                    if (!ECN_Framework_BusinessLayer.Collector.Survey.IsSurveyGroup(groupDataFields.GroupID))
                    {
                        if (groupDataFields.ShortName.Contains(" "))
                            errorList.Add(new ECNError(Entity, Method, "ShortName cannot contain spaces"));
                        else if (System.Text.RegularExpressions.Regex.IsMatch(groupDataFields.ShortName.Trim().Substring(0, 1), @"^\d+") ||
                             (!ECN_Framework_Common.Functions.RegexUtilities.IsValidUDFName(groupDataFields.ShortName.Trim())))
                            errorList.Add(new ECNError(Entity, Method, "ShortName has invalid characters"));

                    }

                    if (errorList.Count == 0)  //to avoid generating 2 errors for ShortName
                    {
                        if (IsReservedWord(groupDataFields.ShortName))
                            errorList.Add(new ECNError(Entity, Method, groupDataFields.ShortName + " is a reserved word"));
                        else if (Exists(groupDataFields.ShortName, groupDataFields.GroupDataFieldsID, groupDataFields.GroupID, groupDataFields.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "ShortName already exists in the group"));
                    }
                }
                else //if ShortName is EMPTY - just throw an error
                {
                    errorList.Add(new ECNError(Entity, Method, "ShortName cannot be empty"));
                }

                if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(groupDataFields.GroupID, groupDataFields.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));

                if (groupDataFields.DatafieldSetID != null && !ECN_Framework_BusinessLayer.Communicator.DataFieldSets.Exists(groupDataFields.DatafieldSetID.Value, groupDataFields.GroupID))
                {
                    errorList.Add(new ECNError(Entity, Method, "DataFieldSetID is invalid"));
                }

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(groupDataFields.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (groupDataFields.GroupDataFieldsID < 0 && (groupDataFields.CreatedUserID == null || (groupDataFields.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(groupDataFields.CreatedUserID.Value, false)))))
                    {
                        if (groupDataFields.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(groupDataFields.CreatedUserID.Value, groupDataFields.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }
                    if (groupDataFields.GroupDataFieldsID > 0 && (groupDataFields.UpdatedUserID == null || (groupDataFields.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(groupDataFields.UpdatedUserID.Value, false)))))
                    {
                        if (groupDataFields.GroupDataFieldsID > 0 && (groupDataFields.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(groupDataFields.UpdatedUserID.Value, groupDataFields.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    scope.Complete();
                }
            }
            if (groupDataFields.IsPublic.Trim().ToUpper() != "Y" && groupDataFields.IsPublic.Trim().ToUpper() != "N")
                errorList.Add(new ECNError(Entity, Method, "IsPublic is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static Hashtable GetGroupUDFHashtable(int GroupID, KMPlatform.Entity.User user)
        {
            Hashtable htUDF = new Hashtable();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> listGDF = new List<ECN_Framework_Entities.Communicator.GroupDataFields>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                listGDF = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(GroupID, user);
                scope.Complete();
            }

            foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in listGDF)
            {
                if (!htUDF.ContainsKey(gdf.GroupDataFieldsID))
                {
                    htUDF.Add(gdf.GroupDataFieldsID, gdf.ShortName);
                }
            }

            return htUDF;
        }

        public static Hashtable GetGroupUDFHashtable_NoAccessCheck(int GroupID)
        {
            Hashtable htUDF = new Hashtable();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> listGDF = new List<ECN_Framework_Entities.Communicator.GroupDataFields>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                listGDF = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(GroupID);
                scope.Complete();
            }

            foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in listGDF)
            {
                if (!htUDF.ContainsKey(gdf.GroupDataFieldsID))
                {
                    htUDF.Add(gdf.GroupDataFieldsID, gdf.ShortName);
                }
            }

            return htUDF;
        }

        public static ECN_Framework_Entities.Communicator.GroupDataFields GetByShortName(string ShortName, int GroupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.GroupDataFields gdf;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                gdf = ECN_Framework_DataLayer.Communicator.GroupDataFields.GetByShortName(ShortName, GroupID);
                scope.Complete();
            }

            return gdf;
        }
    }
}
