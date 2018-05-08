using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

namespace ecn.communicator.main.blasts.Interfaces
{
    public interface IReportViewer
    {
        void DataBind();
        IReadOnlyCollection<byte> Render(string format, string deviceInfo, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings);
        void SetParameters(ReportParameter[] parameters);
    }
}
