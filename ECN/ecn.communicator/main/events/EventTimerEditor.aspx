<%@ Page Language="c#" Inherits="ecn.communicator.main.events.EventTimerEditor" CodeBehind="EventTimerEditor.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveDateTime" %>
<%@ Register TagPrefix="uc1" TagName="EventTimerEditor" Src="../../includes/EventTimerEditor.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
        <table>
            <tr>
                <td>
                    <uc1:EventTimerEditor ID="EventTimerEditor1" runat="Server"></uc1:EventTimerEditor>
                </td>
            </tr>
        </table>
</asp:content>
