using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecn.gateway.Models;
using System.Configuration;
using System.Data;
using System.Text;
using ECN_Framework_Entities.Communicator;
using System.Collections;
using KM.Common.Entity;
using KM.Common.Web;
using BusinessGateway = ECN_Framework_BusinessLayer.Communicator.Gateway;
using BusinessEmail = ECN_Framework_BusinessLayer.Communicator.Email;
using BusinessEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup;

namespace ecn.gateway.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private const string GatewayEmailCookie = "Gateway_Email";
        private const string StyleFormExternal = "external";
        private const string StyleFormUpload = "upload";
        private const string ImageDomainPathKey = "Image_DomainPath";
        private const string AccessTokenKey = "ECNEngineAccessKey";
        private const string LoginMethodName = "Account.Login.Get";
        private const string ApplicationIdKey = "KMCommon_Application";
        private const string ModelPropertyName = "LoginModel.Invalid";
        private const string IncorrectPublicationCodeError = "Incorrect Publication Code";
        private const string ConfirmationAction = "ConfirmationPage";
        private const string AccountControllerName = "Account";
        private const string XmlHeader = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>";
        private static KMPlatform.Entity.User _User;
        private static readonly TimeSpan SlidingExpiration = TimeSpan.FromMinutes(15);

        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string pubCode, string typeCode)
        {
            var model = new LoginModel();
            dynamic sessionWrapper = new SessionWrapper(Session);
            InitDefaultLoginStates(sessionWrapper);

            var result = LoadGatewayAndInitStates(returnUrl, pubCode, typeCode, model, sessionWrapper);
            if (result != null)
            {
                return result;
            }

            InitStyleConfig(model, sessionWrapper);
            InitGatewayValues(model);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl, string command, string pubCode, string typeCode)
        {
            dynamic sessionWrapper = new SessionWrapper(Session);

            ViewBag.PubCode = pubCode;
            ViewBag.TypeCode = typeCode;
            sessionWrapper.PubCode = pubCode;
            sessionWrapper.TypeCode = typeCode;
            sessionWrapper.UserName = null;

            var localGateWayValues = model.Gateway.GatewayValues;

            LoaduserFromCache();

            var dbGateway = BusinessGateway.GetByGatewayID(model.Gateway.GatewayID);
            model.Gateway = dbGateway;
            model.Gateway.GatewayValues = localGateWayValues;

            return ValidateLogin(model, pubCode, typeCode, dbGateway, sessionWrapper, localGateWayValues) ??
                   ValidateEmail(model, pubCode, typeCode, sessionWrapper) ??
                   View(model);
        }

        private ActionResult ValidateEmail(LoginModel model, string pubCode, string typeCode, dynamic sessionWrapper)
        {
            if (model.Gateway.LoginOrCapture.IndexOf("CAPTURE", StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                return null;
            }

            var emailAddress = model.EMail.Trim();
            if (emailAddress.Length <= 0)
            {
                ModelState.AddModelError("LoginModel.Email", "Please enter an email address");
                return View(model);
            }

            var email = BusinessEmail.GetByEmailAddress(emailAddress, model.Gateway.CustomerID);
            return email != null 
                ? ValidateExistingEmail(model, pubCode, typeCode, sessionWrapper, emailAddress, email) 
                : RegisterNewEmail(model, pubCode, typeCode, sessionWrapper, emailAddress);
        }

        private ActionResult RegisterNewEmail(
            LoginModel model,
            string pubCode,
            string typeCode,
            dynamic sessionWrapper,
            string emailAddress)
        {
            RegisterNewEmailOnEvent(model, sessionWrapper, emailAddress);
            SetGatewayEmailCookie(model);

            if (BlankFields(model))
            {
                ModelState.AddModelError("LoginModel.GVP.blankFields", "Required Field(s) missing");
                model.Gateway.GatewayValues.RemoveAll(x => x.IsStatic);
                return View(model);
            }

            model.Gateway = PopulateStaticFields(model.Gateway);
            model.Gateway.GatewayValues.RemoveAll(x => string.IsNullOrWhiteSpace(x.Value));

            SaveCaptureInfo(model);
            var parameters = new {pubcode = pubCode, typecode = typeCode, gwId = model.Gateway.GatewayID};
            return RedirectToAction(
                ConfirmationAction,
                AccountControllerName,
                parameters);
        }

        private static void RegisterNewEmailOnEvent(LoginModel model, dynamic sessionWrapper, string emailAddress)
        {
            CheckAccessImportEmail(model, sessionWrapper, emailAddress);
            sessionWrapper.UserName = emailAddress;

            var email = BusinessEmail.GetByEmailAddress(emailAddress, model.Gateway.CustomerID);
            try
            {
                ECN_Framework_BusinessLayer.Communicator.EventOrganizer.Event(
                    model.Gateway.CustomerID,
                    model.Gateway.GroupID,
                    email.EmailID,
                    _User,
                    null);
            }
            catch (Exception ex)
            {
                ApplicationLog.LogNonCriticalError(ex,
                    "AccountController.APP_Login",
                    Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
            }
        }

        private static void CheckAccessImportEmail(LoginModel model, dynamic sessionWrapper, string emailAddress)
        {
            var sbProfileXml = new StringBuilder();
            sbProfileXml.Append(XmlHeader);
            sbProfileXml.Append($"<Emails><emailaddress>{emailAddress}</emailaddress></Emails>");

            BusinessEmailGroup.ImportEmails_NoAccessCheck(
                _User,
                model.Gateway.CustomerID,
                model.Gateway.GroupID,
                sbProfileXml + "</XML>",
                $"{XmlHeader}</XML>",
                "HTML",
                "S",
                true,
                source: "ecn.gateway.controllers.accountcontroller.login");
        }

        private ActionResult ValidateExistingEmail(
            LoginModel model,
            string pubCode,
            string typeCode,
            dynamic sessionWrapper,
            string emailAddress,
            Email email)
        {
            var emailGroup = BusinessEmailGroup.GetByEmailAddressGroupID_NoAccessCheck(
                emailAddress,
                model.Gateway.GroupID);
            return emailGroup == null 
                ? ValidateAndRegister(model, pubCode, typeCode, sessionWrapper, emailAddress, email) 
                : ShowAlreadyExist(model, pubCode, typeCode, sessionWrapper, email);
        }

        private ActionResult ShowAlreadyExist(
            LoginModel model,
            string pubCode,
            string typeCode,
            dynamic sessionWrapper,
            Email email)
        {
            SetGatewayEmailCookie(model);
            if (BlankFields(model))
            {
                ModelState.AddModelError("LoginModel.GVP.blankFields", "Required Field(s) missing");
                model.Gateway.GatewayValues.RemoveAll(x => x.IsStatic);
                return View(model);
            }

            SaveCaptureInfo(model);
            sessionWrapper.UserName = email.EmailAddress;

            var parameters = new { pubcode = pubCode, typecode = typeCode, gwId = model.Gateway.GatewayID };
            return RedirectToAction(
                ConfirmationAction,
                AccountControllerName,
                parameters);
        }

        private ActionResult ValidateAndRegister(
            LoginModel model,
            string pubCode,
            string typeCode,
            dynamic sessionWrapper,
            string emailAddress,
            Email email)
        {
            RegisterExistingEmailOnEvent(model, sessionWrapper, emailAddress, email);
            SetGatewayEmailCookie(model);

            if (BlankFields(model))
            {
                ModelState.AddModelError("LoginModel.GVP.blankFields", "Required Field(s) missing");
                model.Gateway.GatewayValues.RemoveAll(x => x.IsStatic);
                return View(model);
            }

            model.Gateway = PopulateStaticFields(model.Gateway);
            model.Gateway.GatewayValues.RemoveAll(x => string.IsNullOrWhiteSpace(x.Value));

            SaveCaptureInfo(model);
            var parameters = new {pubcode = pubCode, typecode = typeCode, gwId = model.Gateway.GatewayID};
            return RedirectToAction(
                ConfirmationAction,
                AccountControllerName,
                parameters);
        }

        private static void RegisterExistingEmailOnEvent(
            LoginModel model,
            dynamic sessionWrapper,
            string emailAddress,
            Email email)
        {
            CheckAccessImportEmail(model, sessionWrapper, emailAddress);            

            try
            {
                ECN_Framework_BusinessLayer.Communicator.EventOrganizer.Event(model.Gateway.CustomerID,
                    model.Gateway.GroupID,
                    email.EmailID,
                    _User,
                    null);
            }
            catch (Exception ex)
            {
                ApplicationLog.LogNonCriticalError(ex,
                    "AccountController.APP_Login",
                    Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
            }
        }

        private ActionResult ValidateLogin(
            LoginModel model,
            string pubCode,
            string typeCode,
            Gateway dbGateway,
            dynamic sessionWrapper,
            List<GatewayValue> localGateWayValues)
        {
            if (model.Gateway.LoginOrCapture.IndexOf("LOGIN", StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                return null;
            }

            // Validating Subscriber
            try
            {
                var emailAddressObj = BusinessEmailGroup.GetByEmailAddressGroupID_NoAccessCheck(model.EMail, model.Gateway.GroupID);
                if (emailAddressObj == null)
                {
                    ModelState.AddModelError("LoginModel.Password", " The Email Address you entered is invalid.");
                    return View(model);
                }

                var email = BusinessEmail.GetByEmailIDGroupID_NoAccessCheck(
                    emailAddressObj.EmailID,
                    emailAddressObj.GroupID);
                if (email == null)
                {
                    ModelState.AddModelError("LoginModel.Password", " The Email Address you entered is invalid.");
                    return View(model);
                }

                if (email.EmailAddress.Equals(model.EMail, StringComparison.OrdinalIgnoreCase) &&
                    ValidatePassword(dbGateway.ValidatePassword, email.Password, model.Password))
                {
                    return LoadUserDataAndRedirectToConfirm(model, pubCode, typeCode, dbGateway, sessionWrapper, localGateWayValues, email);
                }

                if (email.Password != model.Password)
                {
                    ModelState.AddModelError("LoginModel.Password",
                        " The Email address or password you entered is invalid.");
                    return View(model);
                }

                if (!email.EmailAddress.Equals(model.EMail, StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("LoginModel.Password",
                        " The Email Address or Password you entered is invalid.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.LogCriticalError(ex,
                    "AccountController.Login.Post",
                    Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
            }

            return null;
        }

        private ActionResult LoadUserDataAndRedirectToConfirm(
            LoginModel model,
            string pubCode,
            string typeCode,
            Gateway dbGateway,
            dynamic sessionWrapper,
            List<GatewayValue> localGateWayValues,
            Email email)
        {
            sessionWrapper.UserName = email.EmailAddress;
            SetGatewayEmailCookie(model);

            model.Gateway.ValidatePassword = dbGateway.ValidatePassword;
            if (BlankFields(model))
            {
                ModelState.AddModelError("LoginModel.GVP.blankFields", "Required Field(s) missing");
                model.Gateway.GatewayValues.RemoveAll(x => x.IsStatic);
                return View(model);
            }

            dbGateway.GatewayValues = getDbGatewayValues(dbGateway);
            var screenGateway = new Gateway {GatewayValues = localGateWayValues};
            if (ValidateCustomFields(model, dbGateway, screenGateway))
            {
                var routeValues = new { pubcode = pubCode, typecode = typeCode, gwId = model.Gateway.GatewayID };
                return RedirectToAction(
                    ConfirmationAction,
                    AccountControllerName,
                    routeValues);
            }

            ModelState.AddModelError("LoginModel.GateWayVSProfile", "Login failed.");
            model.Gateway.GatewayValues.RemoveAll(x => x.IsStatic);
            foreach (var gatewayValues in model.Gateway.GatewayValues)
            {
                gatewayValues.Value = string.Empty;
            }

            return View(model);
        }

        private static void LoaduserFromCache()
        {
            var cacheKey = $"cache_user_by_AccessKey_{ConfigurationManager.AppSettings["ECNEngineAccessKey"]}";
            // POSSIBLE BUG: Bad class design. Assigning to static field in the controller.
            _User = (KMPlatform.Entity.User) HttpRuntime.Cache[cacheKey];
            if (_User != null)
            {
                return;
            }

            _User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"],
                true);
            
            HttpRuntime.Cache.Add(
                cacheKey,
                _User,
                null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                SlidingExpiration,
                System.Web.Caching.CacheItemPriority.Normal,
                null);
        }

        private void SetGatewayEmailCookie(LoginModel model)
        {
            try
            {
                if (model.RememberMe && Request.Cookies[GatewayEmailCookie] == null)
                {
                    var myCookie = new HttpCookie(GatewayEmailCookie)
                    {
                        Expires = DateTime.Now.AddDays(364),
                        Value = model.EMail
                    };
                    Response.Cookies.Add(myCookie);
                }
                else if (model.RememberMe)
                {
                    var myCookie = Request.Cookies[GatewayEmailCookie];
                    myCookie.Value = model.EMail;
                    Response.Cookies.Set(myCookie);
                }
            }
            catch
            {
                // POSSIBLE BUG: exception didn't logged
            }
        }

        private static List<GatewayValue> getDbGatewayValues(Gateway dbGateway)
        {
            dbGateway.GatewayValues = ECN_Framework_BusinessLayer.Communicator.GatewayValue.GetByGatewayID(dbGateway.GatewayID);
            return dbGateway.GatewayValues;
        }

        private static bool BlankFields(LoginModel model)
        {
            bool blankFields = false;
            foreach (var gValue in model.Gateway.GatewayValues.Where(gValue => string.IsNullOrWhiteSpace(gValue.Value)))
            {
                gValue.IsBlank = true;
                blankFields = true;
            }
            return blankFields;
        }        

        private Hashtable getUDFsForList(int groupID, KMPlatform.Entity.User user)
        {
            Hashtable fields = new Hashtable();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);
            if (gdfList.Count > 0)
            {
                foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdfList)
                {
                    fields.Add(gdf.ShortName.ToLower(), gdf.GroupDataFieldsID);
                }
            }

            return fields;
        }


        private string cleanXMLString(string text)
        {

            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("á", "a");
            return text;
        }

        private void SaveCaptureInfo(LoginModel model)
        {
            StringBuilder sbProfileXML = new StringBuilder();
            sbProfileXML.Append(XmlHeader);
            sbProfileXML.Append("<Emails><emailaddress>" + model.EMail + "</emailaddress><password>" + model.Password + "</password>");
            StringBuilder sbUDFXML = new StringBuilder();
            sbUDFXML.Append(XmlHeader);

            Hashtable hGDFFields = getUDFsForList(model.Gateway.GroupID, _User);
            bool rowCreated = false;
            foreach (var gValue in model.Gateway.GatewayValues)
            {
                if (!rowCreated)
                {
                    sbUDFXML.Append("<row>");
                    sbUDFXML.Append("<ea>" + model.EMail + "</ea>");
                    rowCreated = true;
                }
                if (!string.IsNullOrWhiteSpace(gValue.Field))
                {
                    sbUDFXML.Append("<udf id=\"" + hGDFFields[gValue.Field.ToLower()].ToString() + "\">");
                }
                else
                {
                    sbUDFXML.Append("<udf id=\"" + hGDFFields[gValue.Label.ToLower()].ToString() + "\">");
                }
                
                sbUDFXML.Append("<v><![CDATA[" + cleanXMLString(gValue.Value).Replace("&amp;", "&") + "]]></v>");
                sbUDFXML.Append("</udf>");

                //"<User_" + gValue.Field + ">" + gValue.Value + "</User_" + gValue.Field + ">");
            }
            sbProfileXML.Append("</Emails>");

            if (rowCreated)
                sbUDFXML.Append("</row>");
            ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(_User, model.Gateway.CustomerID, model.Gateway.GroupID, sbProfileXML.ToString() + "</XML>", sbUDFXML.ToString() + "</XML>", "HTML", "S", false, "", "Ecn.gateway.controllers.accountcontroller.SaveCaptureInfo");
        }

        private bool ValidateCustomFields(LoginModel model, Gateway dbGateway, Gateway screenGateway)
        {
            DataTable emailProfilesWithUDF = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetGroupEmailProfilesWithUDF(model.Gateway.GroupID, model.Gateway.CustomerID, "AND Emails.EmailAddress = '" + model.EMail + "'", "'S'", "ProfilePlusAllUDFs");
            foreach (GatewayValue gValues in model.Gateway.GatewayValues)
            {
                if (gValues.IsStatic)
                {
                    bool profileMatch = MatchProfileValuesStatic(gValues, emailProfilesWithUDF, gValues.NOT);
                    if (!profileMatch)
                    {
                        return false;
                    }
                }
                else  //non-static
                {
                    GatewayValue dbGatewayValueToPass = screenGateway.GatewayValues.FirstOrDefault(x => x.GatewayValueID == gValues.GatewayValueID);
                    bool profileMatch = MatchProfileValuesNotStatic(gValues, dbGatewayValueToPass, gValues.NOT);
                    if (!profileMatch)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool MatchProfileValuesStatic(GatewayValue gValues, DataTable emailProfilesWithUDF, bool notFlag)
        {
            DataColumnCollection columns = emailProfilesWithUDF.Columns;
            
            if (columns.Contains(gValues.Field))
            {
                string colName = emailProfilesWithUDF.Columns[gValues.Field].ToString();
                foreach (DataRow row in emailProfilesWithUDF.Rows)
                {
                    string profileValue = row[colName].ToString();
                    bool result = ProcessValidate(gValues, profileValue, notFlag);
                    if (!result)
                    {
                        gValues.HasFailed = true;
                        return false;
                    }
                }
                gValues.HasFailed = false;
                return true;
            }
            return false;
        }

        private bool MatchProfileValuesNotStatic(GatewayValue gValues, GatewayValue dbGatewayValue, bool notFlag)
        {
            bool result = ProcessValidate(gValues, dbGatewayValue.Value, notFlag);
            if (!result)
            {
                gValues.HasFailed = true;
                return false;
            }
             
            return true;
            
        }

        private bool ProcessValidate(GatewayValue gValues, string pValue, bool notFlag)
        {
            switch (gValues.FieldType)
            {
                case "":
                case "string":
                case "VARCHAR(500)":
                    return ProcessStrings(gValues, pValue, notFlag);
                case "Number":
                case "INT":
                    return ProcessNumbers(gValues, pValue, notFlag);
                case "Date [MM/DD/YYYY]" :
                case "DATETIME" :
                    return ProcessDates(gValues, pValue, notFlag);
            }
            return false;
        }

        private bool ProcessStrings(GatewayValue gValues, string pValue, bool notFlag)
        {
            if (notFlag)
            {
                switch (gValues.Comparator.ToLower().Trim())
                {
                    case "equals":
                    case "equals [ = ]":
                        if (gValues.Value.ToLower().Trim() != pValue.ToLower().Trim())
                        {
                            return true;
                        }
                        break;
                    case "contains":
                        if (!gValues.Value.ToLower().Trim().Contains(pValue.ToLower().Trim()))
                        {
                            return true;
                        }
                        break;
                    case "ending with":
                    case "ends with":
                        if (!gValues.Value.ToLower().Trim().EndsWith(pValue.ToLower().Trim()))
                        {
                            return true;
                        }
                        break;
                    case "starting with":
                    case "starts with":
                        if (!gValues.Value.ToLower().Trim().StartsWith(pValue.ToLower().Trim()))
                        {
                            return true;
                        }
                        break;
                }
            }
            else
            {
                switch (gValues.Comparator.ToLower().Trim())
                {
                    case "equals":
                    case "equals [ = ]":
                        if (gValues.Value.ToLower().Trim() == pValue.ToLower().Trim())
                        {
                            return true;
                        }
                        break;
                    case "contains":
                        if (gValues.Value.ToLower().Trim().Contains(pValue.ToLower().Trim()))
                        {
                            return true;
                        }
                        break;
                    case "ending with":
                    case "ends with":
                        if (gValues.Value.ToLower().Trim().EndsWith(pValue.ToLower().Trim()))
                        {
                            return true;
                        }
                        break;
                    case "starting with":
                    case "starts with":
                        if (gValues.Value.ToLower().Trim().StartsWith(pValue.ToLower().Trim()))
                        {
                            return true;
                        }
                        break;
                }
            }
           
            return false;
        }

        private bool ProcessNumbers(GatewayValue gValues, string pValue, bool notFlag)
        {
            if (!string.IsNullOrWhiteSpace(gValues.Value)&&!string.IsNullOrWhiteSpace(pValue))
            {
                int num1;
                int num2;
                bool test1 = int.TryParse(gValues.Value, out num1);
                bool test2 = int.TryParse(gValues.Value, out num2);
                if (!(test1 && test2))
                {
                    return false;
                }

                if (notFlag)
                {
                    switch (gValues.Comparator.ToLower().Trim())
                    {
                        case "equals":
                            if (gValues.Value.ToLower().Trim() != pValue.ToLower().Trim())
                            {
                                return true;
                            }
                            break;
                        case "greater than":
                        case "greater than [ > ]":
                            if (!(Convert.ToInt32(gValues.Value) < Convert.ToInt32(pValue.ToLower().Trim())))
                            {
                                return true;
                            }
                            break;
                        case "less than [ < ]":
                        case "less than":
                            if (!(Convert.ToInt32(gValues.Value) > Convert.ToInt32(pValue.ToLower().Trim())))
                            {
                                return true;
                            }
                            break;
                    }
                }
                else
                {
                    switch (gValues.Comparator.ToLower().Trim())
                    {
                        case "equals":
                            if (gValues.Value.ToLower().Trim() == pValue.ToLower().Trim())
                            {
                                return true;
                            }
                            break;
                        case "greater than":
                        case "greater than [ > ]":
                            if (Convert.ToInt32(gValues.Value) < Convert.ToInt32(pValue.ToLower().Trim()))
                            {
                                return true;
                            }
                            break;
                        case "less than [ < ]":
                        case "less than":
                            if (Convert.ToInt32(gValues.Value) > Convert.ToInt32(pValue.ToLower().Trim()))
                            {
                                return true;
                            }
                            break;
                    }
                }
            }
            
            return false;
        }

        private bool ProcessDates(GatewayValue gValues, string pValue, bool notFlag)
        {
            DateTime dateTime;
            if (!DateTime.TryParse(gValues.Value, out dateTime)){ return false; }
            DateTime dateTime2;
            if (!DateTime.TryParse(pValue, out dateTime2)){ return false; }

            if (notFlag)
            {
                switch (gValues.Comparator.ToLower().Trim())
                {
                    case "equals":
                    case "equals [ = ]":
                        if (Convert.ToDateTime(gValues.Value) != Convert.ToDateTime(pValue.Trim()))
                        {
                            return true;
                        }
                        break;
                    case "greater than":
                    case "greater than [ > ]":
                        if (!(Convert.ToDateTime(gValues.Value) < Convert.ToDateTime(pValue.Trim())))
                        {
                            return true;
                        }
                        break;
                    case "less than":
                    case "less than [ < ]":
                        if (!(Convert.ToDateTime(gValues.Value) > Convert.ToDateTime(pValue.Trim())))
                        {
                            return true;
                        }
                        break;
                    case "is empty":
                        DateTime valueOne = Convert.ToDateTime(gValues.Value);
                        DateTime valueTwo = Convert.ToDateTime(gValues.Value);
                        if (!(valueOne == DateTime.MinValue && valueTwo == DateTime.MinValue))
                        {
                            return true;
                        }
                        break;
                }
            }
            else
            {
                switch (gValues.Comparator.ToLower().Trim())
                {
                    case "equals":
                    case "equals [ = ]":
                        if (Convert.ToDateTime(gValues.Value) == Convert.ToDateTime(pValue.Trim()))
                        {
                            return true;
                        }
                        break;
                    case "greater than":
                    case "greater than [ > ]":
                        if (Convert.ToDateTime(gValues.Value) < Convert.ToDateTime(pValue.Trim()))
                        {
                            return true;
                        }
                        break;
                    case "less than":
                    case "less than [ < ]":
                        if (Convert.ToDateTime(gValues.Value) > Convert.ToDateTime(pValue.Trim()))
                        {
                            return true;
                        }
                        break;
                    case "is empty":
                        DateTime valueOne = Convert.ToDateTime(gValues.Value);
                        DateTime valueTwo = Convert.ToDateTime(gValues.Value);
                        if (valueOne == DateTime.MinValue && valueTwo == DateTime.MinValue)
                        {
                            return true;
                        }
                        break;
                }
            }
           
            return false;
        }

        private bool ValidatePassword(bool ValidatePassword, string emailPassword, string modelPassword)
        {
            if (ValidatePassword == false){ return true; }
            return emailPassword == modelPassword;
        }

        //
        // GET: /Account/ConfirmationPage
        [AllowAnonymous]
        public ActionResult ConfirmationPage(string typecode, string pubcode, int gwId = 0)
        {
            try
            {
                Gateway gateway = ECN_Framework_BusinessLayer.Communicator.Gateway.GetByGatewayID(gwId);
                if ((Session["PubCode"] != null && Session["PubCode"].ToString().Equals(pubcode)) && (Session["TypeCode"] != null && Session["TypeCode"].ToString().Equals(typecode)))
                {
                    // Redirect user to login page if link parameters are not shown
                    if (Session["UserName"] != null)
                    {
                        if (Session["UserName"].ToString() == "")
                        {
                            return RedirectToAction("Login", AccountControllerName);
                        }
                    }
                    else if (Session["UserName"] == null)
                    {
                        return RedirectToAction("Login", AccountControllerName);
                    }

                    // Retrieving the Type Code from the URL
                    if (typecode == null)
                    {
                        RedirectToAction("Login", AccountControllerName);
                    }

                    // Redirecting User to The Login Page If AutoReDirect = True
                    bool useRedirect = gateway.UseRedirect;
                    bool useConfirmation = gateway.UseConfirmation;

                    if (useRedirect && useConfirmation)
                    {
                        //delayed redirect 
                        ConfirmationModel confirmationModel = new ConfirmationModel { Gateway = gateway };
                        confirmationModel.Gateway.RedirectDelay = confirmationModel.Gateway.RedirectDelay * 1000;
                        return View(confirmationModel);
                    }
                    if (useRedirect && !useConfirmation)
                    {
                        //auto redirect
                        //if (string.IsNullOrWhiteSpace(gateway.RedirectURL))
                        //{
                            ConfirmationModel confirmationModel = new ConfirmationModel { Gateway = gateway, HasError = true, ErrorMsg = "Missing Redirect URL" };
                            return View(confirmationModel);
                        //}
                        //return Redirect(gateway.RedirectURL);
                    }
                    if (!useRedirect && useConfirmation)
                    {                        
                        ViewBag.EditionURL = gateway.RedirectURL;
                        ConfirmationModel confirmationModel = new ConfirmationModel { Gateway = gateway };
                        return View(confirmationModel);
                    }
                }
                else
                {
                    return RedirectToAction("Login", AccountControllerName, new { pubcode = pubcode, typecode = typecode });
                }

            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "AccountController.ConfirmationPage.Get", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult SignUp()
        {
            Gateway gateway = null;
            if (Request.QueryString["pubcode"] != null && Request.QueryString["typecode"] != null)
            {
                if (ValidatePubCode(Request.QueryString["pubcode"].ToString(), Request.QueryString["typecode"].ToString()))
                {
                    gateway = ECN_Framework_BusinessLayer.Communicator.Gateway.GetByGatewayPubCode(Request.QueryString["pubcode"], Request.QueryString["typecode"]);
                }
                else
                {
                    return RedirectToAction("Login", AccountControllerName);
                }
            }
            else
            {
                return RedirectToAction("Login", AccountControllerName);
            }
            
            
            try
            {
                // Retrieving Edition URL Digital Edition
                string subscriberURL = gateway.SignupURL;
                ViewBag.SubscriberURL = subscriberURL;
                // Redirecting User to The Login Page If AutoReDirect = True
                return Redirect(subscriberURL);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "AccountController.Signup.Get", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
            }
            return View();
        }

        //
        // GET: /Account/SendPassword
        [AllowAnonymous]
        public ActionResult SendPassword()
        {
            //return null;

            // Redirect user to login page if link parameters are not shown
            // Retrieving the Pub Code from the URL
            if (Request.QueryString["pubcode"] != null && Request.QueryString["typecode"] != null)
            {
                if (ValidatePubCode(Request.QueryString["pubcode"].ToString(), Request.QueryString["typecode"].ToString()))
                {
                    Gateway gateway = ECN_Framework_BusinessLayer.Communicator.Gateway.GetByGatewayPubCode(Request.QueryString["pubcode"], Request.QueryString["typecode"]);
                    
                    ViewData["PubCode"] = Request.QueryString["pubcode"].ToString().ToUpper();
                    Session["PubCode"] = Request.QueryString["pubcode"].ToString().ToUpper();
                    ViewData["TypeCode"] = Request.QueryString["typecode"].ToString().ToUpper();
                    Session["TypeCode"] = Request.QueryString["typecode"].ToString().ToUpper();
                    ViewData["SubscribeURL"] = gateway.SignupURL;
                    Session["SubscribeURL"] = ViewData["SubscribeURL"].ToString().ToUpper();
                    Session["PageTitle"] = gateway.Header;
                    //Session["PageHeader"] = gateway.Header; //name
                }
                else
                {
                    return RedirectToAction("Login", AccountControllerName);
                }
            }
            else
            {
                return RedirectToAction("Login", AccountControllerName);
            }
            return View();
        }

        //
        // POST: /Account/SendPassword
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SendPassword(SendPasswordModel model)
        {
            //return null;

            Gateway gateway = null;
            if (Request.QueryString["pubcode"] != null && Request.QueryString["typecode"] != null)
            {
                if (ValidatePubCode(Request.QueryString["pubcode"].ToString(), Request.QueryString["typecode"].ToString()))
                {
                    gateway = ECN_Framework_BusinessLayer.Communicator.Gateway.GetByGatewayPubCode(Request.QueryString["pubcode"], Request.QueryString["typecode"]);
                }
                else
                {
                    return RedirectToAction("Login", AccountControllerName);
                }
            }
            else
            {
                return RedirectToAction("Login", AccountControllerName);
            }

            //

            if (!string.IsNullOrEmpty(model.EMail))
            {
                try
                {
                    if (HttpRuntime.Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
                    {
                        _User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), true);
                        HttpRuntime.Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), _User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                    else
                    {
                        _User = (KMPlatform.Entity.User)HttpRuntime.Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
                    }

                    // Resetting password
                    model.UserPassword = System.Guid.NewGuid().ToString().Substring(0, 7);

                    //model.Save(model.EMail, model.UserPassword, Convert.ToInt32(ReadXMLFileContent("ID")), user);
                    int groupID = gateway.GroupID;
                    //int groupID = Convert.ToInt32(ReadXMLFileContent("ID"));

                    if (model.EMail.Length > 0)
                    {

                        ECN_Framework_Entities.Communicator.EmailGroup eg = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(model.EMail, groupID);


                        if (eg != null && eg.SubscribeTypeCode.ToUpper() == "S")
                        {
                            ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailIDGroupID_NoAccessCheck(eg.EmailID, groupID);
                            if (email != null)
                            {

                                //email doesn't exist, adding it
                                StringBuilder sbProfileXML = new StringBuilder();
                                sbProfileXML.Append(XmlHeader);
                                sbProfileXML.Append("<Emails><emailaddress>" + email.EmailAddress + "</emailaddress><password>" + model.UserPassword.ToString() + "</password></Emails>");

                                ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(_User, gateway.CustomerID, groupID, sbProfileXML.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", "HTML", "S", false, "", "Ecn.gateway.controllers.accountcontroller.SendPassword");

                                ECN_Framework_Entities.Communicator.EmailDirect ed = new EmailDirect();
                                ed.CustomerID = gateway.CustomerID;
                                ed.EmailAddress = email.EmailAddress;
                                ed.EmailSubject = "Knowledge Marketing Gateway - Reset Password";
                                
                                ed.FromName = "Gateway";
                                ed.Process = "Gateway - SendPassword";
                                ed.Source = "Gateway";
                                ed.ReplyEmailAddress = ConfigurationManager.AppSettings["FromEmail"].ToString();
                                ed.FromEmailAddress = ConfigurationManager.AppSettings["FromEmail"].ToString();
                                ed.SendTime = DateTime.Now;
                                ed.Status = ECN_Framework_Common.Objects.EmailDirect.Enums.Status.Pending.ToString();
                                ed.CreatedUserID = _User.UserID;
                                ed.Content = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"><html>" +
                "<head><title></title></head><body><table border='0' width='750'><tbody><tr><td align='center' style='font-family: Arial, Helvetica, sans-serif; font-size: 10px;'> </td></tr><tr><td align='left'><a href='http://www.ecn5.com/index.htm'><img border='0' src='http://www.ecn5.com/ecn.images/Channels/12/kmlogo.jpg'></a></td>" +
                "</tr><tr><td align='center' bgcolor='#666666' height='10'> </td></tr><tr><td align='center' style='font-family: Arial, Helvetica, sans-serif; font-size: 10px;'> </td></tr><tr><td align='left' style='font-family: Arial, Helvetica, sans-serif; font-size: 12px;'>" +
                "Hello,<br>Your new password is %%newpassword%%.</td></tr><tr><td style='font-family: Arial, Helvetica, sans-serif; font-size: 12px;'><a href='%%URL%%'>Click here</a> to login.</td></tr><tr><td align='center' style='font-family: Arial, Helvetica, sans-serif; font-size: 20px;'>" +
                "</td></tr></tbody></table></body></html>";

                                string returnURL = "";
                                returnURL = Request.Url.ToString().Substring(0, Request.Url.ToString().IndexOf(".com") + 4);
                                returnURL += "?pubcode=" + gateway.PubCode + "&typecode=" + gateway.TypeCode;
                                ed.Content = ed.Content.Replace("%%newpassword%%", model.UserPassword);
                                ed.Content = ed.Content.Replace("%%URL%%", returnURL);
                                try
                                {
                                    ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
                                    // Send password to users account
                                    ViewBag.SuccessMessage = "Your new password has been sent to your e-mail account.";
                                }
                                catch(ECN_Framework_Common.Objects.ECNException ecn)
                                {
                                    StringBuilder sbErrorMessage = new StringBuilder();
                                    foreach (ECN_Framework_Common.Objects.ECNError e in ecn.ErrorList)
                                    {
                                        if (sbErrorMessage.Length > 0)
                                            sbErrorMessage.Append("<br />");
                                        sbErrorMessage.Append(e.ErrorMessage);
                                    }
                                    ViewBag.SuccessMessage = sbErrorMessage.ToString();
                                }
                                catch (Exception exc)
                                {
                                    KM.Common.Entity.ApplicationLog.LogCriticalError(exc, "AccountController.SendPassword.Post", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                                    ModelState.AddModelError("SendPassword.Invalid", "An error occurred");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("SendPassword.Invalid", "Email Address not found");
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("SendPassword.Invalid", "Email Address not found");
                            return View(model);
                        }
                    }
                }
                catch (ECN_Framework_Common.Objects.ECNException ecnEX)
                {
                    string errorMessage = string.Empty;
                    foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnEX.ErrorList)
                    {
                        errorMessage = errorMessage + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                    }
                    ModelState.AddModelError("SendPassword.Invalid", errorMessage);
                }
                catch (Exception ex)
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "AccountController.SendPassword.Post", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                    ModelState.AddModelError("SendPassword.Invalid", "An error occurred");
                }
            }

            return View(model);
        }

        private static void InitGatewayValues(LoginModel loginModel)
        {
            loginModel.Gateway.GatewayValues.RemoveAll(x => x.IsStatic);
            foreach (var item in loginModel.Gateway.GatewayValues)
            {
                if (!item.IsStatic)
                {
                    item.Value = string.Empty;
                }
            }
        }

        private static void InitStyleConfig(LoginModel loginModel, dynamic sessionWrapper)
        {
            if (loginModel.Gateway.UseStyleFrom.Equals(StyleFormExternal))
            {
                sessionWrapper.StyleSheet = loginModel.Gateway.Style;
            }
            else if (loginModel.Gateway.UseStyleFrom.Equals(StyleFormUpload))
            {
                sessionWrapper.StyleSheet = $"{ConfigurationManager.AppSettings[ImageDomainPathKey]}{loginModel.Gateway.Style}";
            }
        }

        private ActionResult LoadGatewayAndInitStates(
            string returnUrl,
            string pubCode,
            string typeCode,
            LoginModel lm,
            dynamic sessionWrapper)
        {
            try
            {
                if (Request.Cookies[GatewayEmailCookie] != null)
                {
                    lm.EMail = Request.Cookies[GatewayEmailCookie].Value;
                }

                if (string.IsNullOrWhiteSpace(pubCode) || string.IsNullOrWhiteSpace(typeCode))
                {
                    AddIncorrectPublicationCodeError();
                    return View();
                }

                lm.Gateway = BusinessGateway.GetByGatewayPubCode(pubCode, typeCode);
                if (lm.Gateway == null)
                {
                    AddIncorrectPublicationCodeError();
                    return View();
                }

                InitGatewayLoginStates(pubCode, typeCode, lm, sessionWrapper);
                lm.Gateway = PopulateCustomFields(lm.Gateway);

                // POSSIBLE BUG: Bad class design. Assigning to static field in the controller.
                _User = KMPlatform.BusinessLogic.User.GetByAccessKey(
                    ConfigurationManager.AppSettings[AccessTokenKey],
                    true);
                ViewBag.ReturnUrl = returnUrl;
            }
            catch (Exception ex)
            {
                ApplicationLog.LogCriticalError(ex,
                    LoginMethodName,
                    Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationIdKey]));
            }

            return null;
        }

        private void InitGatewayLoginStates(string pubCode, string typeCode, LoginModel lm, dynamic sessionWrapper)
        {
            ViewBag.PubCode = pubCode.ToUpper();
            ViewBag.TypeCode = typeCode.ToUpper();
            ViewBag.SubscribeURL = lm.Gateway.SignupURL;

            sessionWrapper.PubCode = pubCode.ToUpper();
            sessionWrapper.TypeCode = typeCode.ToUpper();
            sessionWrapper.SubscribeURL = lm.Gateway.SignupURL.ToUpper();
            sessionWrapper.PageTitle = lm.Gateway.Header;
            sessionWrapper.Footer = lm.Gateway.Footer;
        }

        private void InitDefaultLoginStates(dynamic sessionWrapper)
        {
            ViewBag.TypeCode = string.Empty;
            ViewBag.PubCode = string.Empty;
            sessionWrapper.PubCode = string.Empty;
            sessionWrapper.TypeCode = string.Empty;
            sessionWrapper.UserName = string.Empty;
            sessionWrapper.LinkParameter = string.Empty;
        }

        private void AddIncorrectPublicationCodeError()
        {
            ModelState.AddModelError(ModelPropertyName, IncorrectPublicationCodeError);
        }

        private static Gateway PopulateCustomFields(Gateway gateway)
        {
            gateway.GatewayValues = ECN_Framework_BusinessLayer.Communicator.GatewayValue.GetByGatewayID(gateway.GatewayID);
            return gateway;
        }

        private static Gateway PopulateStaticFields(Gateway gateway)
        {
            gateway.GatewayValues.AddRange(ECN_Framework_BusinessLayer.Communicator.GatewayValue.GetByGatewayID(gateway.GatewayID));
            return gateway;
        }

        private static bool ValidatePubCode(string pubcode, string typecode)
        {
            var gateway = BusinessGateway.GetByGatewayPubCode(pubcode, typecode);
            return gateway != null;
        }
    }
}
