using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;

namespace KMPS.MD.Controls
{
    public partial class FolderSystem : System.Web.UI.UserControl
    {
        private const string ROOT_ID = "0";

        //private int customerID = 0;
        private string folderType = "";
        private string folderName = "";
        private string folderDesc = "";
        protected int width = 0;
        protected int height = 0;
        protected string css = "";
        protected bool nodesExpanded = false;
        protected bool childrenExpanded = false;
        private int heightPercentage = 0;

        #region Properties and public fields

        public event EventHandler FolderEvent;
        //public int CustomerID
        //{
        //    get { return customerID; }
        //    set { customerID = value; }
        //}
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public string CssClass
        {
            get { return css; }
            set { css = value; }
        }
        public string FolderName
        {
            get { return folderName; }
            set { folderName = value; }
        }
        public string FolderDescription
        {
            get { return folderDesc; }
            set { folderDesc = value; }
        }
        public string FolderType
        {
            get { return folderType; }
            set { folderType = value; }
        }
        public string SelectedFolderID
        {
            get
            {
                try
                {
                    return trvFolders.SelectedNode.Value;
                }
                catch
                {
                    return "0";
                }
            }

        }
        public string SelectedFolderName
        {
            get
            {
                return trvFolders.SelectedNode.Text;
            }
        }
        public bool Enabled
        {
            set { this.trvFolders.Enabled = value; }
        }
        public bool NodesExpanded
        {
            get { return nodesExpanded; }
            set { nodesExpanded = value; }
        }
        public bool ChildrenExpanded
        {
            get { return childrenExpanded; }
            set { childrenExpanded = value; }
        }

        public int HeightPercentage
        {
            get { return heightPercentage; }
            set { heightPercentage = value; }
        }
        #endregion

        List<ECN_Framework_Entities.Communicator.Folder> folderList;
        List<KMPlatform.Entity.Client> clientList;

        private ECNSession _usersession = null;

        public ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECNSession.CurrentSession() : _usersession;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            LoadFolderTree();
        }

        #region Data Access Methods

        private List<KMPlatform.Entity.Client> GetClient()
        {
            List<KMPlatform.Entity.Client> CurrentUserClientGroupClients = UserSession.CurrentUserClientGroupClients;

            CurrentUserClientGroupClients = (from c in CurrentUserClientGroupClients
                                             where c.IsActive == true
                                             select c).ToList();

            return CurrentUserClientGroupClients;
        }

        private List<ECN_Framework_Entities.Communicator.Folder> GetFolders(int customerID)
        {
            List<ECN_Framework_Entities.Communicator.Folder> myFolderList = ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(customerID, FolderType, UserSession.CurrentUser);
            return myFolderList;
        }

        #endregion

        #region Methods
        public void LoadFolderTree()
        {
            trvFolders.Nodes.Clear();
            trvFolders.Width = System.Web.UI.WebControls.Unit.Pixel(Width);
            trvFolders.CssClass = CssClass;

            clientList = GetClient();
            foreach (KMPlatform.Entity.Client client in clientList)
            {
                TreeNode root = CreateTreeNode(client.ClientID.ToString(), client.ClientName);
                trvFolders.Nodes.Add(root);
            }
        }

        private void loadParent()
        {
            foreach (KMPlatform.Entity.Client client in clientList)
            {
                TreeNode root = CreateTreeNode(client.ClientID.ToString(), client.ClientName);
            }
        }

        private void LoadChildren(TreeNode parentNode, string parentID, int customerID)
        {
            List<ECN_Framework_Entities.Communicator.Folder> childFolderList = (from src in folderList
                                                                                where src.ParentID == Convert.ToInt32(parentID)
                                                                                select src).ToList();
            parentNode.ChildNodes.Clear();

            foreach (ECN_Framework_Entities.Communicator.Folder folder in childFolderList)
            {
                TreeNode childnode = CreateTreeNode(folder.FolderID.ToString() + "|" + customerID, folder.FolderName.ToString());
                LoadChildren(childnode, folder.FolderID.ToString(), customerID);
                parentNode.ChildNodes.Add(childnode);
                
            }
        }

        private TreeNode CreateTreeNode(string nodeValue, string nodeText)
        {
            TreeNode node = new TreeNode(nodeText, nodeValue);
            return node;
        }

        #endregion

        protected void trvFolders_SelectedNodeChanged(object sender, EventArgs e)
        {
           
            if (FolderEvent != null)
            {
                if (trvFolders.SelectedNode.Parent == null)
                {
                    int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(Convert.ToInt32(trvFolders.SelectedNode.Value), false).CustomerID;

                    folderList = GetFolders(CustomerID);
                    LoadChildren(trvFolders.SelectedNode, "0", CustomerID);
                    trvFolders.SelectedNode.Expanded = true;
                    FolderEvent(sender, e);
                }
                else
                {
                    FolderEvent(sender, e);
                }
            }
        }
    }
}