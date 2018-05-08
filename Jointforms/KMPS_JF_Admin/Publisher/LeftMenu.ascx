<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenu.ascx.cs" Inherits="KMPS_JF_Setup.Publisher.LeftMenu" %>
<JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%" Title="Publication Options">
    <div class="leftNav">
      
            <a <%=CurrentMenu.ToString().ToUpper()=="UDF"?"class='labelselected'":""  %> href="Pub_UDF.aspx?PubId=<%= Request.QueryString["PubId"] %>&PubName=<%= pubCode %>">
                UDFs</a><br />
            <a  <%=CurrentMenu.ToString().ToUpper()=="CATEGORY"?"class='labelselected'":""  %> href="Pub_Categories.aspx?PubId=<%= Request.QueryString["PubId"] %>&PubName=<%= pubCode %>">
                Categories</a><br />
            <a <%=CurrentMenu.ToString().ToUpper()=="NEWSLETTERS"?"class='labelselected'":""  %> href="Pub_Newsletters.aspx?PubId=<%= Request.QueryString["PubId"] %>&PubName=<%= pubCode %>">
                Newsletters</a><br />
            <a <%=CurrentMenu.ToString().ToUpper()=="PUBS"?"class='labelselected'":""  %> href="Pub_RelatedPubs.aspx?PubId=<%= Request.QueryString["PubId"] %>&PubName=<%= pubCode %>">
                Related Pubs</a><br />
            <a <%=CurrentMenu.ToString().ToUpper()=="PAGES"?"class='labelselected'":""  %> href="Pub_CustomPage.aspx?PubId=<%= Request.QueryString["PubId"] %>&PubName=<%= pubCode %>">
                Custom Pages</a><br />
            <a <%=CurrentMenu.ToString().ToUpper()=="EVENTS"?"class='labelselected'":""  %> href="Pub_Events.aspx?PubId=<%= Request.QueryString["PubId"] %>&PubName=<%= pubCode %>">
                Events</a><br />
            <a <%=CurrentMenu.ToString().ToUpper()=="FORMS"?"class='labelselected'":""  %> href="Pub_Forms.aspx?PubId=<%= Request.QueryString["PubId"] %>&PubName=<%= pubCode %>">
                Subscription Forms</a><br />
            <a href="PublisherList.aspx">Home</a><br />
      
    </div>
</JF:BoxPanel>
