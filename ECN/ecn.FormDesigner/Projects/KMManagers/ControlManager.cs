using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using System.Configuration;
using KMDbManagers;
using KMEntities;
using KMModels;
using KMModels.PostModels;
using MC = KMModels.Controls;
using ControlPostModel = KMModels.Controls.Control;
using KMManagers.APITypes;
using KMEnums;
using System.Data.SqlClient;
using HtmlAgilityPack;

namespace KMManagers
{
    public class ControlManager : ManagerBase
    {
        private FormManager fm = new FormManager();
        private static IEnumerable<ControlProperty> properties = null;

        private ControlDbManager CM
        {
            get
            {
                return DB.ControlDbManager;
            }
        }

        private static IEnumerable<ControlProperty> Properties
        {
            get
            {
                if (properties == null)
                {
                    lock (typeof(ControlManager))
                    {
                        if (properties == null)
                        {
                            ControlPropertyManager cpm = new ControlPropertyManager();
                            properties = cpm.GetAll();
                        }
                    }
                }

                return properties;
            }
        }

        public IEnumerable<TModel> GetAllByFormID<TModel>(int formId) where TModel : ModelBase, new()
        {
            return CM.GetAllByFormID(formId).ConvertToModels<TModel>();
        }

        public IEnumerable<TModel> GetAllStandardByFormID<TModel>(int formId) where TModel : ModelBase, new()
        {
            return CM.GetAllStandardByFormID(formId).ConvertToModels<TModel>();
        }

        public IEnumerable<TModel> GetAllCustomWithFieldByFormID<TModel>(int formId) where TModel : ModelBase, new()
        {
            return CM.GetAllCustomWithFieldByFormID(formId).ConvertToModels<TModel>();
        }

        public bool HasCustomWithField(int formId)
        {
            return CM.HasCustomWithField(formId);
        }

        public IEnumerable<ControlFieldModel> GetAllCustomWithFieldByFormIDWithFieldNames(int ChannelID, string apiKey, int formId)
        {
            var models = GetAllCustomWithFieldByFormID<ControlFieldModel>(formId);
            var fields = fm.GetFieldsByFormID(ChannelID, apiKey, formId);

            foreach (var m in models)
            {
                SetFieldName(m, fields);
                yield return m;
            }


        }



        private void SetFieldName(ControlFieldModel m, List<ECN_Framework_Entities.Communicator.GroupDataFields> fields)
        {
            var f = fields.SingleOrDefault(x => x.GroupDataFieldsID == m.FieldId);
            if (f != null)
            {
                m.FieldName = f.ShortName;
            }
        }

        public IEnumerable<TModel> GetAllValuedByFormID<TModel>(int formId) where TModel : ModelBase, new()
        {
            var controls = CM.GetAllValuedByFormID(formId);
            foreach (var c in controls)
            {
                var model = c.ConvertToModel<TModel>();
                if (model is ControlModel)
                {
                    ControlModel c_model = model as ControlModel;
                    if (c_model.Type == KMEnums.ControlType.TextBox)
                    {
                        c_model.SetDataType(c.GetFormPropertyValue(HTMLGenerator.DataType_Property));
                    }
                    if (c.ControlType.Name == "State" && c_model.Type == KMEnums.ControlType.DropDown) // Overwrite any values and set States based on Countries if any.
                    {
                        List<string> values = new List<string>();
                        foreach (var co in controls) // Add list of states
                        {
                            var model2 = co.ConvertToModel<TModel>();
                            if (model2 is ControlModel)
                            {
                                ControlModel co_model = model2 as ControlModel;
                                if (co.ControlType.Name == "Country" && co_model.Type == KMEnums.ControlType.DropDown)
                                {
                                    for (int j = 0; j < co_model.SelectableItems.Length; j++)
			                        {
                                        if (co_model.SelectableItems[j].Label.ToLower() == "united states" || co_model.SelectableItems[j].Label.ToLower() == "canada" || co_model.SelectableItems[j].Label.ToLower() == "mexico")
                                        {
                                            List<string> listStates = GetStatesByCountryName(co_model.SelectableItems[j].Label);
                                            values.AddRange(listStates);
                                        }
                                        else if (!values.Contains("52|Foreign State"))
                                        {         
                                            values.Insert(0, "52|Foreign State");
                                        }                                        
			                        }
                                }
                            }
                        }
                            
                        c_model.SelectableItems = new SelectableItem[values.Count];
                        int i = 0;
                        foreach (string v in values)
                        {
                            string[] items = v.Split('|');
                            c_model.SelectableItems[i] = new SelectableItem();
                            c_model.SelectableItems[i].Id = Convert.ToInt32(items[0]);
                            c_model.SelectableItems[i].Label = items[1];
                            i++;
                        }
                    }
                }

                yield return model;
            }
        }

        public IEnumerable<TItemed> GetAllValuedByFormIDForConditions<TItemed>(int formId) where TItemed : ModelBase, IItemed, new()
        {
            IEnumerable<TItemed> controls = GetAllValuedByFormID<TItemed>(formId);
            List<TItemed> result = new List<TItemed>();
            foreach (var c in controls)
            {
                if (c.SelectableItems == null)
                {
                    result.Add(c);
                }
                else
                {
                    foreach (var item in c.SelectableItems)
                    {
                        result.Add((TItemed)c.GetItem(item));
                    }
                }
            }

            return result;
        }

        public IEnumerable<TModel> GetAllVisibleByFormID<TModel>(int formId) where TModel : ModelBase, new()
        {
            return CM.GetAllVisibleByFormID(formId).ConvertToModels<TModel>();
        }
        public IEnumerable<TModel> GetAllVisibleOverwriteDataFormID<TModel>(int formId) where TModel : ModelBase, new()
        {
            return CM.GetAllVisibleOverwriteDataFormID(formId).ConvertToModels<TModel>();
        }
        public IEnumerable<TModel> GetAllRequestQueryByFormID<TModel>(int formId) where TModel : ModelBase, new()
        {
            return CM.GetAllRequestQueryByFormID(formId).ConvertToModels<TModel>();
        }
        public IEnumerable<TModel> GetPageBreaksByFormID<TModel>(int formId) where TModel : ModelBase, new()
        {
            return CM.GetPageBreaksByFormID(formId).ConvertToModels<TModel>();
        }

        public bool Save(KMPlatform.Entity.User User, int ChannelID, string apiKey, FormControlsPostModel model, IEnumerable<ControlPostModel> controls, ref List<string> OldGrids)
        {
            bool res = false;
            Form form = fm.GetByID(ChannelID, model.Id);

            if (form != null)
            {
                form.UpdatedBy = User.UserName;
                form.LastUpdated = DateTime.Now;
                res = true;
                OldGrids.Add("200");
                //newId = form.Form_Seq_ID;
                //if (form.Status != (int)FormStatus.Saved && form.Form1.Count == 0)
                //{
                //    newId = FullCopy(model, form);
                //    Form newForm = fm.GetByID(newId);
                //    RewriteIDs(controls, GetOldToNewDictionary<Control>(form.Controls, newForm.Controls, x => x.Control_ID, GetEqualFunc));
                //    SaveControls(newForm, controls);
                //}
                //else
                //{
                //    SaveControls(form, controls);
                //}

                //fill entities
                Dictionary<Control, ControlPostModel> list = new Dictionary<Control, ControlPostModel>();
                foreach (var c in controls)
                {
                    InitializeControl(c, form.Form_Seq_ID, list);
                }

                // Add UDF to Newsletter Group
                foreach (var edited in list.Keys)
                {
                    ControlPostModel m = list[edited];
                    switch ((KMEnums.ControlType)edited.Type_Seq_ID)
                    {
                        case KMEnums.ControlType.NewsLetter:
                            List<GroupModel> groups = ((MC.NewsLetter)m).Groups.ToList();
                            foreach (var group in groups)
                            {
                                List<ECN_Framework_Entities.Communicator.GroupDataFields> fields = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(group.GroupID).ToList();
                                foreach (var groupUdf in group.UDFs)
                                {
                                    var field = fields.SingleOrDefault(x => x.ShortName.Trim().ToLower() == groupUdf.ShortName.Trim().ToLower());
                                    if (field == null)
                                    {
                                        ECN_Framework_Entities.Communicator.GroupDataFields gdf = new ECN_Framework_Entities.Communicator.GroupDataFields();
                                        gdf.CustomerID = group.CustomerID;
                                        gdf.GroupID = group.GroupID;
                                        gdf.ShortName = groupUdf.ShortName;
                                        gdf.LongName = groupUdf.ShortName;
                                        gdf.CreatedUserID = User.UserID;
                                        gdf.IsPublic = "Y";
                                        groupUdf.NewsletterDataFieldID = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf, User);
                                    }
                                    else
                                    {
                                        groupUdf.NewsletterDataFieldID = field.GroupDataFieldsID;
                                    }
                                }
                            }
                            break;
                    }
                }

                //save
                using (TransactionScope t = new TransactionScope())
                {
                    //save header and footer
                    form.HeaderHTML = model.Header;
                    form.HeaderJs = model.HeaderJs;
                    form.FooterHTML = model.Footer;
                    form.FooterJs = model.FooterJs;
                    fm.SaveChanges();

                    //delete
                    List<Control> toRemove = form.Controls.Where(x => !controls.Select(y => y.Id).Contains(x.Control_ID)).ToList();
                    foreach (var c in toRemove)
                    {
                        DB.ThirdPartyQueryValueDbManager.Remove(c.ThirdPartyQueryValues);
                        IEnumerable<int> removed = DB.ConditionGroupDbManager.Remove(c.Rules.Select(x => x.ConditionGroup));
                        DB.RuleDbManager.Remove(c.Rules);
                        IEnumerable<int> IDs = DB.ConditionGroupDbManager.GetConditionGroupIDsByControl(c).Except(removed);
                        IEnumerable<Rule> rules = DB.RuleDbManager.GetByCondGroupIDs(IDs);
                        IEnumerable<int> notifications = DB.NotificationDbManager.GetIDsByCondGroupIDs(IDs);
                        DB.RuleDbManager.Remove(rules);
                        DB.NotificationDbManager.Remove(notifications);
                        DB.ConditionGroupDbManager.Remove(IDs);
                        CM.Remove(c);
                    }
                    if (toRemove.Count > 0)
                    {
                        DB.ThirdPartyQueryValueDbManager.SaveChanges();
                        DB.RuleDbManager.SaveChanges();
                        DB.NotificationDbManager.SaveChanges();
                        DB.ConditionGroupDbManager.SaveChanges();
                        CM.SaveChanges();
                    }

                    //edit and add
                    //List<Control> existing = form.Controls.Where(x => controls.Select(y => y.Id).Contains(x.Control_ID)).ToList();
                    ControlPropertyManager cpm = new ControlPropertyManager();
                    foreach (var edited in list.Keys)
                    {
                        //Control edited = list.Keys.Single(x => x.Control_ID == c.Control_ID);
                        ControlPostModel m = list[edited];
                        if (edited.Control_ID == 0)
                        {
                            CM.Add(edited);
                            CM.SaveChanges();
                        }
                        if (m.IsStandard)
                        {
                            if (m.Type == KMEnums.ControlType.Gender)
                            {
                                MC.DropDown dd = (MC.DropDown)m;
                                MC.Standard.Uncommon.Gender gender = (MC.Standard.Uncommon.Gender)dd;
                                RewriteRequiredFormProperty(edited, gender.Required ? "1" : null, cpm);
                                RewriteFormPropertyByName(edited, HTMLGenerator.PrepopulateFrom_Property, ((int)gender.PopulationType).ToString(), cpm);
                                RewriteFormPropertyByName(edited, HTMLGenerator.QueryString_Property, gender.PopulationType == KMEnums.PopulationType.Querystring ? gender.Parameter : null, cpm);
                                RewriteValueFormProperty(edited, gender.Items, cpm);
                            }
                            else if (m.Type == KMEnums.ControlType.Country)
                            {
                                OldGrids.Add("country");
                                foreach (var item in edited.FormControlPropertyGrids)
                                    OldGrids.Add(item.FormControlPropertyGrid_Seq_ID.ToString() + "," + item.DataText);

                                MC.DropDown dd = (MC.DropDown)m;
                                MC.Standard.Common.Country country = (MC.Standard.Common.Country)dd;
                                RewriteRequiredFormProperty(edited, country.Required ? "1" : null, cpm);
                                RewriteFormPropertyByName(edited, HTMLGenerator.PrepopulateFrom_Property, ((int)country.PopulationType).ToString(), cpm);
                                RewriteFormPropertyByName(edited, HTMLGenerator.QueryString_Property, country.PopulationType == KMEnums.PopulationType.Querystring ? country.Parameter : null, cpm);
                                RewriteValueFormProperty(edited, country.Items, cpm);
                            }
                            else if (m.Type == KMEnums.ControlType.State)
                            {
                                //OldGrids.Add("state");
                                //foreach (var item in edited.FormControlPropertyGrids)
                                //    OldGrids.Add(item.FormControlPropertyGrid_Seq_ID.ToString() + "," + item.DataText);

                                MC.DropDown dd = (MC.DropDown)m;
                                MC.Standard.Common.State state = (MC.Standard.Common.State)dd;
                                RewriteRequiredFormProperty(edited, state.Required ? "1" : null, cpm);
                                RewriteFormPropertyByName(edited, HTMLGenerator.PrepopulateFrom_Property, ((int)state.PopulationType).ToString(), cpm);
                                RewriteFormPropertyByName(edited, HTMLGenerator.QueryString_Property, state.PopulationType == KMEnums.PopulationType.Querystring ? state.Parameter : null, cpm);
                                RewriteValueFormProperty(edited, state.Items, cpm);
                            }
                            else if (m.Type == KMEnums.ControlType.Password)
                            {
                                UpdateTextBox(edited, m, cpm);
                                MC.TextBox tb = (MC.TextBox)m;
                                MC.Standard.Uncommon.Password pw = (MC.Standard.Uncommon.Password)tb;
                                RewriteFormPropertyByName(edited, "Confirm Password", (pw.ConfirmPassword).ToString(), cpm);
                                RewriteFormPropertyByName(edited, "Confirm Password LabelHTML", (pw.ConfirmPasswordLabelHTML).ToString(), cpm);
                            }
                            else if (m.Type == KMEnums.ControlType.Email)
                            {
                                UpdateTextBox(edited, m, cpm);
                                MC.TextBox tb = (MC.TextBox)m;
                                MC.Standard.Common.Email email = (MC.Standard.Common.Email)tb;
                                RewriteFormPropertyByName(edited, "Allow Changes", email.AllowChanges, cpm);
                            }
                            else
                            {
                                UpdateTextBox(edited, m, cpm);
                                List<MC.ListItem> values = GetPresetValuesByTypeID(edited.Type_Seq_ID);
                                if (values != null)
                                {
                                    RewriteValueFormProperty(edited, values, cpm);
                                }
                            }
                        }
                        else
                        {
                            switch ((KMEnums.ControlType)edited.Type_Seq_ID)
                            {
                                case KMEnums.ControlType.TextBox:
                                    UpdateTextBox(edited, m, cpm);
                                    break;
                                case KMEnums.ControlType.TextArea:
                                    MC.TextArea ta = (MC.TextArea)m;
                                    RewriteRequiredFormProperty(edited, ta.Required ? "1" : null, cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.FieldSize_Property, ((int)ta.Size).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.StrLen_Property, ta.MinMax, cpm);
                                    break;
                                case KMEnums.ControlType.RadioButton:
                                    MC.RadioButton rb = (MC.RadioButton)m;
                                    List<MC.ControlCategory> categoriesForRB = ((MC.RadioButton)m).Categories.ToList();
                                    IEnumerable<MC.ControlCategory> updateCategoriesForRB = categoriesForRB.Where(x => x.CategoryID > 0);
                                    IEnumerable<MC.ControlCategory> createCategoriesForRB = categoriesForRB.Where(x => x.CategoryID <= 0);
                                    DB.ControlCategoryDbManager.RemoveAllExcept(edited.Control_ID, updateCategoriesForRB.Select(x => x.CategoryID.Value));
                                    foreach (var item in createCategoriesForRB)
                                    {
                                        ControlCategory cc = new ControlCategory();
                                        cc.Control_ID = edited.Control_ID;
                                        cc.LabelHTML = item.CategoryName;
                                        DB.ControlCategoryDbManager.Add(cc);
                                    }
                                    foreach (var item in updateCategoriesForRB)
                                    {
                                        ControlCategory cc = DB.ControlCategoryDbManager.GetByID(item.CategoryID.Value);
                                        cc.LabelHTML = item.CategoryName;
                                    }
                                    DB.ControlCategoryDbManager.SaveChanges();
                                    RewriteRequiredFormProperty(edited, rb.Required ? "1" : null, cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.PrepopulateFrom_Property, ((int)rb.PopulationType).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.QueryString_Property, rb.PopulationType == KMEnums.PopulationType.Querystring ? rb.Parameter : null, cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.NumberofColumns_Property, ((int)rb.Columns).ToString(), cpm);
                                    RewriteValueFormProperty(edited, rb.Items, cpm);
                                    break;
                                case KMEnums.ControlType.CheckBox:
                                    MC.CheckBox cb = (MC.CheckBox)m;
                                    List<MC.ControlCategory> categoriesForCB = ((MC.CheckBox)m).Categories.ToList();
                                    IEnumerable<MC.ControlCategory> updateCategoriesForCB = categoriesForCB.Where(x => x.CategoryID > 0);
                                    IEnumerable<MC.ControlCategory> createCategoriesForCB = categoriesForCB.Where(x => x.CategoryID <= 0);
                                    DB.ControlCategoryDbManager.RemoveAllExcept(edited.Control_ID, updateCategoriesForCB.Select(x => x.CategoryID.Value));
                                    foreach (var item in createCategoriesForCB)
                                    {
                                        ControlCategory cc = new ControlCategory();
                                        cc.Control_ID = edited.Control_ID;
                                        cc.LabelHTML = item.CategoryName;
                                        DB.ControlCategoryDbManager.Add(cc);
                                    }
                                    foreach (var item in updateCategoriesForCB)
                                    {
                                        ControlCategory cc = DB.ControlCategoryDbManager.GetByID(item.CategoryID.Value);
                                        cc.LabelHTML = item.CategoryName;
                                    }
                                    DB.ControlCategoryDbManager.SaveChanges();
                                    RewriteRequiredFormProperty(edited, cb.Required ? "1" : null, cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.PrepopulateFrom_Property, ((int)cb.PopulationType).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.QueryString_Property, cb.PopulationType == KMEnums.PopulationType.Querystring ? cb.Parameter : null, cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.NumberofColumns_Property, ((int)cb.Columns).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.ValuesCount_Property, cb.MinMax, cpm);
                                    RewriteValueFormProperty(edited, cb.Items, cpm);
                                    break;
                                case KMEnums.ControlType.DropDown:
                                    MC.DropDown dd = (MC.DropDown)m;
                                    List<MC.ControlCategory> categoriesForDD = ((MC.DropDown)m).Categories.ToList();
                                    IEnumerable<MC.ControlCategory> updateCategoriesForDD = categoriesForDD.Where(x => x.CategoryID > 0);
                                    IEnumerable<MC.ControlCategory> createCategoriesForDD = categoriesForDD.Where(x => x.CategoryID <= 0);
                                    DB.ControlCategoryDbManager.RemoveAllExcept(edited.Control_ID, updateCategoriesForDD.Select(x => x.CategoryID.Value));
                                    foreach (var item in createCategoriesForDD)
                                    {
                                        ControlCategory cc = new ControlCategory();
                                        cc.Control_ID = edited.Control_ID;
                                        cc.LabelHTML = item.CategoryName;
                                        DB.ControlCategoryDbManager.Add(cc);
                                    }
                                    foreach (var item in updateCategoriesForDD)
                                    {
                                        ControlCategory cc = DB.ControlCategoryDbManager.GetByID(item.CategoryID.Value);
                                        cc.LabelHTML = item.CategoryName;
                                    }
                                    DB.ControlCategoryDbManager.SaveChanges();
                                    RewriteRequiredFormProperty(edited, dd.Required ? "1" : null, cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.PrepopulateFrom_Property, ((int)dd.PopulationType).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.QueryString_Property, dd.PopulationType == KMEnums.PopulationType.Querystring ? dd.Parameter : null, cpm);
                                    RewriteValueFormProperty(edited, dd.Items, cpm);
                                    break;
                                case KMEnums.ControlType.ListBox:
                                    MC.ListBox lb = (MC.ListBox)m;
                                    List<MC.ControlCategory> categoriesForLB = ((MC.ListBox)m).Categories.ToList();
                                    IEnumerable<MC.ControlCategory> updateCategoriesForLB = categoriesForLB.Where(x => x.CategoryID > 0);
                                    IEnumerable<MC.ControlCategory> createCategoriesForLB = categoriesForLB.Where(x => x.CategoryID <= 0);
                                    DB.ControlCategoryDbManager.RemoveAllExcept(edited.Control_ID, updateCategoriesForLB.Select(x => x.CategoryID.Value));
                                    foreach (var item in createCategoriesForLB)
                                    {
                                        ControlCategory cc = new ControlCategory();
                                        cc.Control_ID = edited.Control_ID;
                                        cc.LabelHTML = item.CategoryName;
                                        DB.ControlCategoryDbManager.Add(cc);
                                    }
                                    foreach (var item in updateCategoriesForLB)
                                    {
                                        ControlCategory cc = DB.ControlCategoryDbManager.GetByID(item.CategoryID.Value);
                                        cc.LabelHTML = item.CategoryName;
                                    }
                                    DB.ControlCategoryDbManager.SaveChanges();
                                    RewriteRequiredFormProperty(edited, lb.Required ? "1" : null, cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.PrepopulateFrom_Property, ((int)lb.PopulationType).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.QueryString_Property, lb.PopulationType == KMEnums.PopulationType.Querystring ? lb.Parameter : null, cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.FieldSize_Property, ((int)lb.Size).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.ValuesCount_Property, lb.MinMax, cpm);
                                    RewriteValueFormProperty(edited, lb.Items, cpm);
                                    break;
                                case KMEnums.ControlType.Grid:
                                    MC.Grid g = (MC.Grid)m;
                                    RewriteFormPropertyByName(edited, HTMLGenerator.GridControls_Property, ((int)g.Controls).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.GridValidation_Property, ((int)g.Validation).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.Columns_Property, g.Columns, cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.Rows_Property, g.Rows, cpm);
                                    break;
                                case KMEnums.ControlType.PageBreak:
                                    MC.PageBreak pb = (MC.PageBreak)m;
                                    RewriteFormPropertyByName(edited, HTMLGenerator.PreviousButton_Property, pb.Previous, cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.NextButton_Property, pb.Next, cpm);
                                    break;
                                case KMEnums.ControlType.Hidden:
                                    MC.Hidden h = (MC.Hidden)m;
                                    RewriteFormPropertyByName(edited, HTMLGenerator.PrepopulateFrom_Property, ((int)h.PopulationType).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.QueryString_Property, h.PopulationType == KMEnums.PopulationType.Querystring ? h.Parameter : null, cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.Value_Property, h.Value, cpm);
                                    break;
                                case KMEnums.ControlType.NewsLetter:
                                    MC.NewsLetter n = (MC.NewsLetter)m;
                                    RewriteFormPropertyByName(edited, HTMLGenerator.Letter_Type_Property, (n.Subscribe ? 1 : 0).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.PrepopulateFrom_Property, ((int)(n.IsPrepopulateFromDb ? PopulationType.Database : PopulationType.None)).ToString(), cpm);
                                    RewriteFormPropertyByName(edited, HTMLGenerator.NumberofColumns_Property, ((int)n.Columns).ToString(), cpm);

                                    // Remove Groups First before Categories being removed
                                    List<GroupModel> groups = ((MC.NewsLetter)m).Groups.ToList();
                                    DB.NewsletterGroupDbManager.RemoveAllExcept(edited.Control_ID, groups.Where(x => x.GroupID > 0).Select(x => x.GroupID));
                                    DB.NewsletterGroupDbManager.SaveChanges();

                                    // Remove, create and update Categories
                                    List<MC.ControlCategory> categories = ((MC.NewsLetter)m).Categories.ToList();
                                    IEnumerable<MC.ControlCategory> update = categories.Where(x => x.CategoryID > 0);
                                    IEnumerable<MC.ControlCategory> create = categories.Where(x => x.CategoryID <= 0);
                                    DB.ControlCategoryDbManager.RemoveAllExcept(edited.Control_ID, update.Select(x => x.CategoryID.Value));
                                    foreach (var item in create)
                                    {
                                        ControlCategory cc = new ControlCategory();
                                        cc.Control_ID = edited.Control_ID;
                                        cc.LabelHTML = item.CategoryName;
                                        DB.ControlCategoryDbManager.Add(cc);
                                    }
                                    foreach (var item in update)
                                    {
                                        ControlCategory cc = DB.ControlCategoryDbManager.GetByID(item.CategoryID.Value);
                                        cc.LabelHTML = item.CategoryName;
                                    }
                                    DB.ControlCategoryDbManager.SaveChanges();

                                    // Create and update Groups (A validation in the UI should happen first )
                                    foreach (var group in groups)
                                    {
                                        //RewriteFormPropertyByName(edited, HTMLGenerator.CustomerID_Property, group == null ? null : group.CustomerID.ToString(), cpm);
                                        //RewriteFormPropertyByName(edited, HTMLGenerator.GroupID_Property, group == null ? null : group.GroupID.ToString(), cpm);
                                        //RewriteFormPropertyByName(edited, HTMLGenerator.AccessKey_Property, group == null ? null : ConfigurationManager.AppSettings["MasterAccessKey"].ToString(), cpm);
                                        NewsletterGroup nlg = edited.NewsletterGroups.SingleOrDefault(x => x.GroupID == group.GroupID && x.Control_ID == edited.Control_ID);
                                        if (nlg == null) // Create Group
                                        {
                                            nlg = new NewsletterGroup();
                                            nlg.Control_ID = edited.Control_ID;
                                            nlg.CustomerID = group.CustomerID;
                                            nlg.GroupID = group.GroupID;
                                            DB.NewsletterGroupDbManager.Add(nlg);
                                        }
                                        if (nlg != null) // Update Group
                                        {
                                            ControlCategory cc = DB.ControlCategoryDbManager.GetByName(edited.Control_ID, group.Category.CategoryName);
                                            if (cc == null)
                                                nlg.ControlCategoryID = null;
                                            else
                                                nlg.ControlCategoryID = cc.ControlCategoryID;
                                            nlg.Order = group.Order;
                                            nlg.IsPreSelected = group.Default;
                                            nlg.LabelHTML = group.LabelHTML;
                                        }

                                        foreach (var groupUdf in group.UDFs)
                                        {
                                            DB.NewsletterGroupUDFDbManager.RemoveAll(group.GroupID);
                                            DB.NewsletterGroupUDFDbManager.SaveChanges();
                                            NewsletterGroupUDF nlgudf = new NewsletterGroupUDF();
                                            nlgudf.FormGroupDataFieldID = groupUdf.GroupDataFieldsID;
                                            nlgudf.NewsletterDataFieldID = groupUdf.NewsletterDataFieldID;

                                            nlg.NewsletterGroupUDFs.Add(nlgudf);
                                        }
                                    }

                                    DB.NewsletterGroupDbManager.SaveChanges();
                                    break;
                                case KMEnums.ControlType.Literal:
                                    RewriteFormPropertyByName(edited, HTMLGenerator.Value_Property, ((MC.Literal)m).Text, cpm);
                                    break;
                            }
                        }
                    }
                    DB.FormControlPropertyDbManager.SaveChanges();
                    DB.FormControlPropertyGridDbManager.SaveChanges();
                    CM.SaveChanges();

                    //commit
                    t.Complete();
                }
            }

            return res;
        }

        //private void RewriteIDs(IEnumerable<ControlPostModel> controls, Dictionary<int, int> old_to_new_ids)
        //{
        //    foreach (var c in controls)
        //    {
        //        c.Id = old_to_new_ids.ContainsKey(c.Id) ? old_to_new_ids[c.Id] : c.Id;
        //    }
        //}

        //private Control GetEqualFunc(Control c, ICollection<Control> childs)
        //{
        //    return childs.Single(x => x.HTMLID == c.HTMLID);
        //}

        private void UpdateTextBox(Control edited, ControlPostModel m, ControlPropertyManager cpm)
        {
            MC.TextBox tb = (MC.TextBox)m;
            RewriteRequiredFormProperty(edited, tb.Required || tb.Type == KMEnums.ControlType.Email ? "1" : null, cpm);
            RewriteFormPropertyByName(edited, HTMLGenerator.PrepopulateFrom_Property, ((int)tb.PopulationType).ToString(), cpm);
            RewriteFormPropertyByName(edited, HTMLGenerator.QueryString_Property, tb.PopulationType == KMEnums.PopulationType.Querystring ? tb.Parameter : null, cpm);
            int d_type = (int)tb.DataType;
            if (tb.Type == KMEnums.ControlType.Email)
            {
                d_type = (int)KMEnums.TextboxDataTypes.Email;
            }
            RewriteFormPropertyByName(edited, HTMLGenerator.DataType_Property, d_type.ToString(), cpm);
            RewriteFormPropertyByName(edited, HTMLGenerator.Regex_Property, tb.DataType == KMEnums.TextboxDataTypes.Custom ? tb.CustomRex : null, cpm);
            RewriteFormPropertyByName(edited, HTMLGenerator.StrLen_Property, tb.MinMax, cpm);
        }

        internal void RewriteRequiredFormProperty(Control c, string value, ControlPropertyManager cpm)
        {
            RewriteFormProperty(cpm.GetRequiredPropertyByControl(c), c, value);
        }

        internal void RewriteFormPropertyByName(Control c, string name, string value, ControlPropertyManager cpm)
        {
            RewriteFormProperty(cpm.GetPropertyByNameAndControl(name, c), c, value);
        }

        private void RewriteValueFormProperty(Control edited, IEnumerable<MC.ListItem> items, ControlPropertyManager cpm)
        {
            RewriteFormProperty(cpm.GetValuePropertyByControl(edited), edited, items);
        }

        private void RewriteFormPropertyByName(Control edited, string name, IEnumerable<string> values, ControlPropertyManager cpm)
        {
            RewriteFormProperty(cpm.GetPropertyByNameAndControl(name, edited), edited, values);
        }

        private void RewriteFormProperty(ControlProperty property, Control c, string value)
        {
            if (property != null)
            {
                FormControlProperty fValue = c.FormControlProperties.SingleOrDefault(x => x.ControlProperty_ID == property.ControlProperty_Seq_ID);
                if (fValue == null && value != null)
                {
                    fValue = new FormControlProperty();
                    fValue.Control_ID = c.Control_ID;
                    fValue.ControlProperty_ID = property.ControlProperty_Seq_ID;
                    DB.FormControlPropertyDbManager.Add(fValue);
                }
                if (fValue != null)
                {
                    fValue.Value = value;
                }
            }
        }

        private void RewriteFormProperty(ControlProperty property, Control edited, IEnumerable<string> values)
        {
            if (property != null)
            {
                DB.FormControlPropertyGridDbManager.RemoveAll(edited.Control_ID, property.ControlProperty_Seq_ID);
                foreach (var value in values)
                {
                    FormControlPropertyGrid pg = new FormControlPropertyGrid();
                    pg.Control_ID = edited.Control_ID;
                    pg.ControlProperty_ID = property.ControlProperty_Seq_ID;
                    pg.DataValue = value;
                    DB.FormControlPropertyGridDbManager.Add(pg);
                }
            }
        }

        private void RewriteFormProperty(ControlProperty property, Control edited, IEnumerable<MC.ListItem> items)
        {
            if (property != null)
            {
                IEnumerable<MC.ListItem> update = items.Where(x => x.Id > 0);
                IEnumerable<MC.ListItem> create = items.Where(x => x.Id <= 0);
                DB.FormControlPropertyGridDbManager.RemoveAllExcept(edited.Control_ID, property.ControlProperty_Seq_ID, update.Select(x => x.Id));
                foreach (var item in create)
                {
                    FormControlPropertyGrid pg = new FormControlPropertyGrid();
                    pg.Control_ID = edited.Control_ID;
                    pg.ControlProperty_ID = property.ControlProperty_Seq_ID;
                    pg.DataValue = item.Value;
                    pg.DataText = item.Text;
                    pg.IsDefault = item.Default;
                    ControlCategory cc = DB.ControlCategoryDbManager.GetByName(edited.Control_ID, item.CategoryName);
                    pg.CategoryID = cc == null ? 0 : cc.ControlCategoryID;
                    pg.CategoryName = item.CategoryName;
                    pg.Order = item.Order;
                    DB.FormControlPropertyGridDbManager.Add(pg);
                }
                foreach (var item in update)
                {
                    FormControlPropertyGrid pg = DB.FormControlPropertyGridDbManager.GetByID(item.Id);
                    pg.DataValue = item.Value;
                    pg.DataText = item.Text;
                    pg.IsDefault = item.Default;
                    ControlCategory cc = DB.ControlCategoryDbManager.GetByName(edited.Control_ID, item.CategoryName);
                    pg.CategoryID = cc == null ? 0 : cc.ControlCategoryID;
                    pg.CategoryName = item.CategoryName;
                    pg.Order = item.Order;
                }
            }
        }

        private void InitializeControl(ControlPostModel model, int formId, Dictionary<Control, ControlPostModel> list)
        {
            Control c = null;
            if (model.Id == 0)
            {
                c = new Control();
                c.Form_Seq_ID = formId;
                c.Type_Seq_ID = (int)model.Type;
                c.HTMLID = Guid.NewGuid();
            }
            else
            {
                c = CM.GetByID(model.Id);
            }
            FillControl(c, model);
            list.Add(c, model);
        }

        private void FillControl(Control c, ControlPostModel model)
        {
            c.Order = model.Order;
            if (model is MC.HeadedControl)
            {
                c.FieldLabel = ((MC.HeadedControl)model).Label;
                c.FieldLabelHTML = ((MC.HeadedControl)model).LabelHTML;
            }
            else
            {
                if (model is MC.PageBreak)
                {
                    c.FieldLabel = ((MC.PageBreak)model).PageName;
                }
            }
            c.FieldID = null;
            if (model.FieldId > 0)
            {
                c.FieldID = model.FieldId;
            }
        }

        public void FillControls(FormControlsPostModel controls, int ChannelID, KMPlatform.Entity.User user)
        {
            Form form = fm.GetByID(ChannelID, controls.Id);
            controls.Header = form.HeaderHTML;
            controls.Footer = form.FooterHTML;
            controls.HeaderJs = form.HeaderJs;
            controls.FooterJs = form.FooterJs;
            controls.FormType = (KMEnums.FormType)Enum.Parse(typeof(KMEnums.FormType), form.FormType);
            controls.FillData(form, CM.GetAllByFormID(controls.Id), Properties);

            //set group name
            //foreach (var news in controls.NewsLetter)
            //{
            //    if (news.Groups != null)
            //    {
            //        foreach (var group in news.Groups)
            //        {
            //            group.GroupName = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(group.GroupID).GroupName;// fm.GetGroupByCustomerIDAndID(apiKey, news.Group.CustomerID, news.Group.GroupID).GroupName;
            //        }
            //    }
            //}
        }

        //public Dictionary<string, string> GetPresetValuesByControlID(int controlId)
        //{
        //    Dictionary<string, string> res = null;
        //    Control c = CM.GetByID(controlId);
        //    if (c.ControlType.MainType_ID.HasValue && c.ControlType.MainType_ID.Value != (int)KMEnums.ControlType.TextBox)
        //    {
        //        res = GetPresetValuesByTypeID(c.Type_Seq_ID);
        //    }

        //    return res;
        //}

        //public List<MC.ListItem> GetPresetValuesByStandardTypeID(KMEnums.StandardControlType type)
        //{
        //    return GetPresetValuesByTypeID((int)type);
        //}

        private List<MC.ListItem> GetPresetValuesByTypeID(int typeId)
        {
            List<MC.ListItem> res = null;
            ControlProperty property = DB.ControlPropertyDbManager.GetValuePropertyByType(typeId);
            if (property != null)
            {
                res = property.ControlPropertyGrids.ConvertToModels<MC.ListItem>().ToList();
            }

            return res;
        }

        internal void SaveCPChanges()
        {
            DB.FormControlPropertyDbManager.SaveChanges();
        }

        public List<string> GetCountriesFromDB()
        {
            List<string> countries = new List<string>();
            using (SqlConnection conn = ECN_Framework_DataLayer.DataFunctions.GetSqlConnection("UAD_Lookup"))
            {
                string sql = "SELECT CountryID, ShortName FROM dbo.Country WHERE FullName is not null order by ShortName";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string country = string.Empty;
                        country = reader[1].ToString() == "ÅLAND ISLANDS" ? "ALAND ISLANDS" : reader[1].ToString();
                        countries.Add(reader[0].ToString() + "|" + country);
                    }
                }
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return countries;
        }

        public string GetCountryName(int key)
        {
            string country = string.Empty;
            using (SqlConnection conn = ECN_Framework_DataLayer.DataFunctions.GetSqlConnection("UAD_Lookup"))
            {
                string sql = "SELECT ShortName FROM dbo.Country WHERE CountryID = " + key;
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        country = reader[0].ToString();
                    }
                }
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return country;
        }

        public List<string> GetStatesAll()
        {
            List<string> states = new List<string>();
            using (SqlConnection conn = ECN_Framework_DataLayer.DataFunctions.GetSqlConnection("UAD_Lookup"))
            {
                string sql = "SELECT CountryID, RegionID, RegionName FROM dbo.Region order by CountryID, RegionName";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        states.Add(reader[0].ToString() + "|" + reader[1].ToString() + "|" + reader[2].ToString());
                    }
                }
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return states;
        }

        public List<string> GetStatesByCountryId(int key)
        {
            List<string> states = new List<string>();
            //states.Add(new ListItem { Value = "0DT", Text = "Select a State.." }); // Add default value 0
            using (SqlConnection conn = ECN_Framework_DataLayer.DataFunctions.GetSqlConnection("UAD_Lookup"))
            {
                string sql = "SELECT RegionID, RegionName FROM dbo.Region WHERE CountryID = " + key + " order by RegionName";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        states.Add(reader[0].ToString() + "|" + reader[1].ToString());
                    }
                }
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }

            return states;
        }

        public List<string> GetStatesByCountryName(string key)
        {
            List<string> states = new List<string>();
            using (SqlConnection conn = ECN_Framework_DataLayer.DataFunctions.GetSqlConnection("UAD_Lookup"))
            {
                string sql = "SELECT r.RegionID, r.RegionName FROM dbo.Region r JOIN dbo.Country c ON r.CountryID = c.CountryID WHERE c.ShortName = '" + key + "' order by RegionName";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        states.Add(reader[0].ToString() + "|" + reader[1].ToString());
                    }
                }
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }

            return states;
        }
        
        public string GetStateCode(int key)
        {
            string state = string.Empty;
            using (SqlConnection conn = ECN_Framework_DataLayer.DataFunctions.GetSqlConnection("UAD_Lookup"))
            {
                string sql = "SELECT RegionCode FROM dbo.Region WHERE RegionID = " + key;
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        state = reader[0].ToString();
                    }
                }
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return state;
        }
        public string GetStateName(string key)
        {
            string state = string.Empty;
            using (SqlConnection conn = ECN_Framework_DataLayer.DataFunctions.GetSqlConnection("UAD_Lookup"))
            {
                string sql = "SELECT RegionName FROM dbo.Region WHERE RegionCode = '" + key + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        state = reader[0].ToString();
                    }
                }
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return state;
        }
    }
}