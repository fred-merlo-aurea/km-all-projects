using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.MarketingAutomation.Models
{
    public class Enums
    {
        public enum ControlType
        {
            Start,
            Group,
            CampaignItem,
            NoClick,
            NoOpen,
            Open_NoClick,
            Sent,
            NotSent,
            Suppressed,
            Click,
            Open,
            Direct_Click,
            Direct_Open,
            Subscribe,
            Unsubscribe,
            Wait,
            End,
            Form,
            FormSubmit,
            FormAbandon
        }
      
    }
}