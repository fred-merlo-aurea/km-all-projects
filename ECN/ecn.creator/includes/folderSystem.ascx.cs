using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using KM.Framework.Web.WebForms.FolderSystem;

namespace ecn.creator.includes
{
    public partial class folderSystem : FolderSystemBase
    {
        public static string communicatordb = ConfigurationManager.AppSettings["communicatordb"];

        private static int _customerId = 0;
        private static string _folderType = string.Empty;
        protected string _connectionString = string.Empty;

        public override int CustomerID
        {
            get { return _customerId; }
            set { _customerId = value; }
        }

        public override string FolderType
        {
            get { return _folderType; }
            set { _folderType = value; }
        }

        public string ConnString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        new public int SelectedFolderID {
			get {
				try
                {
                    return Convert.ToInt32(trvFolders.SelectedNode.Value);
                }
                catch {
                    return 0;
                }
			}
		}
		
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFolderTree();
            }
        }
	
		#region Data Access Methods
		private TreeNodeCollection GetChildrenByParentID(int parentNodeID) {
			TreeNodeCollection children = new TreeNodeCollection();
			var conn = new SqlConnection(_connectionString);			
			conn.Open();
		
			string sql = string.Format("SELECT FolderID, FolderName = "+
				" 	CASE "+
				"		WHEN IsSystem = 1 THEN '<Font color=#FF0000>'+FolderName+'</font>' "+
				" 		ELSE FolderName "+
				"	END "+
				"FROM " + communicatordb + ".dbo.Folder WHERE CustomerID = {0} AND ParentID={1} AND FolderType = '"+FolderType+"' ;", CustomerID, parentNodeID);
			SqlCommand cmd = new SqlCommand(sql, conn);			
			SqlDataReader reader = cmd.ExecuteReader();
			while(reader.Read()) {
                TreeNode child = CreateTreeNode(reader["FolderID"].ToString(), reader["FolderName"].ToString());
				children.Add(child);
			}	
			conn.Close();
			return children;
		}

		#endregion

		public override void LoadFolderTree() {
			TreeNode root = CreateTreeNode("0", "Root");				
			trvFolders.Nodes.Clear();
            trvFolders.Nodes.Add(root);	
			trvFolders.Width = System.Web.UI.WebControls.Unit.Pixel(Width);
			trvFolders.CssClass = CssClass;
			LoadChildren(root);
		}

        protected override  TreeNode CreateTreeNode(string nodeValue, string nodeText)
        {
            var node = base.CreateTreeNode(nodeValue, nodeText);
            node.Expanded = NodesExpanded;
            return node;
        }

		protected override void LoadChildren(TreeNode parent)
        {
			TreeNodeCollection children = GetChildrenByParentID(Convert.ToInt32(parent.Value));
			parent.ChildNodes.Clear();
			foreach(TreeNode child in children) {
				child.Expanded = ChildrenExpanded;
				parent.ChildNodes.Add(child);
				LoadChildren(child);
			}
		}
        
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
	
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent() {
		}
		#endregion
	}
}
