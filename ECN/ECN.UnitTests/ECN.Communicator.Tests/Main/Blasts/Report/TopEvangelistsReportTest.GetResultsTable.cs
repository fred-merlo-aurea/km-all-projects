using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Blasts.Report
{
    public partial class TopEvangelistsReportTest
    {
        private const string CbxFacebookControl = "cbxFacebook";
        private const string CbxTwitterControl="cbxTwitter";
        private const string CbxLinkedInControl = "cbxLinkedIn";
        private const string ErrorPlaceHolder = "phError";
        private const string ErrorMessageLabel = "lblErrorMessage";
        private const string CbxForwardToFriendControl = "cbxForwardToFriend";
        private const string EmailIDColumn = "EmailID";
        private const string FacebookColumn = "Facebook";
        private const string TwitterColumn = "Twitter";
        private const string LinkedInColumn = "LinkedIn";
        private const string FriendSharesColumn = "FriendShares";
        private const string EmailAddressColumn = "EmailAddress";
        private const string FirstNameColumn = "FirstName";
        private const string LastNameColumn = "LastName";
        private const string FixSubNameColumn = "FixSubName";
        private const string MergeCtrlColumn = "MergeCtrl";
        private const string TotalNumberOfSharesColumn = "TotalNumberOfShares";
        private const string SampleEmailAddress = "test@test.com";
        private const string SampleFirstName = "SampleFirstName";
        private const string SampleLastName = "SSampleLastName";
        private const string SampleSubName = "SampleSubName";
        private const string CampaignItemDropDown = "ddlCampaignItem";
        private const string GetResultsTableMethodName = "GetResultsTable";

        [Test]
        public void GetResultsTable_WhenAllCheckboxAreChecked_ReturnsResultTable()
        {
            // Arrange
            SetPageControls();
            SetFakesForGetResultsTable();

            // Act
            var resultTable = _privateTestObject.Invoke(GetResultsTableMethodName) as DataTable ;

            // Assert
            resultTable.ShouldSatisfyAllConditions(
                () => resultTable.ShouldNotBeNull(),
                () => resultTable.Rows.Count.ShouldBe(1),
                () => resultTable.Rows[0][TotalNumberOfSharesColumn].ShouldBe("4"),
                () => resultTable.Rows[0][FirstNameColumn].ShouldBe(SampleFirstName),
                () => resultTable.Rows[0][LastNameColumn].ShouldBe(SampleLastName),
                () => resultTable.Rows[0][FixSubNameColumn].ShouldBe(SampleSubName),
                () => resultTable.Rows[0][EmailAddressColumn].ShouldBe(SampleEmailAddress));
        }

        [Test]
        [TestCase(CbxFacebookControl, FacebookColumn)]
        [TestCase(CbxTwitterControl, TwitterColumn)]
        [TestCase(CbxLinkedInControl, LinkedInColumn)]
        [TestCase(CbxForwardToFriendControl, FriendSharesColumn)]
        public void GetResultsTable_WhenCheckBoxControlIsNotChecked_ColumnRemovedFromTable(string controlName, string columnName)
        {
            // Arrange
            SetPageControls();
            SetFakesForGetResultsTable();
            Get<CheckBox>(_privateTestObject, controlName).Checked = false;

            // Act
            var resultTable = _privateTestObject.Invoke(GetResultsTableMethodName) as DataTable;

            // Assert
            resultTable.ShouldSatisfyAllConditions(
                () => resultTable.ShouldNotBeNull(),
                () => resultTable.Rows.Count.ShouldBe(1),
                () => resultTable.Columns.Contains(columnName).ShouldBeFalse(),
                () => resultTable.Rows[0][TotalNumberOfSharesColumn].ShouldBe("3"),
                () => resultTable.Rows[0][FirstNameColumn].ShouldBe(SampleFirstName),
                () => resultTable.Rows[0][LastNameColumn].ShouldBe(SampleLastName),
                () => resultTable.Rows[0][FixSubNameColumn].ShouldBe(SampleSubName),
                () => resultTable.Rows[0][EmailAddressColumn].ShouldBe(SampleEmailAddress));
        }

        [Test]
        public void GetResultsTable_WhenNoCheckBoxIsChecked_ThrowsECNException()
        {
            // Arrange
            SetPageControls(isChecked: false);
            SetFakesForGetResultsTable();

            // Act
            var resultTable = _privateTestObject.Invoke(GetResultsTableMethodName);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => resultTable.ShouldBeNull(),
                () => Get<PlaceHolder>(_privateTestObject,ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.
                            ShouldContain("Social Media type must be selected"));
        }

        private void SetFakesForGetResultsTable()
        {
            ShimTopEvangelistsLists.GetInt32 = (_) => GetDataTable();
        }

        private void SetPageControls(bool isChecked = true)
        {
            var ddlCampaignItem = Get<DropDownList>(_privateTestObject, CampaignItemDropDown);
            ddlCampaignItem.Items.Add(new ListItem { Value = "1" });
            
            Get<CheckBox>(_privateTestObject, CbxFacebookControl).Checked = isChecked;
            Get<CheckBox>(_privateTestObject, CbxTwitterControl).Checked = isChecked;
            Get<CheckBox>(_privateTestObject, CbxLinkedInControl).Checked = isChecked;
            Get<CheckBox>(_privateTestObject, CbxForwardToFriendControl).Checked = isChecked;

        }

        private DataTable GetDataTable()
        {
            var mergeControlValues = new[] { "Email_Share", "SocialNetwork_Share" };

            var dataTable = new DataTable();
            dataTable.Columns.Add(EmailIDColumn, typeof(int));
            dataTable.Columns.Add(FacebookColumn, typeof(int));
            dataTable.Columns.Add(TwitterColumn, typeof(int));
            dataTable.Columns.Add(LinkedInColumn, typeof(int));
            dataTable.Columns.Add(FriendSharesColumn, typeof(int));
            dataTable.Columns.Add(EmailAddressColumn, typeof(string));
            dataTable.Columns.Add(FirstNameColumn, typeof(string));
            dataTable.Columns.Add(LastNameColumn, typeof(string));
            dataTable.Columns.Add(FixSubNameColumn, typeof(string));
            dataTable.Columns.Add(MergeCtrlColumn, typeof(string));

            foreach (var ctrlValue in mergeControlValues)
            {
                var row = dataTable.NewRow();
                row[EmailIDColumn] = 1;
                row[MergeCtrlColumn] = ctrlValue;
                row[FriendSharesColumn] = 1;
                row[EmailAddressColumn] = SampleEmailAddress;
                row[FirstNameColumn] = SampleFirstName;
                row[LastNameColumn] = SampleLastName;
                row[FixSubNameColumn] = SampleSubName;
                row[FacebookColumn] = 1;
                row[TwitterColumn] = 1;
                row[LinkedInColumn] = 1;

                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
    }
}
