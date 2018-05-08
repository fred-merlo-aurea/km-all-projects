namespace ecn.communicator.main.content
{
    public partial class content_main : ECN_Framework.WebPageHelper
    //public partial class content_main : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT;
            Master.SubMenu = "content list";
            Master.Heading = "Content/Messages > Manage Content";
            //Master.Heading = "Manage Content & Messages";
            Master.HelpContent = "<b>Editing Content:</b><br/><div id='par1'><ul><li>If the Content you created contains a link, you may want to create a name for the link, so that it is more easily referenced in the reporting section. You can do this by clicking on the link/alias icon. For example if your link was www.knowledgemarketing.com, you might name the link “Homepage.”</li><li>To preview your Content in HTML, click on the <em>HTML</em> icon and your Content will appear in a browser page.</li><li>To preview your Content in Text format, click on the <em>Text</em> Icon.</li><li>To edit your content, click on the <em>pencil</em> and you will have full access to make any changes.</li></ul><b>Deleting Content:</b><br/><ul><li>To delete your Content, click on the <em>red X</em>.<br/><em class='note'>NOTE:  While Editor defaults to an HTML format to create content, you can also create content using straight Source code.  To enter source code, click the <em>Source checkbox</em> in the upper right hand corner of the editor.  This will refresh the editor screen and you will be able to enter your source code directly or copy and paste existing code into the editor.  When finished, remember to unclick <em>Source checkbox</em> to view your content and save your code.</li></ul></div>";
            Master.HelpTitle = "Content Manager";
            contentExplorer1.ShowArchiveFilter = true;
            contentExplorer1.enableEditMode();
            if (!ECN_Framework_BusinessLayer.Communicator.Content.HasPermission(KMPlatform.Enums.Access.View, Master.UserSession.CurrentUser))
            {
                throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = ECN_Framework_Common.Objects.Enums.SecurityExceptionType.RoleAccess };
            }
        }
    }
}