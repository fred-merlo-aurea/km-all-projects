using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;

namespace KMPS.MD.Controls
{
    public partial class Circulation : System.Web.UI.UserControl
    {
        public Delegate hideCirculationPopup;

        public Enums.ViewType ViewType
        {
            get
            {
                try
                {
                    return (Enums.ViewType)Enum.Parse(typeof(Enums.ViewType), ViewState["ViewType"].ToString());
                }
                catch
                {
                    return Enums.ViewType.None;
                }
            }
            set
            {
                ViewState["ViewType"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.mpeCirculation.Show();
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if(!IsPostBack)
            {
                lstCategoryCodeType.DataSource = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select().FindAll(x=>x.IsActive == true);
                lstCategoryCodeType.DataBind();

                List<FrameworkUAD_Lookup.Entity.CategoryCode> lcc = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode().Select().FindAll(x => x.IsActive == true);

                var catcode = (from cc in lcc
                                 select new { cc.CategoryCodeID, CategoryCodeName = cc.CategoryCodeValue + ". " + cc.CategoryCodeName });

                lstCategoryCodes.DataSource = catcode;
                lstCategoryCodes.DataBind();

                lstTransaction.DataSource = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType().Select().FindAll(x => x.IsActive == true); 
                lstTransaction.DataBind();

                List<FrameworkUAD_Lookup.Entity.TransactionCode> ltc = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().Select().FindAll(x => x.IsActive == true);

                var transcode = (from tc in ltc
                                 select new { tc.TransactionCodeID, TransactionCodeName = tc.TransactionCodeValue + ". " + tc.TransactionCodeName });

                lstTransactionCode.DataSource = transcode;
                lstTransactionCode.DataBind();

                List<FrameworkUAD_Lookup.Entity.Code> lqsourcetype = new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source_Type).FindAll(x => x.IsActive == true).OrderBy(y => y.DisplayName).ToList();

                lstQsourceType.DataSource = lqsourcetype;
                lstQsourceType.DataBind();

                List<FrameworkUAD_Lookup.Entity.Code> lqualsourcecode = new List<FrameworkUAD_Lookup.Entity.Code>();

                foreach (FrameworkUAD_Lookup.Entity.Code c in lqsourcetype)
                {
                    lqualsourcecode.AddRange(new FrameworkUAD_Lookup.BusinessLogic.Code().SelectChildren(c.CodeId).FindAll(x => x.IsActive == true).OrderBy(y => y.DisplayName).ToList());
                }

                lstQsourceCode.DataSource = lqualsourcecode;
                lstQsourceCode.DataBind();
            }
        }

        public void LoadCirculationControls()
        {
            if (ViewType == Enums.ViewType.ProductView || ViewType == Enums.ViewType.CrossProductView)
            {
                phOtherHeader.Visible = true;
                phOther.Visible = true;
            }
        }

        public List<Field> GetCirculationFilters()
        {
            List<Field> fields = new List<Field>();

            string listvalues;

            listvalues = Utilities.getListboxSelectedValues(lstCategoryCodeType);
            if (listvalues != string.Empty)
                fields.Add(new Field("Category Type", listvalues, Utilities.getListboxText(lstCategoryCodeType), "", Enums.FiltersType.Circulation, "CATEGORYTYPE"));

            listvalues = Utilities.getListboxSelectedValues(lstCategoryCodes);
            if (listvalues != string.Empty)
                fields.Add(new Field("Category Code", listvalues, Utilities.getListboxText(lstCategoryCodes), "", Enums.FiltersType.Circulation, "CATCODES"));

            listvalues = Utilities.getListboxSelectedValues(lstTransaction);
            if (listvalues != string.Empty)
                fields.Add(new Field("XACT", listvalues, Utilities.getListboxText(lstTransaction), "", Enums.FiltersType.Circulation, "TRANSACTION"));

            listvalues = Utilities.getListboxSelectedValues(lstTransactionCode);
            if (listvalues != string.Empty)
                fields.Add(new Field("Transaction Code", listvalues, Utilities.getListboxText(lstTransactionCode), "", Enums.FiltersType.Circulation, "TRANSACTIONCODE"));

            listvalues = Utilities.getListboxSelectedValues(lstQsourceType);
            if (listvalues != string.Empty)
                fields.Add(new Field("QSource Type", listvalues, Utilities.getListboxText(lstQsourceType), "", Enums.FiltersType.Circulation, "QUALSOURCETYPE"));

            listvalues = Utilities.getListboxSelectedValues(lstQsourceCode);
            if (listvalues != string.Empty)
                fields.Add(new Field("QSource Code", listvalues, Utilities.getListboxText(lstQsourceCode), "", Enums.FiltersType.Circulation, "QUALSOURCECODE"));

            if (ViewType == Enums.ViewType.ProductView || ViewType == Enums.ViewType.CrossProductView)
            {
                listvalues = Utilities.getListboxSelectedValues(lstMedia);
                if (listvalues != string.Empty)
                    fields.Add(new Field("Media", listvalues, Utilities.getListboxText(lstMedia), "", Enums.FiltersType.Circulation, "Media"));

                listvalues = Utilities.getListboxSelectedValues(lstYear);
                if (listvalues != string.Empty)
                    fields.Add(new Field("Qualification Year", listvalues, Utilities.getListboxText(lstYear), "", Enums.FiltersType.Circulation, "QualificationYear"));

                if (txtQDateFrom.Text != string.Empty)
                    fields.Add(new Field("QFrom", txtQDateFrom.Text, txtQDateFrom.Text, "", Enums.FiltersType.Circulation, "QDate"));

                if (txtQDateTo.Text != string.Empty)
                    fields.Add(new Field("QTo", txtQDateTo.Text, txtQDateTo.Text, "", Enums.FiltersType.Circulation, "QDate"));

                if (ddlWaveMailing.SelectedValue != "")
                    fields.Add(new Field("Wave Mailing", ddlWaveMailing.SelectedValue, ddlWaveMailing.SelectedItem.Text, "", Enums.FiltersType.Circulation, "WaveMailed"));
            }

            return fields;
        }

        public void LoadCirculationFilters(Field field)
        {
            switch (field.Name.ToUpper())
            {
                case "CATEGORY TYPE":
                    Utilities.SelectFilterListBoxes(lstCategoryCodeType, field.Values);
                    break;
                case "CATEGORY CODE":
                    Utilities.SelectFilterListBoxes(lstCategoryCodes, field.Values);
                    break;
                case "XACT":
                    Utilities.SelectFilterListBoxes(lstTransaction, field.Values);
                    break;
                case "TRANSACTION CODE":
                    Utilities.SelectFilterListBoxes(lstTransactionCode, field.Values);
                    break;
                case "QSOURCE TYPE":
                    Utilities.SelectFilterListBoxes(lstQsourceType, field.Values);
                    break;
                case "QSOURCE CODE":
                    Utilities.SelectFilterListBoxes(lstQsourceCode, field.Values);
                    break;
                case "MEDIA":
                    Utilities.SelectFilterListBoxes(lstMedia, field.Values);
                    break;
                case "QUALIFICATION YEAR":
                    Utilities.SelectFilterListBoxes(lstYear, field.Values);
                    break;
                case "QFROM":
                    txtQDateFrom.Text = field.Values;
                    break;
                case "QTO":
                    txtQDateTo.Text = field.Values;
                    break;
                case "WAVE MAILING":
                    ddlWaveMailing.SelectedValue = field.Values;
                    break;
            }
        }

        public void Reset()
        {
            lstCategoryCodeType.ClearSelection();
            lstCategoryCodes.ClearSelection();
            lstTransaction.ClearSelection();
            lstQsourceType.ClearSelection();
            lstQsourceCode.ClearSelection();
            lstTransactionCode.ClearSelection();
            lstMedia.ClearSelection();
            lstYear.ClearSelection();
            txtQDateFrom.Text = string.Empty;
            txtQDateFrom.Text = string.Empty;
            ddlWaveMailing.ClearSelection();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            hideCirculationPopup.DynamicInvoke();
            this.mpeCirculation.Hide();
        }
        protected void lstCategoryCodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstCategoryCodes.ClearSelection();

            foreach (int i in lstCategoryCodeType.GetSelectedIndices())
            {
                List<FrameworkUAD_Lookup.Entity.CategoryCode> lCategoryCode = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode().Select().FindAll(x => x.CategoryCodeTypeID == Convert.ToInt32(lstCategoryCodeType.Items[i].Value));

                foreach (FrameworkUAD_Lookup.Entity.CategoryCode c in lCategoryCode)
                {
                    foreach (ListItem li in lstCategoryCodes.Items)
                    {
                        if (c.CategoryCodeID == Convert.ToInt32(li.Value))
                            li.Selected = true;
                    }
                }
            }
        }

        protected void lstTransaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstTransactionCode.ClearSelection();

            foreach (int i in lstTransaction.GetSelectedIndices())
            {
                List<FrameworkUAD_Lookup.Entity.TransactionCode> lTransactionCode = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().Select().FindAll(x => x.TransactionCodeTypeID == Convert.ToInt32(lstTransaction.Items[i].Value));

                foreach (FrameworkUAD_Lookup.Entity.TransactionCode c in lTransactionCode)
                {
                    foreach (ListItem li in lstTransactionCode.Items)
                    {
                        if (c.TransactionCodeID == Convert.ToInt32(li.Value))
                            li.Selected = true;
                    }
                }
            }
        }


        protected void lstQsourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstQsourceCode.ClearSelection();

            foreach (int i in lstQsourceType.GetSelectedIndices())
            {
                List<FrameworkUAD_Lookup.Entity.Code> lqsourceCode = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectChildren(Convert.ToInt32(lstQsourceType.Items[i].Value));

                foreach (FrameworkUAD_Lookup.Entity.Code c in lqsourceCode)
                {
                    foreach (ListItem li in lstQsourceCode.Items)
                    {
                        if (c.CodeId == Convert.ToInt32(li.Value))
                            li.Selected = true;
                    }
                }
            }
        }
    }
}