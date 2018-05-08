using System;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public interface IEstimatedSendTime
    {
        DateTime? EstSendTime { get; set; }
    }
}