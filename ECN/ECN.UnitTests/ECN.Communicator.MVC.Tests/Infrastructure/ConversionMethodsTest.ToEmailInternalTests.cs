using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using ecn.communicator.mvc.Infrastructure;
using Shouldly;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;

namespace ECN.Communicator.MVC.Tests
{
    public partial class ConversionMethodsTest
    {
        private const string SampleEmailAddress = "sampleEmailAddress";
        private const string SampleTitle = "sampleTitle";
        private const string SampleFirstName = "sampleFirstName";
        private const string SampleLastName = "sampleLastName";
        private const string SampleFullName = "sampleFullName";
        private const string SampleCompany = "sampleCompany";
        private const string SampleOccupation = "sampleOccupation";
        private const string SampleAddress = "sampleAddress";
        private const string SampleAddress2 = "sampleAddress2";
        private const string SampleCity = "sampleCity";
        private const string SampleState = "sampleState";
        private const string SampleZip = "sampleZip";
        private const string SampleCountry = "sampleCountry";
        private const string SampleVoice = "sampleVoice";
        private const string SampleMobile = "sampleMobile";
        private const string SampleFax = "sampleFax";
        private const string SampleWebsite = "sampleWebsite";
        private const string SampleAge = "sampleAge";
        private const string SampleIncome = "sampleIncome";
        private const string SampleGender = "sampleGender";
        private const string SampleUser1 = "sampleUser1";
        private const string SampleUser2 = "sampleUser2";
        private const string SampleUser3 = "sampleUser3";
        private const string SampleUser4 = "sampleUser4";
        private const string SampleUser5 = "sampleUser5";
        private const string SampleUser6 = "sampleUser6";
        private const string SampleUserEvent1 = "sampleUserEvent1";
        private const string SampleUserEvent2 = "sampleUserEvent2";
        private const string SamplePassword = "samplePassword";
        private const string SampleBirthdate = "2018.2.1";
        private const string SampleUserEvent1Date = "2018.2.1";
        private const string SampleUserEvent2Date = "2018.2.1";
        private const string SampleFormatTypeCode = "sampleFormatTypeCode";
        private const string SampleSubscribeTypeCode = "sampleSubscribeTypeCode";
        private const string SampleNotes = "sampleNotes";
        private const int SampleBounceScore = 1;
        private const int SampleSoftBounceScore = 1;
        private const int SampleEmailId = 1;
        private const string SampleDummy = "Dummy";
        private const string ExceptionMessageFormatBirthdate = "Bad Birthdate format";
        private const string ExceptionMessageFormatUserEvent1Date = "Bad UserEvent1Date format";
        private const string ExceptionMessageFormatUserEvent2Date = "Bad UserEvent2Date format";
        private const string ExceptionMessageEmailRequired = "Email address cannot be empty.";

        [Test]
        public void ToEmail_Internal_CheckOneToOne_NoException()
        {
            // Arrange
            var inputMail = CreateSampleEmail();
            var inputUser = new KMPlatform.Entity.User();
            ShimEmail.GetByEmailIDInt32User = (p1, p2) => new Email();

            // Act
            var internalEmail = ConversionMethods.ToEmail_Internal(inputMail, inputUser);

            // Assert
            internalEmail.ShouldNotBeNull();
            CheckInternalEmail(internalEmail);           
        }

        [Test]
        public void ToEmail_Internal_CheckEmptyMail_NoException()
        {
            // Arrange
            var inputMail = new ecn.communicator.mvc.Models.Email();
            inputMail.EmailAddress = SampleEmailAddress;
            inputMail.EmailID = SampleEmailId;
            var inputUser = new KMPlatform.Entity.User();
            ShimEmail.GetByEmailIDInt32User = (p1, p2) => new Email();

            // Act
            var internalEmail = ConversionMethods.ToEmail_Internal(inputMail, inputUser);

            // Assert
            internalEmail.ShouldNotBeNull();
        }
        
        [Test]
        public void ToEmail_Internal_CheckFormatExceptions()
        {
            // Arrange
            var inputMail = new ecn.communicator.mvc.Models.Email();
            inputMail.Birthdate = SampleDummy;
            inputMail.UserEvent1Date = SampleDummy;
            inputMail.UserEvent2Date = SampleDummy;
            var inputUser = new KMPlatform.Entity.User();
            ShimEmail.GetByEmailIDInt32User = (p1, p2) => new Email();

            // Act
            try
            {
                var internalEmail = ConversionMethods.ToEmail_Internal(inputMail, inputUser);

            // Assert
                internalEmail.ShouldBeNull();
            }
            catch (ECNException ex)
            {
                ex.ErrorList.ShouldNotBeNull();
                ex.ErrorList.Count.ShouldBe(4);
                ex.ErrorList[0].ErrorMessage.ShouldBe(ExceptionMessageFormatBirthdate);
                ex.ErrorList[1].ErrorMessage.ShouldBe(ExceptionMessageFormatUserEvent1Date);
                ex.ErrorList[2].ErrorMessage.ShouldBe(ExceptionMessageFormatUserEvent2Date);
                ex.ErrorList[3].ErrorMessage.ShouldBe(ExceptionMessageEmailRequired);
            }
        }

        private ecn.communicator.mvc.Models.Email CreateSampleEmail()
        {
            var sampleMail = new ecn.communicator.mvc.Models.Email();
            sampleMail.EmailAddress = SampleEmailAddress;
            sampleMail.Title = SampleTitle;
            sampleMail.FirstName = SampleFirstName;
            sampleMail.LastName = SampleLastName;
            sampleMail.FullName = SampleFullName;
            sampleMail.Company = SampleCompany;
            sampleMail.Occupation = SampleOccupation;
            sampleMail.Address = SampleAddress;
            sampleMail.Address2 = SampleAddress2;
            sampleMail.City = SampleCity;
            sampleMail.State = SampleState;
            sampleMail.Zip = SampleZip;
            sampleMail.Country = SampleCountry;
            sampleMail.Voice = SampleVoice;
            sampleMail.Mobile = SampleMobile;
            sampleMail.Fax = SampleFax;
            sampleMail.Website = SampleWebsite;
            sampleMail.Age = SampleAge;
            sampleMail.Income = SampleIncome;
            sampleMail.Gender = SampleGender;
            sampleMail.User1 = SampleUser1;
            sampleMail.User2 = SampleUser2;
            sampleMail.User3 = SampleUser3;
            sampleMail.User4 = SampleUser4;
            sampleMail.User5 = SampleUser5;
            sampleMail.User6 = SampleUser6;
            sampleMail.UserEvent1 = SampleUserEvent1;
            sampleMail.UserEvent2 = SampleUserEvent2;
            sampleMail.Password = SamplePassword;
            sampleMail.Birthdate = SampleBirthdate;
            sampleMail.UserEvent1Date = SampleUserEvent1Date;
            sampleMail.UserEvent2Date = SampleUserEvent2Date;
            sampleMail.FormatTypeCode = SampleFormatTypeCode;
            sampleMail.SubscribeTypeCode = SampleSubscribeTypeCode;
            sampleMail.Notes = SampleNotes;
            sampleMail.BounceScore = SampleBounceScore;
            sampleMail.SoftBounceScore = SampleSoftBounceScore;
            sampleMail.EmailID = SampleEmailId;

            return sampleMail;
        }

        private void CheckInternalEmail(Email internalEmail)
        {
            internalEmail.EmailAddress.ShouldBe(SampleEmailAddress);
            internalEmail.Title.ShouldBe(SampleTitle);
            internalEmail.FirstName.ShouldBe(SampleFirstName);
            internalEmail.LastName.ShouldBe(SampleLastName);
            internalEmail.FullName.ShouldBe(SampleFullName);
            internalEmail.Company.ShouldBe(SampleCompany);
            internalEmail.Occupation.ShouldBe(SampleOccupation);
            internalEmail.Address.ShouldBe(SampleAddress);
            internalEmail.Address2.ShouldBe(SampleAddress2);
            internalEmail.City.ShouldBe(SampleCity);
            internalEmail.State.ShouldBe(SampleState);
            internalEmail.Zip.ShouldBe(SampleZip);
            internalEmail.Country.ShouldBe(SampleCountry);
            internalEmail.Voice.ShouldBe(SampleVoice);
            internalEmail.Mobile.ShouldBe(SampleMobile);
            internalEmail.Fax.ShouldBe(SampleFax);
            internalEmail.Website.ShouldBe(SampleWebsite);
            internalEmail.Age.ShouldBe(SampleAge);
            internalEmail.Income.ShouldBe(SampleIncome);
            internalEmail.Gender.ShouldBe(SampleGender);
            internalEmail.User1.ShouldBe(SampleUser1);
            internalEmail.User2.ShouldBe(SampleUser2);
            internalEmail.User3.ShouldBe(SampleUser3);
            internalEmail.User4.ShouldBe(SampleUser4);
            internalEmail.User5.ShouldBe(SampleUser5);
            internalEmail.User6.ShouldBe(SampleUser6);
            internalEmail.UserEvent1.ShouldBe(SampleUserEvent1);
            internalEmail.UserEvent2.ShouldBe(SampleUserEvent2);
            internalEmail.Password.ShouldBe(SamplePassword);
            internalEmail.Birthdate.ShouldBe(DateTime.Parse(SampleBirthdate));
            internalEmail.UserEvent1Date.ShouldBe(DateTime.Parse(SampleUserEvent1Date));
            internalEmail.UserEvent2Date.ShouldBe(DateTime.Parse(SampleUserEvent2Date));
            internalEmail.FormatTypeCode.ShouldBe(SampleFormatTypeCode);
            internalEmail.SubscribeTypeCode.ShouldBe(SampleSubscribeTypeCode);
            internalEmail.Notes.ShouldBe(SampleNotes);
            internalEmail.BounceScore.ShouldBe(SampleBounceScore);
            internalEmail.SoftBounceScore.ShouldBe(SampleSoftBounceScore);
        }
    }
}
