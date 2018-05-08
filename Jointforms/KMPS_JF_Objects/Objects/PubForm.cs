using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using KM.Common;

namespace KMPS_JF_Objects.Objects
{
    [Serializable]
    public class PubForm
    {        
        //pubforms
        public int PFID { get; set; }
        public int PubID { get; set; }
        public string FormName { get; set; }
        public string Description { get; set; }
        public bool ShowDigital { get; set; }
        public bool ShowPrint { get; set; }
        public bool ShowPrintAsDigital { get; set; }                                                            
        public bool IsPaid { get; set; }
        public double PaidPrice { get; set; }
        public string NewsletterPosition { get; set; }
        public bool PreSelectNewsletters { get; set; }
        public string NQPrintRedirectUrl { get; set; }
        public string NQDigitalRedirectUrl { get; set; }
        public string NQBothRedirectUrl { get; set; }
        public bool IsNonQualSetup { get; set; }  
        public bool DisableNonQualForDigital { get; set; }

        public bool SuspendECNPostforBoth { get; set; }  
        
        public bool ShowNQPrintAsDigital { get; set; }  
        public string PRINTDIGITALQuestion { get; set; }                     
        public string SUBSCRIPTIONQuestion { get; set; }

        //PubFormLandingPages
        public string PrintThankYouPageHTML { get; set; }
        public string DigitalThankYouPageHTML { get; set; }   
        public string DefaultThankYouPageHTML { get; set; }  
        public string NQResponsePageHTML { get; set; }
        public string NQResponseCounPageHTML { get; set; }        

        public bool SetupResponseEmail { get; set; }
        public bool EnablePrintAndDigital { get; set; }
        public bool ShowNewsletterAsCollapsed { get; set; }     
        public bool ShowNewsletterSearch { get; set; } 

        private List<PubFormField> _Fields = null;
        public List<PubFormField> Fields
        {
            get
            {
                if (_Fields == null)
                    _Fields = PubFormField.GetPubFormFields(PFID);         

                return _Fields;
            }
            set { _Fields = value; }
        }

        public PubForm()
        { 
        
        }

        public static PubForm GetPubForm(int PubFormID)  
        {
            PubForm pf = null;

            if (CacheUtil.IsCacheEnabled())  
            {
                pf = (PubForm)CacheUtil.GetFromCache("PubForm_" + PubFormID, "JOINTFORMS");

                if (pf == null)
                {
                    pf = GetData(PubFormID);
                    CacheUtil.AddToCache("PubForm_" + PubFormID, pf, "JOINTFORMS");
                }
                return pf;
            }
            else
            {
                return GetData(PubFormID);    
            }
        }

        private static PubForm GetData(int PubFormID)
        {
            PubForm pf = new PubForm();

            SqlCommand cmd = new SqlCommand(string.Format("select * from PubForms pf  with (NOLOCK) left outer join PubFormLandingPages plp  with (NOLOCK) on pf.PFID = plp.pfID where pf.PFID = {0}", PubFormID));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;

            DataTable dt = DataFunctions.GetDataTable(cmd);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                pf.PFID = PubFormID;
                pf.PubID = Convert.ToInt32(dr["PubID"]);
                pf.FormName = dr["FormName"].ToString();
                pf.Description = dr["Description"].ToString();
                pf.ShowDigital = dr.IsNull("ShowDigital") ? true : Convert.ToBoolean(dr["ShowDigital"]);
                pf.ShowPrint = dr.IsNull("ShowPrint") ? true : Convert.ToBoolean(dr["ShowPrint"]);
                pf.ShowPrintAsDigital = dr.IsNull("ShowPrintAsDigital") ? false : Convert.ToBoolean(dr["ShowPrintAsDigital"]);
                pf.IsPaid = dr.IsNull("IsPaid") ? false : Convert.ToBoolean(dr["IsPaid"]);
                pf.PaidPrice = Convert.ToDouble(dr["PaidPrice"]);
                pf.NewsletterPosition = dr["NewsletterPosition"].ToString();
                pf.PreSelectNewsletters = dr.IsNull("PreSelectNewsletters") ? false : Convert.ToBoolean(dr["PreSelectNewsletters"]);
                pf.NQPrintRedirectUrl = dr.IsNull("NQPrintRedirectUrl") ? string.Empty : dr["NQPrintRedirectUrl"].ToString();
                pf.NQDigitalRedirectUrl = dr.IsNull("NQDigitalRedirectUrl") ? string.Empty : dr["NQDigitalRedirectUrl"].ToString();
                pf.NQBothRedirectUrl = dr.IsNull("NQBothRedirectUrl") ? string.Empty : dr["NQBothRedirectUrl"].ToString();
                pf.IsNonQualSetup = dr.IsNull("IsNonQualSetup") ? false : Convert.ToBoolean(dr["IsNonQualSetup"].ToString());
                pf.DisableNonQualForDigital = dr.IsNull("DisableNonQualDigital") ? false : Convert.ToBoolean(dr["DisableNonQualDigital"].ToString());
                pf.SuspendECNPostforBoth = dr.IsNull("SuspendECNPostforBoth") ? false : Convert.ToBoolean(dr["SuspendECNPostforBoth"].ToString());
                pf.ShowNQPrintAsDigital = dr.IsNull("ShowNQPrintAsDigital") ? false : Convert.ToBoolean(dr["ShowNQPrintAsDigital"].ToString());
                pf.PRINTDIGITALQuestion = dr.IsNull("PRINTDIGITALQuestion") ? string.Empty : dr["PRINTDIGITALQuestion"].ToString();
                pf.SUBSCRIPTIONQuestion = dr.IsNull("SUBSCRIPTIONQuestion") ? string.Empty : dr["SUBSCRIPTIONQuestion"].ToString();

                pf.PrintThankYouPageHTML = dr.IsNull("PrintThankYouPageHTML") ? string.Empty : dr["PrintThankYouPageHTML"].ToString();
                pf.DigitalThankYouPageHTML = dr.IsNull("DigitalThankYouPageHTML") ? string.Empty : dr["DigitalThankYouPageHTML"].ToString();
                pf.DefaultThankYouPageHTML = dr.IsNull("DefaultThankYouPageHTML") ? string.Empty : dr["DefaultThankYouPageHTML"].ToString();
                pf.NQResponsePageHTML = dr.IsNull("NQResponsePageHTML") ? string.Empty : dr["NQResponsePageHTML"].ToString();
                pf.NQResponseCounPageHTML = dr.IsNull("NQResponseCounPageHTML") ? string.Empty : dr["NQResponseCounPageHTML"].ToString();

                pf.SetupResponseEmail = dr.IsNull("SetupResponseEmail") ? false : Convert.ToBoolean(Convert.ToInt32(dr["SetupResponseEmail"].ToString()));
                pf.EnablePrintAndDigital = dr.IsNull("EnablePrintAndDigital") ? false : Convert.ToBoolean(dr["EnablePrintAndDigital"].ToString());
                pf.ShowNewsletterAsCollapsed = dr.IsNull("ShowNewsletterAsCollapsed") ? true : Convert.ToBoolean(dr["ShowNewsletterAsCollapsed"].ToString());
                pf.ShowNewsletterSearch = dr.IsNull("ShowNewsletterAsCollapsed") ? false : Convert.ToBoolean(dr["ShowNewsletterSearch"].ToString());

                pf.Fields = PubFormField.GetPubFormFields(pf.PFID);
            }
            return pf;
        }
    }
}


