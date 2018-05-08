using System.Configuration;
using System.Data;
using System.Data.Fakes;
using System.IO.Fakes;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Folders
{
	public partial class FoldersEditorTest
	{
		private const string MouseAltSt = "<div style=\\'background-color:#FFFFFF;BORDER-TOP: #B6BCC6 1px solid;BORDER-LEFT: #B6BCC6 1px solid;BORDER-RIGHT: #B6BCC6 1px solid;	BORDER-BOTTOM: #B6BCC6 1px solid;position:absolute;\\'><table border=0 width=300 height=100 cellpadding=3><tr><TD valign=top style=\\'font-family:Arial Verdana; font-size:10px\\'>";
		private const string MouseAltEnd = "</TD></TR></TABLE></div>";
		private const string NoFolderDescAvailable = "No Folder description available.";
		private string[] _invalidImgFolderNames = new string[]
		{
			"&nbsp;",
			 "<sub><img src='/ecn.images/images/L.gif'></sub>",
			 "<sub><img src='/ecn.images/images/Lg_folder_Yel.gif'></sub>",
			 "<sub><img src='/ecn.images/images/Sm_folder_Yel.gif'></sub>"
		};
		private string[] _invalidFolderNames = new string[]
		{
			"&nbsp;",
			 "<sub><img src='/ecn.images/images/L.gif'></sub>",
			 "<sub><img src='/ecn.images/images/Lg_folder_Yel.gif'></sub>",
			 "<sub><img src='/ecn.images/images/Sm_folder_Yel.gif'></sub>",
			 "<sub><img src='/ecn.communicator/images/ecn-icon-folder.png'></sub>"
		};

		[Test]
		public void FoldersList_ItemDataBound_EditItemImgValue()
		{
			// Arrange
			var validFolderName = "validFolderName";
			var inValidFolderName = string.Join("", _invalidImgFolderNames) + validFolderName;
			var editFolderNameTextBox = new TextBox();
			InitTest_EditItem("IMG", inValidFolderName, null, editFolderNameTextBox, null);
			var dataListItemArg = new DataListItemEventArgs(new DataListItem(0, ListItemType.EditItem));

			// Act
			_foldersEditorPrivateObject.Invoke("FoldersList_ItemDataBound", new object[] { null, dataListItemArg });

			// Assert
			editFolderNameTextBox.Text.ShouldBe(validFolderName);
		}

		[Test]
		public void FoldersList_ItemDataBound_EditItem()
		{
			// Arrange
			var validFolderName = "validFolderName";
			var folderDesc = "folderDesc";
			var inValidFolderName = string.Join("", _invalidFolderNames) + validFolderName;
			var editFolderNameTextBox = new TextBox();
			var editFolderDescTextBox = new TextBox();
			InitTest_EditItem(string.Empty, inValidFolderName, folderDesc, editFolderNameTextBox, editFolderDescTextBox);
			var dataListItemArg = new DataListItemEventArgs(new DataListItem(0, ListItemType.EditItem));

			// Act
			_foldersEditorPrivateObject.Invoke("FoldersList_ItemDataBound", new object[] { null, dataListItemArg });

			// Assert
			editFolderNameTextBox.Text.ShouldBe(validFolderName);
			editFolderDescTextBox.Text.ShouldBe(folderDesc);
		}

		[TestCase(ListItemType.Item, "5")]
		[TestCase(ListItemType.Item, "notNumber")]
		[TestCase(ListItemType.AlternatingItem, "5")]
		[TestCase(ListItemType.AlternatingItem, "notNumber")]
		public void FoldersList_ItemDataBound_GRPFolderType(ListItemType itemType, string itemCount)
		{
			// Arrange
			var dataListItemArg = new DataListItemEventArgs(new DataListItem(0, itemType));
			var descBtn = new Label();
			var deleteBtn = new LinkButton();
			InitTest_AlternatingItem(itemCount, "GRP", "FolderDesc", descBtn, deleteBtn);

			// Act
			_foldersEditorPrivateObject.Invoke("FoldersList_ItemDataBound", new object[] { null, dataListItemArg });

			// Assert
			if (int.TryParse(itemCount, out int count))
			{
				deleteBtn.ShouldSatisfyAllConditions(
					() => deleteBtn.Enabled.ShouldBeFalse(),
					() => deleteBtn.Attributes["style"].ShouldBe("cursor:hand;padding:0;margin:0;"),
					() => deleteBtn.Attributes["onclick"].ShouldBe("alert('Groups exist in this Folder. Delete is not allowed !!');"));
			}
			else
			{
				deleteBtn.Attributes["onclick"].ShouldBe("return confirm('Folder ID: " + 0 + " - Are you sure that you want to delete this Folder ?')");
			}
		}

		[TestCase(ListItemType.Item, "5")]
		[TestCase(ListItemType.Item, "notNumber")]
		public void FoldersList_ItemDataBound_CNTFolderType(ListItemType itemType, string itemCount)
		{
			// Arrange
			var dataListItemArg = new DataListItemEventArgs(new DataListItem(0, itemType));
			var descBtn = new Label();
			var deleteBtn = new LinkButton();
			InitTest_AlternatingItem(itemCount, "CNT", "FolderDesc", descBtn, deleteBtn);

			// Act
			_foldersEditorPrivateObject.Invoke("FoldersList_ItemDataBound", new object[] { null, dataListItemArg });

			// Assert
			if (int.TryParse(itemCount, out int count))
			{
				deleteBtn.ShouldSatisfyAllConditions(
					() => deleteBtn.Enabled.ShouldBeFalse(),
					() => deleteBtn.Attributes["style"].ShouldBe("cursor:hand;padding:0;margin:0;"),
					() => deleteBtn.Attributes["onclick"].ShouldBe("alert('Content exists in this Folder. Delete is not allowed !!');"));
			}
			else
			{
				deleteBtn.Attributes["onclick"].ShouldBe("return confirm('Folder ID: " + 0 + " - Are you sure that you want to delete this Folder ?')");
			}
		}

		[TestCase(ListItemType.Item, 0)]
		[TestCase(ListItemType.Item, 1)]
		public void FoldersList_ItemDataBound_IMGFolderType(ListItemType itemType, int filesCount)
		{
			// Arrange
			var descBtn = new Label();
			var deleteBtn = new LinkButton();
			var dataListItemArg = new DataListItemEventArgs(new DataListItem(0, itemType));
			ConfigurationManager.AppSettings["Images_VirtualPath"] = "Images_VirtualPath";
			ShimHttpServerUtility.AllInstances.MapPathString = (s, path) => string.Empty;
			ShimDirectory.GetFilesString = (s) => new string[filesCount];
			ShimPage.AllInstances.ServerGet = (p) => new ShimHttpServerUtility();
			InitTest_AlternatingItem("0", "IMG", "FolderDesc", descBtn, deleteBtn);

			// Act
			_foldersEditorPrivateObject.Invoke("FoldersList_ItemDataBound", new object[] { null, dataListItemArg });

			// Assert
			if (filesCount > 0)
			{
				deleteBtn.ShouldSatisfyAllConditions(
					() => deleteBtn.Enabled.ShouldBeFalse(),
					() => deleteBtn.Attributes["style"].ShouldBe("cursor:hand;padding:0;margin:0;"),
					() => deleteBtn.Attributes["onclick"].ShouldBe("alert('Image files exist in this Folder. Delete is not allowed !!');"));
			}
			else
			{
				deleteBtn.Attributes["onclick"].ShouldBe("return confirm('Folder: " + 0 + " - Are you sure that you want to delete this Folder ?')");
			}
		}

		[TestCase("")]
		[TestCase("folderDesc")]
		public void FoldersList_ItemDataBound_DescriptionLinkPopulation(string folderDesc)
		{
			// Arrange
			var dataListItemArg = new DataListItemEventArgs(new DataListItem(0, ListItemType.Item));
			var descBtn = new Label();
			var deleteBtn = new LinkButton();
			var evaluatedFolderDesc = folderDesc.Length > 0 ? folderDesc : NoFolderDescAvailable;
			InitTest_AlternatingItem(string.Empty, string.Empty, folderDesc, descBtn, deleteBtn);
			var onmouseoverAttrValue = BuildDescBtnMouseOverFn(evaluatedFolderDesc);

			// Act
			_foldersEditorPrivateObject.Invoke("FoldersList_ItemDataBound", new object[] { null, dataListItemArg });

			// Assert
			descBtn.ShouldSatisfyAllConditions(
				() => descBtn.Attributes["onmouseout"].ShouldBe("return nd();"),
				() => descBtn.Attributes["onmouseover"].ShouldBe(onmouseoverAttrValue));
		}

		private void InitTest_EditItem(string folderTypeValue, string folderName, string folderDesc, TextBox editFolderNameTextBox, TextBox editFolderDescTextBox)
		{
			ShimDataKeyCollection.AllInstances.ItemGetInt32 = (collection, index) =>
			{
				if (folderTypeValue == "IMG")
					return folderName;
				return 0;
			};
			ShimListControl.AllInstances.SelectedValueGet = (c) => folderTypeValue;
			_foldersEditorPrivateObject.SetField("FolderType", BindingFlags.NonPublic | BindingFlags.Instance, new RadioButtonList());
			_foldersEditorPrivateObject.SetField("FoldersList", BindingFlags.NonPublic | BindingFlags.Instance, new DataList());
			var dataTable = new DataTable();
			dataTable.Columns.Add("FolderName");
			dataTable.Columns.Add("FolderDescription");
			var dataRow = dataTable.NewRow();
			dataRow["FolderName"] = folderName;
			dataRow["FolderDescription"] = folderDesc;
			ShimDataTable.AllInstances.SelectString = (dt, selectString) => new DataRow[] { dataRow };
			ShimControl.AllInstances.FindControlString = (c, id) =>
			{
				switch (id)
				{
					case "Edit_FolderName": return editFolderNameTextBox;
					case "Edit_FolderDesc": return editFolderDescTextBox;
				}
				return null;
			};
		}

		private void InitTest_AlternatingItem(string folderItemLblText, string folderTypeValue, string folderDescTextBoxText, Label descBtn, LinkButton deleteBtn)
		{
			ShimDataKeyCollection.AllInstances.ItemGetInt32 = (collection, index) => 0;
			ShimDataTable.AllInstances.Select = (dt) => new DataRow[] { };
			ShimControl.AllInstances.FindControlString = (c, id) =>
			{
				switch (id)
				{
					case "FolderDelete": return deleteBtn;
					case "FolderItemsLbl": return new Label() { Text = folderItemLblText };
					case "FolderDescLinkBtn": return descBtn;
					case "HDNFolderDescTxtBx": return new TextBox() { Text = folderDescTextBoxText };
				}
				return null;
			};
			ShimListControl.AllInstances.SelectedValueGet = (c) => folderTypeValue;
			_foldersEditorPrivateObject.SetField("FolderType", BindingFlags.NonPublic | BindingFlags.Instance, new RadioButtonList());
			_foldersEditorPrivateObject.SetField("FoldersList", BindingFlags.NonPublic | BindingFlags.Instance, new DataList());
		}

		private string BuildDescBtnMouseOverFn(string folderDesc)
		{
			return "return overlib('" + (MouseAltSt + folderDesc + MouseAltEnd) + "', FULLHTML,VAUTO,HAUTO,RIGHT,WIDTH,350,HEIGHT,130);";
		}
	}
}
