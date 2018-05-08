using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using ECN_Framework.Common.Interfaces;
using ECN_Framework_Common.Objects;

namespace ECN_Framework.Common
{
    public class ReportPageBase : WebPageHelper
    {
        private const string ErrorMessageFormat = "{0}<br/>{1}: {2}";

        protected IReportDefinitionProvider _reportDefinitionProvider;
        protected IReportContentGenerator _reportContentGenerator;

        public ReportPageBase()
        {
        }

        public ReportPageBase(IReportContentGenerator reportContentGenerator)
        {
            _reportContentGenerator = reportContentGenerator;
        }

        public ReportPageBase(
            IReportDefinitionProvider reportDefinitionProvider,
            IReportContentGenerator reportContentGenerator)
        {
            _reportDefinitionProvider = reportDefinitionProvider;
            _reportContentGenerator = reportContentGenerator;
        }

        protected void CreateReportResponse(
            ReportViewer viewer,
            Stream definitionStream,
            ReportDataSource dataSource,
            ReportParameter[] parameters,
            string outputType,
            string outputFileName)
        {
            string responseContentType;

            var reportContentGenerator = GetReportContentGenerator(viewer);
            var bytes = reportContentGenerator.CreateReportContent(
                definitionStream,
                dataSource,
                parameters,
                outputType,
                out responseContentType);

            UpdateResposeWithReportData(outputFileName, responseContentType, bytes);
        }

        protected void CreateReportResponse(
            ReportViewer viewer,
            string path,
            ReportDataSource dataSource,
            ReportParameter[] parameters,
            string outputType,
            string outputFileName)
        {
            string responseContentType;

            var reportContentGenerator = GetReportContentGenerator(viewer);
            var bytes = reportContentGenerator.CreateReportContent(
                path,
                dataSource,
                parameters,
                outputType,
                out responseContentType);

            UpdateResposeWithReportData(outputFileName, responseContentType, bytes);
        }

        protected virtual IReportContentGenerator GetReportContentGenerator(ReportViewer viewer)
        {
            if (_reportContentGenerator != null)
            {
                return _reportContentGenerator;
            }

            return new ReportContentGenerator(viewer);
        }

        protected virtual IReportDefinitionProvider GetReportDefinitionProvider()
        {
            if (_reportDefinitionProvider != null)
            {
                return _reportDefinitionProvider;
            }

            return new AssemblyReportDefinitionProvider();
        }

        private void UpdateResposeWithReportData(string outputFileName, string responseContentType, byte[] bytes)
        {
            Response.ContentType = responseContentType;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + outputFileName);
            Response.BinaryWrite(bytes);
            Response.End();
        }

        protected void SetEcnError(
            ECNException ecnException,
            PlaceHolder errorPlaceHolder,
            Label errorMessageLabel)
        {
            errorPlaceHolder.Visible = true;
            errorMessageLabel.Text = string.Empty;

            foreach (var ecnError in ecnException.ErrorList)
            {
                errorMessageLabel.Text = string.Format(
                    ErrorMessageFormat,
                    errorMessageLabel.Text,
                    ecnError.Entity,
                    ecnError.ErrorMessage);
            }
        }

        protected void ThrowEcnException(
            ECNError ecnError,
            PlaceHolder errorPlaceHolder,
            Label errorMessageLabel)
        {
            var errorList = new List<ECNError>
            {
                ecnError
            };

            var exception = new ECNException(errorList, Enums.ExceptionLayer.WebSite);
            SetEcnError(exception, errorPlaceHolder, errorMessageLabel);
        }
    }
}








