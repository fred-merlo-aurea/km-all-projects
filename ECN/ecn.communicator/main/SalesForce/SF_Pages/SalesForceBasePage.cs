using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AddressValidator;
using ecn.communicator.main.Salesforce.Controls;
using ecn.communicator.main.Salesforce.Entity;
using ecn.communicator.main.Salesforce.Extensions;
using ecn.communicator.main.Salesforce.SF_Pages.Constants;
using ecn.communicator.main.Salesforce.SF_Pages.Converters;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using BusinessEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup;
using BusinessGroup = ECN_Framework_BusinessLayer.Communicator.Group;
using KMUser = KMPlatform.Entity.User;

namespace ecn.communicator.main.Salesforce.SF_Pages
{
    public abstract class SalesForceBasePage : SessionStoragePage
    {
        private const int Zero = 0;
        private const string MasterSubMenuName = "Sync";
        private const string SfCountKey = "SF_SelectedCount";
        private const string EcnCountKey = "ECN_SelectedCount";
        private const string SfAddressKey = "Address_SF";
        private const string EcnAddressKey = "Address_ECN";
        private const string FilterDiffDataValue = "DiffData";

        private Dictionary<Color, Action<string, GroupType>> _actionsOnRowColor = new Dictionary<Color, Action<string, GroupType>>();

        protected virtual string SfSortDirKey { get; }
        protected virtual string EcnSortDirKey { get; }
        protected virtual string EcnSortExpKey { get; }
        protected virtual string SfSortExpKey { get; }
        protected virtual string EcnCheckBoxId { get; }
        protected virtual string SfCheckBoxId { get; }
        protected virtual string EcnHeaderCheckBoxId { get; }
        protected virtual string SfHeaderCheckBoxId { get; }
        protected virtual GridView EcnGrid { get; }
        protected virtual GridView SfGrid { get; }
        protected virtual Message MessageControl { get; }
        protected virtual EcnSfConverterBase ViewModelConverter { get; }

        protected AddressLocation Address_SF
        {
            get
            {
                return Get<AddressLocation>(SfAddressKey, null);
            }
            set
            {
                Set(SfAddressKey, value);
            }
        }

        protected AddressLocation Address_ECN
        {
            get
            {
                return Get<AddressLocation>(EcnAddressKey, null);
            }
            set
            {
                Set(EcnAddressKey, value);
            }
        }

        protected SortDirection SFSortDir
        {
            get
            {
                return Get(SfSortDirKey, SortDirection.Ascending);
            }
            set
            {
                Set(SfSortDirKey, value);
            }
        }

        protected SortDirection ECNSortDir
        {
            get
            {
                return Get(EcnSortDirKey, SortDirection.Ascending);
            }
            set
            {
                Set(EcnSortDirKey, value);
            }
        }

        protected string ECNSortExp
        {
            get
            {
                return Get(EcnSortExpKey, string.Empty);
            }
            set
            {
                Set(EcnSortExpKey, value);
            }
        }

        protected string SFSortExp
        {
            get
            {
                return Get(SfSortExpKey, string.Empty);
            }
            set
            {
                Set(SfSortExpKey, value);
            }
        }

        protected int SF_SelectedCount
        {
            get
            {
                return Get(SfCountKey, 0);
            }
            set
            {
                Set(SfCountKey, value);
            }
        }

        protected int ECN_SelectedCount
        {
            get
            {
                return Get(EcnCountKey, 0);
            }
            set
            {
                Set(EcnCountKey, value);
            }
        }

        protected void Setup(MasterPages.Communicator master)
        {
            master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.SALESFORCE;
            master.SubMenu = MasterSubMenuName;
            master.Heading = string.Empty;
        }

        protected void UpdateControlsOnCheck(CheckBox checkbox, GroupType grp)
        {
            if (checkbox.IsGreyDark())
            {
                DisableAllExceptCurrent(checkbox, grp);
                UpdateButtonsSingle(grp);
            }
            else if (checkbox.IsGreyLight() && checkbox.Checked)
            {
                UpdateButtons_Multi(grp);
            }
            else
            {
                ResetButtons();
            }

            var count = CalculateCount(grp, checkbox.Checked);
            SetSelectedCount(grp, count);

            if (count == 0)
            {
                ResetButtons();
            }

            Uncheck(grp.Opposite());
        }

        protected abstract void ResetButtons();

        protected abstract void UpdateButtons_Multi(GroupType grp);

        protected abstract void PopulateActionsOnRowColor(GroupType grp);

        protected void AddActionByColor(Color color, Action<string, GroupType> action)
        {
            _actionsOnRowColor.Add(color, action);
        }

        protected void DisableAllExceptCurrent(CheckBox sender, GroupType grp)
        {
            var isChecked = sender.Checked;
            var checkboxId = GetIdByType(grp);
            var grid = GetGridByType(grp);
            foreach (GridViewRow gvr in grid.Rows)
            {
                var checkbox = (CheckBox)gvr.FindControl(checkboxId);
                if (checkbox != null && checkbox != sender)
                {
                    checkbox.Checked = false;
                    checkbox.Enabled = !isChecked;
                    checkbox.Font.Italic = isChecked;
                    if (grp.IsEcn())
                    {
                        checkbox.ForeColor = isChecked ? Color.Gray : Color.Black;
                    }
                }
            }
            sender.Enabled = true;
        }

        protected void HideHeaderCheckBox(GridViewRow row, string checkboxId, string selectedValue)
        {
            if (row.RowType == DataControlRowType.Header)
            {
                var headerCheckBox = row.FindControl(checkboxId) as CheckBox;
                if (headerCheckBox != null)
                {
                    var visible = selectedValue != FilterDiffDataValue;
                    headerCheckBox.Visible = visible;
                }
            }
        }

        protected void Uncheck(GroupType grp)
        {
            SetSelectedCount(grp, 0);
            var grid = GetGridByType(grp);
            var checkboxId = GetIdByType(grp);
            var headerCheckboxId = GetHeaderCheckBoxId(grp);
            foreach (GridViewRow row in grid.Rows)
            {
                var checkBox = (CheckBox)row.FindControl(checkboxId);
                checkBox.Checked = false;
                checkBox.Enabled = true;
            }

            if (grid.HeaderRow != null)
            {
                var checkBox = (CheckBox)grid.HeaderRow.FindControl(headerCheckboxId);
                checkBox.Checked = false;
            }
        }

        protected int GetSelectedCount(GroupType grp)
        {
            return grp.IsEcn() ? ECN_SelectedCount : SF_SelectedCount;
        }

        protected void SetSelectedCount(GroupType grp, int count)
        {
            if (grp.IsEcn())
            {
                ECN_SelectedCount = count;
            }
            else
            {
                SF_SelectedCount = count;
            }
        }

        protected int CalculateCount(GroupType grp, bool isChecked)
        {
            var currentCount = GetSelectedCount(grp);
            if (isChecked)
            {
                currentCount++;
            }
            else
            {
                currentCount--;
            }

            return currentCount >= 0 ? currentCount : 0;
        }

        protected string GetIdByType(GroupType grp)
        {
            return grp.IsEcn() ? EcnCheckBoxId : SfCheckBoxId;
        }

        protected string GetHeaderCheckBoxId(GroupType grp)
        {
            return grp.IsEcn() ? EcnHeaderCheckBoxId : SfHeaderCheckBoxId;
        }

        protected GridView GetGridByType(GroupType grp)
        {
            return grp.IsEcn() ? EcnGrid : SfGrid;
        }

        protected string GetCommandName(GroupType grp)
        {
            return grp.ToString();
        }

        protected Hashtable CreateUDF(int groupID, KMPlatform.Entity.User user)
        {
            const string LongIdName = "SalesforceID";
            const string LongTypeName = "Salesforce Type";
            const string ShortIdName = "SFID";
            const string ShortTypeName = "SFType";

            TrySaveGroup(groupID, LongIdName, ShortIdName, user);
            TrySaveGroup(groupID, LongTypeName, ShortTypeName, user);

            return LoadFields(groupID, user);
        }

        private void TrySaveGroup(int groupId, string longName, string shortName, KMPlatform.Entity.User user)
        {
            const string No = "N";
            try
            {
                var fields = new ECN_Framework_Entities.Communicator.GroupDataFields
                {
                    GroupID = groupId,
                    IsPublic = No,
                    LongName = longName,
                    ShortName = shortName,
                    CustomerID = user.CustomerID,
                    CreatedUserID = user.UserID
                };

                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(fields, user);
            }
            catch
            {
                //ToDo: add logging
                //It was commented before refactoring was started. Not sure its nessessary. Leat as is 
            }
        }

        private Hashtable LoadFields(int groupId, KMPlatform.Entity.User user)
        {
            var fields = new Hashtable();
            var gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupId, user);
            if (gdfList.Count > 0)
            {
                foreach (var groupField in gdfList)
                {
                    fields.Add($"user_{groupField.ShortName.ToLower()}", groupField.GroupDataFieldsID);
                }
            }

            return fields;
        }

        protected virtual LocationEntity GetLocationEntity(string token, string whereExpression)
        {
            return new LocationEntity();
        }

        protected EcnSfViewModel BuildViewModel(List<Email> emails, string command, string arg)
        {
            var items = GetItems(emails, command, arg);
            Email email = items.Item1;
            LocationEntity entity = items.Item2;

            var ecnAddress = BuildValidAddress(email);
            var sfAddress = BuildValidAddress(entity);

            return ViewModelConverter.Convert(email, entity, ecnAddress, sfAddress);
        }

        protected void FillHidden(EcnSfViewModel model, HiddenField ecn, HiddenField sf)
        {
            ecn.Value = model.EcnId;
            sf.Value = model.SfId;
        }

        protected void Fill(Label ecnLabel, Label sfLabel, RadioButtonList radioButtonList, ItemViewModel model)
        {
            ecnLabel.Text = model.EcnText;
            sfLabel.Text = model.SfText;
            radioButtonList.Visible = model.Visible;
            SetLabelsColor(model.EcnColor, ecnLabel);
            SetLabelsColor(model.SfColor, sfLabel);
        }

        public Group GetGroup(KMUser user, string groupName, string groupType, string existGroupId, string folderValue)
        {
            const string newValue = "new";
            const string existValue = "existing";

            if (groupType.Equals(newValue, StringComparison.OrdinalIgnoreCase))
            {
                return CreateGroup(user, groupName, folderValue);
            }

            var groupId = -1;
            if (groupType.Equals(existValue, StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(existGroupId, out groupId) && groupId > 0)
                {
                    return BusinessGroup.GetByGroupID(groupId, user);
                }
                else
                {
                    ShowError(UserMessages.SelectGroup);
                }
            }

            return null;
        }

        private Group CreateGroup(KMUser user, string groupName, string folderValue)
        {
            Group ecnGroup = null;
            try
            {
                var folderId = 0;
                int.TryParse(folderValue, out folderId);
                var clearGroupName = SF_Utilities.CleanStringSqlInjection(groupName);

                if (string.IsNullOrEmpty(groupName))
                {
                    ShowError(UserMessages.EmptyGroupNameMessage);
                    return null;
                }

                if (BusinessGroup.Exists(-1, clearGroupName, folderId, user.CustomerID))
                {
                    ShowError(UserMessages.AlreadyExistGroupMessage);
                    return null;
                }

                ecnGroup = BuildGroup(user, clearGroupName, folderId);
                ecnGroup.GroupID = BusinessGroup.Save(ecnGroup, user);
            }
            catch (ECNException)
            {
                // ToDo: possible place to add logs, not sure it will be useful 
            }

            if (ecnGroup == null || ecnGroup.GroupID <= 0)
            {
                ShowError(UserMessages.UnableToCreateGroupMessage);
                return null;
            }

            return ecnGroup;
        }

        private Group BuildGroup(KMUser user, string name, int folderId)
        {
            const string Yes = "Y";
            const string customer = "customer";

            return new Group
            {
                GroupName = name,
                FolderID = folderId,
                AllowUDFHistory = Yes,
                OwnerTypeCode = customer,
                CreatedUserID = user.UserID,
                CustomerID = user.CustomerID,
                PublicFolder = 0,
                IsSeedList = false,
                MasterSupression = 0
            };
        }

        protected DataTable ImportEmails(KMUser user, int groupId, string profile, string xmlUdf)
        {
            const int MinProfileEntries = 15;
            const string Format = "HTML";
            const string SubscriberType = "S";

            const bool onlyEmail = false;
            DataTable dtResults = null;

            if (profile.Length > MinProfileEntries && groupId > Zero)
            {
                try
                {
                    var source = GetType().FullName;
                    var udfFormat = $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlUdf}</XML>";

                    dtResults = BusinessEmailGroup.ImportEmails(user, user.CustomerID, groupId, profile, udfFormat, Format, SubscriberType, onlyEmail, string.Empty, source);
                }
                catch (Exception ex)
                {
                    SF_Utilities.LogException(ex);
                    ShowError(UserMessages.UnableToSyncMessage);
                }
            }

            return dtResults;
        }

        protected Tuple<string, string> BuildUdf(LocationEntity[] entities, Hashtable fields)
        {
            const int EmailColumn = 1;
            var xmlUdf = new StringBuilder();
            var xmlprofileUdf = new StringBuilder();
            xmlprofileUdf.Append("<XML>");

            foreach (GridViewRow gvr in SfGrid.Rows)
            {
                var checkbox = gvr.FindControl(SfCheckBoxId) as CheckBox;
                if (checkbox != null && checkbox.Checked)
                {
                    var email = gvr.Cells[EmailColumn].Text;
                    var entity = entities.First(x => x.Email == email);
                    try
                    {
                        if (entity.State.Trim().Length > 2)
                        {
                            entity.State = SF_Utilities.GetStateAbbr(entity.State);
                        }

                        xmlprofileUdf.Append(entity.ToXml());

                        if (fields.Count > 0)
                        {
                            xmlUdf.Append(BuildUdf(entity, fields));
                        }
                    }
                    catch
                    {
                        xmlprofileUdf.Append("</Emails>");
                    }
                }
            }
            xmlprofileUdf.Append("</XML>");

            return new Tuple<string, string>(xmlprofileUdf.ToString(), xmlUdf.ToString());
        }
        protected string BuildUdf(LocationEntity entity, Hashtable fields)
        {
            var udfBuilder = new StringBuilder();
            //SF ID UDF
            udfBuilder.Append("<row>");
            udfBuilder.Append($"<ea>{entity.Email}</ea>");
            udfBuilder.Append($"<udf id=\"{fields["user_sfid"].ToString()}\">");
            udfBuilder.Append($"<v>{entity.Id}</v>");
            udfBuilder.Append("</udf>");
            udfBuilder.Append("</row>");

            //SF Type UDF
            udfBuilder.Append("<row>");
            udfBuilder.Append($"<ea>{entity.Email}</ea>");
            udfBuilder.Append($"<udf id=\"{fields["user_sftype"].ToString()}\">");
            udfBuilder.Append("<v>Lead</v>");
            udfBuilder.Append("</udf>");
            udfBuilder.Append("</row>");

            return udfBuilder.ToString();
        }

        protected void ShowError(string message)
        {
            const string ErrorTitle = "ERROR";
            ShowMessage(message, ErrorTitle, Message.Message_Icon.error);
        }

        protected void ShowMessage(string message, string title, Message.Message_Icon icon)
        {
            if (MessageControl != null)
            {
                MessageControl.Show(message, title, icon);
            }
        }

        private void SetLabelsColor(ColorName color, Label lbl)
        {
            switch (color)
            {
                case ColorName.BlueLight:
                    lbl.BackColor = KM_Colors.BlueLight;
                    lbl.ForeColor = Color.White;
                    break;

                case ColorName.GreyDark:
                    lbl.BackColor = KM_Colors.GreyDark;
                    lbl.ForeColor = Color.White;
                    break;

                case ColorName.Transparent:
                    lbl.BackColor = Color.Transparent;
                    lbl.ForeColor = Color.Black;
                    break;

                default:
                    return;
            }
        }

        private void UpdateButtonsSingle(GroupType grp)
        {
            var id = string.Empty;
            var grid = GetGridByType(grp);
            var selectedRow = grid.Rows[0];

            foreach (GridViewRow gvr in grid.Rows)
            {
                var checkboxId = GetIdByType(grp);
                var checkbox = (CheckBox)gvr.FindControl(checkboxId);
                if (checkbox != null && checkbox.Checked)
                {
                    id = grid.DataKeys[gvr.RowIndex].Value.ToString();
                    selectedRow = gvr;
                }
            }

            PopulateActionsOnRowColor(grp);
            if (_actionsOnRowColor.ContainsKey(selectedRow.BackColor))
            {
                var action = _actionsOnRowColor[selectedRow.BackColor];
                action(id, grp);
            }
            _actionsOnRowColor.Clear();
        }

        private Tuple<Email, LocationEntity> GetItems(List<Email> emails, string command, string arg)
        {
            var email = new Email();
            var entity = new LocationEntity();
            const string ecnCommand = "ECN";
            var token = SF_Authentication.Token.access_token;

            if (command.Equals(ecnCommand, StringComparison.OrdinalIgnoreCase))
            {
                var emailId = Convert.ToInt32(arg);
                email = emails.First(x => x.EmailID == emailId);
                entity = GetLocationEntity(token, $"WHERE Email = '{email.EmailAddress}'");
            }
            else
            {
                entity = GetLocationEntity(token, $"WHERE Id = '{arg}'");
                email = emails.First(x => x.EmailAddress.Equals(entity.Email, StringComparison.OrdinalIgnoreCase));
            }

            return new Tuple<Email, LocationEntity>(email, entity);
        }

        private AddressLocation BuildValidAddress(Email email)
        {
            var location = email.BuildAddressLocation();
            var valid = location.GetValidAddress();
            if (valid != null)
            {
                Address_ECN = valid;
            }

            return Address_ECN;
        }

        private AddressLocation BuildValidAddress(LocationEntity entity)
        {
            var location = entity.BuildAddressLocation();
            var valid = location.GetValidAddress();
            if (valid != null)
            {
                Address_SF = valid;
            }

            return Address_SF;
        }
    }
}