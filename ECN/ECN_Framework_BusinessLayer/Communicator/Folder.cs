using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Folder
    {
        //SUNIL - TODO - support for content  / group folder.
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Folder;

        public static bool SubfoldersExist(int folderID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Folder.SubfoldersExist(folderID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int folderID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Folder.Exists(folderID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int folderID, int customerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes type)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Folder.Exists(folderID, customerID, type);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int folderID, string folderName, int parentID, int customerID, string folderType)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Folder.Exists(folderID, folderName, parentID, customerID, folderType);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.Folder GetByFolderID(int folderID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Folder folder = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                folder = ECN_Framework_DataLayer.Communicator.Folder.GetByFolderID(folderID);
                scope.Complete();
            }

            
            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(folder, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return folder;
        }

        public static ECN_Framework_Entities.Communicator.Folder GetByFolderID_UseAmbientTransaction(int folderID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Folder folder = null;
            using (TransactionScope scope = new TransactionScope())
            {
                folder = ECN_Framework_DataLayer.Communicator.Folder.GetByFolderID(folderID);
                scope.Complete();
            }


            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(folder, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return folder;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.Folder GetByFolderID_NoAccessCheck(int folderID)
        {
            ECN_Framework_Entities.Communicator.Folder folder = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                folder = ECN_Framework_DataLayer.Communicator.Folder.GetByFolderID(folderID);
                scope.Complete();
            }

            return folder;
        }

        public static ECN_Framework_Entities.Communicator.Folder GetByFolderID_NoAccessCheck_UseAmbientTransaction(int folderID)
        {
            ECN_Framework_Entities.Communicator.Folder folder = null;
            using (TransactionScope scope = new TransactionScope())
            {
                folder = ECN_Framework_DataLayer.Communicator.Folder.GetByFolderID(folderID);
                scope.Complete();
            }

            return folder;
        }

        public static List<ECN_Framework_Entities.Communicator.Folder> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Folder> folderList = new List<ECN_Framework_Entities.Communicator.Folder>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                folderList = ECN_Framework_DataLayer.Communicator.Folder.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(folderList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return folderList;
        }

        public static List<ECN_Framework_Entities.Communicator.Folder> GetByType(int customerID, string type, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Folder> folderList = new List<ECN_Framework_Entities.Communicator.Folder>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                folderList = ECN_Framework_DataLayer.Communicator.Folder.GetByType(customerID, type);
                scope.Complete();
            }


            //Removed on 3/26/2015 due to performance issues.  Not needed as we get by customer id and if they try to access something in a folder that they don't have 
            //rights to we stop them then.
            //foreach (ECN_Framework_Entities.Communicator.Folder folder in folderList)
            //{
            //    if (folder.FolderType == ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString() && !ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(folder, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.contentpriv, user))
            //        throw new ECN_Framework_Common.Objects.SecurityException();

            //    if (folder.FolderType == ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString() && !ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(folder, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.grouppriv, user))
            //        throw new ECN_Framework_Common.Objects.SecurityException();
            //} 

            return folderList;
        }

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.Folder> folderList, KMPlatform.Entity.User user)
        //{
        //    if (folderList != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from f in folderList
        //                                join c in custList on f.CustomerID equals c.CustomerID
        //                                select new { f.FolderID };

        //            if (securityCheck.Count() != folderList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from f in folderList
        //                                where f.CustomerID != user.CustomerID
        //                                select new { f.FolderID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.Folder folder, KMPlatform.Entity.User user)
        //{
        //    if (folder != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (folder.CustomerID.Value != user.CustomerID)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public static void Delete(int folderID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(folderID, user.CustomerID))
            {
                if (SubfoldersExist(folderID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Folder has subfolders"));
                    throw new ECNException(errorList);
                }
                else
                {
                    //this does security check as well
                    ECN_Framework_Entities.Communicator.Folder folder = ECN_Framework_BusinessLayer.Communicator.Folder.GetByFolderID(folderID, user);
                    switch (folder.FolderType.Trim().ToLower())
                    {
                        case "cnt":
                            if (ECN_Framework_BusinessLayer.Communicator.Content.FolderUsed(folderID))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Folder is used in Content"));
                                throw new ECNException(errorList);
                            }
                            else if (ECN_Framework_BusinessLayer.Communicator.Layout.FolderUsed(folderID))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Folder is used in Layout(s)"));
                                throw new ECNException(errorList);
                            }
                            using (TransactionScope scope = new TransactionScope())
                            {
                                ECN_Framework_DataLayer.Communicator.Folder.Delete(folderID, user.CustomerID, user.UserID);
                                scope.Complete();
                            }
                            break;
                        case "grp":
                            if (ECN_Framework_BusinessLayer.Communicator.Group.FolderUsed(folderID))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Folder is used in Group(s)"));
                                throw new ECNException(errorList);
                            }
                            using (TransactionScope scope = new TransactionScope())
                            {
                                ECN_Framework_DataLayer.Communicator.Folder.Delete(folderID, user.CustomerID, user.UserID);
                                scope.Complete();
                            }
                            break;
                        case "img":
                            System.IO.DirectoryInfo dir = null;
                            string path = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + user.CustomerID + "/images/" + folder.FolderName.Trim() + "/";
			                dir = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath(path));	
				            if(dir.GetFiles().Length > 0)
                            {
                                errorList.Add(new ECNError(Entity, Method, "Images exist in this Folder. Delete is not allowed"));
                                throw new ECNException(errorList);
				            }
                            else
                            {
					            dir.Delete(true);
				            }
                            break;
                        default:
                            errorList.Add(new ECNError(Entity, Method, "Unknown folder type"));
                            throw new ECNException(errorList);
                    }
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Folder does not exist"));
                throw new ECNException(errorList);
            }
        }

        public static void Validate(ECN_Framework_Entities.Communicator.Folder folder)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (folder.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(folder.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (folder.FolderID < 0)
                    {
                    if (folder.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(folder.CreatedUserID.Value, folder.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }
                    else
                    {
                    if (folder.FolderID > 0 && (folder.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(folder.UpdatedUserID.Value, folder.CustomerID.Value)))
                        errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }

                    scope.Complete();
                }


                if (folder.ParentID > 0 && !Folder.Exists(folder.ParentID, folder.CustomerID.Value))
                {
                    errorList.Add(new ECNError(Entity, Method, "ParentID is invalid"));
                }
                else
                {
                    if (folder.FolderName == null || folder.FolderName.Trim() == string.Empty)
                        errorList.Add(new ECNError(Entity, Method, "FolderName cannot be empty"));
                    else if (Exists(folder.FolderID, folder.FolderName, folder.ParentID, folder.CustomerID.Value, folder.FolderType))
                        errorList.Add(new ECNError(Entity, Method, "FolderName already exists in this folder"));
                    else if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(folder.FolderName))
                        errorList.Add(new ECNError(Entity, Method, "FolderName has invalid characters"));
                }
            }

            if (folder.FolderType == null || folder.FolderType.Trim() == string.Empty || (folder.FolderType.Trim() != ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString() && folder.FolderType.Trim() != ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString() && folder.FolderType.Trim() != ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.IMG.ToString()))
                errorList.Add(new ECNError(Entity, Method, "FolderType is invalid"));
            if (folder.IsSystem == null)
                errorList.Add(new ECNError(Entity, Method, "IsSystem is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.Folder folder, KMPlatform.Entity.User user)
        {
            if (folder.FolderID > 0 && folder.CreatedUserID == null)
                folder.CreatedUserID = folder.UpdatedUserID;
            if (folder.FolderID > 0 && folder.CreatedDate == null)
                folder.CreatedDate = folder.UpdatedDate;
            Validate(folder);

            if (folder.FolderType == ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString() && !KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.ContentFolder, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (folder.FolderType == ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString() && !KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.GroupFolder, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(folder, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                folder.FolderID = ECN_Framework_DataLayer.Communicator.Folder.Save(folder);
                scope.Complete();
            }

            return folder.FolderID;
        }

        public static DataTable GetFolderTree(int customerID, int userID, string folderType, KMPlatform.Entity.User user)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Folder.GetFolderTree(customerID, userID, folderType);
                scope.Complete();
            }
            return dt;
            //foreach (DataRow row in dtFolder.Rows)
            //{
            //    if(!row["FolderID"].ToString().Equals("0"))
            //        GetByFolderID(Convert.ToInt32(row["FolderID"].ToString()), user);
            //}
            //return dtFolder;
        }

        public static DataTable GetBlastCategoryFolders(int customerID, KMPlatform.Entity.User user)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Folder.GetBlastCategoryFolders(customerID);
                scope.Complete();
            }
            return dt;
        }

        public static List<ECN_Framework_Entities.Communicator.Folder> GetByContentSearch(int customerID, string type, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Folder> folderList = new List<ECN_Framework_Entities.Communicator.Folder>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                folderList = ECN_Framework_DataLayer.Communicator.Folder.GetByType(customerID, type);
                scope.Complete();
            }

            return folderList;
        }

        public static int GetFolderIDByName(int customerID, string folderName, string folderType)
        {
            int fID = -1;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                fID = ECN_Framework_DataLayer.Communicator.Folder.GetFolderIDByName(customerID, folderName, folderType);
                scope.Complete();
            }
            return fID;
        }
    }
}
