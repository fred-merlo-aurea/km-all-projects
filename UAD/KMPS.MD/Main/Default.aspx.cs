using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using RKLib.ExportData;
using System.Xml;
using KMPS.MD.Objects;

namespace KMPS.MD.Main
{
    public partial class Default : KMPS.MD.Main.WebPageHelper
    {
        public Panel CategoryPanel, CategoryCodesPanel, TransactionPanel, QsourcePanel, GeocodePanel, ContactPanel, OtherPanel, PubsPanel, QFromToPanel, AdHocPanel;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Dashboard, KMPlatform.Enums.Access.View))
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                Response.Redirect("dashboard.aspx");
            }
        }
    }
}