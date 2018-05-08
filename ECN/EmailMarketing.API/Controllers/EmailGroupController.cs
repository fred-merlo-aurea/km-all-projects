using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http.Description;

using EmailMarketing.API.Models.EmailGroup;
using ECN_Framework_Common.Objects;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

using EmailMarketing.API.ExtentionMethods;
using KM.Common;
using Enums = ECN_Framework_Common.Objects.Enums;

namespace EmailMarketing.API.Controllers
{
    using ProfileInputTableMetaData = Tuple<String, SqlDbType, int?, Action<int, SqlDataRecord, Profile>>;

    /// <summary>
    /// API methods exposing email group subscription management
    /// </summary>
    [RoutePrefix("api/emailgroup")]
    public class EmailGroupController : AuthenticatedUserControllerBase
    {
        /// <summary>
        /// When false, a request to manage profiles specifying more than a single group will raise an exception
        /// </summary>
        public const bool ALLOW_MULTIGROUP_PROFILE_MANAGEMENT = false;
        private const string ACTION_COLUMN_NAME = "Action";
        private const string COUNTS_COLUMN_NAME = "Counts";
        private const int EMAIL_PROFILES_IMPORT_BATCH_SIZE = 5000;


        #region abstract member implementations

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.EmailGroup; }
        }

        override public string ControllerName { get { return "emailgroup"; } }

        #endregion abstract member implementations
        #region Manage Subscribers (without profile)
        /// <summary>
        /// Add one more email addresses to one or more groups.
        /// </summary>
        /// <param name="emailList">list of subscription email directives</param>
        /// <returns></returns>
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/emailgroup/methods/ManageSubscribers HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// APIAccessKey: <YOUR-API-KEY-HERE>
        /// X-Customer-ID: 77777
        /// Host: api.ecn5.com
        /// Content-Length: 344
        ///
        /// [
        ///    {
        ///       "EmailAddress":'foo@bar.com',
        ///       "GroupID": 99999,
        ///       "Format": "HTML",
        ///       "SubscribeType": "Unsubscribe"
        ///    },
        ///    {
        ///       "EmailAddress":'foo@bar.com',
        ///       "GroupID": 88888,
        ///       "Format": "HTML",
        ///       "SubscribeType": "Subscribe"
        ///    },
        ///    {
        ///       "EmailAddress":'baz@qwx.com',
        ///       "GroupID": 77777,
        ///       "Format": "HTML",
        ///       "SubscribeType": "Unsubscribe"
        ///    }
        /// ]
        /// ]]></example>
        ///<example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Fri, 24 Jul 2015 15:53:51 GMT
        /// Content-Length: 386
        ///
        /// [
        ///    {
        ///       "GroupID":7777,
        ///       "EmailID":null,
        ///       "EmailAddress":"baz@qwx.com",
        ///       "Status":"None",
        ///       "Result":"Skipped, UnknownSubscriber"
        ///    },
        ///    {
        ///       "GroupID":88888,
        ///       "EmailID":286227507,
        ///       "EmailAddress":"foo@bar.com",
        ///       "Status":"S",
        ///       "Result":"New, Subscribed"
        ///    },
        ///    {
        ///       "GroupID":99999,
        ///       "EmailID":null,
        ///       "EmailAddress":"foo@bar.com",
        ///       "Status":"U",
        ///       "Result":"Updated, Unsubscribed"
        ///    }
        /// ]
        /// ]]></example>
        //[HttpPost]
        //[Route("methods/ManageSubscribers")]
        ////public DataTable AddSubscriber([FromBody] IEnumerable<Email> emailList)
        //public IEnumerable<SubscriptionResult> ManageSubscribers([FromBody] List<Email> emailList)
        //{
        //    Validate(emailList);
        //    IEnumerable<SubscriptionResult> results =
        //        ManageSubscribers(APIUser.UserID, APICustomer.CustomerID, emailList);
        //    TriggerGroupEvent(results);
        //    return results;
        //}
        #endregion Manage Subscribers (without profile)
        #region Manage Subscribers With Profile

        /// <summary>
        ///Add one or more subscriber profiles (Email Address, Name, Custom Fields, etc.) to a single group.  If the Email Address already exists the profile data will be updated.
        ///<b>Note: To update an Email Address please use the UpdateEmailAddress method.</b>
        /// </summary>
        /// <param name="profileList"></param>
        /// <returns></returns>
        ///<example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/emailgroup/methods/ManageSubscriberWithProfile HTTP/1.1
        /// Accept: application/json
        /// Content-type: application/json
        /// APIAccessKey: <YOUR-API-KEY-HERE>
        /// X-Customer-ID: 77777
        /// Host: api.ecn5.com
        /// Content-Length: 1352
        ///
        ///     {
        ///         "GroupID":123456,
        ///         "Format":"HTML",
        ///         "SubscribeType":"U",
        ///         "Profiles":[
        ///         {
        ///             "EmailAddress":"someone@nowhere.com",
        ///             "Title":"Mr",
        ///             "FirstName":"Som",
        ///             "LastName":"eone",
        ///             "FullName":"Som Eone",
        ///             "Company":"A Fine Place",
        ///             "Occupation":"Stuff and things",
        ///             "Address":"123 My Street",
        ///             "Address2":"Schweet Sweet Suite",
        ///             "City":"A Little Place",
        ///             "State":"Confused",
        ///             "Zip":"90210",
        ///             "Country":"UWF",
        ///             "Voice":"612.555.1212",
        ///             "Mobile":"651.555.1212",
        ///             "Fax":"952.555.1212",
        ///             "Website":"http://www.ecn5.com",
        ///             "Age":"18",
        ///             "Income":"19",
        ///             "Gender":"20",
        ///             "User1":"sample string 21",
        ///             "User2":"sample string 22",
        ///             "User3":"sample string 23",
        ///             "User4":"sample string 24",
        ///             "User5":"sample string 25",
        ///             "User6":"sample string 26",
        ///             "UserEvent1":"sample string 27",
        ///             "UserEvent1Date":"2015-06-16",
        ///             "UserEvent2":"sample string 28",
        ///             "UserEvent2Date":"2015-06-17",
        ///             "Birthdate":"1915-06-18",
        ///             "Notes":"a note can be placed here",
        ///             "Password":"a password could be stored here",
        ///             "SubscribeType": "S",
        ///             "Format":"HTML",
        ///             "CustomFields": [
        ///             {
        ///                 "Name": "sample string 1",
        ///                 "Value": "sample string 2"
        ///             },
        ///             {
        ///                 "Name": "sample string 1",
        ///                 "Value": "sample string 2"
        ///             }
        ///             ]
        ///          },
        ///          {
        ///             "EmailAddress":"testing@nowhere.com",
        ///             "Title":"Mr",
        ///             "FirstName":"Som",
        ///             "LastName":"eone",
        ///             "FullName":"Som Eone",
        ///             "Company":"A Fine Place",
        ///             "Occupation":"Stuff and things",
        ///             "Address":"123 My Street",
        ///             "Address2":"Schweet Sweet Suite",
        ///             "City":"A Little Place",
        ///             "State":"Confused",
        ///             "Zip":"90210",
        ///             "Country":"UWF",
        ///             "Voice":"612.555.1212",
        ///             "Mobile":"651.555.1212",
        ///             "Fax":"952.555.1212",
        ///             "Website":"http://www.ecn5.com",
        ///             "Age":"18",
        ///             "Income":"19",
        ///             "Gender":"20",
        ///             "User1":"sample string 21",
        ///             "User2":"sample string 22",
        ///             "User3":"sample string 23",
        ///             "User4":"sample string 24",
        ///             "User5":"sample string 25",
        ///             "User6":"sample string 26",
        ///             "UserEvent1":"sample string 27",
        ///             "UserEvent1Date":"2015-06-16",
        ///             "UserEvent2":"sample string 28",
        ///             "UserEvent2Date":"2015-06-17",
        ///             "Birthdate":"1915-06-18",
        ///             "Notes":"a note can be placed here",
        ///             "Password":"a password could be stored here",
        ///             "SubscribeType":"S",
        ///             "Format":"HTML",
        ///             "CustomFields": [
        ///             {
        ///                 "Name": "sample string 1",
        ///                 "Value": "sample string 2"
        ///             },
        ///             {
        ///                 "Name": "sample string 1",
        ///                 "Value": "sample string 2"
        ///             }
        ///             ]
        ///         }
        ///         ]
        ///     }
        ///]]></example>
        ///<example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Fri, 19 Jun 2015 22:09:31 GMT
        /// Content-Length: 117
        ///
        /// {
        ///    "New":2,
        ///    "Updated":0,
        ///    "Skipped":0,
        ///    "Duplicate":0,
        ///    "MasterSuppressed":0,
        ///    "Total":2
        /// }
        /// ]]></example>
        [HttpPost]
        [Route("methods/ManageSubscriberWithProfile")]
        public ImportResult ManageSubscribersWithProfile([FromBody] ManageProfile profileList)
        {
            SubscribeTypes st = SubscribeTypes.S;
            Formats f = Formats.HTML;
            if (profileList == null)
            {
                RaiseInvalidMessageException("No model in request body");
            }
            else if (string.IsNullOrEmpty(GetSubscribeTypeCode(profileList.SubscribeType)))//!Enum.TryParse<SubscribeTypes>(profileList.SubscribeType.ToString(), out st))
            {
                if (string.IsNullOrEmpty(profileList.SubscribeType.ToString()) || profileList.SubscribeType.ToString().Equals("0"))
                    RaiseInvalidMessageException("SubscribeType is missing");
                else
                    RaiseInvalidMessageException("SubscribeType is invalid");
            }
            else if (!Enum.TryParse<Formats>(profileList.Format.ToString(), out f))
            {
                RaiseInvalidMessageException("Format is invalid");
            }
            else if (profileList.Profiles == null)
            {
                RaiseInvalidMessageException("No Profiles in request body");
            }

            string subscribeTypeCode = GetSubscribeTypeCode(profileList.SubscribeType);
            if (subscribeTypeCode != "S" && subscribeTypeCode != "U" && subscribeTypeCode != "P")
            {
                profileList.SubscribeType = SubscribeTypes.S;
            }

            if ((profileList.GroupID == null || profileList.GroupID == 0) || !ECN_Framework_BusinessLayer.Communicator.Group.Exists(profileList.GroupID, APICustomer.CustomerID))
            {
                RaiseInvalidMessageException("Group ID is invalid");
            }
            Validate(profileList);
            ImportResult results =
                ManageSubscribersWithProfile(APIUser.UserID, CustomerID, profileList, APIUser);
            List<ECN_Framework_Entities.Communicator.LayoutPlans> groupTrigger = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByGroupID(profileList.GroupID, APICustomer.CustomerID, APIUser);
            if (groupTrigger != null && groupTrigger.Count(x => x.Status.ToUpper().Equals("Y")) > 0)
            {
                TriggerGroupEvent_ManageSubscriberWithProfile(profileList);
            }
            return results;
        }

        /// <summary>
        /// - This is the same as the existing ManageSubscriberWithProfile with an additional parameter of TriggerID.
        /// - This TriggerID needs to make it all the way to the Eventer as an optional parameter.If it is passed to the Eventer we would check to see if we have that in the database as a LayoutPlanID.If we do, we use that trigger.  If we don't we follow the normal path and look for any other triggers on that group.
        /// - This method should not appear in the online documentation.
        /// - This method replaces the old AddSubscriberUsingSmartForm.
        /// </summary>
        /// <param name="profileList"></param>
        /// <param name="TriggerID"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("methods/ManageSubscriberWithProfileAndTrigger")]
        public ImportResult ManageSubscriberWithProfileAndTrigger([FromBody] ManageProfileAndTrigger profileList)
        {
            SubscribeTypes st = SubscribeTypes.S;
            Formats f = Formats.HTML;
            if (profileList == null)
                RaiseInvalidMessageException("No model in request body");
            else if (string.IsNullOrEmpty(GetSubscribeTypeCode(profileList.SubscribeType)))//!Enum.TryParse<SubscribeTypes>(profileList.SubscribeType.ToString(), out st))
                if (string.IsNullOrEmpty(profileList.SubscribeType.ToString()) || profileList.SubscribeType.ToString().Equals("0"))
                    RaiseInvalidMessageException("SubscribeType is missing");
                else
                    RaiseInvalidMessageException("SubscribeType is invalid");
            else if (!Enum.TryParse<Formats>(profileList.Format.ToString(), out f))
                RaiseInvalidMessageException("Format is invalid");
            else if (profileList.Profiles == null)
                RaiseInvalidMessageException("No Profiles in request body");
            else if ((profileList.GroupID == null || profileList.GroupID == 0) || !ECN_Framework_BusinessLayer.Communicator.Group.Exists(profileList.GroupID, APICustomer.CustomerID))
                RaiseInvalidMessageException("Group ID is invalid");
            else if (profileList.TriggerID == null || profileList.TriggerID == 0)
                RaiseInvalidMessageException("Trigger ID is invalid");

            string subscribeTypeCode = GetSubscribeTypeCode(profileList.SubscribeType);
            if (subscribeTypeCode != "S" && subscribeTypeCode != "U" && subscribeTypeCode != "P")
                profileList.SubscribeType = SubscribeTypes.S;

            Validate(profileList);
            ImportResult results = ManageSubscribersWithProfile(APIUser.UserID, CustomerID, profileList, APIUser);
            ECN_Framework_Entities.Communicator.LayoutPlans groupTrigger = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(profileList.TriggerID, APIUser);
            if (groupTrigger != null && groupTrigger.Status.ToUpper().Equals("Y"))
                TriggerGroupEvent_ManageSubscriberWithProfileAndTrigger(profileList,"", groupTrigger);

            return results;
        }

        /// <summary>
        /// - This is the same as the existing ManageSubscriberWithProfile with an additional parameter of TriggerID.  It also must incorporate any logic necessary from the old web service we are replacing.
        /// - This TriggerID needs to make it all the way to the Eventer as an optional parameter.If it is passed to the Eventer we would check to see if we have that in the database as a LayoutPlanID.If we do, we use that trigger.  If we don't we follow the normal path and look for any other triggers on that group.
        /// - This method should not appear in the online documentation.
        /// - This method replaces the old AddSubscriberWithDupesUsingSmartForm.
        /// </summary>
        /// <param name="profileList"></param>
        /// <param name="TriggerID"></param>
        /// <param name="compositeKey"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("methods/ManageSubscriberWithDupeAndTrigger")]
        public ImportResult ManageSubscriberWithDupeAndTrigger([FromBody] ManageProfileWithDupeAndTrigger profileList)
        {
            SubscribeTypes st = SubscribeTypes.S;
            Formats f = Formats.HTML;
            if (profileList == null)
                RaiseInvalidMessageException("No model in request body");
            else if (string.IsNullOrEmpty(GetSubscribeTypeCode(profileList.SubscribeType)))//!Enum.TryParse<SubscribeTypes>(profileList.SubscribeType.ToString(), out st))
                if (string.IsNullOrEmpty(profileList.SubscribeType.ToString()) || profileList.SubscribeType.ToString().Equals("0"))
                    RaiseInvalidMessageException("SubscribeType is missing");
                else
                    RaiseInvalidMessageException("SubscribeType is invalid");
            else if (!Enum.TryParse<Formats>(profileList.Format.ToString(), out f))
                RaiseInvalidMessageException("Format is invalid");
            else if (profileList.Profiles == null)
                RaiseInvalidMessageException("No Profiles in request body");
            else if ((profileList.GroupID == null || profileList.GroupID == 0) || !ECN_Framework_BusinessLayer.Communicator.Group.Exists(profileList.GroupID, APICustomer.CustomerID))
                RaiseInvalidMessageException("Group ID is invalid");
            else if (profileList.TriggerID == null || profileList.TriggerID == 0)
                RaiseInvalidMessageException("Trigger ID is invalid");
            else if (profileList.CompositeKey == null ||( profileList.CompositeKey.ToLower() != "user1" && profileList.CompositeKey.ToLower() != "user2" && profileList.CompositeKey.ToLower() != "user3" && profileList.CompositeKey.ToLower() != "user4" && profileList.CompositeKey.ToLower() != "user5" && profileList.CompositeKey.ToLower() != "user6"))
                RaiseInvalidMessageException("INVALID COMPOSITE KEY");

            string subscribeTypeCode = GetSubscribeTypeCode(profileList.SubscribeType);
            if (subscribeTypeCode != "S" && subscribeTypeCode != "U" && subscribeTypeCode != "P")
                profileList.SubscribeType = SubscribeTypes.S;

            Validate(profileList);
            APIUser.CurrentClient = new KMPlatform.BusinessLogic.Client().Select(APICustomer.PlatformClientID, true);
            ImportResult results = ManageSubscribersWithProfileWithDupes(APIUser.UserID, CustomerID, profileList, APIUser, profileList.CompositeKey);
            ECN_Framework_Entities.Communicator.LayoutPlans groupTrigger = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(profileList.TriggerID, APIUser);
            if (groupTrigger != null && groupTrigger.Status.ToUpper().Equals("Y"))
                TriggerGroupEvent_ManageSubscriberWithProfileAndTrigger(profileList, profileList.CompositeKey, groupTrigger);

            return results;
        }

        #endregion Manage Subscribers With Profile
        #region Update Email Address

        /// <summary>
        ///Updates a subscriber’s Email Address and profile data across the entire customer account.
        ///<b>Note: Under normal circumstances please use UpdateEmailAddress to update a subscriber’s Email Address or ManageSubscriberWithProfile to update a subscriber’s profile data and check with your Digital Specialist before using this method as it may have unintended results.</b>
        /// </summary>
        /// <param name="profileList">list of subscriber profiles requiring change of email address</param>
        /// <returns>subscription result</returns>
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/emailgroup/methods/UpdateEmailAddressForGroup HTTP/1.1
        /// Host: api.ecn5.com
        /// Content-Length: 531
        /// Content-type: application/json
        /// Accept: application/json
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        ///
        ///
        ///  {
        ///     "OldEmailAddress": "sample string 1",
        ///     "GroupID": 2,
        ///     "EmailAddress": "sample string 3",
        ///     "Title": "sample string 4",
        ///     "FirstName": "sample string 5",
        ///     "LastName": "sample string 6",
        ///     "FullName": "sample string 7",
        ///     "Company": "sample string 8",
        ///     "Occupation": "sample string 9",
        ///     "Address": "sample string 10",
        ///     "Address2": "sample string 11",
        ///     "City": "sample string 12",
        ///     "State": "sample string 13",
        ///     "Zip": "sample string 14",
        ///     "Country": "sample string 15",
        ///     "Voice": "sample string 16",
        ///     "Mobile": "sample string 17",
        ///     "Fax": "sample string 18",
        ///     "Website": "sample string 19",
        ///     "Age": "sample string 20",
        ///     "Income": "sample string 21",
        ///     "Gender": "sample string 22",
        ///     "User1": "sample string 23",
        ///     "User2": "sample string 24",
        ///     "User3": "sample string 25",
        ///     "User4": "sample string 26",
        ///     "User5": "sample string 27",
        ///     "User6": "sample string 28",
        ///     "Birthdate": "2015-12-10T11:47:32.0098991-06:00",
        ///     "UserEvent1": "sample string 29",
        ///     "UserEvent1Date": "2015-12-10T11:47:32.0098991-06:00",
        ///     "UserEvent2": "sample string 30",
        ///     "UserEvent2Date": "2015-12-10T11:47:32.0098991-06:00",
        ///     "Notes": "sample string 31",
        ///     "Password": "sample string 32",
        ///     "SubscribeType": "Subscribe",
        ///     "Format": "HTML",
        ///     "CustomFields": [
        ///     {
        ///         "Name": "sample string 1",
        ///         "Value": "sample string 2"
        ///     },
        ///     {
        ///         "Name": "sample string 1",
        ///         "Value": "sample string 2"
        ///     }
        ///     ]
        ///  }
        /// ]]></example>
        ///<example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 03 Nov 2015 17:55:43 GMT
        /// Connection: close
        /// Content-Length: 124
        ///
        /// {
        ///    "New":1,
        ///    "Updated":0,
        ///    "Skipped":0,
        ///    "Duplicate":0,
        ///    "MasterSuppressed":0,
        ///    "Total":1
        /// }
        /// ]]></example>
        [HttpPost]
        [Route("methods/UpdateEmailAddressForGroup")]
        public ImportResult UpdateEmailAddressForGroup([FromBody] UpdateEmailAddressForGroup profileList)
        {
            if (profileList == null)
                RaiseEcnValidationException("no model in request body");
            else if (profileList.GroupID == null || profileList.GroupID == 0)
                RaiseEcnValidationException("GroupID not found");
            else if (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(profileList.EmailAddress))
                RaiseEcnValidationException("Email Address is not valid");
            else if (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(profileList.OldEmailAddress))
                RaiseEcnValidationException("Old Email Address is not valid");

            ECN_Framework_Entities.Communicator.Group MSGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(APICustomer.CustomerID, APIUser);
            ECN_Framework_Entities.Communicator.EmailGroup msEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(profileList.EmailAddress, MSGroup.GroupID, APIUser);
            if (msEmailGroup != null && msEmailGroup.CustomerID.HasValue)
            {
                RaiseEcnValidationException("New email address is Master Suppressed. Updating is not allowed");
            }
            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList_List =
                ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(APICustomer.BaseChannelID.Value, profileList.EmailAddress.Replace("'", "''"), APIUser);
            if (channelMasterSuppressionList_List.Count > 0)
            {
                RaiseEcnValidationException("New email address is Channel Master Suppressed. Updating is not allowed");
            }

            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalMasterSuppressionList_List =
                ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress(profileList.EmailAddress.Replace("'", "''"), APIUser);
            if (globalMasterSuppressionList_List.Count > 0)
            {
                RaiseEcnValidationException("New email address is Global Master Suppressed. Updating is not allowed");
            }
            Validate(profileList);

            if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(profileList.GroupID, APICustomer.CustomerID))
            {
                RaiseEcnValidationException("Group ID is invalid");
            }

            ImportResult results = new ImportResult();

            if (ECN_Framework_BusinessLayer.Communicator.Email.ExistsByGroup(profileList.EmailAddress.Trim(), profileList.GroupID))
            {
                results.Duplicate += 1;
                results.Total += 1;
                return results;
            }
            if (!ECN_Framework_BusinessLayer.Communicator.Email.ExistsByGroup(profileList.OldEmailAddress.Trim(), profileList.GroupID))
            {
                results.Skipped += 1;
                results.Total += 1;
                return results;
            }

            ECN_Framework_BusinessLayer.Communicator.Email.
                UpdateEmailAddress(profileList.GroupID, APICustomer.CustomerID, profileList.EmailAddress, profileList.OldEmailAddress, APIUser, "EmailMarketing.API.EmailGroupController.UpdateEmailAddressForGroup");

            results = ManageSubscribers(APIUser.UserID, APICustomer.CustomerID, profileList, APIUser);
            List<ECN_Framework_Entities.Communicator.LayoutPlans> groupTrigger = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByGroupID(profileList.GroupID, APICustomer.CustomerID, APIUser);
            if (groupTrigger != null && groupTrigger.Count(x => x.Status.ToUpper().Equals("Y")) > 0)
            {
                TriggerGroupEvent(profileList.GroupID, profileList);
            }
            return results;
        }

        /// <summary>
        ///Updates a subscriber’s Email Address by Master Suppressing the old Email Address and creating a new Email Address record with the old profile data.
        ///<b>Note: This functions the same as the Email Marketing web site with the exception that this update is limited in scope to the Customer as opposed to the Base Channel.
        ///When you submit the request an email is sent to the old email address asking them to confirm the change to their new email address. Until the recipient verifies the change the email address will not be updated in ECN.</b>
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/emailgroup/methods/UpdateEmailAddress HTTP/1.0
        /// Host: api.ecn5.com
        /// Content-Length: 531
        /// Content-type: application/json
        /// Accept: application/json
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        ///
        ///     {
        ///         "OldEmailAddress": "prior@email-address.com",
        ///         "NewEmailAddress": "new@email-address.com"
        ///     }
        /// ]]></example>
        ///
        ///<example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 03 Nov 2015 17:55:43 GMT
        /// Connection: close
        /// Content-Length: 124
        ///
        ///
        ///    {
        ///         "New":1,
        ///         "Updated":0,
        ///         "Skipped":0,
        ///         "MasterSuppressed":0,
        ///         "Duplicate":0,
        ///         "Total":1
        ///    }
        ///
        /// ]]></example>
        [HttpPost]
        [Route("methods/UpdateEmailAddress")]
        public ImportResult UpdateEmailAddress([FromBody] UpdateEmailAddress update)
        {
            if (update == null)
            {
                RaiseInvalidMessageException("no model in request body");
            }
            ImportResult result = new ImportResult();

            if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(update.OldEmailAddress.Trim()))
            {
                if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(update.NewEmailAddress.Trim()))
                {
                    if (ECN_Framework_BusinessLayer.Communicator.Email.Exists(update.OldEmailAddress.Trim(), APICustomer.CustomerID))
                    {
                        ECN_Framework_Entities.Communicator.Group MSGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(APICustomer.CustomerID, APIUser);
                        ECN_Framework_Entities.Communicator.EmailGroup msEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(update.NewEmailAddress.Trim(), MSGroup.GroupID, APIUser);
                        if (msEmailGroup != null && msEmailGroup.CustomerID.HasValue)
                        {
                            RaiseEcnValidationException("New email address is Master Suppressed. Updating is not allowed");
                        }

                        List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList_List =
                            ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(APICustomer.BaseChannelID.Value, update.NewEmailAddress.Trim().Replace("'", "''"), APIUser);
                        if (channelMasterSuppressionList_List.Count > 0)
                        {
                            RaiseEcnValidationException("New email address is Channel Master Suppressed. Updating is not allowed");
                        }

                        List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalMasterSuppressionList_List =
                            ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress(update.NewEmailAddress.Trim().Replace("'", "''"), APIUser);
                        if (globalMasterSuppressionList_List.Count > 0)
                        {
                            RaiseEcnValidationException("New email address is Global Master Suppressed. Updating is not allowed");
                        }

                        result = UpdateEmailAddressDblOptIn(update);
                    }
                    else
                    {
                        RaiseInvalidMessageException("Old Email Address does not exist");
                    }
                }
                else
                {
                    RaiseInvalidMessageException("New Email Address is not valid");
                }
            }
            else
            {
                RaiseInvalidMessageException("Old Email Address is not valid");
            }

            return result;
        }

        #endregion Update Email Address
        #region Get Subscriber Count By Group ID - GetSubscriberCount()

        /// <summary>
        /// Provides the number of subscribers associated with a given group.
        /// </summary>
        /// <param name="id">Group ID</param>
        /// <returns>the number of subscribers as an integer</returns>
        /// <example for="request"><![CDATA[
        /// GET /api/emailgroup/456789/Count HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        ///
        /// ]]></example>
        ///
        /// <example for="response"><![CDATA[
        ///HTTP/1.1 200 OK
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Mon, 10 Aug 2015 17:15:33 GMT
        ///Content-Length: 1
        ///
        ///{
        /// "Result": 4
        ///}
        /// ]]></example>
        [Route("{id}/Count")]
        [HttpGet]
        public Models.IntResult CountSubscribers(int id)
        {
            try
            {
                List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> listFilters = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
                System.Data.DataTable dt =
                    ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastEmailListForDynamicContent(CustomerID, 0, id, listFilters, "", "", "", true, true);
                int returnValue = Convert.ToInt32(dt.Rows[0][0].ToString());
                return new Models.IntResult() { Result = returnValue };
            }
            catch
            {
                RaiseNotFoundException(id);
            }

            return new Models.IntResult() { Result = -1 };
        }

        #endregion Get Subscriber Count By Group ID - GetSubscriberCount()
        #region Add To Master Suppression List

        /// <summary>
        /// Add subscribers to the master suppression list.
        /// </summary>
        /// <param name="emailAddresses">email addresses to be master suppressed, specified in the request body a list strings</param>
        /// <example for="request"><![CDATA[
        /// POST /api/emailGroup/methods/MasterSuppress HTTP/1.1
        /// Content-Type: application/json
        /// Accept: application/json
        /// APIAccessKey: <PUT-YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 55555
        /// Host: api.ecn5.com
        /// Content-Length: 28
        ///
        /// [ "foo-suppressed@bar.com" ]
        /// ]]></example>
        /// <example for="response">
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Wed, 02 Sep 2015 23:35:37 GMT
        /// Content-Length: 119
        /// [{"GroupID":93708,"EmailID":303284124,"EmailAddress":"foo-suppressed@bar.com","Status":"S","Result":"New, Subscribed"}]
        /// </example>
        [Route("methods/MasterSuppress")]
        [HttpPost]
        public IEnumerable<SubscriptionResult> AddToMasterSuppressionList([FromBody] string[] emailAddresses)
        {
            if (emailAddresses == null || emailAddresses.Count() < 1)
            {
                RaiseInvalidMessageException("no subscribers");
            }

            ECN_Framework_Entities.Communicator.Group masterSuppressionGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(APICustomer.CustomerID, APIUser);
            if (masterSuppressionGroup == null || 1 > masterSuppressionGroup.GroupID)
            {
                RaiseInternalServerError("no master suppression group for customer " + APICustomer.CustomerID);
            }

            StringBuilder sbProfile = new StringBuilder();


            foreach (string s in emailAddresses)
            {
                sbProfile.Append("<Emails>");
                sbProfile.Append("<emailaddress>" + s + "</emailaddress>");
                sbProfile.Append("</Emails>");
            }


            DataTable dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(APIUser, APICustomer.CustomerID,masterSuppressionGroup.GroupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + sbProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", "HTML", "M", false, "", "EmailMarketing.API.Controllers.EmailGroupController.AddToMasterSuppressionList");


            return ManageSubscribers(APIUser.UserID, APICustomer.CustomerID,
                    from x in emailAddresses
                    select new Email()
                    {
                        EmailAddress = x,
                        GroupId = masterSuppressionGroup.GroupID,
                        Format = Formats.HTML,
                        SubscribeType = SubscribeTypes.Subscribe
                    });
        }
        #endregion Add To Master Suppression List
        #region Delete Subscriber

        // really a rest, method....   the only one?
        /// <summary>
        /// Delete a subscriber from an Email Group by Email Address
        /// </summary>
        /// <param name="id">an Email Group ID</param>
        /// <param name="emailAddress">a subscriber Email Address</param>
        /// <example for="request"><![CDATA[
        /// DELETE http://api.ecn5.com/api/emailGroup/49195/DeleteByEmailAddress HTTP/1.1
        /// Content-Type: application/json
        /// Accept: application/json
        /// APIAccessKey: <PUT-YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 55555
        /// Host: api.ecn5.com
        /// Content-Length: 23
        ///
        /// "someone@somewhere.com"
        /// ]]></example>
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 204 No Content
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Thu, 03 Sep 2015 00:22:24 GMT
        ///
        /// ]]></example>
        [HttpDelete]
        [Route("{id}/DeleteByEmailAddress")]
        public void DeleteSubscriber(int id, [FromBody] string emailAddress)
        {
            if (String.IsNullOrWhiteSpace(emailAddress))
            {
                RaiseInvalidMessageException("email address is required");
            }
            APIUser.CustomerID = APICustomer.CustomerID;
            ECN_Framework_Entities.Communicator.EmailGroup emailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(emailAddress, id, APIUser);
            if (emailGroup == null)
            {
                RaiseNotFoundException(id, emailAddress);
            }
            else if (1 > emailGroup.EmailID)
            {
                RaiseNotFoundException(-1, String.Format(@"Email ""{0}"" for group {1}", emailAddress, id));
            }


            ECN_Framework_BusinessLayer.Communicator.EmailGroup.Delete(id, emailGroup.EmailID, APIUser);
        }

        #endregion Delete Subscriber

        #region Best Profile For Email Address

        /// <summary>
        /// Get the profile information for a given email address within a customer for a specific group.
        /// If the email address is associated with the given group we will return standard profile fields
        /// as well as their relationship to the group and any UDF data.  If the email address exists within
        /// the customer but is not associated with the given group we will only return standard profile
        /// information.  If the email address is not associated with the customer we will return a null/empty
        /// result set.
        /// </summary>
        /// <param name="id">Email Group ID</param>
        /// <param name="emailAddress">Email Address</param>
        /// <returns>Profile Information as a collection of key/value pairs</returns>
        /// <summary>
        /// Get Profile information including UDF for subscribers with the given email address
        /// </summary>
        /// <param name="id">Email Group ID</param>
        /// <param name="emailAddress">Email Address</param>
        /// <returns>Profile Information as a collection of key/value pair sets for each subscriber</returns>
        /// <example for="request"><![CDATA[
        /// GET /api/emailGroup/123245/BestProfileForEmailAddress HTTP/1.1
        /// Content-Type: application/json
        /// Accept: application/json
        /// APIAccessKey: <PUT-YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 55555
        /// Host: api.ecn5.com
        /// Content-Length: 35
        ///
        /// "someone@somewhere.com"
        /// ]]></example>
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Wed, 02 Sep 2015 23:27:05 GMT
        /// Content-Length: 1224
        /// [
        ///    {
        ///       "EmailID":"123456789",
        ///       "EmailAddress":"someone@somewhere.com",
        ///       "Title":"RestTest1",
        ///       "FirstName":"Jim",
        ///       "LastName":"Bob",
        ///       "FullName":"Jim Bob",
        ///       "Company":"ABC",
        ///       "Occupation":"",
        ///       "Address":"",
        ///       "Address2":"",
        ///       "City":"",
        ///       "State":"",
        ///       "Zip":"",
        ///       "Country":"USA",
        ///       "Voice":"",
        ///       "Mobile":"",
        ///       "Fax":"",
        ///       "Website":"",
        ///       "Age":"",
        ///       "Income":"",
        ///       "Gender":"",
        ///       "User1":"a",
        ///       "User2":"125.25",
        ///       "User3":"",
        ///       "User4":"",
        ///       "User5":"",
        ///       "User6":"",
        ///       "Birthdate":"",
        ///       "UserEvent1":"",
        ///       "UserEvent1Date":"",
        ///       "UserEvent2":"",
        ///       "UserEvent2Date":"",
        ///       "password":"",
        ///       "CreatedOn":"2/2/2011 2:37:05 PM",
        ///       "LastChanged":"12/8/2014 12:47:41 PM",
        ///       "FormatTypeCode":"html",
        ///       "SubscribeTypeCode":"S",
        ///       "GroupID":"123245",
        ///       "SMSEnabled":"true",
        ///       "MyUDF":"a value for my custom field"
        ///    }
        /// ]
        /// /// ]]></example>
        [HttpGet]
        [HttpPost]
        [Route("{id}/BestProfileForEmailAddress")]
        public Dictionary<string, string> GetBestProfileForEmailAddress(int id, [FromBody] string emailAddress)
        {
            if (String.IsNullOrWhiteSpace(emailAddress))
            {
                RaiseInvalidMessageException("email address is required");
            }

            DataTable dt = ECN_Framework_BusinessLayer.Communicator.EmailGroup.
                GetBestProfileForEmailAddress(id, APICustomer.CustomerID, emailAddress);
            if (dt == null)
            {
                RaiseNotFoundException(-1, String.Format(@"Email ""{0}"" for group {1}", emailAddress, id));
            }

            bool existsInGivenGroup = false;
            string idString = id.ToString();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    if (row["GroupID"].ToString().Equals(idString, StringComparison.InvariantCultureIgnoreCase))
                    {  // include all, exclude list
                        return dt.ToSimpleDictionary(null, new string[] { "temp_EmailID", "entryID" }).FirstOrDefault();
                    }
                }
                catch { }
            }
            string[] includeFields = new string[] {
                            "EmailID","EmailAddress","Title","FirstName","LastName","FullName","Company","Occupation",
                            "Address","Address2","City","State","Zip","Country","Voice","Mobile","Fax","Website","Age",
                            "Income","Gender","User1","User2","User3","User4","User5","User6","Birthdate",
                            "UserEvent1","UserEvent1Date","UserEvent2","UserEvent2Date","password", "Notes"
                        };
            return dt.ToSimpleDictionary(includeFields).FirstOrDefault();
        }

        #endregion Best Profile For Email Address

        #region Get Email Profiles By Email Address

        /// <summary>
        /// Get Profile information including UDF for subscribers with the given email address
        /// </summary>
        /// <param name="id">Email Group ID</param>
        /// <param name="emailAddress">Email Address</param>
        /// <returns>Profile Information as a collection of key/value pair sets for each subscription of the given email address to the given group</returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/emailGroup/49195/ProfilesByEmailAddress HTTP/1.1
        /// Content-Type: application/json
        /// Accept: application/json
        /// APIAccessKey: <PUT-YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 55555
        /// Host: api.ecn5.com
        /// Content-Length: 35
        ///
        /// "someone@somewhere.com"
        /// ]]></example>
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Wed, 02 Sep 2015 23:27:05 GMT
        /// Content-Length: 1224
        ///    {
        ///       "EmailID":"123456789",
        ///       "EmailAddress":"someone@somewhere.com",
        ///       "Title":"RestTest1",
        ///       "FirstName":"Jim",
        ///       "LastName":"Bob",
        ///       "FullName":"Jim Bob",
        ///       "Company":"ABC",
        ///       "Occupation":"",
        ///       "Address":"",
        ///       "Address2":"",
        ///       "City":"",
        ///       "State":"",
        ///       "Zip":"",
        ///       "Country":"USA",
        ///       "Voice":"",
        ///       "Mobile":"",
        ///       "Fax":"",
        ///       "Website":"",
        ///       "Age":"",
        ///       "Income":"",
        ///       "Gender":"",
        ///       "User1":"a",
        ///       "User2":"125.25",
        ///       "User3":"",
        ///       "User4":"",
        ///       "User5":"",
        ///       "User6":"",
        ///       "Birthdate":"",
        ///       "UserEvent1":"",
        ///       "UserEvent1Date":"",
        ///       "UserEvent2":"",
        ///       "UserEvent2Date":"",
        ///       "password":"",
        ///       "CreatedOn":"2/2/2011 2:37:05 PM",
        ///       "LastChanged":"12/8/2014 12:47:41 PM",
        ///       "GroupID":"49195",
        ///       "FormatTypeCode":"html",
        ///       "SubscribeTypeCode":"S",
        ///       "MyUDF":"a value for my custom field"
        ///    }
        /// /// ]]></example>
        [HttpGet]
        [HttpPost]
        [Route("{id}/ProfilesByEmailAddress")]
        public IEnumerable<Dictionary<string, string>> GetProfilesByEmailAddress(int id, [FromBody] string emailAddress)
        {
            if (String.IsNullOrWhiteSpace(emailAddress))
            {
                RaiseInvalidMessageException("Email address is required");
            }

            if (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(emailAddress))
            {
                RaiseInvalidMessageException("Email address " + emailAddress.ToString() + "  is invalid");
            }

            DataTable dt = ECN_Framework_BusinessLayer.Communicator.EmailGroup.
                GetGroupEmailProfilesWithUDF(id, APICustomer.CustomerID,
                    " and emails.emailaddress = '" + emailAddress + "' ", "'S','P','U'");

            if (dt == null)
            {
                RaiseNotFoundException(-1, String.Format(@"Email ""{0}"" for group {1}", emailAddress, id));
            }

            return dt.ToSimpleDictionary(null, new string[] { "Notes", "temp_EmailID", "entryID" });
        }

        #endregion Get Email Profiles By Email Address
        #region Get Subscriber Status

        /// <summary>
        /// Get group subscription status
        /// </summary>
        /// <param name="emailAddress">Email Address</param>
        /// <returns>a list of subscriptions for <code>emailAddress</code> containing GroupID, GroupName and SubscribeTypeCode</returns>
        /// <example for="request"><![CDATA[
        /// GET /api/emailGroup/methods/StatusByEmailAddress HTTP/1.1
        /// Content-Type: application/json
        /// Accept: application/json
        /// APIAccessKey: <PUT-YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 55555
        /// Host: api.ecn5.com
        /// Content-Length: 26
        ///
        /// "someone@somwhere.com"
        /// ]]></example>
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Wed, 02 Sep 2015 20:06:57 GMT
        /// Content-Length: 169
        ///
        /// [
        ///    {
        ///       "GroupID":"123",
        ///       "GroupName":"First Group",
        ///       "SubscribeTypeCode":"S"
        ///    },
        ///    {
        ///       "GroupID":"456",
        ///       "GroupName":"Second Group",
        ///       "SubscribeTypeCode":"U"
        ///    }
        /// ]
        /// /// ]]></example>
        [Route("methods/StatusByEmailAddress")]
        [HttpGet]
        [HttpPost]

        public IEnumerable<Dictionary<string, string>> GetSubscriberStatus([FromBody] string emailAddress)
        {
            if (String.IsNullOrEmpty(emailAddress))
            {
                RaiseEcnValidationException("emailAddress is required");
            }

            DataTable dtemails = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetSubscriberStatus(emailAddress, APIUser);
            if (dtemails == null)
            {
                RaiseNotFoundException(-1, String.Format(@"Email ""{0}""", emailAddress));
            }

            return dtemails.ToSimpleDictionary(new string[] { "GroupID", "GroupName", "SubscribeTypeCode" });
        }

        #endregion Get Subscriber Status
        //*  allowing us to dump out the model during prototyping
        internal IEnumerable<Models.EmailGroup.Profile> GetSample()
        {
            return new List<Profile>
            {
               new Profile
               {
                   EmailAddress = "foo@bar.com"

               },
               new Profile
               {
                   EmailAddress = "baz@qwx.com"

               }
            };
        }
        //*/

        private ImportResult UpdateEmailAddressDblOptIn(UpdateEmailAddress update)
        {
            ImportResult ir = new ImportResult();
            StringBuilder sbEmail = new StringBuilder();
            ECN_Framework_Entities.Accounts.LandingPageAssign lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(APICustomer.BaseChannelID.Value).FirstOrDefault(x => x.LPID == 5);
            if ((lpa == null) || (lpa != null && lpa.LPAID > 0 && (!lpa.BaseChannelDoesOverride.HasValue || (lpa.BaseChannelDoesOverride.HasValue && !lpa.BaseChannelDoesOverride.Value))))
            {
                lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetDefault().Find(x => x.LPID == 5);
            }
            lpa.AssignContentList = ECN_Framework_BusinessLayer.Accounts.LandingPageAssignContent.GetByLPAID(lpa.LPAID);

            if (lpa.AssignContentList.Exists(x => x.LPOID == 25))
            {
                sbEmail.AppendLine(lpa.AssignContentList.FirstOrDefault(x => x.LPOID == 25).Display);
            }
            if (lpa.AssignContentList.Exists(x => x.LPOID == 27))
            {
                sbEmail.AppendLine(lpa.AssignContentList.FirstOrDefault(x => x.LPOID == 27).Display);
            }
            KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));

            string encryptedQuery = string.Empty;
            string queryString = string.Empty;

            queryString = "oldemail=" + update.OldEmailAddress.Trim() + "&newemail=" + update.NewEmailAddress.Trim() + "&c=" + APICustomer.CustomerID.ToString();

            encryptedQuery = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));

            sbEmail.AppendLine("</BR>Please confirm this change by clicking <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + "/engines/UpdateEmailAddress.aspx?" + encryptedQuery + "\">here</a>");
            if (lpa.AssignContentList.Exists(x => x.LPOID == 26))
            {
                sbEmail.AppendLine(lpa.AssignContentList.FirstOrDefault(x => x.LPOID == 26).Display);
            }

            ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
            ed.CustomerID = CustomerID;
            ed.FromName = "Web API";
            ed.Process = "Web API - UpdateEmailAddress";
            ed.Source = "Web API  - UpdateEmailAddress";
            ed.CreatedUserID = lpa.CreatedUserID.Value;

            ed.Content = sbEmail.ToString();
            ed.ReplyEmailAddress = lpa.AssignContentList.FirstOrDefault(x => x.LPOID == 28).Display;
            ed.EmailAddress = update.OldEmailAddress.Trim();
            ed.EmailSubject = lpa.AssignContentList.FirstOrDefault(x => x.LPOID == 29).Display;

            try
            {
                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
            catch (ECNException ecn)
            {
                RaiseEcnValidationException(ecn.Message);
            }
            ir.New = 1;
            ir.Total = 1;
            return ir;
        }

        private void TriggerGroupEvent(int groupID, UpdateEmailAddressForGroup profile)
        {

            ECN_Framework_Entities.Communicator.EmailGroup email = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(profile.EmailAddress, groupID, APIUser);
            if (email != null)
            {
                ECN_Framework_BusinessLayer.Communicator.EventOrganizer.Event(APICustomer.CustomerID, groupID, email.EmailID, APIUser, null);
            }
        }

        private void TriggerGroupEvent_ManageSubscriberWithProfile(ManageProfile results)
        {
            foreach (Profile p in results.Profiles)
            {
                ECN_Framework_Entities.Communicator.EmailGroup email = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(p.EmailAddress, results.GroupID, APIUser);
                if (email != null)
                {
                    ECN_Framework_BusinessLayer.Communicator.EventOrganizer.Event(APICustomer.CustomerID, results.GroupID, email.EmailID, APIUser, null);
                }
            }
        }

        private void TriggerGroupEvent_ManageSubscriberWithProfileAndTrigger(ManageProfile results, string compositeKey, ECN_Framework_Entities.Communicator.LayoutPlans groupTrigger)
        {
            if (!string.IsNullOrEmpty(compositeKey))
            {
                foreach (Profile p in results.Profiles)
                {
                    DataTable email = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetGroupEmailProfilesWithUDF(results.GroupID,
                                                                                                                       groupTrigger.CustomerID.Value,
                                                                                                                       " AND (EmailAddress = '" + p.EmailAddress + "' and " + compositeKey + " = '" + GetCompositeKeyValue(p, compositeKey) + "')",
                                                                                                                       "'S','U'",
                                                                                                                       "ProfileOnly");
                    if (email != null && email.Rows.Count > 0)
                    {
                        DataRow drEmail = email.Rows[0];
                        if (drEmail != null)
                        {
                            ECN_Framework_BusinessLayer.Communicator.EventOrganizer.Event(groupTrigger, Convert.ToInt32(drEmail["EmailID"].ToString()), APIUser);
                        }
                    }
                }
            }
            else
            {
                foreach (Profile p in results.Profiles)
                {
                    ECN_Framework_Entities.Communicator.EmailGroup email = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(p.EmailAddress, results.GroupID, APIUser);
                    if (email != null)
                    {
                        ECN_Framework_BusinessLayer.Communicator.EventOrganizer.Event(groupTrigger, email.EmailID, APIUser);
                    }
                }
            }
        }

        private static string GetCompositeKeyValue(Profile p, string compositeKey)
        {
            string value = "";
            switch (compositeKey.ToLower())
            {
                case "user1":
                    value = p.User1;
                    break;
                case "user2":
                    value = p.User2;
                    break;
                case "user3":
                    value = p.User3;
                    break;
                case "user4":
                    value = p.User4;
                    break;
                case "user5":
                    value = p.User5;
                    break;
                case "user6":
                    value = p.User6;
                    break;
            }
            return value;
        }

        #region items that belong in the Business Layer (validation)

        private static void RaiseEcnValidationException(string message)
        {
            throw new ECNException(
                new List<ECNError> {
                    new ECNError(Enums.Entity.SubscriptionManagement, Enums.Method.Validate, message) },
                Enums.ExceptionLayer.API);
        }

        public static void Validate(IEnumerable<IHasEmailAddress> emailList)
        {
            // 1. ensure we have some input
            if (null == emailList || 1 > emailList.Count())
            {
                RaiseEcnValidationException("no subscribers");
            }
            else if (emailList.Any(x => String.IsNullOrWhiteSpace(x.EmailAddress)))
            {
                int missingEmailAddressCount = emailList.Count(x => String.IsNullOrWhiteSpace(x.EmailAddress));
                string suffix = missingEmailAddressCount > 1 ? "s" : "";
                RaiseEcnValidationException(String.Format(
                    "email address is required (missing for {0:n0} subscriber{1})",
                    missingEmailAddressCount, suffix));
            }
        }

        private static void Validate(List<Profile> profileList)
        {
            // 1. delegate to the interface based method for general checks
            Validate((IEnumerable<IHasEmailAddress>)profileList);

            // 2. verify emailAddress are distinct
            if (profileList.GroupBy(x => new { x.EmailAddress }).Any(x => x.Count() > 1))
            {
                // duplicate actions for a given email address pairing
                var s = "Duplicate email address within a group: "
                    + String.Join(",",
                    profileList
                        .GroupBy(x => new { x.EmailAddress })
                        .Where(g => g.Count() > 1)
                        .Select(y => String.Format("{0}x{1}", y.Key.EmailAddress, y.Count()))
                        .ToList());
                RaiseEcnValidationException(s);
            }

            //// 3. explicitly disallow request to manipulate more than one group at once
            //if (false == ALLOW_MULTIGROUP_PROFILE_MANAGEMENT)
            //{
            //    bool hasMultupleGroups = profileList.Any(x => x.Groups.Count() != 1);
            //    bool hasDifferentGroups = profileList.GroupBy(x => x.Groups.FirstOrDefault().GroupID).Count() != 1;
            //    if (hasMultupleGroups || hasDifferentGroups)
            //    {
            //        RaiseEcnValidationException(Strings.Errors.MuligroupProfileManamangetUnsupported);
            //    }
            //}
        }

        private static void Validate(List<Email> emailList)
        {
            // 1. delegate to the interface based method for general checks
            Validate((IEnumerable<IHasEmailAddress>)emailList);

            // 2. verify emailAddress are distinct per group
            if (emailList.GroupBy(x => new { x.EmailAddress, x.GroupId }).Any(x => x.Count() > 1))
            {
                // duplicate actions for a given email address/group pairing
                var s = "Duplicate email address within a group: "
                    + String.Join(",",
                    emailList
                        .GroupBy(x => new { x.EmailAddress, x.GroupId })
                        .Where(g => g.Count() > 1)
                        .Select(y => String.Format("{0}(group:{1})x{2}", y.Key.EmailAddress, y.Key.GroupId, y.Count()))
                        .ToList());
                RaiseEcnValidationException(s);
            }
        }

        private static string BuildXMLProfile(Profile p)
        {
            StringBuilder sbProfile = new StringBuilder();

            sbProfile.Append("<Emails>");
            foreach (System.Reflection.PropertyInfo pi in p.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string) || pi.PropertyType == typeof(DateTime?))
                {
                    string value = string.Empty;
                    try
                    {
                        value = p.GetType().GetProperty(pi.Name).GetValue(p).ToString();
                    }
                    catch { }

                    if (!string.IsNullOrEmpty(value))
                        sbProfile.Append("<" + pi.Name.ToLower() + ">" + cleanXMLString(value) + "</" + pi.Name.ToLower() + ">");

                }
                else if (pi.PropertyType == typeof(SubscribeTypes))
                {
                    string value = GetSubscribeTypeCode((SubscribeTypes)Enum.Parse(typeof(SubscribeTypes), p.GetType().GetProperty(pi.Name).GetValue(p).ToString()));
                    if (!string.IsNullOrEmpty(value))
                        sbProfile.Append("<subscribetypecode>" + value + "</subscribetypecode>");
                }
                else if (pi.PropertyType == typeof(Formats))
                {
                    string value = GetFormatTypeCode((Formats)Enum.Parse(typeof(Formats), p.GetType().GetProperty(pi.Name).GetValue(p).ToString()));
                    if (!string.IsNullOrEmpty(value))
                        sbProfile.Append("<formattypecode>" + value + "</formattypecode>");
                }

            }
            sbProfile.Append("</Emails>");

            return sbProfile.ToString();
        }

        private static string BuildXMLUDF(Profile p, int groupID, KMPlatform.Entity.User user, bool isDupe = false, string compositeKey = "")
        {
            StringBuilder sbUDFs = new StringBuilder();
            bool bRowCreated = false;
            if (p.CustomFields != null)
            {
                string cKValue = "";
                if (isDupe)
                    cKValue = GetCompositeKeyValue(p, compositeKey);

                bRowCreated = false;
                List<ECN_Framework_Entities.Communicator.GroupDataFields> udfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, user);

                foreach (ProfileCustomField cf in p.CustomFields)
                {
                    if (cf.Name == null)
                    {
                        RaiseEcnValidationException("invalid constraint: missing \"Name\"");
                    }
                    if (udfList.Any(x => x.ShortName.ToLower().Equals(cf.Name.ToLower())))
                    {
                        if (!bRowCreated)
                        {
                            sbUDFs.Append("<row>");
                            if (isDupe)
                            {
                                sbUDFs.Append("<ea kv=\"" + cKValue + "\">" + cleanXMLString(p.EmailAddress.ToString()) + "</ea>");
                            }
                            else
                            {
                                sbUDFs.Append("<ea>" + cleanXMLString(p.EmailAddress.ToString()) + "</ea>");
                            }
                            bRowCreated = true;

                        }

                        if (cf.Value != null)
                        {
                            sbUDFs.Append("<udf id=\"" + udfList.First(x => x.ShortName.ToLower().Equals(cf.Name.ToLower())).GroupDataFieldsID.ToString() + "\">");

                            sbUDFs.Append("<v><![CDATA[" + cleanXMLString(Convert.ToString(cf.Value)) + "]]></v>");

                            sbUDFs.Append("</udf>");
                        }
                    }
                }

                if (bRowCreated)
                    sbUDFs.Append("</row>");

            }
            return sbUDFs.ToString();
        }

        private static string GetSubscribeTypeCode(SubscribeTypes sCode)
        {
            string returnValue = "";
            switch (sCode.ToString().ToUpper())
            {
                case "SUBSCRIBE":
                case "S":
                    returnValue = "S";
                    break;
                case "U":
                case "UNSUBSCRIBE":
                    returnValue = "U";
                    break;
                case "P":
                case "PENDING":
                    returnValue = "P";
                    break;
                default:
                    returnValue = "";
                    break;
            }
            return returnValue;
        }

        private static string GetFormatTypeCode(Formats fCode)
        {
            string returnValue = "";
            switch (fCode.ToString().ToUpper())
            {
                case "HTML":
                    returnValue = "HTML";
                    break;
                case "TEXT":
                    returnValue = "TEXT";
                    break;
                default:
                    returnValue = "";
                    break;
            }
            return returnValue;
        }

        private static string cleanXMLString(string text)
        {

            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("á", "a");
            return text;
        }
        #endregion validation
        #region items that belong in the Data Layer

        internal static ImportResult ManageSubscribersWithProfile(int userID, int customerID, ManageProfile profileList, KMPlatform.Entity.User user)
        {
            var xmlProfile = new StringBuilder();
            var xmlUDFs = new StringBuilder();

            var importResults = new ImportResult();
            var profiles = profileList.Profiles.ToList();
            for (var i = 0; i < profiles.Count; i++)
            {
                xmlProfile.Append(BuildXMLProfile(profiles[i]));
                xmlUDFs.Append(BuildXMLUDF(profiles[i], profileList.GroupID, user));

                if ((i != 0) && (i % EMAIL_PROFILES_IMPORT_BATCH_SIZE == 0) || (i == profiles.Count - 1))
                {
                    var dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(
                        user,
                        customerID,
                        profileList.GroupID,
                        "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>",
                        "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDFs.ToString() + "</XML>",
                        GetFormatTypeCode(profileList.Format),
                        GetSubscribeTypeCode(profileList.SubscribeType),
                        false,
                        "",
                        "EmailMarketing.API.Controllers.EmailGroupController.ManageSubscribersWithProfile");

                    UpdateImportResultsWithActionCounts(dtRecords, importResults);
                    xmlProfile = new StringBuilder();
                    xmlUDFs = new StringBuilder();
                }
            }
            return importResults;
        }

        internal static ImportResult ManageSubscribersWithProfileWithDupes(int userID, int customerID, ManageProfile profileList, KMPlatform.Entity.User user, string compositeKey)
        {
            var xmlProfile = new StringBuilder();
            var xmlUDFs = new StringBuilder();

            var importResults = new ImportResult();
            var profiles = profileList.Profiles.ToList();
            for (var i = 0; i < profiles.Count; i++)
            {
                xmlProfile.Append(BuildXMLProfile(profiles[i]));
                xmlUDFs.Append(BuildXMLUDF(profiles[i], profileList.GroupID, user, true, compositeKey));

                if ((i != 0) && (i % EMAIL_PROFILES_IMPORT_BATCH_SIZE == 0) || (i == profiles.Count - 1))
                {
                    var dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailsWithDupes(
                        user,
                        profileList.GroupID,
                        "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>",
                        "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDFs.ToString() + "</XML>",
                        GetFormatTypeCode(profileList.Format),
                        GetSubscribeTypeCode(profileList.SubscribeType),
                        false,
                        compositeKey,
                        false,
                        "EmailMarketing.API.Controllers.EmailGroupController.ManageSubscribersWithProfileWithDupes");

                    UpdateImportResultsWithActionCounts(dtRecords, importResults);

                    xmlProfile = new StringBuilder();
                    xmlUDFs = new StringBuilder();
                }
            }
            return importResults;
        }

        internal static ImportResult ManageSubscribers(int userID, int customerID, UpdateEmailAddressForGroup profileList, KMPlatform.Entity.User user)
        {
            var xmlProfile = new StringBuilder();
            var xmlUDFs = new StringBuilder();

            var importResults = new ImportResult();

            xmlProfile.Append(BuildXMLProfile(profileList));
            xmlUDFs.Append(BuildXMLUDF(profileList, profileList.GroupID, user));

            var dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(
                user,
                customerID,
                profileList.GroupID,
                "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>",
                "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDFs.ToString() + "</XML>",
                GetFormatTypeCode(profileList.Format),
                GetSubscribeTypeCode(profileList.SubscribeType),
                false,
                "",
                "EmailMarketing.API.Controllers.EmailGroupController.ManageSubscribers");

            UpdateImportResultsWithActionCounts(dtRecords, importResults);
            return importResults;
        }

        internal static IEnumerable<SubscriptionResult> ExecuteManageSubscribers(SqlCommand cmd)
        {
            List<SubscriptionResult> retList = new List<SubscriptionResult>();

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(
                cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (null != rdr)
                {
                    var builder = DynamicBuilder<SubscriptionResult>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        SubscriptionResult retItem = builder.Build(rdr);
                        if (null != retItem)
                        {
                            // manual parsing steps here...
                            Statuses result;
                            if (false == Enum.TryParse<Statuses>(rdr["Result"].ToString(), out result))
                            {
                                result = Statuses.Unknown;
                            }
                            retItem.Result = result;

                            SubscribeTypes statuses;
                            if (false == Enum.TryParse<SubscribeTypes>(rdr["Status"].ToString(), out statuses))
                            {
                                retItem.Status = SubscribeTypes.Subscribe;
                            }
                            else
                            {
                                retItem.Status = statuses;
                            }

                            retList.Add(retItem);
                        }
                    }
                }
            }

            return retList;
        }

        //internal static DataTable AddSubscriber(int userID, int customerID, IEnumerable<Email> emailList)
        internal static IEnumerable<SubscriptionResult> ManageSubscribers(int userID, int customerID, IEnumerable<Email> emailList, string source = "EmailMarketing.API.Controllers.EmailGroupController.ManageSubscribers")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailGroup_ManageSubscribers";

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
            cmd.Parameters["@CustomerID"].Value = customerID;

            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userID;

            cmd.Parameters.Add("@source", SqlDbType.VarChar);
            cmd.Parameters["@source"].Value = source;

            IEnumerable<Microsoft.SqlServer.Server.SqlDataRecord> tableValueInputParamValue = CreateManageSubscribersSqlDataRecords(emailList);

            SqlParameter tableValueInputParam = cmd.Parameters.AddWithValue("@EmailTable", tableValueInputParamValue);
            tableValueInputParam.SqlDbType = SqlDbType.Structured;
            tableValueInputParam.TypeName = "dbo.ManageSubscribersInputTableType";

            //return ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
            return ExecuteManageSubscribers(cmd);
        }

        internal static IEnumerable<Microsoft.SqlServer.Server.SqlDataRecord> CreateManageSubscribersSqlDataRecords(IEnumerable<Email> emailList)
        {
            List<Microsoft.SqlServer.Server.SqlMetaData> metaData = new List<Microsoft.SqlServer.Server.SqlMetaData>()
            {
                new Microsoft.SqlServer.Server.SqlMetaData("GroupID", SqlDbType.Int),
                new Microsoft.SqlServer.Server.SqlMetaData("EmailAddress", SqlDbType.VarChar, 255),
                new Microsoft.SqlServer.Server.SqlMetaData("FormatTypeCode", SqlDbType.VarChar,16),
                new Microsoft.SqlServer.Server.SqlMetaData("SubscribeTypeCode", SqlDbType.VarChar, 32)
            };

            Microsoft.SqlServer.Server.SqlDataRecord record = new Microsoft.SqlServer.Server.SqlDataRecord(metaData.ToArray());
            foreach (Email email in emailList)
            {
                record.SetInt32(0, email.GroupId);
                record.SetString(1, email.EmailAddress);
                record.SetString(2, email.Format.ToString().ToLower());
                record.SetString(3, email.SubscribeType.ToString().ToUpper());
                yield return record;
            }
        }

        private static void UpdateImportResultsWithActionCounts(DataTable dataTable, ImportResult importResults)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                switch (row[ACTION_COLUMN_NAME].ToString())
                {
                    case "I":
                        importResults.New += Convert.ToInt32(row[COUNTS_COLUMN_NAME].ToString());
                        break;
                    case "U":
                        importResults.Updated += Convert.ToInt32(row[COUNTS_COLUMN_NAME].ToString());
                        break;
                    case "M":
                        importResults.MasterSuppressed += Convert.ToInt32(row[COUNTS_COLUMN_NAME].ToString());
                        break;
                    case "D":
                        importResults.Duplicate += Convert.ToInt32(row[COUNTS_COLUMN_NAME].ToString());
                        break;
                    case "S":
                        importResults.Skipped += Convert.ToInt32(row[COUNTS_COLUMN_NAME].ToString());
                        break;
                    case "T":
                        importResults.Total += Convert.ToInt32(row[COUNTS_COLUMN_NAME].ToString());
                        break;
                }
            }
        }

        #endregion items that belong in the data layer
    }
}
