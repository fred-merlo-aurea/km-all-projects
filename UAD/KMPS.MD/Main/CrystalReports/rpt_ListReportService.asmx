<%@ webservice language="C#" class="rpt_ListReportService" %>

using System;
using System.Web.Services;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Web.Services;

[ WebService( Namespace="http://crystaldecisions.com/reportwebservice/9.1/" ) ]
public class rpt_ListReportService : ReportServiceBase
{
    public rpt_ListReportService()
    {
        //
        // TODO: Add any constructor code required
        //
        this.ReportSource = this.Server.MapPath( "rpt_ListReport.rpt" );
    }
}


