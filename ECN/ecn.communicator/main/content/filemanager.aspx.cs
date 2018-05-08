using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ecn.communicator.contentmanager
{
    public partial class filemanager : ECN_Framework.WebPageHelper
    {
        protected System.Web.UI.WebControls.Button SaveButton;
        protected System.Web.UI.WebControls.Button UpdateButton;

        decimal storageSpacePurchased = 0.0M;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT; ;
            Master.SubMenu = "manage images";
            Master.Heading = "Content/Messages > Images/Storage";
            Master.HelpContent = "<b>Uploading images</b><br/><div id='par1'><ul><li>Click on <em>Upload Images</em>.</li><li>Click the <em>browse</em> button and find the image you want to upload from your hard drive.</li>&#13;&#10;<li>Click <em>Add</em> (the image name will appear in the blank box).</li><li>Click Upload.</li><li>To view the images, click on Browse Images.</li></ul>&#13;&#10;&#13;&#10;";
            Master.HelpTitle = "Image Manager";

            string StoragePath = "/customers/" + Master.UserSession.CurrentUser.CustomerID;
            string ImagesWebPath = StoragePath + "/images/";
            string DataWebPath = StoragePath + "/data";

            ECN_Framework_BusinessLayer.Communicator.NewFile.CreateDirectoryIfDNE(ImagesWebPath, ECN_Framework_Common.Objects.Enums.Entity.ImageFolder);
            ECN_Framework_BusinessLayer.Communicator.NewFile.CreateDirectoryIfDNE(DataWebPath, ECN_Framework_Common.Objects.Enums.Entity.ImageFolder);

            if (ECN_Framework_BusinessLayer.Communicator.Content.HasPermission(KMPlatform.Enums.Access.View, Master.UserSession.CurrentUser))
            {
                string channelID = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
                string custID = Master.UserSession.CurrentUser.CustomerID.ToString();


                string ImagesFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + ImagesWebPath);
                string DataFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + DataWebPath);
                maingallery.imageDirectory = ImagesWebPath;

                decimal MB = 0.0M;
                MB = ECN_Framework_BusinessLayer.Communicator.NewFile.StorageUsed(ImagesFilePath, DataFilePath);
                storageSpacePurchased = ECN_Framework_BusinessLayer.Communicator.NewFile.StoragePurchased(custID, Master.UserSession.CurrentUser);
                decimal percentageUsed = ECN_Framework_BusinessLayer.Communicator.NewFile.StoragePercentageUsed(MB, storageSpacePurchased);


                INFO_StorageCanBeUsedLBL1.Text = INFO_StorageCanBeUsedLBL2.Text = storageSpacePurchased.ToString();


                if (MB <= 0.0M)
                {
                    StorageUsedLBL.Text = "0";
                    StorageCanBeUsedLBL.Text = storageSpacePurchased.ToString();
                    StorageAvailableLBL.Text = storageSpacePurchased.ToString();
                    capacityBar.Style.Add("WIDTH", "0%");
                    capacityBarArrow.Style.Add("WIDTH", "0%");
                }
                else
                {
                    if (Convert.ToDecimal(storageSpacePurchased) > MB)
                    {
                        StorageUsedLBL.Text = MB.ToString();
                        StorageCanBeUsedLBL.Text = storageSpacePurchased.ToString();
                        StorageAvailableLBL.Text = Convert.ToString(storageSpacePurchased - MB);
                        percentageUsed = (MB / Convert.ToDecimal(storageSpacePurchased)) * 100;
                        capacityBar.Style.Add("WIDTH", percentageUsed + "%");
                        capacityBarArrow.Style.Add("WIDTH", percentageUsed + "%");
                    }
                    else
                    {
                        StorageUsedLBL.Text = MB.ToString();
                        StorageCanBeUsedLBL.Text = storageSpacePurchased.ToString();
                        StorageUsedLBL.Text = MB.ToString();
                        StorageAvailableLBL.Text = "0";
                        capacityBar.Style.Add("WIDTH", "99%");
                        capacityBarArrow.Style.Add("WIDTH", "99%");
                    }
                }

            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }
    }

}
