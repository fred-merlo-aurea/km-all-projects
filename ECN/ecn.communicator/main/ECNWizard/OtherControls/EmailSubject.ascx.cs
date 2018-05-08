using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace ecn.communicator.main.ECNWizard.OtherControls
{
    public partial class EmailSubject : System.Web.UI.UserControl
    {
        public string GetEmailSubject
        {
            get 
            {
                if (string.IsNullOrEmpty(txtEmailSubject.Value))
                {
                    List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECN_Framework_Common.Objects.ECNError>();
                    errorList.Add(new ECN_Framework_Common.Objects.ECNError(ECN_Framework_Common.Objects.Enums.Entity.Blast, ECN_Framework_Common.Objects.Enums.Method.Validate, "Please enter an Email Subject"));
                    throw new ECN_Framework_Common.Objects.ECNException("Please enter an Email Subject", errorList, ECN_Framework_Common.Objects.Enums.ExceptionLayer.WebSite);
                }
                else
                {
                    return txtEmailSubject.Value;
                }
            }

        }

        public void SetEmailSubject(string subject)
        {
            txtEmailSubject.Value = subject;
            //divSubject.Text = ConvertEmailSubject(subject);

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (dlEmoji.Items.Count == 0)
            //{
            //    List<ECN_Framework_Entities.Communicator.UnicodeLookup> glyphs = ECN_Framework_BusinessLayer.Communicator.UnicodeLookup.GetAllActive();
            //    List<UnicodeImage> listGlyphs = new List<UnicodeImage>();
            //    foreach (ECN_Framework_Entities.Communicator.UnicodeLookup ul in glyphs)
            //    {
            //        UnicodeImage ui = new UnicodeImage();
            //        if (!string.IsNullOrEmpty(ul.Base64String))
            //        {
            //            ui.dataURL = ul.Base64String;
            //        }
            //        else
            //        {
            //            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(ul.Glyph + "");
            //            string base64 = Convert.ToBase64String(bytes);
            //            ui.dataURL = "data:image/png;base64," + base64;
            //            ui.glyph = ul.Glyph;
            //        }
            //        ui.Id = ul.Id.ToString();

            //        listGlyphs.Add(ui);
            //    }
            //    dlEmoji.DataSource = listGlyphs;
            //    dlEmoji.DataBind();
            //}
            //divSubject.InnerHtml = ConvertEmailSubject(txtEmailSubject.Value);


        }

        protected void dlEmoji_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ECN_Framework_Entities.Communicator.UnicodeLookup ul = (ECN_Framework_Entities.Communicator.UnicodeLookup)e.Item.DataItem;
            ImageButton imgbtnGlyph = (ImageButton)e.Item.FindControl("imgbtnEmoji");
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(ul.Glyph + "");
            string base64 = Convert.ToBase64String(bytes);

            imgbtnGlyph.ImageUrl = "data:image/png;base64," + base64;
            imgbtnGlyph.CommandArgument = ul.UnicodeNum;
        }

        private string ConvertEmailSubject(string subject)
        {
            Regex regUNI = new Regex("ECN.UNI.(.*?).ECN.UNI");

            MatchCollection mc = regUNI.Matches(subject);

            if (mc != null && mc.Count > 0)
            {
                foreach (Match m in mc)
                {
                    ECN_Framework_Entities.Communicator.UnicodeLookup ul = ECN_Framework_BusinessLayer.Communicator.UnicodeLookup.GetById(Convert.ToInt32(m.Groups[1].Value));

                    string img = "<img id=\"" + ul.Id.ToString() + "\" src=\"" + ul.Base64String + "\" style=\"width:23px;height:23px;\" />";
                    subject = subject.Replace(m.Value, img);
                }
            }



            return subject;
        }


    }

    public class UnicodeImage
    {
        public string dataURL { get; set; }
        public string glyph { get; set; }

        public string Id { get; set; }
    }
}