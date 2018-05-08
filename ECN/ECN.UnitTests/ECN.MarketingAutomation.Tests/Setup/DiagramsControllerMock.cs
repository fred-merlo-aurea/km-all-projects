using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.MarketingAutomation.Controllers;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Tests.Setup.Interfaces;
using Moq;

namespace ecn.MarketingAutomation.Tests.Setup
{
    [ExcludeFromCodeCoverage]
    public class DiagramsControllerMock : Mock<IDiagramsController>
    {
        public DiagramsControllerMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ShimAutomationBaseController.AllInstances.SaveCampaignItemCampaignItemBoolean = SaveCampaignItemCampaignIte;
            ShimAutomationBaseController.AllInstances.UpdateGlobalControlListWithEcnidControlBase
                = UpdateGlobalControlListWithECNID;
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = FindParentWait;
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = FindParentCampaignItem;
            ShimAutomationBaseController.AllInstances.SaveSmartSegmentEmail_ClickControlBaseControlBaseBooleanBoolean =
                SaveSmartSegmentEmailClick;
            ShimAutomationBaseController.AllInstances.SaveMessageTriggerControlBaseControlBaseControlBase =
                SaveMessageTrigger;
            ShimAutomationBaseController.AllInstances.FindParentControlBase = FindParent;
            ShimAutomationBaseController.AllInstances.FindFormParentControlBase = FindFormParent;
            ShimAutomationBaseController.AllInstances.SaveFormControlTriggerControlBaseControlBaseControlBaseControlBase =
                SaveFormControlTrigger;
            ShimAutomationBaseController.AllInstances.FindParentDirectEmailWait = FindParentDirectEmail;
            ShimAutomationBaseController.AllInstances.SaveTriggerPlanControlBaseControlBaseControlBaseBoolean = SaveTriggerPlan;
            ShimAutomationBaseController.AllInstances.SaveGroupTriggerControlBaseControlBase = SaveGroupTrigger;
        }

        private int SaveGroupTrigger(AutomationBaseController controller, ControlBase trigger, ControlBase parentWait)
        {
            return Object.SaveGroupTrigger(trigger, parentWait);
        }

        private int SaveTriggerPlan(
            AutomationBaseController controller,
            ControlBase trigger,
            ControlBase parentWait,
            ControlBase parentDirectObject,
            bool isHome = false)
        {
            return Object.SaveTriggerPlan(trigger, parentWait, parentDirectObject);
        }

        private ControlBase FindParentDirectEmail(AutomationBaseController controller, Wait wait)
        {
            return Object.FindParentDirectEmail(wait);
        }

        private int SaveFormControlTrigger(
            AutomationBaseController controller,
            ControlBase trigger,
            ControlBase parentWait,
            ControlBase parentCampaignItem,
            ControlBase parentForm)
        {
            return Object.SaveFormControlTrigger(trigger, parentWait, parentCampaignItem, parentForm);
        }

        private ControlBase FindFormParent(AutomationBaseController controller, ControlBase control)
        {
            return Object.FindFormParent(control);
        }

        private ControlBase FindParent(AutomationBaseController controller, ControlBase control)
        {
            return Object.FindParent(control);
        }

        private int SaveMessageTrigger(
            AutomationBaseController controller,
            ControlBase trigger,
            ControlBase parentWait,
            ControlBase waitParent)
        {
            return Object.SaveMessageTrigger(trigger, parentWait, waitParent);
        }

        private int SaveSmartSegmentEmailClick(
            AutomationBaseController controller,
            ControlBase smartSegment,
            ControlBase parent,
            bool keepPaused,
            bool isHome)
        {
            return Object.SaveSmartSegmentEmailClick(smartSegment, parent, keepPaused);
        }

        private CampaignItem FindParentCampaignItem(AutomationBaseController controller, ControlBase control)
        {
            return Object.FindParentCampaignItem(control);
        }

        private Wait FindParentWait(AutomationBaseController controller, ControlBase control)
        {
            return Object.FindParentWait(control);
        }

        private void UpdateGlobalControlListWithECNID(AutomationBaseController controller, ControlBase control)
        {
            Object.UpdateGlobalControlListWithECNID(control);
        }

        private int SaveCampaignItemCampaignIte(AutomationBaseController controller, CampaignItem item, bool keepPaused)
        {
            return Object.SaveCampaignItem(item, keepPaused);
        }
    }
}
