using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Entities.Communicator;
using KM.Common;

namespace ecn.communicator.main.Omniture
{
    public class OmniArgs
    {
        public OmniArgs() {}

        public OmniArgs(
            ImageButton imgBtnOmni,
            ITextControl lblOmniture,
            RadioButtonList rblReqOmni,
            RadioButtonList rblCustomOmni,
            DropDownList ddlOmniDefault)
        {
            ImgBtnOmni = imgBtnOmni;
            LblOmniture = lblOmniture;
            RblReqOmni = rblReqOmni;
            RblCustomOmni = rblCustomOmni;
            DdlOmniDefault = ddlOmniDefault;
        }

        public void EnsureNotNull()
        {
            Guard.NotNull(LtpList, nameof(LtpList));
            Guard.NotNullOrWhitespace(ParamValueOmniture, nameof(ParamValueOmniture));
            Guard.NotNull(ImgBtnOmni, nameof(ImgBtnOmni));
            Guard.NotNull(LblOmniture, nameof(LblOmniture));
            Guard.NotNull(RblReqOmni, nameof(RblReqOmni));
            Guard.NotNull(RblCustomOmni, nameof(RblCustomOmni));
            Guard.NotNull(DdlOmniDefault, nameof(DdlOmniDefault));
        }

        public IReadOnlyCollection<LinkTrackingParam> LtpList { get; set; }
        public int BaseChannelId { get; set; }
        public string ParamValueOmniture { get; set; }
        public ImageButton ImgBtnOmni { get; set; }
        public ITextControl LblOmniture { get; set; }
        public RadioButtonList RblReqOmni { get; set; }
        public RadioButtonList RblCustomOmni { get; set; }
        public DropDownList DdlOmniDefault { get; set; }
    }
}