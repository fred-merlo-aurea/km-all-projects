using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KMPS.MD
{
    public partial class Default : KMPS.MD.Main.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.CrossProductView, KMPlatform.Enums.Access.View))
            {
                hlCrossProductImg.HRef = "#";
                hlCrossProduct.HRef = "#";
            }

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ProductView, KMPlatform.Enums.Access.View))
            {
                hlProductImg.HRef = "#";
                hlProduct.HRef = "#";
            }

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecencyView, KMPlatform.Enums.Access.View))
            {
                hlRecencyImg.HRef = "#";
                hlRecency.HRef = "#";
            }

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ConsensusView, KMPlatform.Enums.Access.View))
            {
                hlConsensusImg.HRef = "#";
                hlConsensus.HRef = "#";
            }
        }
    }
}