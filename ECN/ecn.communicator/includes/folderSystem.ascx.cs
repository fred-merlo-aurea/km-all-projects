using System.Collections.Generic;
using ECN_Framework_Entities.Communicator;
using KM.Framework.Web.WebForms.FolderSystem;

namespace ecn.communicator.includes
{
    public partial class folderSystem : FolderSystemBase
    {
        protected override List<Folder> GetFolders()
        {
            CustomerID = CurrentUser.CustomerID;

            return base.GetFolders();
        }
    }
}
