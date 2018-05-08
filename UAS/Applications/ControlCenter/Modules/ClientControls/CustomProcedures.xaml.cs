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
    /// Interaction logic for CustomProcedures.xaml
    /// </summary>
    public partial class CustomProcedures : UserControl
    {
        public CustomProcedures(int id)
        {
            InitializeComponent();
            LoadCustomData(id);
        }

        private FrameworkUAS.Entity.ClientCustomProcedure originalClient { get; set; }
        private List<FrameworkUAS.Entity.ClientCustomProcedure> clientEdits { get; set; }
        private List<FrameworkUAS.Entity.ClientCustomProcedure> clients { get; set; }
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClientCustomProcedure> clientData { get; set; }

        private bool insertCheck = false;
        private int currentID = 0;
        private bool cancelCheck = false;

        private void LoadCustomData(int id)
        {
            int[] nums = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            grdcbExecution.ItemsSource = nums;
            originalClient = new FrameworkUAS.Entity.ClientCustomProcedure();
            clientEdits = new List<FrameworkUAS.Entity.ClientCustomProcedure>();
            clientData = FrameworkServices.ServiceClient.UAS_ClientCustomProcedureClient();
            clients = AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[id].ClientCustomProceduresList;
            currentID = id;

            grdCustom.ItemsSource = null;
            grdCustom.ItemsSource = clients;
            grdCustom.Rebind();
        }

        #region GridWork
        private void CheckForChanges(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            FrameworkUAS.Entity.ClientCustomProcedure myClient = (FrameworkUAS.Entity.ClientCustomProcedure)grdCustom.SelectedItem;
            if (cancelCheck == false)
            {
                if (e.EditOperationType == GridViewEditOperationType.Edit)
                {
                    bool hasChanged = false;

                    if (!myClient.IsActive.Equals(originalClient.IsActive))
                        hasChanged = true;
                    //REFACTOR
                    if (!myClient.ExecutionPointID.Equals(originalClient.ExecutionPointID))
                        hasChanged = true;
                    if (!myClient.ExecutionOrder.Equals(originalClient.ExecutionOrder))
                        hasChanged = true;
                    if (!myClient.ProcedureName.Equals(originalClient.ProcedureName))
                        hasChanged = true;

                    if (hasChanged == true)
                    {
                        originalClient = AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[myClient.ClientID].ClientCustomProceduresList.First();// clientData.SelectClient(myClient.ClientID).Where(o => o.ClientID == myClient.ClientID).First();
                        int replaceIndex = clientEdits.IndexOf(clientEdits.Where(x => x.ClientCustomProcedureID == myClient.ClientCustomProcedureID).FirstOrDefault());
                        if (replaceIndex != -1)
                            clientEdits.RemoveAt(replaceIndex);
                        clientEdits.Add(myClient);
                        //rbSave.IsEnabled = true;
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

        private void grdCustom_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (insertCheck == false)
            {
                originalClient = (FrameworkUAS.Entity.ClientCustomProcedure)grdCustom.SelectedItem;
                originalClient = AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[originalClient.ClientID].ClientCustomProceduresList.Where(o => o.ClientCustomProcedureID == originalClient.ClientCustomProcedureID).First();//clientData.SelectClient(originalClient.ClientID).Where(o => o.ClientCustomProcedureID == originalClient.ClientCustomProcedureID).First();
            }
        }

        private void grdCustom_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (insertCheck == false)
            {
                if(e.OldData != null)
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
        private bool ClientEditIsValid(FrameworkUAS.Entity.ClientCustomProcedure c)
        {
            FrameworkUAS.Entity.ClientCustomProcedure origin = clients.Where(x => x.ClientCustomProcedureID == c.ClientCustomProcedureID).FirstOrDefault();

            bool hasChanged = false;
            //REFACTOR
            if (!c.IsActive.Equals(origin.IsActive))
                hasChanged = true;
            if (!c.ExecutionPointID.Equals(origin.ExecutionPointID))
                hasChanged = true;
            if (!c.ExecutionOrder.Equals(origin.ExecutionOrder))
                hasChanged = true;
            if (!c.ProcedureName.Equals(origin.ProcedureName))
                hasChanged = true;

            return hasChanged;
        }

        private void ClientSave()
        {
            grdCustom.CommitEdit();
            if (insertCheck == false)
            {
                //clients = clientData.SelectClient(currentID).ToList();
                //FrameworkUAS.Entity.ClientCustomProcedure myClient = (FrameworkUAS.Entity.ClientCustomProcedure)grdCustom.SelectedItem;
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
            clients = clientData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, currentID).Result; //Reload master list.

            AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[originalClient.ClientID].ClientCustomProceduresList = clients;
            //AppData.myAppData.AuthorizedUser.User.ClientGroups.Single(x => x.ClientGroupID == AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.ClientGroupID).Clients.Single(x => x.ClientID == originalClient.ClientID).ClientCustomProceduresList = clients;

            clientEdits = new List<FrameworkUAS.Entity.ClientCustomProcedure>(); //Reload edits list.
            MessageBoxResult confirm = MessageBox.Show("Your changes have been saved.", "Data Saved", MessageBoxButton.OK);
        }
        #endregion

        #region buttonMethods
        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            if (rbSave.IsEnabled)
            {
                cancelCheck = true;
                MessageBoxResult confirm = MessageBox.Show("Your pending changes will be lost. Do you wish to continue?", "Unsaved Data", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    grdCustom.CancelEdit();
                    rbSave.IsEnabled = false;
                    rbSave.BorderBrush = null;
                    rbNewRow.IsEnabled = true;
                    LoadCustomData(currentID);
                    cancelCheck = false;
                    insertCheck = false;
                }
                else
                    return;
            }
        }

        private void rbSave_Click(object sender, RoutedEventArgs e)
        {
            ClientSave();
        }

        private void rbNewRow_Click(object sender, RoutedEventArgs e)
        {
            if (rbSave.IsEnabled == true)
            {
                MessageBoxResult confirm = MessageBox.Show("Save current changes before continuing?", "Unsaved Data", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    ClientSave();
                    insertCheck = true;
                    grdCustom.BeginInsert();
                }
                else
                    return;
            }
            else
            {
                rbNewRow.IsEnabled = false;
                rbSave.IsEnabled = true;
                insertCheck = true;
                var rows = grdCustom.ChildrenOfType<GridViewRow>(); 
                foreach (GridViewRow x in rows)
                    x.IsEnabled = false;
                grdCustom.BeginInsert();
            }
        }
        #endregion
    }
}
