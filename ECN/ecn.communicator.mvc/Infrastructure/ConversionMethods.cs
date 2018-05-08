using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ECN_Framework_Common.Objects;
using EmailBusiness = ECN_Framework_BusinessLayer.Communicator.Email;
using EmailEntity = ECN_Framework_Entities.Communicator.Email;
using KmStringFunctions = KM.Common.StringFunctions;
using UserEntity = KMPlatform.Entity.User;

namespace ecn.communicator.mvc.Infrastructure
{
    public static class ConversionMethods
    {
        private const string EmailAddressCannotBeEmpty = "Email address cannot be empty.";
        private const string BadBirthdateFormat = "Bad Birthdate format";
        private const string BadUserevent1DateFormat = "Bad UserEvent1Date format";
        private const string BadUserevent2DateFormat = "Bad UserEvent2Date format";

        private const Enums.Method MethodValidate = Enums.Method.Validate;
        private const Enums.Entity EntityEmailGroup = Enums.Entity.EmailGroup;

        public static ECN_Framework_Entities.Communicator.Group ToGroup_InternalModel(this Models.Group group)
        {
            ECN_Framework_Entities.Communicator.Group internalGroup = new ECN_Framework_Entities.Communicator.Group();
            internalGroup.AllowUDFHistory = group.AllowUDFHistory;
            internalGroup.Archived = group.Archived;
            internalGroup.CreatedDate = group.CreatedDate;
            internalGroup.CreatedUserID = group.CreatedUserID;
            internalGroup.CustomerID = group.CustomerID;
            internalGroup.FolderID = group.FolderID;
            internalGroup.GroupDescription = ECN_Framework_Common.Functions.StringFunctions.CleanString(group.GroupDescription.ToString().Trim());
            internalGroup.GroupID = group.GroupID;
            internalGroup.GroupName = ECN_Framework_Common.Functions.StringFunctions.CleanString(group.GroupName.ToString().Trim());
            internalGroup.IsSeedList = group.IsSeedList;
            internalGroup.MasterSupression = group.MasterSuppression;
            internalGroup.OptinFields = group.OptinFields;
            internalGroup.OptinHTML = group.OptinHTML;
            internalGroup.OwnerTypeCode = group.OwnerTypeCode;
            internalGroup.PublicFolder = group.PublicFolder;
            internalGroup.UpdatedUserID = group.UpdatedUserID;
            return internalGroup;
        }

        public static EmailEntity ToEmail_Internal(this Models.Email email, UserEntity user)
        {
            var errorList = new List<ECNError>();
            var emailAddress = ValidateParam(email.EmailAddress);
            var birthDate = TryParseBirthdate(email, errorList, MethodValidate, EntityEmailGroup);
            var userEvent1Date = TryParseEvent1Date(email, errorList, MethodValidate, EntityEmailGroup);
            var userEvent2Date = TryParseEvent2Date(email, errorList, MethodValidate, EntityEmailGroup);

            if (string.IsNullOrEmpty(emailAddress))
            {
                errorList.Add(new ECNError(EntityEmailGroup, MethodValidate, EmailAddressCannotBeEmpty));
            }
            if (errorList.Any())
            {
                throw new ECNException(errorList);
            }

            int emailIdResult;
            int.TryParse(email.EmailID.ToString(), out emailIdResult);

            var internalEmail = EmailBusiness.GetByEmailID(emailIdResult, user);
            internalEmail.EmailAddress = emailAddress;
            internalEmail.Title = ValidateParam(email.Title);
            internalEmail.FirstName = ValidateParam(email.FirstName);
            internalEmail.LastName = ValidateParam(email.LastName);
            internalEmail.FullName = ValidateParam(email.FullName);
            internalEmail.Company = ValidateParam(email.Company);
            internalEmail.Occupation = ValidateParam(email.Occupation);
            internalEmail.Address = ValidateParam(email.Address);
            internalEmail.Address2 = ValidateParam(email.Address2);
            internalEmail.City = ValidateParam(email.City);
            internalEmail.State = ValidateParam(email.State);
            internalEmail.Zip = ValidateParam(email.Zip);
            internalEmail.Country = ValidateParam(email.Country);
            internalEmail.Voice = ValidateParam(email.Voice);
            internalEmail.Mobile = ValidateParam(email.Mobile);
            internalEmail.Fax = ValidateParam(email.Fax);
            internalEmail.Website = ValidateParam(email.Website);
            internalEmail.Age = ValidateParam(email.Age);
            internalEmail.Income = ValidateParam(email.Income);
            internalEmail.Gender = ValidateParam(email.Gender);
            internalEmail.User1 = ValidateParam(email.User1);
            internalEmail.User2 = ValidateParam(email.User2);
            internalEmail.User3 = ValidateParam(email.User3);
            internalEmail.User4 = ValidateParam(email.User4);
            internalEmail.User5 = ValidateParam(email.User5);
            internalEmail.User6 = ValidateParam(email.User6);
            internalEmail.Birthdate = birthDate;
            internalEmail.UserEvent1 = ValidateParam(email.UserEvent1);
            internalEmail.UserEvent2 = ValidateParam(email.UserEvent2);
            internalEmail.UserEvent1Date = userEvent1Date;
            internalEmail.UserEvent2Date = userEvent2Date;
            internalEmail.Password = ValidateParam(email.Password);
            internalEmail.BounceScore = email.BounceScore;
            internalEmail.SoftBounceScore = email.SoftBounceScore;
            internalEmail.FormatTypeCode = ValidateParam(email.FormatTypeCode);
            internalEmail.SubscribeTypeCode = ValidateParam(email.SubscribeTypeCode);
            internalEmail.Notes = ValidateParam(email.Notes);

            return internalEmail;
        }

        private static DateTime? TryParseBirthdate(
            Models.Email email,
            ICollection<ECNError> errorList,
            Enums.Method method,
            Enums.Entity entity)
        {
            if (!string.IsNullOrEmpty(email.Birthdate))
            {
                DateTime birthDate;
                if (!DateTime.TryParse(email.Birthdate, out birthDate))
                {
                    errorList.Add(new ECNError(entity, method, BadBirthdateFormat));
                }
                else
                {
                    return birthDate;
                }
            }

            return null;
        }

        private static DateTime? TryParseEvent1Date(
            Models.Email email, 
            ICollection<ECNError> errorList, 
            Enums.Method method, 
            Enums.Entity entity)
        {
            if (!string.IsNullOrEmpty(email.UserEvent1Date))
            {
                DateTime userEvent1Date;
                if (!DateTime.TryParse(email.UserEvent1Date, out userEvent1Date))
                {
                    errorList.Add(new ECNError(entity, method, BadUserevent1DateFormat));
                }
                else
                {
                    return userEvent1Date;
                }
            }

            return null;
        }

        private static DateTime? TryParseEvent2Date(
            Models.Email email, 
            ICollection<ECNError> errorList, 
            Enums.Method method, 
            Enums.Entity entity)
        {
            if (!string.IsNullOrEmpty(email.UserEvent2Date))
            {
                DateTime userEvent2Date;
                if (!DateTime.TryParse(email.UserEvent2Date, out userEvent2Date))
                {
                    errorList.Add(new ECNError(entity, method, BadUserevent2DateFormat));
                }
                else
                {
                    return userEvent2Date;
                }
            }

            return null;
        }

        private static string ValidateParam(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                ? KmStringFunctions.CleanString(value)
                : string.Empty;
        }

        public static IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> ToKendoTree(this IEnumerable<ECN_Framework_Entities.Communicator.Folder> folderList)
        {
            Queue<Kendo.Mvc.UI.TreeViewItemModel> queue = new Queue<Kendo.Mvc.UI.TreeViewItemModel>();
            var root = new Kendo.Mvc.UI.TreeViewItemModel();
            root.Id = "0";
            root.Text = "Root";
            root.Expanded = true;
            queue.Enqueue(root);
            // Assume tree structure -- then we don't need to worry about depleting folderList. 
            // If graph is disconnected, folders will obviously be missed.
            // It will terminate because there are no cycles.
            while (queue.Count > 0) 
            {
                var cur = queue.Dequeue();
                var children = (from folder in folderList
                                where cur.Id == folder.ParentID.ToString()
                                select folder).ToList();
                foreach(var folder in children)
                {
                    var treeItem = folder.ToKendoTreeItem();
                    queue.Enqueue(treeItem);
                    cur.Items.Add(treeItem);
                }
            }
            return new List<Kendo.Mvc.UI.TreeViewItemModel>{ root };
        }
        public static Kendo.Mvc.UI.TreeViewItemModel ToKendoTreeItem(this ECN_Framework_Entities.Communicator.Folder folder)
        {
            var treeItem = new Kendo.Mvc.UI.TreeViewItemModel();
            treeItem.Text = folder.FolderName;
            treeItem.Id = folder.FolderID.ToString();
            return treeItem;
        }

        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            object convertedValue = null;
                            if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                try
                                {
                                    convertedValue = System.Convert.ChangeType(row[prop.Name], Nullable.GetUnderlyingType(propertyInfo.PropertyType));
                                    propertyInfo.SetValue(obj, convertedValue, null);
                                }
                                catch (InvalidCastException)
                                {

                                }
                            }
                            else
                            {
                                propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                            }
                            
                        }
                        catch(Exception e)
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static List<ecn.communicator.mvc.Models.Group> DataTableToListGroups(this DataTable table)
        {
            try
            {
                List<ecn.communicator.mvc.Models.Group> list = new List<ecn.communicator.mvc.Models.Group>();

                foreach (var data in table.AsEnumerable())
                {

                    ecn.communicator.mvc.Models.Group internalGroup = new ecn.communicator.mvc.Models.Group();
                    internalGroup.Archived = Convert.ToBoolean(data["Archived"]);
                    //internalGroup.FolderID = data.ItemArray[3] != null ? (int)data.ItemArray[3] : 0;
                    internalGroup.FolderName = string.IsNullOrWhiteSpace(data["FolderName"].ToString()) ? "" : data["FolderName"].ToString();
                    internalGroup.GroupID = (int)data["GroupID"];
                    internalGroup.GroupName = data["GroupName"].ToString();
                    internalGroup.Subscribers = (int)data["Subscribers"];
                    internalGroup.IsSeedList = Convert.ToBoolean(data["IsSeedList"]);
                    internalGroup.TotalCount = (int)data["TotalCount"];
                    list.Add(internalGroup);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static List<ecn.communicator.mvc.Models.Email> DataTableToListEmails(this DataTable table)
        {
            try
            {
                List<ecn.communicator.mvc.Models.Email> list = new List<ecn.communicator.mvc.Models.Email>();
                DateTime dummyDate = new DateTime();
                int dummyInt = 0;
                foreach (var data in table.AsEnumerable())
                {
                    ecn.communicator.mvc.Models.Email internalEmail = new ecn.communicator.mvc.Models.Email();
                    internalEmail.Address = data["Address"] != null ? data["Address"].ToString() : "";
                    internalEmail.Address2 = data["Address2"] != null ? data["Address2"].ToString() : "";
                    internalEmail.Age = data["Age"] != null ? data["Age"].ToString() : "";
                    internalEmail.Birthdate = data["BirthDate"] != null ? data["BirthDate"].ToString() : "";
                    internalEmail.BounceScore = data["BounceScore"] != null && data["BounceScore"].ToString().Length > 0  ? Convert.ToInt32(data["BounceScore"].ToString()) : 0;
                    internalEmail.CarrierCode = data["CarrierCode"] != null ? data["CarrierCode"].ToString() : "";
                    internalEmail.City = data["City"] != null ? data["City"].ToString() : "";
                    internalEmail.Company = data["Company"] != null ? data["Company"].ToString() : "";
                    internalEmail.Country = data["Country"] != null ? data["Country"].ToString() : "";
                    internalEmail.CustomerID = data["CustomerID"] != null ? Convert.ToInt32(data["CustomerID"].ToString()) : -1;
                    internalEmail.DateAdded = data["DateAdded"] != null ? DateTime.Parse(data["DateAdded"].ToString()) : new DateTime();
                    internalEmail.DateUpdated = data["DateUpdated"] != null && DateTime.TryParse(data["DateUpdated"].ToString(), out dummyDate) ? dummyDate: new DateTime();
                    internalEmail.EmailAddress = data["EmailAddress"] != null ? data["EmailAddress"].ToString() : "";
                    internalEmail.EmailID = data["EmailID"] != null ? Convert.ToInt32(data["EmailID"].ToString()) : -1;
                    internalEmail.Fax = data["Fax"] != null ? data["Fax"].ToString() : "";
                    internalEmail.FirstName = data["FirstName"] != null ? data["FirstName"].ToString() : "";
                    internalEmail.FormatTypeCode = data["FormatTypeCode"] != null ? data["FormatTypeCode"].ToString() : "";
                    internalEmail.FullName = data["FullName"] != null ? data["FullName"].ToString() : "";
                    internalEmail.Gender = data["Gender"] != null ? data["Gender"].ToString() : "";
                    internalEmail.Income = data["Income"] != null ? data["Income"].ToString() : "";
                    internalEmail.LastName = data["LastName"] != null ? data["LastName"].ToString() : "";
                    internalEmail.Mobile = data["Mobile"] != null ? data["Mobile"].ToString() : "";
                    internalEmail.Notes = data["Notes"] != null ? data["Notes"].ToString() : "";
                    internalEmail.Occupation = data["Occupation"] != null ? data["Occupation"].ToString() : "";
                    internalEmail.Password = data["Password"] != null ? data["Password"].ToString() : "";
                    internalEmail.SMSOptIn = data["SMSOptIn"] != null ? data["SMSOptIn"].ToString() : "";
                    internalEmail.SoftBounceScore = data["SoftBounceScore"] != null && int.TryParse(data["SoftBounceScore"].ToString(), out dummyInt) ? Convert.ToInt32(data["SoftBounceScore"].ToString()) : 0;
                    internalEmail.State = data["State"] != null ? data["State"].ToString() : "";
                    internalEmail.SubscribeTypeCode = data["SubscribeTypeCode"] != null ? data["SubscribeTypeCode"].ToString() : "";
                    internalEmail.Title = data["Title"] != null ? data["Title"].ToString() : "";
                    internalEmail.User1 = data["User1"] != null ? data["User1"].ToString() : "";
                    internalEmail.User2 = data["User2"] != null ? data["User2"].ToString() : "";
                    internalEmail.User3 = data["User3"] != null ? data["User3"].ToString() : "";
                    internalEmail.User4 = data["User4"] != null ? data["User4"].ToString() : "";
                    internalEmail.User5 = data["User5"] != null ? data["User5"].ToString() : "";
                    internalEmail.User6 = data["User6"] != null ? data["User6"].ToString() : "";
                    internalEmail.UserEvent1 = data["UserEvent1"] != null ? data["UserEvent1"].ToString() : "";
                    internalEmail.UserEvent1Date = data["UserEventDate1"] != null ? data["UserEventDate1"].ToString() : "";
                    internalEmail.UserEvent2 = data["UserEvent2"] != null ? data["UserEvent2"].ToString() : "";
                    internalEmail.UserEvent2Date = data["UserEvent2Date"] != null ? data["UserEvent2Date"].ToString() : "";
                    internalEmail.Voice = data["Voice"] != null ? data["Voice"].ToString() : "";
                    internalEmail.Website = data["Website"] != null ? data["Website"].ToString() : "";
                    internalEmail.Zip = data["Zip"] != null ? data["Zip"].ToString() : "";
                    internalEmail.TotalCount = data["TotalCount"] != null && int.TryParse(data["TotalCount"].ToString(), out dummyInt) ? dummyInt : 0;
                    list.Add(internalEmail);
                }

                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}