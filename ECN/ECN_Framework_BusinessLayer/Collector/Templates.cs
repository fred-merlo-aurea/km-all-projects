using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Collector
{
    [Serializable]
    public class Templates
    {
        public static List<ECN_Framework_Entities.Collector.Templates> GetbyCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Collector.Templates> itemList = new List<ECN_Framework_Entities.Collector.Templates>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Collector.Templates.GetByCustomerID(customerID, user.UserID);
                scope.Complete();
            }
            return itemList;
        }

        public static ECN_Framework_Entities.Collector.Templates GetByTemplateID(int TemplateID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Collector.Templates item = new ECN_Framework_Entities.Collector.Templates();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Collector.Templates.GetByTemplateID(TemplateID);
                scope.Complete();
            }
            return item;
        }

        public static void Save(ECN_Framework_Entities.Collector.Templates Template, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Template.TemplateID = ECN_Framework_DataLayer.Collector.Templates.Save(Template, user.UserID);
                scope.Complete();
            }
        }

        public static bool Exists(string TemplateName, int customerID, KMPlatform.Entity.User user)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Collector.Templates.Exists(TemplateName, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static string RenderStyle(ECN_Framework_Entities.Collector.Templates sStyle)
        {
            char[] delimiter = { ' ' };
            string hMargin = sStyle.hMargin.Replace("px", "");
            string[] ahMargin = hMargin.Split(delimiter);

            string hleftmargin = ahMargin[0];
            string htopmargin = ahMargin[1];
            string hbottommargin = ahMargin[2];
            string hrightmargin = ahMargin[3];

            string fMargin = sStyle.fMargin.Replace("px", "");
            string[] afMargin = fMargin.Split(delimiter);

            string fleftmargin = afMargin[0];
            string ftopmargin = afMargin[1];
            string fbottommargin = afMargin[2];
            string frightmargin = afMargin[3];

            return "<style>.surveybody {height:100%;background-color:" + sStyle.pbgcolor + "; FONT-FAMILY:" + sStyle.pfontfamily + ";text-align:" + sStyle.pAlign + "} " +
                " .outertable {background-color:" + sStyle.bbgcolor + ";" + (sStyle.pBorder ? ";BORDER:" + sStyle.pBordercolor + " 1px solid;" : "") + " MARGIN:0px " + (sStyle.pAlign.ToLower() == "left" ? "" : " auto") + "; OVERFLOW:hidden; WIDTH:" + sStyle.pWidth + "; text-align:left;}" +
                " .divHeader { background-color:" + sStyle.hbgcolor + "; TEXT-ALIGN:" + sStyle.hAlign + ";}" +
                " .divHeaderIMG { MARGIN-left:" + hleftmargin + "px;MARGIN-top:" + htopmargin + "px;MARGIN-bottom:" + hbottommargin + "px;MARGIN-right:" + hrightmargin + "px; }" +
                " .divpageHeader { PADDING-left:15px;PADDING-right:5px;PADDING-top:8px;PADDING-bottom:8px; FONT-WEIGHT: " + (sStyle.qbold ? "bold" : "normal") + "; FONT-SIZE: " + sStyle.phfontsize + "; background-color: " + sStyle.phbgcolor + "; COLOR: " + sStyle.phcolor + ";}" +
                " .divpageDesc { PADDING-LEFT:20px;PADDING-right:20px;PADDING-top:6px;PADDING-bottom:6px; FONT-WEIGHT: " + (sStyle.pdbold ? "bold" : "normal") + "; FONT-SIZE: " + sStyle.pdfontsize + "; background-color: " + sStyle.pdbgcolor + "; COLOR: " + sStyle.pdcolor + ";}" +
                " .surveytable { PADDING:20px 20px 20px 20px;}" +
                " .questionno {visibility:" + (sStyle.ShowQuestionNo ? "visible" : "hidden") + "}" +
                " .question { PADDING:10px 10px 5px 20px;FONT-WEIGHT:" + (sStyle.qbold ? "bold" : "normal") + "; FONT-SIZE:" + sStyle.qfontsize + "; COLOR:" + sStyle.qcolor + "}" +
                " .vstyle { PADDING:10px 10px 5px 20px;FONT-WEIGHT:" + (sStyle.qbold ? "bold" : "normal") + "; FONT-SIZE:" + sStyle.qfontsize + "; COLOR:red; background-color:#ffffff;}" +
                " .answer { PADDING:5px 5px 5px 40px; FONT-WEIGHT:" + (sStyle.abold ? "bold" : "normal") + "; FONT-SIZE:" + sStyle.afontsize + "; COLOR:" + sStyle.acolor + "}" +
                " .answer select {FONT-WEIGHT:" + (sStyle.abold ? "bold" : "normal") + "; FONT-SIZE:" + sStyle.afontsize + "; COLOR:" + sStyle.acolor + "}" +
                " .gridColumn { text-align:center; FONT-WEIGHT:" + (sStyle.abold ? "bold" : "normal") + "; FONT-SIZE:" + sStyle.afontsize + "; COLOR:" + sStyle.acolor + "}" +
                " .gridRow { text-align:left; FONT-WEIGHT:" + (sStyle.abold ? "bold" : "normal") + "; FONT-SIZE:" + sStyle.afontsize + "; COLOR:" + sStyle.acolor + "}" +
                " .divFooter { background-color:" + sStyle.fbgcolor + ";text-align:" + sStyle.fAlign + ";}" +
                " .tblSurveyGrid {background-color:white;} " +
                " .tblSurveyGrid td {border:1px #cccccc solid } " +
                " .progresslabel {font-size:10px;	font-family:Arial, Helvetica, sans-serif;	color:#000000;	font-weight:normal;} " +
                " .divFooterIMG { MARGIN-left:" + fleftmargin + "px;MARGIN-top:" + ftopmargin + "px;MARGIN-bottom:" + fbottommargin + "px;MARGIN-right:" + frightmargin + "px;}</style>";
        }
    }
}
