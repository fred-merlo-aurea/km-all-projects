using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Linq;
using KM.Framework.Web.WebForms.FolderSystem;

namespace ecn.editor.ckeditor.controls
{
    public partial class folderSystem : FolderSystemBase
    {
        protected override  List<ECN_Framework_Entities.Communicator.Folder> GetFolders()
        {
            CustomerID = CurrentUser.CustomerID;

            return base.GetFolders();
        }
    }
}
