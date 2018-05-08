using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.listsmanager
{
    public partial class lists_main : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS;
            Master.SubMenu = "groups list";
            Master.Heading = "Groups > Manage Groups";
            Master.HelpContent = "<b>Editing a Group</b><div id='par1'><ul><li>Choose the folder your group is in.</li><li>Click the <em>Edit Button (pencil)</em> to the right of the group name.</li><li>Change the Name, Description, or Folder</li><li>Click <em>Update</em>.</li></ul></div>&#13;&#10;<b>Deleting a Group</b><div id='par2'><ul><li>Choose the folder your group is in.</li><li>Click the <em>Delete Button (red �X�)</em> to the right of the group name.</li><li>Click <em>OK</em> on the dialog box.</li></ul></div><b>Addding a New UDF(User Defined Field) to a Group</b><div id='par3'><ul>&#13;&#10;<li>Find the group you would like to add the UDF in the Group List.</li><li>Click the <em>Add/Edit User Defined Field</em> button to the right of the group name.</li><li>Add a name, and a description (Adding descriptions will help you remember what each field was created for in the future).</li>&#13;&#10;<li>Click <em>Add Field.</em></li><li>You will see that the UDF has been added to the UDF list.</li></ul></div><b>To Edit a Group�s UDF:</b><div id='par4'><ul><li>Find the group which contains the UDF you would like to edit in the Group List.</li>&#13;&#10;<li>Click the <em>Add/Edit User Defined Fields</em> button to the right of the group name.</li><li>Find the UDF you would like to edit, and click the <em>Edit</em> link to the right of the UDF name.</li><li>Make the required changes, and click <em>Update Field</em>.</li></ul></div>&#13;&#10;<b>Deleting a Group�s UDF</b><div id='par5'><ul><li>Find the group which contains the UDF you would like to delete in the Group List</li><li>Click the <em>Add/Edit User Defined Fields</em> button to the right of the group name.</li>&#13;&#10;<li>Find the UDF you would like to delete, and click the <em>Delete</em> link to the right of the UDF name.</li><li>This will delete the UDF and all of the data contained within that field.</li></ul></div><b>Adding/Editing smartForms</b>&#13;&#10;<p>A Double Opt-in smartForm is perfect for Newsletter subscription web forms. It will not only validate the email address when entered into the form� It will also send a verification email to the address entered, and the user must open and click the link contained within the verification to prove that it is actually their address they are using. &#13;&#10;This helps add a separate layer of protection and allows you to maintain a clean email list. All pages and emails can be customized to match your marketing or communication efforts. When an email address is entered and verified, it will then create an email profile in the group you specify.</p>&#13;&#10;<b>To Create a new Double Opt-in smartForm for a Group</b><div id='par6'><ul><li>Click on the <em>Create SmartForms for Opt-in HTML</em> (this brings up the Create New smartForms page � default is Double Opt-in form).</li>&#13;&#10;<li>The list along the left side lets you choose which fields you would like included in the form.</li><li>Simply <em>Ctrl+click</em> on each field that you would like included.</li><li>Once they are all selected, click the <em>Rebuild smartForm</em> button.</li>&#13;&#10;<li>This will automatically insert the requested fields into the smartForm.</li><li>Now you are ready to add this form to your web site:</li><li>Click on the checkbox next to <em>Source</em> found in the upper right hand corner of the formbuilder editor (this displays the HTML source code used to generate the smartForm).</li>&#13;&#10;<li>Click inside the editor text box, and click <em>Edit->Select All</em> from the Windows Menu Bar.</li><li>Click <em>Edit->Copy</em> to copy the source code to the clipboard.</li><li>Inside your favorite HTML editor, open up the page that you would like to add the smartForm and paste in the code.</li>&#13;&#10;<li>When you publish this form to your web site, any data that is collected with this form will automatically be entered into your ECN profile.</li></ul></div><b>To create a Single Opt-in form</b><div id='par7'><ul><li>Click on <em>Single Opt-in smartForm</em>.</li>&#13;&#10;<li>Simply <em>Ctrl+click</em> on each field that you would like included.</li><li>Once they are all selected, click the <em>Create as a new smartForm</em> button (this will insert the requested fields into the List of Single Opt-in smartForms)</li>&#13;&#10;<li>To view smartForm, click on the <em>pencil</em>. (Optional notification to the company and a Thank You response to the customer are suggested)</li><li>To utilize company notifcation, complete the Web Admin Notification section including Admin�s Email, Email Subject and Email Body.   For User Response Notification, complete the Email From, Email Subject and Email Body.</li> &#13;&#10;<li>When complete, click <em>Save smartForm</em>.</li><li>Inside your favorite HTML editor, open up the page that you would like to add the smartForm and paste in the code.</li><li>When you publish the web site, any data that is collected with this form will automatically be entered into your ECN profile.</li></ul></div>&#13;&#10;&#13;&#10; &#13;&#10;&#13;&#10;&#13;&#10;&#13;&#10;<b>Adding/Editing Filters</b><div id='par8'><ul><li>Find the Group you want to create a filter for.</li><li>Click on the <em>Funnel icon</em> for that group.</li><li>Enter a title for your filter (for example, pet owners), Click <em>Create new filter</em>.</li><li>Under filter names, click on the <em>pencil (Add/Edit Filter Attributes)</em> icon to define the filter attributes.</li><li>In the Compare Field section, use the drop down menu and click on profile field to define attributes of your filter.</li>&#13;&#10;<li>In the Comparator section you have the option of making the field equal to (=), contains, ends with, or starts with.</li><li>In the Compare Value field, enter the information you would want the system to filter (for example, dog).</li><li>The Join Filters allow you to select And, or, Or.</li><li>To add, click <em>Add this Filter</em>.</li><li>Repeat this process several times to fully develop the attributes you are looking for (for example, dog, dogs, cat, cats, dog owners, etc.)</li> &#13;&#10;<li>After all fields and attributes have been selected and added, Click <em>Preview filtered e-mails</em> button to view emails in your filtered list.</li><li>When filter is complete, Click on <em>Return to Filters List</em>.</li></ul></div><b>Viewing/Editing Email Profiles (including adding notes to profile)</b><div id='par9'><ul><li>Click on the <em>pencil</em> for the group you want to view.</li><li>Click the <em>pencil</em> again for the specific email address you want to view.</li>&#13;&#10;<li>Edit information within the fields as needed; click <em>Update</em> when finished.</li><li>To enter or view notes on this subscriber, click the <em>View Notes</em> button.</li>&#13;&#10;<li>To see the history of what messages have been sent to this subscriber, click the <em>View Logs</em> button. To see what message was sent, click on the blast title and a preview will appear.</li>&#13;&#10;<li>To view the number of opens or pages the person has clicked on, click the <em>Profile Manager</em>, then click on <em>Email Opens Activity</em> or <em>Email Clicks Activity</em>.</li></ul></div>&#13;&#10;<b>Exporting Your Customer List</b><div id='par10'><ul><li>Click the <em>pencil</em> on the group list you want to export.</li><li>On the right side of the screen above the list of email addresses, you will find Export this view to� use the dropdown list to select the type of file you want to use (XML, Excel or CSV), then click <em>Export</em>.</li><li>Save the file to your computer.   (Default file name is emails.xxx)</li></ul></div>&#13;&#10;";
            Master.HelpTitle = "Groups Manager";
            groupExplorer1.enableEditMode();
            if(!Page.IsPostBack)
                groupExplorer1.reset();
            //if (!(KM.Platform.User.IsAdministratorOrHasUserPermission(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Groups)))
            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.View))	
            {
                Response.Redirect("/ecn.accounts/main/default.aspx");
            }

        }

    }
}