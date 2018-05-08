using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecn.communicator.mvc.Models;
using ecn.communicator.mvc.Infrastructure;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;
using System.Configuration;
using System.Data;
using System.Xml;
using System.IO;
using System.Collections;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using KMSite;
using System.Text;
using System.Web.UI;

namespace ecn.communicator.mvc.Controllers
{
    public class DataFieldController : BaseController
    {
        public ActionResult Index(int id)
        {
            int GroupID = 0;
            if (id > 0)
                GroupID = id;
            else
                return RedirectToAction("Error", "Error", new { E = "InvalidLink" });

            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupUDFs, KMPlatform.Enums.Access.View))
            {

                KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
                ECN_Framework_Entities.Communicator.Group dbGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, User);

                if (dbGroup != null && dbGroup.GroupID > 0)
                {
                    List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList =
                        ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(GroupID, User, true);

                    List<DataFieldGridList> resultSet = (from src in groupDataFieldsList
                                                         select new DataFieldGridList
                                                         {
                                                             IsPublic = src.IsPublic == "N" ? "No" : "Yes",
                                                             GroupDataFieldsID = src.GroupDataFieldsID,
                                                             ShortName = src.ShortName,
                                                             GroupID = src.GroupID,
                                                             LongName = src.LongName,
                                                             CodeSnippet = "%%" + src.ShortName + "%%",
                                                             GroupingName = (src.DatafieldSetID == null) ? "" : (ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByDataFieldsetID(src.DatafieldSetID.Value, src.GroupID, false)).Name,
                                                             Transactional = (src.DatafieldSetID == null) ? "No" : "Yes",
                                                             UseDefaultValue = src.DefaultValue != null && src.DefaultValue.GDFID > 0 ? true : false,
                                                             DefaultType = src.DefaultValue != null && !string.IsNullOrWhiteSpace(src.DefaultValue.SystemValue) ? "system" : "default",
                                                             SystemValue = src.DefaultValue != null && !string.IsNullOrWhiteSpace(src.DefaultValue.SystemValue) ? src.DefaultValue.SystemValue : "",
                                                             DefaultValue = src.DefaultValue != null && !string.IsNullOrWhiteSpace(src.DefaultValue.DataValue) ? src.DefaultValue.DataValue : ""
                                                         }).ToList();

                    DataFields Model = new DataFields(resultSet, GroupID, dbGroup.GroupName);

                    //Write Permissions
                    if (KM.Platform.User.HasAccess(User, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.Edit))
                    {
                        Model.canWrite = true;
                    }

                    //Read Permissions
                    if (KM.Platform.User.HasAccess(User, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.View))
                    {
                        Model.canRead = true;
                    }
                    else
                    {
                        return RedirectToAction("Index", "Group");
                    }
                    return View(Model);
                }
                else
                {
                    return RedirectToAction("Error", "Error", new { E = "InvalidLink" });
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
        }

        public ActionResult LoadAddUDF(int NewGroupID)
        {
            ViewBag.UDFGroupID = NewGroupID;
            return PartialView("Partials/Modals/_AddUDF", new ECN_Framework_Entities.Communicator.GroupDataFields() );
        }

        public ActionResult AddUDF(ecn.communicator.mvc.Models.DataFieldGridList model)
        {
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            int GroupID = model.GroupID;
            string shortNameTxt = model.ShortName ?? "";
            shortNameTxt = shortNameTxt.Replace(" ", "_");
            ECN_Framework_Entities.Communicator.GroupDataFields groupDataFeilds = new ECN_Framework_Entities.Communicator.GroupDataFields();
            groupDataFeilds.ShortName = StringFunctions.CleanString(shortNameTxt);
            groupDataFeilds.LongName = StringFunctions.CleanString(model.LongName ?? "");
            groupDataFeilds.GroupID = GroupID;
            groupDataFeilds.IsPublic = model.IsPublic;
            groupDataFeilds.CreatedUserID = User.UserID;
            groupDataFeilds.CustomerID = User.CustomerID;
            if (model.DatafieldSetID > 0)
            {
                groupDataFeilds.DatafieldSetID = model.DatafieldSetID;
            }

            List<string> response = new List<string>();
            try
            {
                groupDataFeilds.GroupDataFieldsID = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(groupDataFeilds, User);

                if (model.UseDefaultValue)
                {
                    ECN_Framework_Entities.Communicator.GroupDataFieldsDefault gdfd = new ECN_Framework_Entities.Communicator.GroupDataFieldsDefault();
                    gdfd.GDFID = groupDataFeilds.GroupDataFieldsID;
                    if (model.DefaultType.ToLower().Equals("default"))
                    {
                        gdfd.DataValue = model.DefaultValue.Trim();
                        gdfd.SystemValue = "";
                    }
                    else
                    {
                        gdfd.DataValue = "";
                        gdfd.SystemValue = model.SystemValue;
                    }
                    ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.Save(gdfd);
                }
            }
            catch (ECNException ex)
            {
                string err = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                {
                    err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                response.Add("500");
                response.Add(err);
                return Json(response);
            }

            return LoadUDFGrid(GroupID);
        }

        public ActionResult AddTransaction (string TransactionName, int groupID)
        {
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            int GroupID = groupID;
            ECN_Framework_Entities.Communicator.DataFieldSets dataFieldSets = new ECN_Framework_Entities.Communicator.DataFieldSets();
            dataFieldSets.GroupID = GroupID;
            dataFieldSets.MultivaluedYN = "Y";
            dataFieldSets.Name = TransactionName;

            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.DataFieldSets.Save(dataFieldSets, User);
            }
            catch (ECNException ex)
            {
                string err = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                {
                    err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                response.Add("500");
                response.Add(err);
                return Json(response);
            }

            response.Add("200");
            response.Add("Done");
            return Json(response);
        }

        public ActionResult LoadCopyUDFs(int currentGroupID)
        {
            Models.CopyUDF model = new Models.CopyUDF();
            model.CurrentGroupID = currentGroupID;
            List<ECN_Framework_Entities.Communicator.Group> groupList =
            ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(ConvenienceMethods.GetCurrentUser().CustomerID, ConvenienceMethods.GetCurrentUser());
            var result = (from src in groupList
                          orderby src.GroupName
                          select src).ToList();

            result.Insert(0, new ECN_Framework_Entities.Communicator.Group() { GroupName = "--- Select Group Name ---", GroupID = 0 });
            model.Groups = result;
            return PartialView("Partials/Modals/_CopyUDF", model);
        }

        public string LoadCopyUDFsList(int sourceGroupID, int destGroupID)
        {
            int GroupID = sourceGroupID;
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFieldsList_source = 
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(GroupID, User);

            List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFieldsList_destination = 
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(destGroupID, User);

            List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFieldsList_sourceAvailable = 
                new List<ECN_Framework_Entities.Communicator.GroupDataFields>();

            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFieldSource in GroupDataFieldsList_source)
            {
                bool present = false;
                foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFieldDest in GroupDataFieldsList_destination)
                {
                    if (groupDataFieldSource.ShortName.ToLower().Equals(groupDataFieldDest.ShortName.ToLower()))
                    {
                        present = true;
                        break;
                    }
                }
                if (!present)
                    GroupDataFieldsList_sourceAvailable.Add(groupDataFieldSource);
            }

            string cb = string.Empty;
            if (GroupDataFieldsList_sourceAvailable.Count > 0)
            {
                cb = "<table style=\"width: 100 %;\"><tbody><tr style=\"background: #BFC0C2\"><th style=\"width:5%;\"></th>" +
                    "<th align =\"left\" scope=\"col\">Short Name</th><th align = \"left\" scope= \"col\" > Long Description</th><th align = \"left\" scope= \"col\" > Transaction Name</th></tr>";
                foreach (var udf in GroupDataFieldsList_sourceAvailable)
                {
                    cb += "<tr><td><input type=\"checkbox\" value=\"" + udf.GroupDataFieldsID + "\" style=\"width: 13px;height: 13px;padding: 0;margin: 0;\" checked=\"checked\" /></td>";
                    cb += "<td align=\"left\">" + udf.ShortName + "</td>";
                    cb += "<td align=\"left\">" + udf.LongName + "</td>";
                    string Name = (udf.DatafieldSetID == null) ? "" : (ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByDataFieldsetID(udf.DatafieldSetID.Value, udf.GroupID, false)).Name;
                    cb += "<td align=\"left\">" + Name + "</td></tr>";
                }
                cb += "</tbody></table>";
            }
            else
            {
                cb = "No UDFs Available";
            }
            return cb;
        }

        public ActionResult CopyUDFs(int[] selected, int sourceGroup, int currentGroup)
        {
            List<string> response = new List<string>();
            
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> listUDFs = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(currentGroup, User);
            List<string> missingUDFs = new List<string>();
            List<ECN_Framework_Entities.Communicator.Filter> filtersToCopy = new List<ECN_Framework_Entities.Communicator.Filter>();

            foreach (int udfID in selected)
            {
                try
                {
                    ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields = 
                        ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByID(udfID, sourceGroup, User);
                    groupDataFields.GroupID = currentGroup;
                    groupDataFields.GroupDataFieldsID = -1;
                    groupDataFields.CreatedUserID = User.UserID;
                    if (groupDataFields.DatafieldSetID > 0)
                    {
                        string TransactionName = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByDataFieldsetID(groupDataFields.DatafieldSetID.Value, sourceGroup, false).Name;

                        ECN_Framework_Entities.Communicator.DataFieldSets dfs = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByGroupIDName(currentGroup, TransactionName);

                        if (dfs == null)
                        {
                            ECN_Framework_Entities.Communicator.DataFieldSets dataFieldSets_source = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByDataFieldsetID(groupDataFields.DatafieldSetID.Value, sourceGroup, false);

                            ECN_Framework_Entities.Communicator.DataFieldSets dataFieldSets = new ECN_Framework_Entities.Communicator.DataFieldSets();
                            dataFieldSets.Name = dataFieldSets_source.Name;
                            dataFieldSets.GroupID = currentGroup;
                            dataFieldSets.MultivaluedYN = dataFieldSets_source.MultivaluedYN;

                            ECN_Framework_BusinessLayer.Communicator.DataFieldSets.Save(dataFieldSets, User);
                            groupDataFields.DatafieldSetID = dataFieldSets.DataFieldSetID;
                        }
                        else
                        {
                            groupDataFields.DatafieldSetID = dfs.DataFieldSetID;
                        }
                    }
                    groupDataFields.GroupDataFieldsID = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(groupDataFields, User);
                    ECN_Framework_Entities.Communicator.GroupDataFieldsDefault gdfd = ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.GetByGDFID(udfID);
                    if (gdfd != null && gdfd.GDFID > 0)
                    {
                        gdfd.GDFID = groupDataFields.GroupDataFieldsID;
                        ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.Save(gdfd);
                    }

                }
                catch (ECN_Framework_Common.Objects.ECNException ex)
                {
                    string err = string.Empty;
                    foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                    {
                        err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                    }
                    response.Add("500");
                    response.Add(err);
                    return Json(response);
                }
            }

            return LoadUDFGrid(currentGroup);
        }

        public ActionResult LoadEditUDFs(int GroupDataFieldsID)
        {
            return PartialView("Partials/Modals/_EditUDF", ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByID(GroupDataFieldsID, ConvenienceMethods.GetCurrentUser()));
        }

        public ActionResult EditUDFs(ecn.communicator.mvc.Models.DataFieldGridList model)
        {
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            int GroupID = model.GroupID;
            string shrtNm = StringFunctions.CleanString(model.ShortName ?? "");
            shrtNm = shrtNm.Replace("'", "");
            shrtNm = shrtNm.Replace(" ", "_");
            string longNm = StringFunctions.CleanString(model.LongName ?? "");
            ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields =
            ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByID(model.GroupDataFieldsID, model.GroupID, User);
            groupDataFields.ShortName = shrtNm;
            groupDataFields.LongName = longNm;
            groupDataFields.IsPublic = model.IsPublic;
            groupDataFields.UpdatedUserID = User.UserID;

            List<string> response = new List<string>();
            try
            {
                if (model.UseDefaultValue)
                {
                    ECN_Framework_Entities.Communicator.GroupDataFieldsDefault gdfd = ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.GetByGDFID(model.GroupDataFieldsID);
                    if (gdfd == null)
                        gdfd = new ECN_Framework_Entities.Communicator.GroupDataFieldsDefault();

                    gdfd.GDFID = model.GroupDataFieldsID;
                    if (model.DefaultType.ToLower().Equals("default"))
                    {
                        gdfd.DataValue = model.DefaultValue.Trim();
                        gdfd.SystemValue = "";
                    }
                    else
                    {
                        gdfd.DataValue = "";
                        gdfd.SystemValue = model.SystemValue;
                    }
                    ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(groupDataFields, User);
                    ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.Save(gdfd);
                }
                else
                {
                    ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.Delete(model.GroupDataFieldsID);
                    ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(groupDataFields, User);
                }
            }
            catch (ECNException ex)
            {
                string err = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                {
                    err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                response.Add("500");
                response.Add(err);
                return Json(response);
            }

            return LoadUDFGrid(GroupID);
        }

        public ActionResult DeleteUDF(int Id, int GroupID)
        {
            List<string> response = new List<string>();
            try
            {
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Delete(Id, GroupID, ConvenienceMethods.GetCurrentUser());
                ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.Delete(Id);
            }
            catch (ECNException ex)
            {
                string err = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ex.ErrorList)
                {
                    err += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                response.Add("500");
                response.Add(err);
                return Json(response);
            }

            return LoadUDFGrid(GroupID);
        }

        public ActionResult LoadUDFGrid(int GroupID)
        {
            KMPlatform.Entity.User User = ConvenienceMethods.GetCurrentUser();
            List<string> response = new List<string>();

            List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList =
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(GroupID, User);

            List<DataFieldGridList> resultSet = (from src in groupDataFieldsList
                                                 select new DataFieldGridList
                                                 {
                                                     IsPublic = src.IsPublic == "N" ? "No" : "Yes",
                                                     GroupDataFieldsID = src.GroupDataFieldsID,
                                                     ShortName = src.ShortName,
                                                     GroupID = src.GroupID,
                                                     LongName = src.LongName,
                                                     CodeSnippet = "%%" + src.ShortName + "%%",
                                                     GroupingName = (src.DatafieldSetID == null) ? "" : (ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByDataFieldsetID(src.DatafieldSetID.Value, src.GroupID, false)).Name,
                                                     Transactional = (src.DatafieldSetID == null) ? "No" : "Yes"
                                                 }).ToList();

            response.Add("200");
            response.Add(HtmlHelperMethods.RenderViewToString(this.ControllerContext, "Partials/_UDFsGrid", resultSet));
            return Json(response);
        }
    }
}