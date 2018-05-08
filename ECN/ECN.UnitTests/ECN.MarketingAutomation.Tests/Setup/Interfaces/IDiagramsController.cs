using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
namespace ecn.MarketingAutomation.Tests.Setup.Interfaces
{
    public interface IDiagramsController
    {
        int SaveCampaignItem(CampaignItem ciObject, bool keepPaused);
        void UpdateGlobalControlListWithECNID(ControlBase control);
        Wait FindParentWait(ControlBase control);
        CampaignItem FindParentCampaignItem(ControlBase control);
        int SaveSmartSegmentEmailClick(ControlBase smartSegment, ControlBase parent, bool keepPaused);
        int SaveMessageTrigger(ControlBase trigger, ControlBase parentWait, ControlBase waitParent);
        ControlBase FindParent(ControlBase control);
        ControlBase FindFormParent(ControlBase control);
        int SaveFormControlTrigger(ControlBase trigger, ControlBase parentWait, ControlBase parentCampaignItem, ControlBase parentForm);
        ControlBase FindParentDirectEmail(Wait wait);
        int SaveTriggerPlan(ControlBase trigger, ControlBase parentWait, ControlBase parentDirectObject);
        int SaveGroupTrigger(ControlBase trigger, ControlBase parentWait);
    }
}
