using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Fakes;
using System.Configuration.Fakes;
using System.Data;
using System.Data.Common.Fakes;
using System.Data.Fakes;
using System.IO.Fakes;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Activity.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECNTools.SMTPLog.Fakes;
using NUnit.Framework;
using Shouldly;
using ActEntities = ECN_Framework_Entities.Activity;
using DataLayer = ECN_Framework_DataLayer.Communicator;
using static ECNTools.Tests.SMTPLog.CommonHelper;

namespace ECNTools.Tests.SMTPLog
{
    public partial class Port25Test
    {
        private const string EDG_AppSettingsOutLogKey = "OutLog";
        private const string EDG_AppSettingsOutLogValue = "OutLogFileName";
        private const string EDG_ErrorMsgMarker = "[ERROR]";
        private const string EDG_NoBlastMsg = "There no blasts sent to this group during the dates specified";
        private const string EDG_NoSendsMsg = "No sends found to process";
        private const string EDG_FromDatePickerId = "dtFrom";
        private const string EDG_ToDatePickerId = "dtTo";
        private const string EDG_BackgroundWorkerId = "backgroundWorker1";
        private const string EDG_FolderBrowserDialogId = "folderBrowserDialog1";
        private const string EDG_NoBlastFoundMsg = "No blast found to process";
        private const string EDG_NoBPAFileMsg = "Error creating BPALog file for blast";
        private const string EDG_ExportCompletedMsg = "Export Completed";
        private const int EDG_GroupId = 10;
        private readonly DateTime _edg_blastSentDate = new DateTime(2000, 1, 1);
        private TextBox _edg_txtDigitalSplit;
        private TextBox _edg_txtGroupID;
        private DateTimePicker _edg_dtFrom;
        private DateTimePicker _edg_dtTo;
        private FolderBrowserDialog _edg_folderBrowserDialog;
        private int _edg_workerProgress;
        private string _edg_workerMsg;
        private string _edg_MsgBoxText;

        [Test]
        public void ExportDataByGroup_GetGroupException_Error()
        {
            // Arrange
            InitTestForExportDataByGroup();
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (grpId) => throw new Exception();

            // Act
            _privateObject.Invoke("ExportDataByGroup", null);

            // Assert
            _edg_MsgBoxText.ShouldContain(EDG_ErrorMsgMarker);
        }

        [Test]
        public void ExportDataByGroup_BlastOutDates_Error()
        {
            // Arrange
            InitTestForExportDataByGroup(dataGroup: new Group());

            // Act
            _privateObject.Invoke("ExportDataByGroup", null);

            // Assert
            _edg_MsgBoxText.ShouldContain(EDG_NoBlastMsg);
        }

        [Test]
        public void ExportDataByGroup_CancellationPending_Report100Progress()
        {
            // Arrange
            var blasts = CreateBlastCollectionForExportDataByGroup();
            InitTestForExportDataByGroup(dataGroup: new Group(), blastCollection: blasts);
            ShimBackgroundWorker.AllInstances.CancellationPendingGet = (worker) => true;

            // Act
            _privateObject.Invoke("ExportDataByGroup", null);

            // Assert
            _edg_workerProgress.ShouldBe(100);
        }

        [Test]
        public void ExportDataByGroup_NoBlast_Error()
        {
            // Arrange
            var blasts = CreateBlastCollectionForExportDataByGroup(false);
            InitTestForExportDataByGroup(dataGroup: new Group(), blastCollection: blasts);

            // Act
            _privateObject.Invoke("ExportDataByGroup", null);

            // Assert
            _edg_workerMsg.ShouldSatisfyAllConditions(
              () => _edg_workerMsg.ShouldNotBeNull(),
              () => _edg_workerMsg.ShouldContain(EDG_NoBlastFoundMsg));
        }

        [Test]
        public void ExportDataByGroup_NoBPAFile_Error()
        {
            // Arrange
            var blasts = CreateBlastCollectionForExportDataByGroup();
            InitTestForExportDataByGroup(dataGroup: new Group(), blastCollection: blasts);
            ShimPort25.AllInstances.SetupBPAAuditByGroupGroup = (port, grp) => null;

            // Act
            try
            {
                _privateObject.Invoke("ExportDataByGroup", null);
            }
            catch (Exception)
            {
            }
            // Assert
            finally
            {
                _edg_workerMsg.ShouldSatisfyAllConditions(
                    () => _edg_workerMsg.ShouldNotBeNull(),
                    () => _edg_workerMsg.ShouldContain(EDG_NoBPAFileMsg));
            }
        }

        [Test]
        public void ExportDataByGroup_GetActivitySendException_Error()
        {
            // Arrange
            var blasts = CreateBlastCollectionForExportDataByGroup();
            InitTestForExportDataByGroup(dataGroup: new Group(), blastCollection: blasts);
            ShimBlastActivitySends.GetByBlastIDInt32 = (id) => throw new Exception();

            // Act
            _privateObject.Invoke("ExportDataByGroup", null);


            // Assert
            _edg_workerMsg.ShouldSatisfyAllConditions(
                () => _edg_workerMsg.ShouldNotBeNull(),
                () => _edg_workerMsg.ShouldContain(EDG_ErrorMsgMarker));
        }

        [Test]
        public void ExportDataByGroup_NoActivitySends_Error()
        {
            // Arrange
            var blasts = CreateBlastCollectionForExportDataByGroup();
            CreateCollectionDataForExportDataByGroupOrBlast(out List<ActEntities.BlastActivitySends> activitySends, out List<ActEntities.BlastActivityBounces> activityBounces, out DataTable digitalSplitDataTable);
            InitTestForExportDataByGroup(dataGroup: new Group(), blastCollection: blasts, activityBouncesList: activityBounces, activitySendsList: activitySends);
            ShimBlastActivitySends.GetByBlastIDInt32 = (id) => null;

            // Act
            _privateObject.Invoke("ExportDataByGroup", null);

            // Assert
            _edg_workerMsg.ShouldSatisfyAllConditions(
                () => _edg_workerMsg.ShouldNotBeNull(),
                () => _edg_workerMsg.ShouldContain(EDG_NoSendsMsg));
        }

        [Test]
        public void ExportDataByGroup_NoDigitalSplitTable_NoError()
        {
            // Arrange
            var blasts = CreateBlastCollectionForExportDataByGroup();
            CreateCollectionDataForExportDataByGroupOrBlast(out List<ActEntities.BlastActivitySends> activitySends, out List<ActEntities.BlastActivityBounces> activityBounces, out DataTable digitalSplitTable);
            InitTestForExportDataByGroup(dataGroup: new Group(), blastCollection: blasts, activityBouncesList: activityBounces, activitySendsList: activitySends);

            // Act
            _privateObject.Invoke("ExportDataByGroup", null);

            // Assert
            _edg_workerMsg.ShouldSatisfyAllConditions(
               () => _edg_workerMsg.ShouldNotBeNull(),
               () => _edg_workerMsg.ShouldContain(EDG_ExportCompletedMsg),
               () => _edg_workerProgress.ShouldBe(activitySends.Count));
        }

        [Test]
        public void ExportDataByGroup_WithDigitalSplitTable_NoError()
        {
            // Arrange
            var blasts = CreateBlastCollectionForExportDataByGroup();
            CreateCollectionDataForExportDataByGroupOrBlast(out List<ActEntities.BlastActivitySends> activitySends, out List<ActEntities.BlastActivityBounces> activityBounces, out DataTable digitalSplitTable);
            InitTestForExportDataByGroup(dataGroup: new Group(), blastCollection: blasts, activityBouncesList: activityBounces, activitySendsList: activitySends, digitalSplitDataTable: digitalSplitTable);

            // Act
            _privateObject.Invoke("ExportDataByGroup", null);

            // Assert
            _edg_workerMsg.ShouldSatisfyAllConditions(
              () => _edg_workerMsg.ShouldNotBeNull(),
              () => _edg_workerMsg.ShouldContain(EDG_ExportCompletedMsg),
              () => _edg_workerProgress.ShouldBe(digitalSplitTable.Rows.Count));
        }

        private void InitTestForExportDataByGroup(Group dataGroup = null, List<BlastAbstract> blastCollection = null, List<ActEntities.BlastActivitySends> activitySendsList = null, List<ActEntities.BlastActivityBounces> activityBouncesList = null, DataTable digitalSplitDataTable = null)
        {
            CreateClassObject();
            SetPageControlsForExportDataByGroup();
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                var appSettingsCollection = new NameValueCollection();
                appSettingsCollection.Add(EDG_AppSettingsOutLogKey, EDG_AppSettingsOutLogValue);
                return appSettingsCollection;
            };
            ShimTextWriter.AllInstances.WriteLineString = (writer, text) => { };
            ShimStreamWriter.AllInstances.Close = (writer) => { };
            ShimStreamWriter.ConstructorStream = (writer, stream) => { };
            ShimFileStream.ConstructorStringFileMode = (st, path, mode) => { };
            ShimDirectory.CreateDirectoryString = (path) => null;
            ShimFile.ExistsString = (path) => true;
            ShimFile.DeleteString = (path) => { };
            DataLayer.Fakes.ShimGroup.GetByGroupIDInt32 = (id) => dataGroup;
            CopyBlastCollectionForExportDataByGroup(blastCollection, out List<BlastAbstract> blastCollectionCopy);
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, getChildren) =>
            {
                if (id == -1)
                {
                    return null;
                }
                if (blastCollectionCopy == null || blastCollectionCopy.Count() == 0)
                {
                    CopyBlastCollectionForExportDataByGroup(blastCollection, out blastCollectionCopy);
                }
                var blast = blastCollectionCopy[0];
                blastCollectionCopy.RemoveAt(0);
                return blast;
            };
            DataLayer.Fakes.ShimBlast.GetListSqlCommand = (cmd) =>
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add("BlastID");
                if (blastCollection != null)
                {
                    for (var i = 0; i < blastCollection.Count; i++)
                    {
                        var row = dataTable.NewRow();
                        row[0] = i;
                        dataTable.Rows.Add(row);
                    }
                }
                return dataTable;
            };
            ShimMessageBox.ShowString = (text) =>
            {
                _edg_MsgBoxText = text;
                return DialogResult.OK;
            };
            ShimPath.GetDirectoryNameString = (path) => string.Empty;
            ShimBackgroundWorker.AllInstances.ReportProgressInt32Object = (worker, progress, msg) =>
            {
                _edg_workerProgress = progress;
                _edg_workerMsg += msg?.ToString();
            };
            ShimDbDataAdapter.AllInstances.FillDataSetInt32Int32String = (adapter, dataSet, startRecord, maxRecords, srcTable) => 0;
            ShimCustomer.GetByCustomerIDInt32Boolean = (id, getChildren) => new Customer() { CustomerName = "custName" };
            ShimDataTableCollection.AllInstances.ItemGetString = (collectio, name) => digitalSplitDataTable;
            ShimBlastActivitySends.GetListSqlCommand = (cmd) => activitySendsList;
            ShimBlastActivityBounces.GetListSqlCommand = (cmd) => activityBouncesList;
        }

        private void SetPageControlsForExportDataByGroup()
        {
            _edg_txtGroupID = Get<TextBox>(TextBoxGroupID);
            _edg_txtGroupID.Text = EDG_GroupId.ToString();
            _edg_dtFrom = Get<DateTimePicker>(EDG_FromDatePickerId);
            _edg_dtFrom.Value = _edg_blastSentDate;
            _edg_dtTo = Get<DateTimePicker>(EDG_ToDatePickerId);
            _edg_dtTo.Value = _edg_blastSentDate;
            _edg_folderBrowserDialog = Get<FolderBrowserDialog>(EDG_FolderBrowserDialogId);
            _edg_folderBrowserDialog.SelectedPath = string.Empty;
            _edg_txtDigitalSplit = Get<TextBox>(TextBoxDigitalSplit);
            _edg_txtDigitalSplit.Text = "dummySplitter";
        }

        private void CopyBlastCollectionForExportDataByGroup(List<BlastAbstract> source, out List<BlastAbstract> destination)
        {
            if (source == null || source.Count == 0)
            {
                destination = null;
                return;
            }
            var blastArray = new BlastAbstract[source.Count];
            source.CopyTo(blastArray);
            destination = blastArray.ToList();
        }

        private List<BlastAbstract> CreateBlastCollectionForExportDataByGroup(bool setId = true)
        {
            var collection = new List<BlastAbstract>();
            for (var i = 0; i < 110; i++)
            {
                collection.Add(new BlastSMS()
                {
                    BlastID = setId ? i : -1,
                    SendTime = _edg_blastSentDate.AddHours(1)
                });
            }
            return collection;
        }
    }
}
