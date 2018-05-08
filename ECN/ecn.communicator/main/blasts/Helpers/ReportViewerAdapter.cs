using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using ecn.communicator.main.blasts.Interfaces;

namespace ecn.communicator.main.blasts.Helpers
{
    public class ReportViewerAdapter : IReportViewer
    {
        private ReportViewer _reportViewer;
        public ReportViewerAdapter(ReportViewer reportViewer)
        {
            _reportViewer = reportViewer;
        }
        public void DataBind()
        {
            _reportViewer.DataBind();
        }

        public IReadOnlyCollection<byte> Render(string format, string deviceInfo, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings)
        {
            return _reportViewer.LocalReport.Render(format, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        }

        public void SetParameters(ReportParameter[] parameters)
        {
            _reportViewer.LocalReport.SetParameters(parameters);
        }
    }
}
