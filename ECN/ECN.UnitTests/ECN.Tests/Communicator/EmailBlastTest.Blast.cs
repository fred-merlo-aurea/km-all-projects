using System.Collections.Generic;
using System.Configuration.Fakes;
using System.Data;
using ecn.common.classes.Fakes;
using ecn.communicator.classes.EmailWriter;
using ecn.communicator.classes.Fakes;
using ecn.communicator.classes.Port25.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Communicator
{
    [TestFixture]
    public partial class EmailBlastTest
    {
        private DataTable _dataTableOfEmails;
        private SocialMedia _socialMedia;
        private const string EmailIDColumn = "EmailID";
        private const string EmailIDValue = "11";
        private const string PersonalizedContentIDColumn = "PersonalizedContentID";
        private const string FormatTypeCodeColumn = "FormatTypeCode";
        private const string EmailAddressColumn = "EmailAddress";
        private const string BounceAddressColumn = "BounceAddress";
        private const string MailRouteColumn = "MailRoute";
        private const string GroupIDColumn = "GroupID";
        private const string HtmlToMatchTransnippetRegex = @"<table id='transnippet_val'><tr id='header'></tr><tr id='detail'></tr></table>";
        private const string HtmlToMatchTransnippetHashTagRegex = "##|test,###|HDR-STYLE=,##TRANSNIPPET|,##TBL-STYLE=|##";
        private const string EmptyValue = "";
        private const string SocialMediaMatchString= "#{2}.*#{2}";
        private const int SocialMediaID=21;

        [Test]
        public void Blast_BreakupFilling_WithSucces()
        {
            //Arrange
            InitializeBlastDependenciesWithShims();
            _emailBlast.EmailsTable = _dataTableOfEmails;

            //Act
            _emailBlast.Blast();

            //Assert
            _emailBlast.ShouldSatisfyAllConditions
                (
                    () =>_emailBlast.BreakupHTMLMail.ShouldNotBeNull(),
                    () => _emailBlast.BreakupSubject.ShouldNotBeNull(),
                    () => _emailBlast.BreakupFromName.ShouldNotBeNull(),
                    () => _emailBlast.BreakupFromEmail.ShouldNotBeNull(),
                    () => _emailBlast.breakupReplyToEmail.ShouldNotBeNull()                
                );
        }

        [Test]
        public void Blast_BreakupFilling_ForSocialShareContent_HTMLMatchWithSucces()
        {
            //Arrange
            InitializeBlastDependenciesWithShims();
            _emailBlast.EmailsTable = _dataTableOfEmails;
            _emailBlast.HTML = HtmlToMatchTransnippetHashTagRegex;

            ShimSocialMedia.GetSocialMediaCanShare =() =>
                {
                    return new List<SocialMedia>() { _socialMedia };
                };

            //Act
            _emailBlast.Blast();

            //Assert
            _emailBlast.SocialShareUsed.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void Blast_SocialShareContentFilling_TextMatchWithSucces()
        {
            //Arrange
            InitializeBlastDependenciesWithShims();
            _emailBlast.EmailsTable = _dataTableOfEmails;
            _emailBlast.TEXT = HtmlToMatchTransnippetHashTagRegex;

            ShimSocialMedia.GetSocialMediaCanShare = () =>
                {
                    return new List<SocialMedia>() { _socialMedia };
                };

            //Act
            _emailBlast.Blast();

            //Assert
            _emailBlast.SocialShareUsed.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void Blast_TextFilling_MatchingRextWithSucces()
        {
            //Arrange
            InitializeBlastDependenciesWithShims();
            _emailBlast.EmailsTable = _dataTableOfEmails;
            _emailBlast.HTML = HtmlToMatchTransnippetRegex;            

            //Act
            _emailBlast.Blast();

            //Assert
            _emailBlast.TEXT.ShouldNotBeEmpty();
        }

        [Test]
        public void Blast_TransnippetHolderFilling_WithSucces()
        {
            //Arrange
            InitializeBlastDependenciesWithShims();
            _emailBlast.EmailsTable = _dataTableOfEmails;
            _emailBlast.HTML = HtmlToMatchTransnippetHashTagRegex;

            //Act
            _emailBlast.Blast();

            //Assert
            TransnippetHolder.TransnippetsCount.ShouldBeGreaterThan(0);
            TransnippetHolder.TransnippetTablesHTML.Count.ShouldBeGreaterThan(0);
            TransnippetHolder.TransnippetTablesTxt.Count.ShouldBeGreaterThan(0);
        }


        private void InitializeBlastDependenciesWithShims()
        {
            ShimDataFunctions.ExecuteScalarString =
                (sql) =>
                {
                    return CustomerId;
                };
            
            ShimLoggingFunctions.LogStatistics =
                () => { return true; };
                        
            KM.Common.Fakes.ShimFileFunctions.LogActivityBooleanStringString =
                (outptToConsole, activity, suffix) => { };

            ShimSocialMedia.GetSocialMediaCanShare =
                () =>
                {
                    return new List<SocialMedia>();
                };

            ShimConfigurationManager.AppSettingsGet = () => { return CreateAppSettings(bool.TrueString.ToLower()); };

            ShimEmailMessageProvider.AllInstances.WriteEmailMessageDataRowStringStringStringStringStringBlastConfig =
                (provider,dr, dynamic_subject, message_id, text_body, boundry_tag, html_body, blastconfig) =>
                {
                    return EmptyValue;
                };

            ShimThreadCoordinator.ConstructorInt32BooleanStringInt32 = 
                (port,blastID, testBlast, smtp, mta) => 
                {
                    var shim = new ShimThreadCoordinator(port);
                    shim.RunThreads = () => { };
                };

            _socialMedia = new SocialMedia()
            {
                MatchString = SocialMediaMatchString,
                SocialMediaID = SocialMediaID
            };

            _dataTableOfEmails = new DataTable();
            _dataTableOfEmails.Columns.Add(EmailIDColumn);
            _dataTableOfEmails.Columns.Add(PersonalizedContentIDColumn);
            _dataTableOfEmails.Columns.Add(FormatTypeCodeColumn);
            _dataTableOfEmails.Columns.Add(EmailAddressColumn);
            _dataTableOfEmails.Columns.Add(BounceAddressColumn);
            _dataTableOfEmails.Columns.Add(MailRouteColumn);
            _dataTableOfEmails.Columns.Add(GroupIDColumn);
            _dataTableOfEmails.Rows.Add(EmailIDValue, EmptyValue,FormatTypeCodeColumn,EmailAddressColumn,BounceAddressColumn, MailRouteColumn,GroupIDColumn);

            var transnippet = new List<string>();
            TransnippetHolder.Transnippet = transnippet;
            TransnippetHolder.TransnippetTablesHTML = transnippet;
            TransnippetHolder.TransnippetTablesTxt = transnippet;
        }
    }
}
