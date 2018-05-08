using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Application;
using KMPlatform.Entity;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using Business = ECN_Framework_BusinessLayer.Communicator;
using Entities = ECN_Framework_Entities.Communicator;

namespace KM.Framework.Web.WebForms.FolderSystem
{
    public partial class FolderSystemBase : UserControl
    {
        private const string RootNodeId = "0";
        private const string RootNodeText = "Root";
        private const string ImagesVirtualPath = "Images_VirtualPath";

        protected List<Entities.Folder> _folderList;

        public event EventHandler FolderEvent;

        protected virtual User CurrentUser
        {
            get
            {
                return ECNSession.CurrentSession().CurrentUser;
            }
        }

        protected virtual string DirectoriesPath
        {
            get
            {
                var imagesVirtualPath = ConfigurationManager.AppSettings[ImagesVirtualPath];
                var virtualPath = string.Format("{0}/customers/{1}/images", imagesVirtualPath, CustomerID);
                var physicalPath = Server.MapPath(virtualPath);

                return physicalPath;
            }
        }

        public virtual int CustomerID { get; set; }

        public virtual int Width { get; set; }

        public virtual int Height { get; set; }

        public virtual string CssClass { get; set; }

        public virtual string FolderType { get; set; }

        public virtual string SelectedFolderID
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

        public virtual bool NodesExpanded { get; set; }

        public virtual bool ChildrenExpanded { get; set; }

        protected virtual List<Entities.Folder> GetFolders()
        {
            var myFolderList = new List<Entities.Folder>(); ;

            if (FolderType.Equals(FolderTypes.IMG.ToString()))
            {
                string directoryName;
                DirectoryInfo directory;
                Entities.Folder folder;
                
                var directories = Directory.GetDirectories(DirectoriesPath);

                for (int i = 0; i <= directories.Length - 1; i++)
                {
                    directory = new DirectoryInfo(directories[i]);
                    directoryName = directory.Name;

                    folder = new Entities.Folder()
                    {
                        FolderID = i,
                        FolderName = directoryName,
                        ParentID = 0
                    };

                    myFolderList.Add(folder);
                }
            }
            else
            {
                myFolderList = Business.Folder.GetByType(CustomerID, FolderType, CurrentUser);
            }
            return myFolderList;
        }

        public virtual void LoadFolderTree()
        {
            var root = CreateTreeNode(RootNodeId, RootNodeText);
            trvFolders.Nodes.Clear();
            trvFolders.Nodes.Add(root);
            trvFolders.Width = Unit.Pixel(Width);
            trvFolders.CssClass = CssClass;
            _folderList = GetFolders();
            LoadChildren(root);
        }

        protected virtual void LoadChildren(TreeNode parentNode)
        {
            int parentId;
            int.TryParse(parentNode.Value, out parentId);

            var childFolderList = (from src in _folderList
                                   where src.ParentID == parentId
                                   select src).ToList();

            foreach (var folder in childFolderList)
            {
                var childNode = CreateTreeNode(folder.FolderID.ToString(), folder.FolderName);
                LoadChildren(childNode);
                parentNode.ChildNodes.Add(childNode);
            }
        }

        protected virtual TreeNode CreateTreeNode(string nodeValue, string nodeText)
        {
            var node = new TreeNode(nodeText, nodeValue);
            return node;
        }

        protected virtual void trvFolders_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (FolderEvent != null)
            {
                FolderEvent(sender, e);
            }
        }
    }
}
