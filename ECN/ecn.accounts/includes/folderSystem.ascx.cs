using KM.Framework.Web.WebForms.FolderSystem;

namespace ecn.accounts.includes
{
    public partial class folderSystem : FolderSystemBase
    {
        private static int _customerID = 0;
        
        public override int CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }
    }
}