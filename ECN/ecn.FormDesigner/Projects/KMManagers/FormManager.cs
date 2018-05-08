using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using KMDbManagers;
using KMEntities;
using KMEnums;
using KMManagers.APITypes;
using KMModels;
using KMModels.PostModels;
using KMPlatform.Entity;
using BusinessLayerGroupDataFields = ECN_Framework_BusinessLayer.Communicator.GroupDataFields;
using CssFile = KMEntities.CssFile;
using Folder = KMManagers.APITypes.Folder;
using Group = KMManagers.APITypes.Group;
using Rule = KMEntities.Rule;

namespace KMManagers
{
    public class FormManager : ManagerBase
    {
        private const string FormSubmitted = "Confirmation Page Message";
        private const string FormInactive = "Inactive Page Message";
        private const string DOI_FromName = "KnowledgeMarketing";
        private const string DOI_Subject = "Double Opt-In Confirmation";
        private const string DOI_Message = "<br />Thank you for your form submission.<br /><br />Please, click on the link below to confirm your subscription.<br /><br />" +
                                            "<a href=\"%url%\">Link to Double Opt-In Landing Page</a><br /><br />If you are unable to click the link, please copy the link and paste " +
                                            "it within the address bar of your web browser.<br /><br /><br />Regards, ";
        private const string DOI_LandingPage = "Thank you for your form submission.<br /><br />Your subscription has been confirmed.";
        private const string KMAPIError = "KM API doesn't work";
        private const string FieldNameSubscriberIdentification = "Subscriber Identification";
        private const string ResultResponseKey = "result";
        private KMPlatform.Entity.User MasterUser = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["MasterAccessKey"].ToString(), false);

        private Dictionary<string, string> ControlField = null;

        private FormDbManager FM
        {
            get
            {
                return DB.FormDbManager;
            }
        }

        private static string FormatErrorMessage(ECNException ecnException)
        {
            var errorMessage = new StringBuilder();

            foreach (var ecnError in ecnException.ErrorList)
            {
                errorMessage.Append($"{ecnError.ErrorMessage}<br />");
            }

            return errorMessage.ToString();
        }

        private static void SetControlFieldMappings(ChangeGroupPostModel model, Form form, IDictionary<string, string> response)
        {
            if (form == null || form.GroupID == model.GroupId || model.Fields == null || !model.Fields.Any())
            {
                return;
            }

            var controls = form.Controls
                .Where(control => model.Fields.Select(field => field.ControlId).Contains(control.Control_ID))
                .ToList();

            foreach (var controlFieldModel in model.Fields)
            {
                var control = controlFieldModel.ControlId == 0
                                  ? new Control() { FieldLabel = FieldNameSubscriberIdentification }
                                  : controls.Single(x => x.Control_ID == controlFieldModel.ControlId);

                response.Add(control.FieldLabel, controlFieldModel.FieldName);
            }
        }

        public TModel GetByID<TModel>(int ChannelID, int id) where TModel : KMModels.ModelBase, new()
        {
            TModel res = null;
            Form form = GetByID(ChannelID, id);
            if (form != null)
            {
                res = form.ConvertToModel<TModel>();
            }

            return res;
        }

        internal Form GetByID(int ChannelID, int id)
        {
            return FM.GetByID(ChannelID, id);
        }

        internal Form GetByID_ChannelID(int ChannelID,int UserID, int id)
        {
            return FM.GetByID_ChannelID(ChannelID, id);
        }

        public IEnumerable<TModel> GetActiveByChannel<TModel>(int ChannelID) where TModel : KMModels.ModelBase, new()
        {
            return FM.GetActiveByChannel(ChannelID).ConvertToModels<TModel>();
        }

        public IEnumerable<TModel> GetInactiveByChannel<TModel>(int ChannelID) where TModel : KMModels.ModelBase, new()
        {
            return FM.GetInactiveByChannel(ChannelID).ConvertToModels<TModel>();
        }


        public bool CheckIfNeedPublishByCustomer(KMPlatform.Entity.User User, int ChannelID)
        {
            bool res = false;
            List<Form> forms = FM.GetMustPublishByChannel(ChannelID).ToList();
            foreach (var form in forms)
            {
                res = true;
                PublishFormByID(User, ChannelID, form.Form_Seq_ID);
            }

            return res;
        }

        public bool CheckNameIsUnique(int ChannelID, string name)
        {
            return CheckNameIsUnique(ChannelID, name, -1, -1);
        }

        public bool CheckNameIsUnique(int ChannelID, string name, int currId)
        {
            int parentId = -1;
            Form form = GetByID(ChannelID, currId);
            if (form != null && form.ParentForm_ID.HasValue)
            {
                parentId = form.ParentForm_ID.Value;
            }

            return CheckNameIsUnique(ChannelID, name, currId, parentId);
        }

        public bool CheckNameIsUnique(int ChannelID, string name, int currId, int parentId)
        {
            return FM.CheckNameIsUnique(ChannelID, name, currId, parentId);
        }

        public void ChangeActiveStateByID(int ChannelID, int id, FormActive state, DateTime? from, DateTime? to)
        {
            Form form = GetByID(ChannelID, id);
            form.Active = (int)state;
            form.ActivationDateFrom = from;
            form.ActivationDateTo = to;
            FM.SaveChanges();
        }

        public int Save(int UserID, FormPostModel model, string apiKey, ControlManager CM)
        {
            Form form = new Form();
            form.Name = model.Name;
            form.Active = (int)model.Active;
            form.ActivationDateFrom = model.ActivationFrom;
            form.ActivationDateTo = model.ActivationTo;
            form.FormType = model.Type.ToString();
            form.TokenUID = Guid.NewGuid();
            form.OptInType = (int)OptInType.Single;
            form.Status = FormStatus.Saved.ToString();
            //form.CssUri = DefaultCssUri;
            form.StylingType = (int)StylingType.Custom;
            form.LanguageTranslationType = true;
            form.Iframe = false;
            form.Delay = 0;
            SetApiData(UserID, apiKey, form, model);

            using (TransactionScope t = new TransactionScope())
            {
                //set default css (Moved to "onSave" custom styling)
                //CssFile css = new CssFile();
                //DB.CssFileDbManager.Add(css);
                //DB.CssFileDbManager.SaveChanges();
                //CssFileManager CssM = new CssFileManager();
                //CssM.CompareAndSplitMin(css.Name);
                //form.CssFile_Seq_ID = css.CssFile_Seq_ID;
                FM.Add(form);
                FM.SaveChanges();

                //add controls (email and pagebreak)
                Control email = new Control();
                email.Form_Seq_ID = form.Form_Seq_ID;
                email.Order = 0;
                email.Type_Seq_ID = (int)KMEnums.ControlType.Email;
                email.FieldLabel = KMEnums.ControlType.Email.ToString();
                email.FieldLabelHTML = KMEnums.ControlType.Email.ToString();
                email.HTMLID = Guid.NewGuid();
                Control pageBreak = new Control();
                pageBreak.Form_Seq_ID = form.Form_Seq_ID;
                pageBreak.Order = 1;
                pageBreak.Type_Seq_ID = (int)KMEnums.ControlType.PageBreak;
                pageBreak.FieldLabel = string.Empty; //KMEnums.ControlType.PageBreak.ToString();
                pageBreak.FieldLabelHTML = string.Empty;
                pageBreak.HTMLID = Guid.NewGuid();
                DB.ControlDbManager.Add(email);
                DB.ControlDbManager.Add(pageBreak);
                DB.ControlDbManager.SaveChanges();
                //add control properties
                ControlPropertyManager cpm = new ControlPropertyManager();
                CM.RewriteRequiredFormProperty(email, "1", cpm);
                CM.RewriteFormPropertyByName(email, HTMLGenerator.DataType_Property, ((int)TextboxDataTypes.Email).ToString(), cpm);
                CM.RewriteFormPropertyByName(pageBreak, HTMLGenerator.PreviousButton_Property, HTMLGenerator.PrevButtonDefaultText, cpm);
                CM.RewriteFormPropertyByName(pageBreak, HTMLGenerator.NextButton_Property, HTMLGenerator.NextButtonDefaultText, cpm);
                CM.SaveCPChanges();

                //add for results
                List<FormResult> results = new List<FormResult>();
                FormResult confirmation = new FormResult();
                confirmation.Form_Seq_ID = form.Form_Seq_ID;
                confirmation.ResultType = (int)FormResultType.ConfirmationPage;
                confirmation.Message = FormSubmitted;
                results.Add(confirmation);
                FormResult inactive = new FormResult();
                inactive.Form_Seq_ID = form.Form_Seq_ID;
                inactive.ResultType = (int)FormResultType.InactiveRedirect;
                inactive.Message = FormInactive;
                results.Add(inactive);
                DB.FormResultDbManager.AddResults(results.ToArray());
                DB.FormResultDbManager.SaveChanges();

                t.Complete();
            }

            return form.Form_Seq_ID;
        }

        public void Save(KMPlatform.Entity.User User, int ChannelID, FormPropertiesPostModel model)
        {
            Form form = FM.GetByID(ChannelID, model.Id);
            form.UpdatedBy = User.UserName;
            form.LastUpdated = DateTime.Now;
            FillData(form, model);
            Notification doubleOptIn = form.Notifications.SingleOrDefault(x => x.IsDoubleOptIn);
            bool isNeedAdd = false;
            if (form.OptInType == (int)OptInType.Double && doubleOptIn == null)
            {
                doubleOptIn = new Notification();
                doubleOptIn.Form_Seq_ID = form.Form_Seq_ID;
                doubleOptIn.IsDoubleOptIn = true;
                doubleOptIn.FromName = DOI_FromName;
                doubleOptIn.Subject = DOI_Subject;
                doubleOptIn.Message = DOI_Message;
                doubleOptIn.LandingPage = DOI_LandingPage;
                isNeedAdd = true;
            }

            List<FormResult> results = new List<FormResult>();
            if (model.ConfirmationPageType.HasValue)
            {
                FormResult fr = form.FormResults.SingleOrDefault(x => x.ResultType == (int)FormResultType.ConfirmationPage);
                if (fr == null)
                {
                    fr = new FormResult();
                    fr.Form_Seq_ID = model.Id;
                    fr.ResultType = (int)FormResultType.ConfirmationPage;
                    results.Add(fr);
                }
                else
                {
                    fr.URL = fr.Message = fr.JsMessage = null;
                }
                if (model.ConfirmationPageType.Value == ResultType.URL)
                {
                    fr.URL = model.ConfirmationPageUrl;
                }
                else if (model.ConfirmationPageType.Value == ResultType.Message)
                {
                    fr.Message = model.ConfirmationPageMessage;
                    if (model.ConfirmationPageJsMessage != null)
                    {
                        fr.JsMessage = model.ConfirmationPageJsMessage;
                    }
                }
                else if (model.ConfirmationPageType.Value == ResultType.MessageAndURL)
                {
                    fr.URL = model.ConfirmationPageMAUUrl;
                    fr.Message = model.ConfirmationPageMAUMessage;
                    if (model.ConfirmationPageJsMAUMessage != null)
                    {
                        fr.JsMessage = model.ConfirmationPageJsMAUMessage;
                    }
                }
            }
            if (model.InactiveRedirectType.HasValue)
            {
                FormResult fr = form.FormResults.SingleOrDefault(x => x.ResultType == (int)FormResultType.InactiveRedirect);
                if (fr == null)
                {
                    fr = new FormResult();
                    fr.Form_Seq_ID = model.Id;
                    fr.ResultType = (int)FormResultType.InactiveRedirect;
                    results.Add(fr);
                }
                else
                {
                    fr.URL = fr.Message = null;
                }
                if (model.InactiveRedirectType.Value == ResultType.URL)
                {
                    fr.URL = model.InactiveRedirectUrl;
                }
                else if (model.InactiveRedirectType.Value == ResultType.Message)
                {
                    fr.Message = model.InactiveRedirectMessage;
                }
            }

            using (TransactionScope t = new TransactionScope())
            {
                if (isNeedAdd)
                {
                    DB.NotificationDbManager.Add(doubleOptIn);
                    DB.NotificationDbManager.SaveChanges();
                }
                if (results.Count > 0)
                {
                    DB.FormResultDbManager.AddResults(results.ToArray());
                    DB.FormResultDbManager.SaveChanges();
                }
                FM.SaveChanges();

                t.Complete();
            }
        }

        public IDictionary<string, string> SaveGroups(int channelId, ChangeGroupPostModel model, string apiKey, User user, out string errorRes)
        {
            var result = true;
            var response = new Dictionary<string, string>();
            errorRes = null;
            var form = FM.GetByID(channelId, model.FormId);
            if (form != null)
            {
                form.UpdatedBy = user.UserName;
                result = GetResultForGroupPostModel(model, user, form, ref errorRes);
            }

            response.Add(ResultResponseKey, result.ToString());

            if (model.ChangeFormGroup)
            {
                RewriteFields(model, form);

                SetApiData(user.UserID, apiKey, form, model);
            }
            else
            {
                SetControlFieldMappings(model, form, response);
            }

            FM.SaveChanges();

            return response;
        }

        private bool GetResultForGroupPostModel(ChangeGroupPostModel model, User user, Form form, ref string errorRes)
        {
            var result = true;

            if (form.GroupID == model.GroupId || model.Fields == null || !model.Fields.Any())
            {
                return true;
            }

            try
            {
                var customer = GetCustomerByID(model.CustomerId);
                var fields = GetFieldsByCustomerAndGroupID(model.GroupId).ToList();

                foreach (var fieldModel in model.Fields)
                {
                    var field = fields.SingleOrDefault(
                        dataFields => dataFields.ShortName.Equals(
                            fieldModel.FieldName,
                            StringComparison.OrdinalIgnoreCase));

                    if (fieldModel.FieldId.HasValue)
                    {
                        continue;
                    }

                    if (field == null)
                    {
                        var groupDataFields = new GroupDataFields
                        {
                            CustomerID = customer.CustomerID,
                            GroupID = model.GroupId,
                            ShortName = fieldModel.FieldName,
                            LongName = fieldModel.FieldName,
                            CreatedUserID = user.UserID,
                            IsPublic = "Y",
                            IsDeleted = false,
                            IsPrimaryKey = false
                        };

                        groupDataFields.GroupDataFieldsID = BusinessLayerGroupDataFields.Save(groupDataFields, user);

                        if (groupDataFields.GroupDataFieldsID < 0)
                        {
                            result = false;
                            errorRes = null;
                            break;
                        }

                        fields.Add(groupDataFields);
                        fieldModel.FieldId = groupDataFields.GroupDataFieldsID;
                    }
                    else
                    {
                        fieldModel.FieldId = field.GroupDataFieldsID;
                    }
                }
            }
            catch (ECNException ecnException)
            {
                errorRes = FormatErrorMessage(ecnException);
                result = false;
            }
            catch (Exception exception)
            {
                result = false;
                errorRes = exception.Message;
            }

            return result;
        }

        private void RewriteFields(ChangeGroupPostModel model, Form form)
        {
            if (form == null || form.GroupID == model.GroupId || model.Fields == null || !model.Fields.Any())
            {
                return;
            }

            var controls = form.Controls
                .Where(control => model.Fields.Select(field => field.ControlId).Contains(control.Control_ID))
                .ToList();

            foreach (var controlFieldModel in model.Fields)
            {
                if (controlFieldModel.ControlId == 0)
                {
                    foreach (var subscriberLogin in form.SubscriberLogins)
                    {
                        int fieldId;
                        if (int.TryParse(subscriberLogin.OtherIdentification, out fieldId))
                        {
                            subscriberLogin.OtherIdentification = controlFieldModel.FieldId.ToString();
                        }
                    }

                    DB.SubscriberLoginDbManager.SaveChanges();
                }
                else
                {
                    var controlFieldModelControl =
                        controls.Single(control => control.Control_ID == controlFieldModel.ControlId);
                    controlFieldModelControl.FieldID = controlFieldModel.FieldId;
                }
            }
        }

        private void FillData(Form form, FormPropertiesPostModel model)
        {
            form.Name = model.Name;
            form.Active = (int)model.Active;
            form.ActivationDateFrom = model.ActivationFrom;
            form.ActivationDateTo = model.ActivationTo;
            form.OptInType = (int)model.OptInType;
            form.LanguageTranslationType = model.LanguageTranslationType;
            form.Iframe = model.Iframe;
            form.Delay = model.Delay;
            form.SubmitButtonText = model.SubmitButtonText;
            if (string.IsNullOrEmpty(form.SubmitButtonText))
            {
                form.SubmitButtonText = FormDbManager.SubmitButtonDefaultText;
            }
            form.LastUpdated = DateTime.Now;
        }

        public int FullCopyByID(int UserID,int ChannelID, int id)
        {
            Form parent = GetByID(ChannelID, id);
            parent.Status = FormStatus.Updated.ToString();
            parent.PublishAfter = null;
            Form child = new Form();
            child.ParentForm_ID = parent.Form_Seq_ID;
            child.Name = parent.Name;
            child.Active = parent.Active;
            child.ActivationDateFrom = parent.ActivationDateFrom;
            child.ActivationDateTo = parent.ActivationDateTo;
            child.FormType = parent.FormType;
            child.TokenUID = parent.TokenUID;
            CopyApiData(child, parent);

            return FullCopy(child, parent);
        }

        public int FullCopy(int UserID, int ChannelID, FormPostModel model, string apiKey, Dictionary<string, string> cf)
        {
            Form parent = GetByID(ChannelID, model.Id);
            Form child = new Form();
            child.Name = model.Name;
            child.ActivationDateFrom = model.ActivationFrom;
            child.ActivationDateTo = model.ActivationTo;
            child.Active = (int)model.Active;
            child.FormType = model.Type;
            child.TokenUID = Guid.NewGuid();
            SetApiData(UserID, apiKey, child, model);
            if (cf != null)
                ControlField = cf;

            return FullCopy(child, parent);
        }

        private int FullCopy(Form child, Form parent)
        {
            Guard.NotNull(child, nameof(child));
            Guard.NotNull(parent, nameof(parent));

            FullCopyChild(child, parent);

            var lstControls = new Dictionary<int, int>();
            var lstCGroups = new Dictionary<int, int>();
            var lstItems = new Dictionary<int, int>();
            using (var transactionScope = new TransactionScope())
            {
                if (parent.CssFile_Seq_ID.HasValue)
                {
                    var css = new CssFile();
                    DB.CssFileDbManager.Add(css, parent.CssFile_Seq_ID.Value);
                    DB.CssFileDbManager.SaveChanges();
                    child.CssFile_Seq_ID = css.CssFile_Seq_ID;
                }
                FM.Add(child);
                FM.SaveChanges();
                var newId = child.Form_Seq_ID;

                var fieldsParent = GetFieldsByCustomerAndGroupID(parent.GroupID).ToList();
                var fieldsChild = GetFieldsByCustomerAndGroupID(child.GroupID).ToList();
                
                foreach (var control in parent.Controls)
                {
                    var newControl = FullCopyParentNewControl(newId, control, fieldsChild, fieldsParent);
                    DB.ControlDbManager.Add(newControl);
                    DB.ControlDbManager.SaveChanges();
                    lstControls.Add(control.Control_ID, newControl.Control_ID);

                    FullCopyParentFormControlProperties(control, newControl);
                    DB.FormControlPropertyDbManager.SaveChanges();

                    FullCopyParentGrids(control, newControl, lstItems);
                    FullCopyParentControlCategories(control, newControl);
                    FullCopyParentNewsletterGroups(control, newControl);
                    FullCopyParentRules(control, lstCGroups, newControl);
                }

                FullCopyRules(parent, lstCGroups, newId, lstControls);
                FullCopyNotifications(parent, lstCGroups, newId, lstControls);
                DB.NotificationDbManager.SaveChanges();

                ConditionManager.CopyAllByForm(parent, lstControls, lstCGroups, lstItems);

                FullCopyFormResults(parent, newId, lstControls);
                FulllCopySubscriberLogin(parent, newId, fieldsChild, fieldsParent);
                DB.SubscriberLoginDbManager.SaveChanges();

                transactionScope.Complete();
            }

            return child.Form_Seq_ID;
        }

        private Control FullCopyParentNewControl(
            int newId, 
            Control control, 
            IEnumerable<GroupDataFields> fieldsChild, 
            IEnumerable<GroupDataFields> fieldsParent)
        {
            Guard.NotNull(control, nameof(control));

            var newControl = new Control();
            newControl.Form_Seq_ID = newId;
            newControl.Order = control.Order;
            newControl.Type_Seq_ID = control.Type_Seq_ID;
            newControl.FieldLabel = control.FieldLabel;
            newControl.FieldLabelHTML = control.FieldLabelHTML;
            // Get Fields by Group
            GroupDataFields field = null;
            if (control.FieldID != null && control.FieldID > 0)
            {
                if (ControlField != null && ControlField.Count > 1)
                {
                    var fieldShortName = ControlField[control.FieldLabel] ?? string.Empty;
                    field = fieldsChild.SingleOrDefault(
                        x => string.Equals(x.ShortName, fieldShortName, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    field = fieldsParent.SingleOrDefault(x => x.GroupDataFieldsID == control.FieldID);
                    field = fieldsChild.SingleOrDefault(
                        x => string.Equals(x.ShortName, field?.ShortName, StringComparison.OrdinalIgnoreCase));
                }
            }

            newControl.FieldID = field?.GroupDataFieldsID ?? control.FieldID;
            newControl.HTMLID = control.HTMLID;
            return newControl;
        }

        private void FullCopyParentRules(Control control, Dictionary<int, int> lstCGroups, Control newControl)
        {
            foreach (var rule in control.Rules)
            {
                var groupId = DB.ConditionGroupDbManager.CopyGroup(rule.ConditionGroup, lstCGroups);
                DB.RuleDbManager.AddRule(rule, null, newControl.Control_ID, groupId);
            }
        }

        private void FullCopyParentControlCategories(Control control, Control newControl)
        {
            Guard.NotNull(control, nameof(control));

            foreach (var controlCategory in control.ControlCategories)
            {
                var newControlCategory = new ControlCategory();
                newControlCategory.Control_ID = newControl.Control_ID;
                newControlCategory.LabelHTML = controlCategory.LabelHTML;
                newControlCategory.Order = controlCategory.Order;
                DB.ControlCategoryDbManager.Add(newControlCategory);
                DB.ControlCategoryDbManager.SaveChanges();
            }
        }

        private void FullCopyParentFormControlProperties(Control control, Control newControl)
        {
            Guard.NotNull(control, nameof(control));

            foreach (var formControlProperty in control.FormControlProperties)
            {
                var newProperty = new FormControlProperty();
                newProperty.Control_ID = newControl.Control_ID;
                newProperty.ControlProperty_ID = formControlProperty.ControlProperty_ID;
                newProperty.Value = formControlProperty.Value;
                DB.FormControlPropertyDbManager.Add(newProperty);
            }
        }

        private void FullCopyParentNewsletterGroups(Control control, Control newControl)
        {
            Guard.NotNull(control, nameof(control));

            foreach (var newsletterGroup in control.NewsletterGroups)
            {
                var newnewsletterGroup = new NewsletterGroup();
                newnewsletterGroup.Control_ID = newControl.Control_ID;
                newnewsletterGroup.CustomerID = newsletterGroup.CustomerID;
                newnewsletterGroup.GroupID = newsletterGroup.GroupID;
                if (newsletterGroup.ControlCategoryID.HasValue)
                {
                    var controlCategoryById =
                        DB.ControlCategoryDbManager.GetByID(newsletterGroup.ControlCategoryID.Value);
                    var controlCategoryByName =
                        DB.ControlCategoryDbManager.GetByName(newnewsletterGroup.Control_ID, controlCategoryById.LabelHTML);
                    if (controlCategoryByName == null)
                    {
                        newnewsletterGroup.ControlCategoryID = null;
                    }
                    else
                    {
                        newnewsletterGroup.ControlCategoryID = controlCategoryByName.ControlCategoryID;
                    }
                }
                else
                {
                    newnewsletterGroup.ControlCategoryID = null;
                }

                newnewsletterGroup.Order = newsletterGroup.Order;
                newnewsletterGroup.IsPreSelected = newsletterGroup.IsPreSelected;
                newnewsletterGroup.LabelHTML = newsletterGroup.LabelHTML;
                foreach (var groupUdf in newsletterGroup.NewsletterGroupUDFs)
                {
                    DB.NewsletterGroupUDFDbManager.RemoveAll(newsletterGroup.GroupID);
                    DB.NewsletterGroupUDFDbManager.SaveChanges();
                    var newsletterGroupUdf = new NewsletterGroupUDF();
                    newsletterGroupUdf.FormGroupDataFieldID = groupUdf.FormGroupDataFieldID;
                    newsletterGroupUdf.NewsletterDataFieldID = groupUdf.NewsletterDataFieldID;

                    newnewsletterGroup.NewsletterGroupUDFs.Add(newsletterGroupUdf);
                }

                DB.NewsletterGroupDbManager.Add(newnewsletterGroup);
                DB.NewsletterGroupDbManager.SaveChanges();
            }
        }

        private void FullCopyParentGrids(Control control, Control newControl, Dictionary<int, int> lstItems)
        {
            Guard.NotNull(control, nameof(control));

            foreach (var formControlPropertyGrid in control.FormControlPropertyGrids)
            {
                var newPropertyGrid = new FormControlPropertyGrid();
                newPropertyGrid.Control_ID = newControl.Control_ID;
                newPropertyGrid.ControlProperty_ID = formControlPropertyGrid.ControlProperty_ID;
                newPropertyGrid.DataValue = formControlPropertyGrid.DataValue;
                newPropertyGrid.DataText = formControlPropertyGrid.DataText;
                newPropertyGrid.IsDefault = formControlPropertyGrid.IsDefault;
                newPropertyGrid.CategoryID = formControlPropertyGrid.CategoryID;
                newPropertyGrid.CategoryName = formControlPropertyGrid.CategoryName;
                newPropertyGrid.Order = formControlPropertyGrid.Order;
                DB.FormControlPropertyGridDbManager.Add(newPropertyGrid);
                DB.FormControlPropertyGridDbManager.SaveChanges();
                lstItems.Add(
                    formControlPropertyGrid.FormControlPropertyGrid_Seq_ID,
                    newPropertyGrid.FormControlPropertyGrid_Seq_ID);
            }
        }

        private void FullCopyRules(
            Form parent, 
            Dictionary<int, int> lstCGroups, 
            int newId, 
            Dictionary<int, int> lstControls)
        {
            Guard.NotNull(parent, nameof(parent));

            foreach (var rule in parent.Rules)
            {
                var groupId = DB.ConditionGroupDbManager.CopyGroup(rule.ConditionGroup, lstCGroups);
                DB.RuleDbManager.AddRule(rule, newId, null, groupId, lstControls);
            }
        }

        private void FullCopyFormResults(Form parent, int newId, Dictionary<int, int> lstControls)
        {
            Guard.NotNull(parent, nameof(parent));

            foreach (var formResult in parent.FormResults)
            {
                var newFormResult = new FormResult();
                newFormResult.Form_Seq_ID = newId;
                newFormResult.ResultType = formResult.ResultType;
                newFormResult.Message = formResult.Message;
                newFormResult.JsMessage = formResult.JsMessage;
                newFormResult.URL = formResult.URL;
                DB.FormResultDbManager.Add(newFormResult);
                DB.FormResultDbManager.SaveChanges();
                ThirdPartyQueryValueManager.CopyAllByFR(formResult, newFormResult.FormResult_Seq_ID, lstControls);
            }
        }

        private void FullCopyNotifications(
            Form parent, 
            Dictionary<int, int> lstCGroups, 
            int newId, 
            Dictionary<int, int> lstControls)
        {
            Guard.NotNull(parent, nameof(parent));

            foreach (var notification in parent.Notifications)
            {
                int? groupId = null;
                if (notification.ConditionGroup_Seq_ID.HasValue)
                {
                    groupId = DB.ConditionGroupDbManager.CopyGroup(notification.ConditionGroup, lstCGroups);
                }

                var newN = new Notification();
                newN.Form_Seq_ID = newId;
                newN.ConditionGroup_Seq_ID = groupId;
                newN.IsConfirmation = notification.IsConfirmation;
                newN.IsInternalUser = notification.IsInternalUser;
                newN.IsDoubleOptIn = notification.IsDoubleOptIn;
                newN.FromName = notification.FromName;
                newN.ToEmail = notification.ToEmail;
                newN.Subject = notification.Subject;
                newN.Message = RewriteSnippent(lstControls, notification.Message);
                newN.LandingPage = notification.LandingPage;
                DB.NotificationDbManager.Add(newN);
            }
        }

        private void FulllCopySubscriberLogin(
            Form parent, 
            int newId, 
            IReadOnlyCollection<GroupDataFields> fieldsChild, 
            IReadOnlyCollection<GroupDataFields> fieldsParent)
        {
            Guard.NotNull(parent, nameof(parent));

            foreach (var subscriberLogin in parent.SubscriberLogins)
            {
                var newSLogin = new SubscriberLogin();
                newSLogin.FormID = newId;
                newSLogin.LoginRequired = subscriberLogin.LoginRequired;

                int fieldId;
                GroupDataFields field = null;
                if (int.TryParse(subscriberLogin.OtherIdentification, out fieldId))
                {
                    if (ControlField != null && ControlField.Count > 1)
                    {
                        var fieldShortName = ControlField[FieldNameSubscriberIdentification] ?? string.Empty;
                        field = fieldsChild.SingleOrDefault(
                            x => string.Equals(x.ShortName, fieldShortName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        field = fieldsParent.SingleOrDefault(x => x.GroupDataFieldsID == fieldId);
                        field = fieldsChild.SingleOrDefault(
                            x => string.Equals(x.ShortName, field.ShortName, StringComparison.OrdinalIgnoreCase));
                    }

                    newSLogin.OtherIdentification =
                        field?.GroupDataFieldsID.ToString() ?? fieldId.ToString();
                }
                else
                {
                    newSLogin.OtherIdentification = subscriberLogin.OtherIdentification;
                }

                newSLogin.PasswordRequired = subscriberLogin.PasswordRequired;
                newSLogin.AutoLoginAllowed = subscriberLogin.AutoLoginAllowed;
                newSLogin.LoginModalHTML = subscriberLogin.LoginModalHTML;
                newSLogin.LoginButtonText = subscriberLogin.LoginButtonText;
                newSLogin.SignUpButtonText = subscriberLogin.SignUpButtonText;
                newSLogin.ForgotPasswordButtonText = subscriberLogin.ForgotPasswordButtonText;
                newSLogin.NewSubscriberButtonText = subscriberLogin.NewSubscriberButtonText;
                newSLogin.ExistingSubscriberButtonText = subscriberLogin.ExistingSubscriberButtonText;
                newSLogin.ForgotPasswordMessageHTML = subscriberLogin.ForgotPasswordMessageHTML;
                newSLogin.ForgotPasswordNotificationHTML = subscriberLogin.ForgotPasswordNotificationHTML;
                newSLogin.ForgotPasswordFromName = subscriberLogin.ForgotPasswordFromName;
                newSLogin.ForgotPasswordSubject = subscriberLogin.ForgotPasswordSubject;
                newSLogin.EmailAddressQuerystringName = subscriberLogin.EmailAddressQuerystringName;
                newSLogin.OtherQuerystringName = subscriberLogin.OtherQuerystringName;
                newSLogin.PasswordQuerystringName = subscriberLogin.PasswordQuerystringName;
                DB.SubscriberLoginDbManager.Add(newSLogin);
            }
        }

        private static void FullCopyChild(Form child, Form parent)
        {
            Guard.NotNull(child, nameof(child));
            Guard.NotNull(parent, nameof(parent));

            child.OptInType = parent.OptInType;
            child.SubmitButtonText = parent.SubmitButtonText;
            child.Status = FormStatus.Saved.ToString();
            child.HeaderHTML = parent.HeaderHTML;
            child.FooterHTML = parent.FooterHTML;
            child.HeaderJs = parent.HeaderJs;
            child.FooterJs = parent.FooterJs;
            child.UpdatedBy = parent.UpdatedBy;
            child.PublishAfter = parent.PublishAfter;
            child.CssUri = parent.CssUri;
            child.StylingType = parent.StylingType;
            child.LanguageTranslationType = parent.LanguageTranslationType;
            child.Iframe = parent.Iframe;
            child.Delay = parent.Delay;
        }

        //private int FullCopy(Form child, Form parent, FormPropertiesPostModel model)
        //{
        //    child.Status = (int)FormStatus.Saved;
        //    child.HeaderHTML = parent.HeaderHTML;
        //    child.FooterHTML = parent.FooterHTML;
        //    child.LastPublished = parent.LastPublished;
        //    child.UpdatedBy = parent.UpdatedBy;
        //    child.CssUri = parent.CssUri;

        //    Dictionary<int, int> lstControls = new Dictionary<int, int>();
        //    Dictionary<int, int> lstCGroups = new Dictionary<int, int>();
        //    using (TransactionScope t = new TransactionScope())
        //    {
        //        if (parent.CssFile_Seq_ID.HasValue)
        //        {
        //            CssFile css = new CssFile();
        //            DB.CssFileDbManager.Add(css, parent.CssFile_Seq_ID.Value);
        //            DB.CssFileDbManager.SaveChanges();
        //            child.CssFile_Seq_ID = css.CssFile_Seq_ID;
        //        }
        //        FM.Add(child);
        //        FM.SaveChanges();
        //        int newId = child.Form_Seq_ID;

        //        foreach (var c in parent.Controls)
        //        {
        //            Control nc = new Control();
        //            nc.Form_Seq_ID = newId;
        //            nc.Order = c.Order;
        //            nc.Type_Seq_ID = c.Type_Seq_ID;
        //            nc.FieldLabel = c.FieldLabel;
        //            nc.FieldID = c.FieldID;
        //            nc.HTMLID = c.HTMLID;
        //            DB.ControlDbManager.Add(nc);
        //            DB.ControlDbManager.SaveChanges();
        //            lstControls.Add(c.Control_ID, nc.Control_ID);

        //            foreach (var p in c.FormControlProperties)
        //            {
        //                FormControlProperty newP = new FormControlProperty();
        //                newP.Control_ID = nc.Control_ID;
        //                newP.ControlProperty_ID = p.ControlProperty_ID;
        //                newP.Value = p.Value;
        //                DB.FormControlPropertyDbManager.Add(newP);
        //            }
        //            DB.FormControlPropertyDbManager.SaveChanges();

        //            foreach (var pg in c.FormControlPropertyGrids)
        //            {
        //                FormControlPropertyGrid newPG = new FormControlPropertyGrid();
        //                newPG.Control_ID = nc.Control_ID;
        //                newPG.ControlProperty_ID = pg.ControlProperty_ID;
        //                newPG.DataValue = pg.DataValue;
        //                newPG.DataText = pg.DataText;
        //                newPG.IsDefault = pg.IsDefault;
        //                DB.FormControlPropertyGridDbManager.Add(newPG);
        //            }
        //            DB.FormControlPropertyGridDbManager.SaveChanges();

        //            foreach (var r in c.Rules)
        //            {
        //                int groupId = DB.ConditionGroupDbManager.CopyGroup(r.ConditionGroup, lstCGroups);
        //                DB.RuleDbManager.AddRule(r, null, nc.Control_ID, groupId);
        //            }
        //        }

        //        foreach (var r in parent.Rules)
        //        {
        //            int groupId = DB.ConditionGroupDbManager.CopyGroup(r.ConditionGroup, lstCGroups);
        //            DB.RuleDbManager.AddRule(r, newId, null, groupId);
        //        }

        //        foreach (var n in parent.Notifications)
        //        {
        //            int? groupId = null;
        //            if (n.ConditionGroup_Seq_ID.HasValue)
        //            {
        //                groupId = DB.ConditionGroupDbManager.CopyGroup(n.ConditionGroup, lstCGroups);
        //            }
        //            Notification newN = new Notification();
        //            newN.Form_Seq_ID = newId;
        //            newN.ConditionGroup_Seq_ID = groupId;
        //            newN.IsConfirmation = n.IsConfirmation;
        //            newN.IsInternalUser = n.IsInternalUser;
        //            newN.FromEmail = n.FromEmail;
        //            newN.FromName = n.FromName;
        //            newN.ToEmail = n.ToEmail;
        //            newN.ReplyEmail = n.ReplyEmail;
        //            newN.Subject = n.Subject;
        //            newN.Message = n.Message;
        //            DB.NotificationDbManager.Add(newN);
        //        }
        //        DB.NotificationDbManager.SaveChanges();

        //        ConditionManager.CopyAllByForm(parent, lstControls, lstCGroups);

        //        IEnumerable<FormResult> formResults = parent.FormResults;//.Where(x => model == null || x.ResultType == (int)FormResultType.ThirdpartyOutput);
        //        foreach (var r in formResults)
        //        {
        //            FormResult newR = new FormResult();
        //            newR.Form_Seq_ID = newId;
        //            newR.ResultType = r.ResultType;
        //            newR.Message = r.Message;
        //            newR.URL = r.URL;
        //            DB.FormResultDbManager.Add(newR);
        //            DB.FormResultDbManager.SaveChanges();
        //            ThirdPartyQueryValueManager.CopyAllByFR(r, newR.FormResult_Seq_ID, lstControls);
        //        }
        //        //if (model != null)
        //        //{
        //        //    if (model.ConfirmationPageType.HasValue)
        //        //    {
        //        //        FormResult fr = new FormResult();
        //        //        fr.Form_Seq_ID = newId;
        //        //        fr.ResultType = (int)FormResultType.ConfirmationPage;
        //        //        if (model.ConfirmationPageType.Value == ResultType.URL)
        //        //        {
        //        //            fr.URL = model.ConfirmationPageUrl;
        //        //        }
        //        //        else if (model.ConfirmationPageType.Value == ResultType.Message)
        //        //        {
        //        //            fr.Message = model.ConfirmationPageMessage;
        //        //        }
        //        //        DB.FormResultDbManager.Add(fr);
        //        //    }
        //        //    if (model.InactiveRedirectType.HasValue)
        //        //    {
        //        //        FormResult fr = new FormResult();
        //        //        fr.Form_Seq_ID = newId;
        //        //        fr.ResultType = (int)FormResultType.InactiveRedirect;
        //        //        if (model.InactiveRedirectType.Value == ResultType.URL)
        //        //        {
        //        //            fr.URL = model.InactiveRedirectUrl;
        //        //        }
        //        //        else if (model.InactiveRedirectType.Value == ResultType.Message)
        //        //        {
        //        //            fr.Message = model.InactiveRedirectMessage;
        //        //        }
        //        //        DB.FormResultDbManager.Add(fr);
        //        //    }
        //        //    if (!string.IsNullOrEmpty(model.ResponseConfirmationPage))
        //        //    {
        //        //        FormResult fr = new FormResult();
        //        //        fr.Form_Seq_ID = newId;
        //        //        fr.ResultType = (int)FormResultType.ResponseConfirmationPage;
        //        //        fr.URL = model.ResponseConfirmationPage;
        //        //        DB.FormResultDbManager.Add(fr);
        //        //    }
        //        //    DB.FormResultDbManager.SaveChanges();
        //        //}

        //        t.Complete();
        //    }

        //    return child.Form_Seq_ID;
        //}

        private void SetApiData(int UserID, string apiKey, Form form, FormPostModelBase model)
        {
            form.CustomerID = model.CustomerId.Value;
            form.UserID = UserID;
            ECN_Framework_Entities.Accounts.Customer c = GetCustomerByID(model.CustomerId.Value);
            form.CustomerName = c.CustomerName;
            form.CustomerAccessKey = ConfigurationManager.AppSettings["MasterAccessKey"].ToString();// c.AccessKey;
            form.GroupID = model.GroupId.Value;
        }

        private void SetApiData(int UserID, string apiKey, Form form, ChangeGroupPostModel model)
        {            
            form.CustomerID = model.CustomerId;
            form.UserID = UserID;
            ECN_Framework_Entities.Accounts.Customer c = GetCustomerByID(model.CustomerId);
            form.CustomerName = c.CustomerName;
            form.CustomerAccessKey = ConfigurationManager.AppSettings["MasterAccessKey"].ToString(); //c.AccessKey;            
            form.GroupID = model.GroupId;            
        }

        private void CopyApiData(Form child, Form parent)
        {
            child.UserID = parent.UserID;
            child.CustomerID = parent.CustomerID;
            child.CustomerName = parent.CustomerName;
            child.CustomerAccessKey = parent.CustomerAccessKey;
            child.GroupID = parent.GroupID;
        }

        public void PublishAfterByID(int UserID,int ChannelID, int id, DateTime? publishDate)
        {
            Form form = FM.GetByID(ChannelID, id);
            form.PublishAfter = publishDate;
            SaveChanges();
        }

        public void PublishFormByID(KMPlatform.Entity.User User,int ChannelID, int id)
        {
            Form form = FM.GetByID(ChannelID, id);
            if (form.Status != FormStatus.Published.ToString())
            {
                form.LastPublished = DateTime.Now;
                form.Status = FormStatus.Published.ToString();
                form.PublishAfter = null;
                form.UpdatedBy = User.UserName;
                int parentID = form.ParentForm_ID.HasValue ? form.ParentForm_ID.Value : 0;
                if (form.ParentForm_ID.HasValue)
                {
                    DeleteByID(User.UserID, ChannelID, form.ParentForm_ID.Value, form);
                }
                FM.SaveChanges();
                if (parentID > 0)
                    ECN_Framework_BusinessLayer.Communicator.MAControl.UpdateECNID(id, parentID);
            }
        }

        public void DeleteByID(int UserID, int ChannelID,int id)
        {
            DeleteByID(UserID, ChannelID, id, null);
        }

        private void DeleteByID(int UserID,int ChannelID, int id, Form child)
        {
            Form form = GetByID(ChannelID, id);
            if (child != null || (form !=null && form.Status == FormStatus.Saved.ToString()))
            {
                using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 6, 0)))
                {
                    IEnumerable<int> controlIDs = form.Controls.Select(x => x.Control_ID);
                    IEnumerable<Rule> rules = DB.RuleDbManager.GetAllByFormOrControls(id, controlIDs);
                    IEnumerable<int> ruleIDs = rules.Select(x => x.ConditionGroup_Seq_ID);
                    IEnumerable<int> notificationIDs = form.Notifications.Where(x => x.ConditionGroup_Seq_ID.HasValue).Select(x => x.ConditionGroup_Seq_ID.Value);
                    IEnumerable<ConditionGroup> groups = DB.ConditionGroupDbManager.GetAllByRulesAndNotifications(ruleIDs, notificationIDs);
                    ICollection<ThirdPartyQueryValue> tpq = DB.ThirdPartyQueryValueDbManager.GetAllByFormID(id).ToList();

                    DB.RuleDbManager.Remove(rules);
                    DB.ConditionGroupDbManager.Remove(groups);
                    DB.NotificationDbManager.RemoveByFormID(id);
                    DB.NotificationDbManager.SaveChanges();
                    DB.RuleDbManager.SaveChanges();
                    DB.ThirdPartyQueryValueDbManager.Remove(tpq);
                    DB.ThirdPartyQueryValueDbManager.SaveChanges();
                    DB.ConditionGroupDbManager.SaveChanges();
                    DB.SubscriberLoginDbManager.Remove(id);
                    DB.SubscriberLoginDbManager.SaveChanges();

                    if (child == null)
                    {
                        if (form.ParentForm_ID.HasValue)
                        {
                            Form parent = FM.GetByID(ChannelID, form.ParentForm_ID.Value);
                            parent.Status = FormStatus.Published.ToString();
                            parent.PublishAfter = null;
                        }
                    }
                    else
                    {
                        //update statistic
                        // DB.FormStaticsticDbManager.Update(id, child.Form_Seq_ID);
                        //  DB.FormStaticsticDbManager.SaveChanges();
                        //ECN_Framework_BusinessLayer.FormDesigner.Form.UpdateStaticstic(id, child.Form_Seq_ID);
                        DB.FormStaticsticDbManager.Update(id, child.Form_Seq_ID);
                        
                        child.ParentForm_ID = child.Form_Seq_ID;
                        FM.SaveChanges();
                    }
                    FM.Remove(form);
                    FM.SaveChanges();
                    if (form.CssFile_Seq_ID.HasValue)
                    {
                        DB.CssFileDbManager.DeleteByID(form.CssFile_Seq_ID.Value);
                        DB.CssFileDbManager.SaveChanges();
                    }
                    if (child != null)
                    {
                        child.ParentForm_ID = null;
                        FM.SaveChanges();
                    }

                    t.Complete();
                }
            }
        }
        public bool IsActiveByID(int ChannelID, int id)
        {
            return GetByID(ChannelID, id).IsActive();
        }
       

        public string GetCustomerNameByFormID(int ChannelID, int id)
        {
            return FM.GetCustomerName(ChannelID, id);
        }

        private string RewriteSnippent(Dictionary<int, int> lstControls, string message)
        {
            foreach (var item in lstControls)
            {
                message = message.Replace(FormControlsPostModel.GetSnippedByID(item.Key), FormControlsPostModel.GetSnippedByID(item.Value));
            }

            return message;
        }

        internal void SaveChanges()
        {
            FM.SaveChanges();
        }

        #region API
        #region Public Methods
        #region Customers
        public IEnumerable<Customer> GetCustomers(string apiKey)
        {
            string responseData = customers[apiKey];
            int resp_code = -1;
            if (responseData == null)
            {
                lock (typeof(FormManager))
                {
                    if (customers[apiKey] == null)
                    {
                        NameValueCollection data = new NameValueCollection();
                        data.Add(APIAccessKey, apiKey);
                        resp_code = SendCommand(GetCURLWithItem("internal/forms/methods/GetFormsCustomersForBaseChannel"), data, out responseData);
                        customers.Add(apiKey, responseData);
                    }
                }
                responseData = customers[apiKey];
            }

            if ((resp_code == 500 || resp_code == -1) && responseData == null)
            {
                throw new System.Net.WebException(KMAPIError);
            }

            return serializer.Deserialize<List<Customer>>(responseData);
        }

        public List<ECN_Framework_Entities.Accounts.Customer> GetCustomers(int baseChannelID)
        {
            return ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(baseChannelID);
        }

        public Customer GetCustomerByID(string apiKey, int custID)
        {
            ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(custID, false);
            var cust = new Customer
            {
                AccessKey = MasterUser.AccessKey.ToString(),
                CustomerID = c.CustomerID.ToString(),
                CustomerName = c.CustomerName
            };
            return cust;
        }

        public ECN_Framework_Entities.Accounts.Customer GetCustomerByID(int custID)
        {
            return ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(custID, false);
        }
        #endregion

        #region Folders
        public Folder GetFolderByCustomerIDAndID(string apiKey, int custID, int ID)
        {
            return GetFolderByCustomerAndID(GetCustomerByID(apiKey, custID), ID);
        }

        public IEnumerable<Folder> GetFoldersByCustomerIDAndParentID(string apiKey, int custID, int parentID)
        {
            return GetFoldersByCustomerAndParentID(GetCustomerByID(apiKey, custID), parentID);
        }

        public IEnumerable<Folder> GetFoldersByCustomerID(string apiKey, int custID)
        {
            return GetFoldersByCustomer(GetCustomerByID(apiKey, custID));
        }
        #endregion

        #region Groups
        public Group GetGroupByCustomerIDAndID(string apiKey, int custID, int ID)
        {
            return GetGroupByCustomerAndID(GetCustomerByID(apiKey, custID), ID);
        }



        public ECN_Framework_Entities.Communicator.Group GetGroupByFormID(int ChannelID, string apiKey, int id)
        {
            ECN_Framework_Entities.Communicator.Group res = null;
            Form form = GetByID(ChannelID, id);
            if (form != null)
            {
                res = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(form.GroupID);

            }

            return res;
        }

        public ECN_Framework_Entities.Communicator.Group GetGroupByFormID(int ChannelID, int id)
        {
            ECN_Framework_Entities.Communicator.Group res = null;
            Form form = GetByID(ChannelID, id);
            if (form != null)
            {
                res = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(form.GroupID);

            }

            return res;
        }

        public IEnumerable<Group> GetAllGroupsByCustomerID(string apiKey, int custID)
        {
            return GetAllGroupsByCustomer(GetCustomerByID(apiKey, custID));
        }

        public List<ECN_Framework_Entities.Communicator.Group> GetAllGroupsByCustomerID(int custID, KMPlatform.Entity.User user)
        {
            return GetAllGroupsByCustomer(custID, user);
        }

        public IEnumerable<Group> GetRootGroupsByCustomerID(string apiKey, int custID)
        {
            return GetGroupsByCustomerAndFolderID(GetCustomerByID(apiKey, custID), 0);
        }

        public IEnumerable<Group> GetGroupsByCustomerIDAndFolderID(string apiKey, int custID, int folderID)
        {
            return GetGroupsByCustomerAndFolderID(GetCustomerByID(apiKey, custID), folderID);
        }

        public ECN_Framework_Entities.Communicator.Group AddGroup(string apiKey, int custID, int folderID, string groupName, string groupDescr, KMPlatform.Entity.User user, out string error)
        {
            return AddGroup(custID, folderID, groupName, groupDescr, user, out error);
        }
        #endregion

        #region Fields
        public List<ECN_Framework_Entities.Communicator.GroupDataFields> GetFieldsByFormID(int ChannelID, string apiKey, int id)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> res = null;
            Form form = GetByID(ChannelID, id);
            if (form != null)
            {

                    res = GetFieldsByCustomerAndGroupID(form.GroupID);
                
            }

            return res;
        }

        public List<ECN_Framework_Entities.Communicator.GroupDataFields> GetFieldsByFormID(int id,int ChannelID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> res = null;
            Form form = GetByID(ChannelID, id);
            if (form != null)
            {

                res = GetFieldsByCustomerAndGroupID(form.GroupID);

            }

            return res;
        }

        public IEnumerable<Field> GetFieldsByGroupID(string apiKey, int custID, int groupID)
        {
            IEnumerable<Field> res = null;
            Customer c = GetCustomerByID(apiKey, custID);
            if (c != null)
            {
                res = GetFieldsByCustomerAndGroupID(c, groupID);
            }

            return res;
        }

        public List<ECN_Framework_Entities.Communicator.GroupDataFields> GetFieldsByGroupID(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> res = null;
            res = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);
            return res;
        }

        public bool CheckFieldID(int UserID, string apiKey, int formID, int fieldID)
        {
            return GetFieldsByFormID(UserID, apiKey, formID).SingleOrDefault(x => x.GroupDataFieldsID == fieldID) != null;
        }

        public Field AddFieldToGroupByFormID(int UserID,int ChannelID, string apiKey, int id, string shortName, string longName, out string error)
        {
            error = null;
            Field res = null;
            Form form = GetByID(ChannelID, id);
            if (form != null)
            {
                Customer c = GetCustomerByForm(apiKey, form);
                if (c != null)
                {
                    res = AddField(c, form.GroupID, shortName, longName, out error);
                }
            }

            return res;
        }

        public Field AddFieldToGroupByID(string apiKey, int custID, int groupID, string shortName, string longName, out string error)
        {
            error = null;
            Field res = null;
            Customer c = GetCustomerByID(apiKey, custID);
            if (c != null)
            {
                res = AddField(c, groupID, shortName, longName, out error);
            }

            return res;
        }
        #endregion
        #endregion

        #region Private Methods
        #region Customers
        private Customer GetCustomerByForm(string apiKey, Form form)
        {
            return GetCustomers(apiKey).SingleOrDefault(x => x.CustomerID == form.CustomerID.ToString());
        }

        private IEnumerable<T> AddCustomer<T>(IEnumerable<T> items, Customer customer) where T : CustomerRelationBase
        {
            foreach (var i in items)
            {
                AddCustomer(i, customer);
            }

            return items;
        }

        private CustomerRelationBase AddCustomer(CustomerRelationBase item, Customer customer)
        {
            if (item.CustomerID == 0)
            {
                item.CustomerID = int.Parse(customer.CustomerID);
            }

            return item;
        }
        #endregion

        #region Folders
        private Folder GetFolderByCustomerAndID(Customer customer, int ID)
        {
            ECN_Framework_Entities.Communicator.Folder f = ECN_Framework_BusinessLayer.Communicator.Folder.GetByFolderID_NoAccessCheck(ID);

            var folder = new Folder
            {
                CreatedDate = f.CreatedDate,
                CustomerID = f.CustomerID.Value,
                FolderDescription = f.FolderDescription,
                CreatedUserID = f.CreatedUserID.Value,
                CustomerName = customer.CustomerName,
                FolderID = f.FolderID,
                FolderName = f.FolderName,
                FolderType = f.FolderType,
                ParentID = f.ParentID,
                UpdatedDate = f.UpdatedDate,
                UpdatedUserID = f.UpdatedUserID.HasValue ? f.UpdatedUserID.Value : -1
            };

            return folder;

            //string responseData = null;
            //NameValueCollection data = new NameValueCollection();
            //data.Add(APIAccessKey, customer.AccessKey);
            //data.Add(X_Customer_ID, customer.CustomerID);
            //SendCommand(GetCURLWithItem("folder/" + ID), data, out responseData);

            //return (Folder)AddCustomer(serializer.Deserialize<Folder>(responseData), customer);
        }

        private IEnumerable<Folder> GetFoldersByCustomer(Customer customer)
        {
            List<ECN_Framework_Entities.Communicator.Folder> listF = ECN_Framework_BusinessLayer.Communicator.Folder.GetByCustomerID(Convert.ToInt32(customer.CustomerID), MasterUser);

            var folders = from f in listF
                          select new Folder
                          {
                              CreatedDate = f.CreatedDate,
                              CustomerID = f.CustomerID.Value,
                              FolderDescription = f.FolderDescription,
                              CreatedUserID = f.CreatedUserID.Value,
                              CustomerName = customer.CustomerName,
                              FolderID = f.FolderID,
                              FolderName = f.FolderName,
                              FolderType = f.FolderType,
                              ParentID = f.ParentID,
                              UpdatedDate = f.UpdatedDate,
                              UpdatedUserID = f.UpdatedUserID.HasValue ? f.UpdatedUserID.Value : -1
                          };

            return folders;
            //string responseData = null;
            //NameValueCollection data = new NameValueCollection();
            //data.Add(APIAccessKey, customer.AccessKey);
            //data.Add(X_Customer_ID, customer.CustomerID);

            //SendCommand(GetCURLWithItem("search/folder"), data, GetSearchCriteriaJson(FoldersJson), out responseData);
            //IEnumerable<Folder> res = serializer.Deserialize<List<KMList<Folder>>>(responseData).Select(x => x.ApiObject);

            //return AddCustomer<Folder>(res, customer);
        }

        private IEnumerable<Folder> GetFoldersByCustomerAndParentID(Customer customer, int parentID)
        {
            List<ECN_Framework_Entities.Communicator.Folder> listF = ECN_Framework_BusinessLayer.Communicator.Folder.GetByCustomerID(Convert.ToInt32(customer.CustomerID), MasterUser).Where(x => x.ParentID == parentID).ToList();
            var folders = from f in listF
                          select new Folder
                          {
                              CreatedDate = f.CreatedDate,
                              CustomerID = f.CustomerID.Value,
                              FolderDescription = f.FolderDescription,
                              CreatedUserID = f.CreatedUserID.Value,
                              CustomerName = customer.CustomerName,
                              FolderID = f.FolderID,
                              FolderName = f.FolderName,
                              FolderType = f.FolderType,
                              ParentID = f.ParentID,
                              UpdatedDate = f.UpdatedDate,
                              UpdatedUserID = f.UpdatedUserID.HasValue ? f.UpdatedUserID.Value : -1
                          };

            return folders;
            //string responseData = null;
            //NameValueCollection data = new NameValueCollection();
            //data.Add(APIAccessKey, customer.AccessKey);
            //data.Add(X_Customer_ID, customer.CustomerID);
            //SendCommand(GetCURLWithItem("search/folder"), data, GetSearchCriteriaJson(FoldersJson + ",{\"name\":\"ParentID\",\"comparator\":\"=\",\"valueSet\":" + parentID + '}'), out responseData);
            //IEnumerable<Folder> res = serializer.Deserialize<List<KMList<Folder>>>(responseData).Select(x => x.ApiObject);

            //return AddCustomer<Folder>(res, customer);
        }
        #endregion

        #region Groups
        private Group GetGroupByCustomerAndID(Customer customer, int ID)
        {
            ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(ID);
            var group = new Group
            {
                CustomerID = g.CustomerID,
                FolderID = g.FolderID.HasValue ? g.FolderID.Value : 0,
                GroupDescription = g.GroupDescription,
                GroupID = g.GroupID,
                GroupName = g.GroupName
            };

            return group;

            //string responseData = null;
            //NameValueCollection data = new NameValueCollection();
            //data.Add(APIAccessKey, customer.AccessKey);
            //data.Add(X_Customer_ID, customer.CustomerID);
            //SendCommand(GetCURLWithItem("group/" + ID), data, out responseData);

            //return (Group)AddCustomer(serializer.Deserialize<Group>(responseData), customer);
        }

        private IEnumerable<Group> GetAllGroupsByCustomer(Customer customer)
        {
            List<ECN_Framework_Entities.Communicator.Group> listG = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID_NoAccessCheck(Convert.ToInt32(customer.CustomerID));
            var groups = from g in listG
                         select new Group
                         {
                             CustomerID = g.CustomerID,
                             FolderID = g.FolderID.HasValue ? g.FolderID.Value : 0,
                             GroupDescription = g.GroupDescription,
                             GroupID = g.GroupID,
                             GroupName = g.GroupName
                         };

            return groups;

            //string responseData = null;
            //NameValueCollection data = new NameValueCollection();
            //data.Add(APIAccessKey, customer.AccessKey);
            //data.Add(X_Customer_ID, customer.CustomerID);

            //SendCommand(GetCURLWithItem("search/group"), data, "[]", out responseData);
            //IEnumerable<Group> res = serializer.Deserialize<List<KMList<Group>>>(responseData).Select(x => x.ApiObject);

            //return AddCustomer<Group>(res, customer);
        }

        private List<ECN_Framework_Entities.Communicator.Group> GetAllGroupsByCustomer(int customerID, KMPlatform.Entity.User user)
        {

            return ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(customerID, user);
        }

        private IEnumerable<Group> GetGroupsByCustomerAndFolderID(Customer customer, int folderID)
        {
            List<ECN_Framework_Entities.Communicator.Group> listG = ECN_Framework_BusinessLayer.Communicator.Group.GetByFolderIDCustomerID_NoAccessCheck(folderID, Convert.ToInt32(customer.CustomerID), MasterUser);
            var groups = from g in listG
                         select new Group
                         {
                             CustomerID = g.CustomerID,
                             FolderID = g.FolderID.HasValue ? g.FolderID.Value : 0,
                             GroupDescription = g.GroupDescription,
                             GroupID = g.GroupID,
                             GroupName = g.GroupName
                         };

            return groups;
            //string responseData = null;
            //NameValueCollection data = new NameValueCollection();
            //data.Add(APIAccessKey, customer.AccessKey);
            //data.Add(X_Customer_ID, customer.CustomerID);

            //SendCommand(GetCURLWithItem("search/group"), data, GetSearchCriteriaJson('{' + string.Format(GroupsJson, folderID) + '}'), out responseData);
            //IEnumerable<Group> res = serializer.Deserialize<List<KMList<Group>>>(responseData).Select(x => x.ApiObject);

            //return AddCustomer<Group>(res, customer);
        }

        private List<ECN_Framework_Entities.Communicator.Group> GetGroupsByCustomerAndFolderID(int customer, int folderID, KMPlatform.Entity.User user)
        {

            return ECN_Framework_BusinessLayer.Communicator.Group.GetByFolderIDCustomerID(folderID, user); ;
        }

        private Group AddGroup(Customer customer, int folderID, string groupName, string groupDescr, KMPlatform.Entity.User user, out string error)
        {
            error = null;
            string responseData = null;
            NameValueCollection data = new NameValueCollection();
            data.Add(APIAccessKey, customer.AccessKey);
            data.Add(X_Customer_ID, customer.CustomerID);
            if (string.IsNullOrEmpty(groupDescr))
            {
                groupDescr = groupName;
            }

            SendCommand(GetCURLWithItem("group"), data, "{\"FolderID\":" + folderID + ",\"GroupName\":\"" + groupName + "\",\"GroupDescription\":\"" + groupDescr + "\"}", out responseData, out error);
            Group group = null;
            try
            {
                if (responseData != null)
                {
                    group = (Group)AddCustomer(serializer.Deserialize<Group>(responseData), customer);
                }
            }
            catch
            { }

            return group;
        }

        private ECN_Framework_Entities.Communicator.Group AddGroup(int customerID, int folderID, string groupName, string groupDescr, KMPlatform.Entity.User user, out string error)
        {
            error = null;
            try
            {

                ECN_Framework_Entities.Communicator.Group g = new ECN_Framework_Entities.Communicator.Group();
                g.FolderID = folderID;
                g.GroupName = groupName;
                g.GroupDescription = groupDescr;
                g.CustomerID = customerID;
                g.GroupID = ECN_Framework_BusinessLayer.Communicator.Group.Save(g, user);

                return g;
            }
            catch
            {

                return null;
            }
        }
        #endregion

        #region Fields
        internal List<ECN_Framework_Entities.Communicator.GroupDataFields> GetFieldsByForm(Form form)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> res = null;
            if (form != null)
            {
                res = GetFieldsByCustomerAndGroupID(form.GroupID);
            }

            return res;
        }

        private IEnumerable<Field> GetFieldsByCustomerAndGroupID(Customer customer, int groupID)
        {
            return GetFieldsByCustomerAndGroupID(customer.CustomerID, customer.AccessKey, groupID);
        }

        private List<ECN_Framework_Entities.Communicator.GroupDataFields> GetFieldsByCustomerAndGroupID(int groupID)
        {
            return ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);
        }

        private IEnumerable<Field> GetFieldsByCustomerAndGroupID(string CustomerID, string AccessKey, int groupID)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> listGDFs = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);
            var gdfs = from g in listGDFs
                       select new Field
                       {
                           GroupDataFieldsID = g.GroupDataFieldsID,
                           GroupID = g.GroupID,
                           IsPublic = g.IsPublic,
                           LongName = g.LongName,
                           ShortName = g.ShortName
                       };

            return gdfs;

            //string responseData = null;
            //NameValueCollection data = new NameValueCollection();
            //data.Add(APIAccessKey, AccessKey);
            //data.Add(X_Customer_ID, CustomerID);
            //SendCommand(GetCURLWithItem("search/customfield"), data, GetSearchCriteriaJson('{' + string.Format(GFieldsJson, groupID) + '}'), out responseData);

            //return serializer.Deserialize<List<KMList<Field>>>(responseData).Select(x => x.ApiObject);
        }

        private Field AddField(Customer customer, int groupID, string shortName, out string error)
        {
            return AddField(customer, groupID, shortName, shortName, true, out error);
        }


        private Field AddField(Customer customer, int groupID, string shortName, string longName, out string error)
        {
            return AddField(customer, groupID, shortName, longName, true, out error);
        }

        private Field AddField(Customer customer, int groupID, string shortName, string longName, bool isPublic, out string error)
        {
            error = null;
            string responseData = null;
            NameValueCollection data = new NameValueCollection();
            data.Add(APIAccessKey, customer.AccessKey); //"15bcb4b3-197e-4ad9-9e83-b9ce837e45ab");
            data.Add(X_Customer_ID, customer.CustomerID);

            SendCommand(GetCURLWithItem("customfield"), data,
                            "{\"GroupID\":" + groupID +
                                ",\"ShortName\":\"" + shortName +
                                "\",\"LongName\":\"" + longName +
                                "\",\"IsPublic\":\"" + (isPublic ? 'Y' : 'N') + "\"}",
                            out responseData, out error);
            Field field = null;
            try
            {
                if (responseData != null)
                {
                    field = serializer.Deserialize<Field>(responseData);
                }
            }
            catch
            { }

            return field;
        }
        #endregion
        #endregion
        #endregion
    }
}