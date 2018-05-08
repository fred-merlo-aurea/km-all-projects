using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Fakes;
using KMPS_Tools.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;

namespace KMPS_Tools.Test
{
    /// <summary>
    /// Unit tests for <see cref="BPALogFix"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BPALogFixTest
    {
        private const int BlastId = 1;
        private const string EmailFrom = "test@test.com";
        private const int IssueId = 3;
        private const int EmailSendId = 250;
        private const string Subject = "Subject";
        private const string ConnectionStringName = "ECNCommunicator";
        private const string ReadDataFromBpaFile = "ReadDatafromBPAFile";
        private const string FileName = "\\1_2008_3_21.txt";
        private const string OpenFile = "openFileDialog1";
        private const string TestFileName = @"1_fileName";
        private const string BtnCloseClickMethodName = "btnClose_Click";
        private const string BtnFileLocationClickMethodName = "btnFileLocation_Click";
        private const string BpaLogFixFormClosingMethodName = "BPALogFix_FormClosing";
        private const string TextFolderLocationFieldName = "txtFolderLocation";
        private const string TestPath = "c:\bla\\mytestpath";
        private static readonly DateTime SendTime = new DateTime(2008, 3, 21, 0, 0, 0);

        private BPALogFix _bpaLogFix;
        private IDisposable _context;
        private Dictionary<string, string> _bounces;
        private Dictionary<string, int> _sends;
        private IList<string> _lines;

        private TextBox txtBPAFileName;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            _bpaLogFix = new BPALogFix();

            SetupFakes();

            _bounces = new Dictionary<string, string>();
            _sends = new Dictionary<string, int>
            {
                [EmailFrom] = 1
            };

            txtBPAFileName = (TextBox)_bpaLogFix.GetFieldValue(nameof(txtBPAFileName));
            txtBPAFileName.Text = TestFileName;
            _bpaLogFix.SetField(nameof(txtBPAFileName), txtBPAFileName);

            _lines = new List<string>
            {
                $"MSGID[{BlastId}] ISSUE[{IssueId}] TO: {EmailFrom}",
                string.Empty,
                $"MSGID[{BlastId}] ISSUE[{IssueId}] TO: {EmailFrom}",
                string.Empty,
                $"MSGID[{BlastId}] ISSUE[{IssueId}] {EmailSendId}",
                string.Empty
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void ReadDatafromBPAFile_WhenCalled_VerifyCorrectEmailDataWriteToFile()
        {
            // Arrange
            StringBuilder expected = new StringBuilder();
            expected.Append($"MSGID[{BlastId}] ISSUE[{IssueId}] DATE: {SendTime:ddd MMM dd HH:mm:ss.fff yyyy}");
            expected.Append($"MSGID[{BlastId}] ISSUE[{IssueId}] FROM: {EmailFrom}");
            expected.Append($"MSGID[{BlastId}] ISSUE[{IssueId}] TO: {EmailFrom}");
            expected.Append($"MSGID[{BlastId}] ISSUE[{IssueId}] SUBJECT: {Subject}");
            expected.Append($"MSGID[{BlastId}] ISSUE[{IssueId}] {EmailSendId} Email Sent Successfully to server [{EmailSendId}]");

            // Act
            StringBuilder result = new StringBuilder();
            ShimTextWriter.AllInstances.WriteLineString = (_, x) =>
            {
                result.Append(x);
            };

            ReflectionHelper.CallMethod(_bpaLogFix, ReadDataFromBpaFile);

            // Assert
            result.ToString()
                .ShouldBe(expected.ToString());
        }

        [Test]
        public void btnCloseClick_CorrectParameters_MenusToggled()
        {
            // Arrange
            var actualFormCloseCalled = false;
            var actualToggleMenusCalled = false;

            _bpaLogFix.MdiParent = new Main();

            ShimMain.AllInstances.ToggleMenusBoolean = (main, show) =>
            {
                actualToggleMenusCalled = show;
            };

            ShimForm.AllInstances.Close = form => { actualFormCloseCalled = true; };

            // Act
            ReflectionHelper.CallMethod(_bpaLogFix, BtnCloseClickMethodName, null, null);

            // Assert
            actualToggleMenusCalled.ShouldSatisfyAllConditions(
                () => actualToggleMenusCalled.ShouldBeTrue(),
                () => actualFormCloseCalled.ShouldBeTrue());
        }

        [Test]
        public void btnFileLocationClick_DialogResultOk_FolderTextSet()
        {
            // Arrange
            ShimCommonDialog.AllInstances.ShowDialog = dialog => DialogResult.OK;
            ShimFolderBrowserDialog.AllInstances.SelectedPathGet = dialog => TestPath;

            // Act
            ReflectionHelper.CallMethod(_bpaLogFix, BtnFileLocationClickMethodName, null, null);
            var txtFolderLocation = ((TextBox)_bpaLogFix.GetField(TextFolderLocationFieldName)).Text;

            // Assert
            txtFolderLocation.ShouldSatisfyAllConditions(
                () => txtFolderLocation.ShouldBe(TestPath));
        }
        
        [Test]
        public void btnFileLocationClick_DialogResultCancel_FolderTextSet()
        {
            // Arrange
            ShimCommonDialog.AllInstances.ShowDialog = dialog => DialogResult.Cancel;
            ShimFolderBrowserDialog.AllInstances.SelectedPathGet = dialog => TestPath;

            // Act
            ReflectionHelper.CallMethod(_bpaLogFix, BtnFileLocationClickMethodName, null, null);
            var txtFolderLocation = ((TextBox)_bpaLogFix.GetField(TextFolderLocationFieldName)).Text;

            // Assert
            txtFolderLocation.ShouldSatisfyAllConditions(
                () => txtFolderLocation.ShouldBe(string.Empty));
        }

        [Test]
        public void BPALogFixFormClosing_Called_ClosesForm()
        {
            // Arrange
            var actualToggleMenusCalled = false;

            _bpaLogFix.MdiParent = new Main();

            ShimMain.AllInstances.ToggleMenusBoolean = (main, show) =>
            {
                actualToggleMenusCalled = show;
            };

            ShimForm.AllInstances.Close = form => throw new ArgumentOutOfRangeException();

            // Act
            ReflectionHelper.CallMethod(_bpaLogFix, BpaLogFixFormClosingMethodName, null, null);

            // Assert
            actualToggleMenusCalled.ShouldSatisfyAllConditions(
                () => actualToggleMenusCalled.ShouldBeTrue());
        }

        private void SetupFakes()
        {
            SetupBlastFakes();
            SetupFileFakes();

            ShimConfigurationManager.ConnectionStringsGet = () => new ConnectionStringSettingsCollection
            {
                new ConnectionStringSettings(ConnectionStringName, string.Empty)
            };
        }

        private void SetupFileFakes()
        {
            ShimStreamReader.ConstructorString = (_, fileName) =>
            {
                fileName.ShouldBe(OpenFile);
            };

            ShimStreamReader.AllInstances.ReadLine = _ =>
            {
                var line = _lines.FirstOrDefault();
                if (line != null)
                {
                    _lines.RemoveAt(0);
                }

                return line;
            };

            ShimFile.ExistsString = fileName =>
            {
                fileName.ShouldBe(FileName);
                return true;
            };

            ShimFile.DeleteString = fileName =>
            {
                fileName.ShouldBe(FileName);
            };

            ShimStreamWriter.Constructor = writer => { };

            ShimStreamWriter.AllInstances.Close = _ => { };
        }

        private void SetupBlastFakes()
        {
            ShimBlast.getBlastDetailsInt32 = id =>
            {
                id.ShouldBe(BlastId);
                return new Blast
                {
                    BlastID = BlastId,
                    EmailFrom = EmailFrom,
                    IssueID = IssueId,
                    Subject = Subject,
                    sendTime = SendTime
                };
            };

            ShimBlast.AllInstances.getBouncesInt32 = (_, id) =>
            {
                id.ShouldBe(BlastId);
                return _bounces;
            };

            ShimBlast.AllInstances.getSendsInt32 = (_, id) =>
            {
                id.ShouldBe(BlastId);
                return _sends;
            };
        }
    }
}
