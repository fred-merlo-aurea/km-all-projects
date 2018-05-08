using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.main.Reports.ReportSettingsControls
{
    public interface IReportSettingsControl
    {
        void SetParameters(int ReportScheduleID);

        string GetParameters();

        bool IsValid();
    }
}

