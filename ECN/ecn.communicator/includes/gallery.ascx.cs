namespace ecn.communicator.includes {
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.communicator.classes;
	using ecn.common.classes;

	
	///		Summary description for gallery.
	
	public partial  class gallery : UserControlEx {
		protected System.Web.UI.WebControls.Panel PanelTabs;
		protected System.Web.UI.WebControls.LinkButton tabFolders;



		#region properties

		protected override DataList ImageRepeaterControl => imagerepeater;
		protected override System.Web.UI.WebControls.Image ImagePreviewControl => ImagePreview;
		protected override LinkButton TabPreviewControl => tabPreview;
		protected override LinkButton TabUploadControl => tabUpload;
		protected override LinkButton TabBrowseControl => tabBrowse;
		protected override Panel PanelBrowseControl => PanelBrowse;
		protected override Panel PanelPreviewControl => PanelPreview;
		protected override Panel PanelUploadControl => PanelUpload;

		public string imageDirectory {
			set {
				imagepath.Text = value;
			}
			get {
				return imagepath.Text;
			}
		}

		public string thumbnailSize {
			set {
				imagesize.Text = value;
			}
			get {
				return imagesize.Text;
			}
		}

		public string imagesPerColumn {
			set {
				imagerepeater.RepeatColumns = Convert.ToInt32(value);
			}
			get {
				return imagerepeater.RepeatColumns.ToString();
			}
		}

		
		string _currentFolder;
		public string currentFolder {
			set {
				_currentFolder = value;
			}
			get {
				try{
					_currentFolder =  Request.QueryString["folder"].ToString();
				}catch{
					_currentFolder = "";
				}

				return _currentFolder;
			}
		}
		#endregion

		protected void Page_Load(object sender, System.EventArgs e) {
			pageuploader.uploadDirectory=Server.MapPath(imageDirectory);
			loadFoldersTable();
			loadBrowseTable();
		}


		public void loadBrowseTable(){
			
			DataTable dtPictures = new DataTable();
			DataColumn dcPicturesImage = new DataColumn("Image",typeof(string));
			DataColumn dcPicturesFileName = new DataColumn("FileName",typeof(string));
			DataColumn dcPicturesPath = new DataColumn("Path",typeof(string));
			DataColumn dcPicturesSize = new DataColumn("Size",typeof(string));
			DataColumn dcPicturesDate = new DataColumn("Date",typeof(string));
			DataRow drPictures;
			dtPictures.Columns.Add(dcPicturesImage);
			dtPictures.Columns.Add(dcPicturesFileName);
			dtPictures.Columns.Add(dcPicturesPath);
			dtPictures.Columns.Add(dcPicturesSize);
			dtPictures.Columns.Add(dcPicturesDate);

			string currentImageDirectory = imageDirectory + currentFolder;

			System.IO.FileInfo file = null;
			string[] files = null;
			string filename = "";	
			files = System.IO.Directory.GetFiles(Server.MapPath(currentImageDirectory),"*.*");
				
			for (int i=0; i<=files.Length-1; i++) {
				//Create a new FileInfo object for this filename
				file = new System.IO.FileInfo(files[i]);
				filename = file.Name.ToString();
				if (filename.ToLower().EndsWith(".jpg") || filename.ToLower().EndsWith(".gif") ) {
					drPictures = dtPictures.NewRow();
					drPictures[0]=System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"]+"/includes/thumbnail.aspx?size="+thumbnailSize+"&image="+Server.UrlPathEncode(currentImageDirectory+"/"+file.Name);
					drPictures[1]=file.Name;
					drPictures[2]=Server.UrlPathEncode(currentImageDirectory+"/"+file.Name);
					drPictures[3]=(file.Length/1000)+"kb";
					drPictures[4]=file.LastWriteTime.ToShortDateString();
					dtPictures.Rows.Add(drPictures);
				}
			}
			DataView dvPictures = new DataView(dtPictures);
			imagerepeater.DataSource = dvPictures;
			imagerepeater.DataBind();
		}

		public void loadFoldersTable(){
			
			DataTable dtFolders = new DataTable();
			DataColumn dcImageUrl		= new DataColumn("ImageURL",typeof(string));
			DataColumn dcFolderName	= new DataColumn("FolderName",typeof(string));
			DataRow drFolders;

			dtFolders.Columns.Add(dcImageUrl);
			dtFolders.Columns.Add(dcFolderName);

			string currentImageDirectory = imageDirectory;

			System.IO.DirectoryInfo dir = null;
			string[] dirs			= null;
			string dirname		= "";	
			string folderImg	= "";
			string cellStyle		= "";
			string cellPadding	= "Padding-top:4px; Padding-bottom:4px; Padding-left:4px; Padding-right:4px";
			dirs = System.IO.Directory.GetDirectories(Server.MapPath(currentImageDirectory));
			dir = new System.IO.DirectoryInfo(Server.MapPath(currentImageDirectory));	

			if(currentFolder.ToString().Length > 0){
				//its not Root Folder that's selected.
				folderImg = "'/ecn.images/icons/folder_img_closed.gif'";
				cellStyle	= "class='gridaltrow'";
			}else{
				folderImg = "'/ecn.images/icons/folder_img_open.gif'";
				cellStyle	= "style='background-color: #FFFFFF;'";
			}
			
			HomeImageURL.Text		="<tr "+cellStyle+"><td valign=top align=center style='"+cellPadding+"' height=100%><a href='filemanager.aspx'><img src="+folderImg+"></a></td>";
			HomeFolderName.Text		="<td valign=top align=left  style='"+cellPadding+"'><b>Root Folder</b><br>["+dir.GetFiles().Length +"&nbsp;Images,&nbsp;"+dir.GetDirectories().Length +"&nbsp;Folders] </td></tr>";

			for (int i=0; i<=dirs.Length-1; i++) {
				//Create a new DirectoryInfo object for this Dir
				dir = new System.IO.DirectoryInfo(dirs[i]);
				dirname = dir.Name.ToString();
				drFolders = dtFolders.NewRow();
				if(currentFolder.Replace("/","").ToString().Equals(dirname)){
					//Folder that's selected.
					folderImg	= "'/ecn.images/icons/folder_img_25_open.gif'";
					cellStyle	= "style='background-color: #FFFFFF;'";
					SelectedImageURL.Text ="<tr "+cellStyle+"><td valign=bottom align=center style='"+cellPadding+"'><a href='filemanager.aspx?folder=/"+dirname+"'><img src="+folderImg+"></a></td>";
					SelectedFolderName.Text ="<td valign=bottom align=left style='"+cellPadding+"'><b>"+dir.Name+"</b><br>["+dir.GetFiles().Length +"&nbsp;Images] </td></tr>";
				}else{
					folderImg = "'/ecn.images/icons/folder_img_25_closed.gif'";
					cellStyle	= "class='gridaltrow'";
					drFolders[0]="<tr "+cellStyle+"><td width=40 valign=bottom align=center style='"+cellPadding+"'><a href='filemanager.aspx?folder=/"+dirname+"'><img src="+folderImg+"></a></td>";
					drFolders[1]="<td valign=bottom align=left style='"+cellPadding+"'><b>"+dir.Name+"</b><br>["+dir.GetFiles().Length +"&nbsp;Images] </td></tr>";
					dtFolders.Rows.Add(drFolders);
				}
			}

			if(SelectedImageURL.Text.Length == 0){
				SelectedImageURL.Visible = false;
				SelectedFolderName.Visible = false;
			}

			DataView dvFolders = new DataView(dtFolders);
			foldersrepeater.DataSource = dvFolders;
			foldersrepeater.DataBind();

			foldersrepeater.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(0);
		}

		public void loadFolderImages(object sender, CommandEventArgs e){
			loadFoldersTable();
		}

		public void previewImage(object sender, CommandEventArgs e){
			
			string previewfile=e.CommandArgument.ToString();
			ImagePreview.ImageUrl=System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"]+"/includes/thumbnail.aspx?size=300&image="+Server.UrlPathEncode(previewfile);
			System.IO.FileInfo file = null;
			file = new System.IO.FileInfo(Server.MapPath(previewfile));
			previewName.Text=file.Name.ToString();
			previewSize.Text=Convert.ToInt32(file.Length/1000).ToString()+"kb";
			previewDate.Text=file.LastWriteTime.ToShortDateString();

			System.Drawing.Image g = System.Drawing.Image.FromFile(Server.MapPath(previewfile));
			previewHeight.Text=g.Height.ToString()+" pixels ";
			previewWidth.Text=g.Width.ToString()+" pixels ";
			previewResolution.Text=g.HorizontalResolution.ToString()+" dpi";
			g.Dispose();

			deleteLink.CommandArgument=previewfile;
			fullsizeLink.NavigateUrl=previewfile;
			showPreview(sender,e);
		}

		public void deleteImage(object sender, CommandEventArgs e){
			ResetImagePreview();
			System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath(e.CommandArgument.ToString()));
			file.Delete();
			loadBrowseTable();
			showBrowse(sender,e);
		}

		public void showBrowse(object sender, System.EventArgs e){
			showPreviewPanel.Visible=false;
			SetControlsVisibility(false, false, true);
			PanelFolders.Visible		= false;
		}

		public void showPreview(object sender, System.EventArgs e){
			showPreviewPanel.Visible=true;
			SetControlsVisibility(true, false, false);
			PanelFolders.Visible		= false;
		}

		public void showUpload(object sender, System.EventArgs e){
			showPreviewPanel.Visible=false;
			SetControlsVisibility(false, true, false);
			PanelFolders.Visible		= false;
		}

		public void showFolders(object sender, System.EventArgs e){
			showPreviewPanel.Visible=false;
			tabPreview.Visible	= false;
			tabUpload.Visible		= true;
			tabBrowse.Visible	= true;

			PanelBrowse.Visible		= false;
			PanelPreview.Visible	= false;
			PanelUpload.Visible		= false;
			PanelFolders.Visible		= true;
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
