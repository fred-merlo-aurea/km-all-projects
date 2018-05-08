using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using FrameworkUAS.Object;
namespace ControlCenter.Modules.ClientControls
{
    /// <summary>
    /// Interaction logic for SpecialFiles.xaml
    /// </summary>
    public partial class SpecialFiles : UserControl
    {
        public SpecialFiles(int id)
        {
            InitializeComponent();
            LoadFilesData(id);
        }

        private FrameworkUAS.Entity.SourceFile originalClient { get; set; }
        private List<FrameworkUAS.Entity.SourceFile> clientEdits { get; set; }
        private List<FrameworkUAS.Entity.SourceFile> clients { get; set; }
        private FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> clientData { get; set; }
        private bool insertCheck = false;
        private int currentID = 0;
        private bool cancelCheck = false;

        private void LoadFilesData(int id)
        {
            string[] extensions = { ".txt", ".csv", ".xls", ".xlsx" };
            grdcbExtensions.ItemsSource = extensions;
            originalClient = new FrameworkUAS.Entity.SourceFile();
            clientEdits = new List<FrameworkUAS.Entity.SourceFile>();
            clientData = FrameworkServices.ServiceClient.UAS_SourceFileClient();
            clients = AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[id].SourceFilesList;
            currentID = id;

            grdSpecialFiles.ItemsSource = null;
            grdSpecialFiles.ItemsSource = clients;
            grdSpecialFiles.Rebind();
        }

        #region GridWork
        public void grdSpecialFiles_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (insertCheck == false)
            {
                originalClient = (FrameworkUAS.Entity.SourceFile)grdSpecialFiles.SelectedItem;
                originalClient = AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[originalClient.ClientID].SourceFilesList.Where(o => o.ClientID == originalClient.ClientID && o.FileName == originalClient.FileName).First();
            }
        }

        public void CheckForChanges(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            FrameworkUAS.Entity.SourceFile myClient = (FrameworkUAS.Entity.SourceFile)grdSpecialFiles.SelectedItem;

            if (cancelCheck == false)
            {
                if (e.EditOperationType == GridViewEditOperationType.Edit)
                {
                    bool hasChanged = false;

                    if (!myClient.IsDeleted.Equals(originalClient.IsDeleted))
                        hasChanged = true;
                    //REFACTOR
                    //if (!myClient.CreatesDataTable.Equals(originalClient.CreatesDataTable))
                    //    hasChanged = true;
                    if (!myClient.FileName.Equals(originalClient.FileName))
                        hasChanged = true;
                    if (!myClient.Extension.Equals(originalClient.Extension))
                        hasChanged = true;
                    if (!myClient.ClientCustomProcedureID.Equals(originalClient.ClientCustomProcedureID))
                        hasChanged = true;
                    //if (!myClient.Method.Equals(originalClient.Method))
                    //    hasChanged = true;

                    if (hasChanged == true)
                    {
                        originalClient = AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[originalClient.ClientID].SourceFilesList.Where(o => o.ClientID == myClient.ClientID && o.FileName == myClient.FileName).First();
                        int replaceIndex = clientEdits.IndexOf(clientEdits.Where(x => x.ClientID == myClient.ClientID).FirstOrDefault());
                        if (replaceIndex != -1)
                            clientEdits.RemoveAt(replaceIndex);
                        clientEdits.Add(myClient);
                    }

                }
                else if (e.EditOperationType == GridViewEditOperationType.Insert)
                {
                    rbSave.IsEnabled = true;
                    rbNewRow.IsEnabled = false;
                    myClient.ClientID = currentID;
                    clientEdits.Add(myClient);
                }
            }
                
        }

        private void grdSpecialFiles_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (insertCheck == false)
            {
                if (e.OldData != null)
                {
                    if (!e.OldData.Equals(e.NewData))
                    {
                        rbSave.IsEnabled = true;
                        System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF0000");
                        rbSave.BorderBrush = new SolidColorBrush(color);
                    }
                }
            }
        }
        #endregion

        #region ClientMethods
        private bool ClientEditIsValid(FrameworkUAS.Entity.SourceFile c)
        {
            FrameworkUAS.Entity.SourceFile origin = clients.Where(x => x.ClientID == c.ClientID).FirstOrDefault();

            bool hasChanged = false;

            if (!c.IsDeleted.Equals(origin.IsDeleted))
                hasChanged = true;
            //REFACTOR
            //if (!c.CreatesDataTable.Equals(origin.CreatesDataTable))
            //    hasChanged = true;
            if (!c.FileName.Equals(origin.FileName))
                hasChanged = true;
            if (!c.Extension.Equals(origin.Extension))
                hasChanged = true;
            if (!c.ClientCustomProcedureID.Equals(origin.ClientCustomProcedureID))
                hasChanged = true;
            //if (!c.Method.Equals(origin.Method))
            //    hasChanged = true;

            return hasChanged;
        }

        private void ClientSave()
        {
            grdSpecialFiles.CommitEdit();
            if (insertCheck == false)
            {
                //clients = clientData.Select(currentID,false,false).ToList();
                //FrameworkUAS.Entity.ClientCustomProcedure myClient = (FrameworkUAS.Entity.ClientCustomProcedure)grdSpecialFiles.SelectedItem;
                for (int i = 0; i < clientEdits.Count; i++)
                {
                    if (ClientEditIsValid(clientEdits[i]) == true)
                        clientData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, clientEdits[i]);
                }
            }
            else
                clientData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, clientEdits[0]);
            rbSave.IsEnabled = false;
            rbNewRow.IsEnabled = true;
            rbSave.BorderBrush = null;
            clients = clientData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, currentID, true, false).Result; //Reload master list.

            AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[originalClient.ClientID].SourceFilesList = clients;
            //AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[originalClient.ClientID].SourceFilesList = clients;

            clientEdits = new List<FrameworkUAS.Entity.SourceFile>(); //Reload edits list.
            MessageBoxResult confirm = MessageBox.Show("Your changes have been saved.", "Data Saved", MessageBoxButton.OK);
        }
        #endregion

        #region buttonMethods
        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            if (rbSave.IsEnabled)
            {
                MessageBoxResult confirm = MessageBox.Show("Your pending changes will be lost. Do you wish to continue?", "Unsaved Data", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    cancelCheck = true;
                    grdSpecialFiles.CancelEdit();
                    rbSave.IsEnabled = false;
                    rbSave.BorderBrush = null;
                    rbNewRow.IsEnabled = true;
                    LoadFilesData(currentID);
                    cancelCheck = false;
                    insertCheck = false;
                }
                else
                    return;
            }
        }
        
        public void rbNewRow_Click(object sender, RoutedEventArgs e)
        {
            if (rbSave.IsEnabled == true)
            {
                MessageBoxResult confirm = MessageBox.Show("Save current changes before continuing?", "Unsaved Data", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    ClientSave();
                    rbNewRow.IsEnabled = false;
                    insertCheck = true;
                    grdSpecialFiles.BeginInsert();
                }
                else
                    return;
            }
            else
            {
                rbNewRow.IsEnabled = false;
                rbSave.IsEnabled = true;
                insertCheck = true;
                var rows = grdSpecialFiles.ChildrenOfType<GridViewRow>();
                foreach (GridViewRow x in rows)
                    x.IsEnabled = false;
                grdSpecialFiles.BeginInsert();
            }
        }

        private void rbSave_Click(object sender, RoutedEventArgs e)
        {
            ClientSave();
        }
        #endregion

    }
}
