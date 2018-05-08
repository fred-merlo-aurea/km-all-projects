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
        private const string EDB_AppSettingsOutLogKey = "OutLog";
        private const string EDB_AppSettingsOutLogValue = "OutLogFileName";
        private const string EDB_BlastIDs = "0,1,2,3,4,5,6,7,8,9";
        private const string EDB_ErrorMsgMarker = "[ERROR]";
        private const string EDB_BackgroundWorkerId = "backgroundWorker1";
        private const string EDB_NoBlastFoundMsg = "No blast found to process";
        private const string EDB_NoBPAFileMsg = "Error creating BPALog file for blast";
        private const string EDB_NoSendsMsg = "No sends found to process";
        private const string EDB_FolderBrowserDialogId = "folderBrowserDialog1";
        private const string EDB_ExportCompletedMsg = "Export Completed";
        private const string EDB_DummyEmailAddress = "EmailAddress";
        private int _edb_workerProgress;
        private string _edb_workerMsg;
        private TextBox _edb_textBoxBlastID;
        private TextBox _edb_txtDigitalSplit;
        private FolderBrowserDialog _edb_folderBrowserDialog;
        private RadioButton _edb_rbGroup;
        private string _edb_MsgBoxText;

        [Test]
        public void ExportDataByBlast_InvalidBlastIds_Error()
        {
            // Arrange
            InitTestForExportDataByBlast(blastIds: "notNumber");

            // Act
            _privateObject.Invoke("ExportDataByBlast", null);

            // Assert
            _edb_MsgBoxText.ShouldContain(EDB_ErrorMsgMarker);
        }

        [Test]
        public void ExportDataByBlast_InvalidBlastCount_Error()
        {
            // Arrange
            InitTestForExportDataByBlast(blastIds: "0,1,2,3,4,5,6,7,8,9,10");

            // Act
            _privateObject.Invoke("ExportDataByBlast", null);

            // Assert
            _edb_MsgBoxText.ShouldContain(EDB_ErrorMsgMarker);
        }

        [Test]
        public void ExportDataByBlast_CancellationPending_Report100Progress()
        {
            // Arrange
            InitTestForExportDataByBlast(blastIds: "1,2");
            ShimBackgroundWorker.AllInstances.CancellationPendingGet = (worker) => true;

            // Act
            _privateObject.Invoke("ExportDataByBlast", null);

            // Assert
            _edb_workerProgress.ShouldBe(100);
        }

        [Test]
        public void ExportDataByBlast_Exception_Error()
        {
            // Arrange
            InitTestForExportDataByBlast(blastIds: "1,2");
            var exceptionString = "Invalid Error";
            ShimPath.GetDirectoryNameString = (path) => throw new Exception(exceptionString);

            // Act
            _privateObject.Invoke("ExportDataByBlast", null);

            // Assert
            _edb_MsgBoxText.ShouldSatisfyAllConditions(
                () => _edb_MsgBoxText.ShouldContain(EDB_ErrorMsgMarker),
                () => _edb_MsgBoxText.ShouldContain(exceptionString));
        }

        [Test]
        public void ExportDataByBlast_NoBlast_Error()
        {
            // Arrange
            InitTestForExportDataByBlast(blastIds: "1,2");
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, getChildren) => null;

            // Act
            _privateObject.Invoke("ExportDataByBlast", null);

            // Assert
            _edb_workerMsg.ShouldSatisfyAllConditions(
                () => _edb_workerMsg.ShouldNotBeNull(),
                () => _edb_workerMsg.ShouldContain(EDB_NoBlastFoundMsg));
        }

        [Test]
        public void ExportDataByBlast_NoBPAFile_Error()
        {
            // Arrange
            var blast = new BlastSMS()
            {
                BlastID = 10,
                CustomerID = 100,
                SendTime = new DateTime(2000, 1, 1)
            };
            InitTestForExportDataByBlast(blast: blast, blastIds: "1,2");
            ShimPort25.AllInstances.SetupBPAAuditStringBlast = (port25, name, bls) => null;

            // Act
            _privateObject.Invoke("ExportDataByBlast", null);

            // Assert
            _edb_workerMsg.ShouldSatisfyAllConditions(
                () => _edb_workerMsg.ShouldNotBeNull(),
                () => _edb_workerMsg.ShouldContain(EDB_NoBPAFileMsg));
        }

        [Test]
        public void ExportDataByBlast_NoDigitalSplit_NoError()
        {
            // Arrange
            var blast = new BlastSMS()
            {
                BlastID = 10,
                CustomerID = 100,
                SendTime = new DateTime(2000, 1, 1)
            };
            CreateCollectionDataForExportDataByGroupOrBlast(out List<ActEntities.BlastActivitySends> activitySendsList, out List<ActEntities.BlastActivityBounces> activityBouncesList, out DataTable digitalSplitDT);
            InitTestForExportDataByBlast(blast: blast, activityBouncesList: activityBouncesList, activitySendsList: activitySendsList);

            // Act
            _privateObject.Invoke("ExportDataByBlast", null);

            // Assert
            var exportCount = _edb_workerMsg.Split(new string[] { EDB_ExportCompletedMsg }, StringSplitOptions.None).Length;
            var expectedExportCount = EDB_BlastIDs.Trim().Split(",".ToCharArray()).Length + 1;
            _edb_workerMsg.ShouldSatisfyAllConditions(
                () => _edb_workerMsg.ShouldNotBeNull(),
                () => _edb_workerMsg.ShouldContain(EDB_ExportCompletedMsg),
                () => _edb_workerProgress.ShouldBe(activitySendsList.Count),
                () => exportCount.ShouldBe(expectedExportCount));
        }

        [Test]
        public void ExportDataByBlast_WithDigitalSplit_NoError()
        {
            // Arrange
            var blast = new BlastSMS()
            {
                BlastID = 10,
                CustomerID = 100,
                SendTime = new DateTime(2000, 1, 1)
            };
            CreateCollectionDataForExportDataByGroupOrBlast(out List<ActEntities.BlastActivitySends> activitySendsList, out List<ActEntities.BlastActivityBounces> activityBouncesList, out DataTable digitalSplitDT);
            InitTestForExportDataByBlast(blast: blast, activityBouncesList: activityBouncesList, activitySendsList: activitySendsList, digitalSplitDataTable: digitalSplitDT);

            // Act
            _privateObject.Invoke("ExportDataByBlast", null);

            // Assert
            var exportCount = _edb_workerMsg.Split(new string[] { EDB_ExportCompletedMsg }, StringSplitOptions.None).Length;
            var expectedExportCount = EDB_BlastIDs.Trim().Split(",".ToCharArray()).Length + 1;
            _edb_workerMsg.ShouldSatisfyAllConditions(
              () => _edb_workerMsg.ShouldNotBeNull(),
              () => _edb_workerMsg.ShouldContain(EDB_ExportCompletedMsg),
              () => _edb_workerProgress.ShouldBe(digitalSplitDT.Rows.Count),
              () => exportCount.ShouldBe(expectedExportCount));
        }

        [Test]
        public void ExportDataByBlast_NoSendList_Error()
        {
            // Arrange
            var blast = new BlastSMS()
            {
                BlastID = 10,
                CustomerID = 100,
                SendTime = new DateTime(2000, 1, 1)
            };
            InitTestForExportDataByBlast(blast: blast, blastIds: "1,2");

            // Act
            _privateObject.Invoke("ExportDataByBlast", null);

            // Assert
            _edb_workerMsg.ShouldSatisfyAllConditions(
                () => _edb_workerMsg.ShouldNotBeNull(),
                () => _edb_workerMsg.ShouldContain(EDB_NoSendsMsg));
        }

        private void InitTestForExportDataByBlast(string blastIds = "",
            DataTable digitalSplitDataTable = null,
            BlastAbstract blast = null,
            List<ActEntities.BlastActivitySends> activitySendsList = null, List<ActEntities.BlastActivityBounces> activityBouncesList = null)
        {
            _edb_workerMsg = string.Empty;
            CreateClassObject();
            SetPageControlsForExportDataByBlast();
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                var appSettingsCollection = new NameValueCollection();
                appSettingsCollection.Add(EDB_AppSettingsOutLogKey, EDB_AppSettingsOutLogValue);
                return appSettingsCollection;
            };
            _edb_textBoxBlastID.Text = string.IsNullOrWhiteSpace(blastIds)
               ? EDB_BlastIDs
               : blastIds;
            ShimMessageBox.ShowString = (text) =>
            {
                _edb_MsgBoxText = text;
                return DialogResult.OK;
            };
            ShimTextWriter.AllInstances.WriteLineString = (writer, text) => { };
            ShimStreamWriter.AllInstances.Close = (writer) => { };
            ShimStreamWriter.ConstructorStream = (writer, stream) => { };
            ShimFileStream.ConstructorStringFileMode = (st, path, mode) => { };
            ShimBackgroundWorker.AllInstances.ReportProgressInt32Object = (worker, progress, msg) =>
            {
                _edb_workerProgress = progress;
                _edb_workerMsg += msg?.ToString();
            };
            ShimPath.GetDirectoryNameString = (path) => string.Empty;
            ShimDbDataAdapter.AllInstances.FillDataSetInt32Int32String = (adapter, dataSet, startRecord, maxRecords, srcTable) => 0;
            ShimDataTableCollection.AllInstances.ItemGetString = (collectio, name) => digitalSplitDataTable;
            DataLayer.Fakes.ShimBlast.GetByBlastIDInt32 = (id) => blast;
            DataLayer.Fakes.ShimBlastFields.GetByBlastIDInt32 = (id) => new BlastFields();
            ShimFile.ExistsString = (path) => true;
            ShimFile.DeleteString = (path) => { };
            ShimCustomer.GetByCustomerIDInt32Boolean = (id, getChildren) => new Customer() { CustomerName = "customerName" };
            ShimDirectory.CreateDirectoryString = (path) => null;
            ShimBlastActivitySends.GetListSqlCommand = (cmd) => activitySendsList;
            ShimBlastActivityBounces.GetListSqlCommand = (cmd) => activityBouncesList;
        }

        private void SetPageControlsForExportDataByBlast()
        {
            _edb_textBoxBlastID = Get<TextBox>(TextBoxBlastID);
            _edb_rbGroup = Get<RadioButton>(RadioButtonGroup);
            _edb_rbGroup.Checked = true;
            _edb_txtDigitalSplit = Get<TextBox>(TextBoxDigitalSplit);
            _edb_folderBrowserDialog = Get<FolderBrowserDialog>(EDB_FolderBrowserDialogId);
            _edb_folderBrowserDialog.SelectedPath = string.Empty;
            _edb_txtDigitalSplit.Text = "dummySplitter";
        }

        private void CreateCollectionDataForExportDataByGroupOrBlast(out List<ActEntities.BlastActivitySends> activitySendsList, out List<ActEntities.BlastActivityBounces> activityBouncesList, out DataTable digitalSplitDT)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("emailAddress");
            for (var i = 0; i < 110; i++)
            {
                var row = dataTable.NewRow();
                row[0] = $"{EDB_DummyEmailAddress}_{i}";
                dataTable.Rows.Add(row);
            }
            digitalSplitDT = dataTable;
            activitySendsList = new List<ActEntities.BlastActivitySends>();
            activityBouncesList = new List<ActEntities.BlastActivityBounces>();
            activitySendsList.Add(new ActEntities.BlastActivitySends() { EmailAddress = $"{EDB_DummyEmailAddress}_1", SMTPMessage = "SMTPM1", EmailID = 1, SendTime = new DateTime(2000, 1, 1) });
            activitySendsList.Add(new ActEntities.BlastActivitySends() { EmailAddress = $"{EDB_DummyEmailAddress}_2", SMTPMessage = null, EmailID = 2, SendTime = new DateTime(2000, 1, 1) });
            activitySendsList.Add(new ActEntities.BlastActivitySends() { EmailAddress = $"{EDB_DummyEmailAddress}_3", SMTPMessage = null, EmailID = 3, SendTime = new DateTime(2000, 1, 1) });
            activitySendsList.Add(new ActEntities.BlastActivitySends() { EmailAddress = $"{EDB_DummyEmailAddress}_4", SMTPMessage = null, EmailID = 10, SendTime = new DateTime(2000, 1, 1), SendID = -1237911168 });
            for (int i = 20; i < 130; i++)
            {
                activitySendsList.Add(new ActEntities.BlastActivitySends() { EmailAddress = EDB_DummyEmailAddress, SMTPMessage = $"SMTPM{i}", EmailID = i, SendTime = new DateTime(2000, 1, 1) });
            }
            activityBouncesList.Add(new ActEntities.BlastActivityBounces() { EmailID = 3 });
            activityBouncesList.Add(new ActEntities.BlastActivityBounces() { EmailID = 4 });
            activityBouncesList.Add(new ActEntities.BlastActivityBounces() { EmailID = 5 });
            activityBouncesList.Add(new ActEntities.BlastActivityBounces() { EmailID = 10, BounceMessage = "bounceMessage10" });
        }
    }
}
