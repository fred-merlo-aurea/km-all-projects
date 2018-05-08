<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="scheduledReportList.aspx.cs" Inherits="ecn.communicator.main.Reports.scheduledReportList" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    .reportDetailColumn
    {
        width: 20%;
    }

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlNoReports" runat="server" Visible="false" Height="400px">
        <div style="padding-top:150px; text-align:center;font-size:large;">
             <asp:Label ID="Label1" runat="server" Text="You do not have any reports scheduled."></asp:Label>
        <asp:HyperLink ID="hlAddReport" runat="server" NavigateUrl="/ecn.communicator/main/Reports/scheduledReportSetup.aspx" Font-Size="Large">Click Here</asp:HyperLink><asp:Label ID="Label2" runat="server" Text=" to add one."></asp:Label>        
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlReportList" runat="server" Visible="false">
        <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
    <br />
    <table width="100%">
        <tr>
            
            <td style="text-align:right;">
                <asp:Button ID="btnAdd" runat="server" Text="Add Scheduled Report" class="ECN-Button-Small" OnClick="btnAdd_Click" />
            </td>
        </tr>
        <tr>
            <%--<td style="text-align:right;padding-top:15px;">
                <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged" />

            </td>--%>
        </tr>

    </table>

    <br />
    <ecnCustom:ecnGridView ID="gvScheduledReports" runat="server" AllowSorting="false" AutoGenerateColumns="false" CssClass="grid"
        Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="ReportScheduleID"
        OnRowCommand="gvScheduledReports_RowCommand" OnRowDataBound="gvScheduledReports_RowDataBound">
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblReportScheduleID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ReportScheduleID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30%" HeaderText="Report">
                <ItemTemplate>
                    <asp:Label ID="lblReportName" runat="server" ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" HeaderText="Schedule Type">
                <ItemTemplate>
                    <asp:Label ID="lblScheduleType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ScheduleType") %>'></asp:Label>
                      <asp:Label ID="lblRecurrenceType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.RecurrenceType") %>' Visible="false"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" HeaderText="Email Subject">
                <ItemTemplate>
                    <asp:Label ID="lblEmailSubject" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.EmailSubject") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" HeaderText="Next Scheduled Date">
                <ItemTemplate>
                      <asp:Label ID="lblRunSunday" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.RunSunday") %>' Visible="false"></asp:Label>
                      <asp:Label ID="lblRunMonday" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.RunMonday") %>' Visible="false"></asp:Label>
                      <asp:Label ID="lblRunTuesday" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.RunTuesday") %>' Visible="false"></asp:Label>
                      <asp:Label ID="lblRunWednesday" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.RunWednesday") %>' Visible="false"></asp:Label>
                      <asp:Label ID="lblRunThursday" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.RunThursday") %>' Visible="false"></asp:Label>
                      <asp:Label ID="lblRunFriday" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.RunFriday") %>' Visible="false"></asp:Label>
                      <asp:Label ID="lblRunSaturday" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.RunSaturday") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lblStartTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.StartTime") %>' Visible="false"></asp:Label>
                      <asp:Label ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.StartDate") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lblEndDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.EndDate") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lblMonthLastDay" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.MonthLastDay") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lblMonthScheduleDay" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.MonthScheduleDay") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lblNextScheduledDate" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Edit">
                <ItemTemplate>
                    <asp:ImageButton ID="imgbtnEdit" runat="server" ImageUrl="/ecn.images/images/icon-edits1.gif" CommandName ="ReportScheduleEdit"
                        CausesValidation="false" OnClick="imgbtnEdit_Click" CommandArgument='<%#Eval("ReportScheduleID")%>' /> 
                    
                     <%-- <a href='scheduledReportSetup.aspx?ReportScheduleID=<%# DataBinder.Eval(Container.DataItem, "ReportScheduleID") %>'>
                        <center>
                            <img src="/ecn.images/images/icon-edits1.gif" alt='Edit Domain Information' border='0'></center>
                    </a>--%>

                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                        CommandName="ReportScheduleDelete" OnClientClick="return confirm('Are you sure you want to delete this scheduled report?')"
                        CausesValidation="false" CommandArgument='<%#Eval("ReportScheduleID")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField >
            <ItemTemplate>
                <asp:LinkButton ID="lnkbtnReportDetails" CommandName="ReportDetails" runat="server">+Details</asp:LinkButton>
                </td> </tr>
                <asp:Panel ID="pnlBlastReport" runat="Server" Visible="false">
                    <tr valign="top" style="top: 10px;">
                        <td colspan="7" align="left">
                    <ecnCustom:ecnGridView ID="gvReportDetails" DataKeyNames="ReportQueueID, ReportScheduleID" OnRowCommand= "gvReportDetails_RowCommand" OnRowDataBound="gvReportDetails_RowDataBound" AutoPostBack="false" AutoGenerateColumns="false" Width="100%" runat="server">
                        <Columns>
                            <asp:BoundField DataField="SendTime" HeaderText="Send Time" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="FinishTime" HeaderText="Completed Time" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Linkbutton ID="lnkResendReport" runat="server" CommandName="resend" Text="Resend Now" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FailureReason" HeaderText="Failure Reason" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                        </Columns>
                    </ecnCustom:ecnGridView>
                   </asp:Panel>
                           </ItemTemplate>
                </asp:TemplateField>
        </Columns>

    </ecnCustom:ecnGridView>
        <br />   <br />
                </ContentTemplate>
            </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>