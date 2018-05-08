using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Entity;
using ECN.Communicator.Tests.Helpers;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity
{
    /// <summary>
    ///     Unit test for <see cref="SF_User"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_UserTest
    {
        private List<SF_User> users;

        /// <summary>
        ///     Setup up <see cref="users"/> which can be utilized in test scope
        /// </summary>
        [SetUp]
        public void SetUp() => users = new List<SF_User>
        {
            new SF_User(),
            new SF_User(),
            new SF_User()
        };

        [Test]
        public void ConvertJsonList_PassNullStringList_ThrowNullReferenceException()
        {
            // Act
            var exp = Assert.Throws<TargetInvocationException>(() =>
                typeof(SF_User).CallMethod("ConvertJsonList", new object[] {null}));

            // Assert
            Assert.That(exp.InnerException, Is.TypeOf<NullReferenceException>());
        }

        [Test]
        public void ConvertJsonList_PassEmptyStringList_ReturnEmptyUserList()
        {
            // Arrange
            var json = new List<string>();

            // Act
            var result =
                (List<SF_User>) typeof(SF_User).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNonNullValues_VerifyUserList()
        {
            // Arrange
            List<string> json = SfUserJsonWithNonNullValues;
            List<SF_User> expectedList = SfUserListWithNonNullValues;

            // Act
            var result =
                (List<SF_User>) typeof(SF_User).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNullValues_VerifyUserList()
        {
            // Arrange
            List<string> json = SfUserJsonWithNullValues;

            List<SF_User> expectedList = GetSfUserListWithNullValues();

            // Act
            var result =
                (List<SF_User>) typeof(SF_User).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        private static List<string> SfUserJsonWithNonNullValues => new List<string>
        {
            " \"Id\": \"Id\" ,",
            " \"AboutMe\": \"AboutMe\" ,",
            " \"Alias\": \"Alias\" ,",
            " \"City\": \"City\" ,",
            " \"CommunityNickName\": \"CommunityNickName\" ,",
            " \"CompanyName\": \"CompanyName\" ,",
            " \"Country\": \"Country\" ,",
            " \"Department\": \"Department\" ,",
            " \"Email\": \"Email\" ,",
            " \"Fax\": \"Fax\" ,",
            " \"FirstName\": \"FirstName\" ,",
            " \"IsActive\": \"true\" ,",
            " \"LastName\": \"LastName\" ,",
            " \"MobilePhone\": \"MobilePhone\" ,",
            " \"Name\": \"Name\" ,",
            " \"Phone\": \"Phone\" ,",
            " \"PostalCode\": \"PostalCode\" ,",
            " \"State\": \"State\" ,",
            " \"Street\": \"Street\" ,",
            " \"Title\": \"Title\" ,",
            " \"Username\": \"UserName\" ,"
        };

        private static List<SF_User> SfUserListWithNonNullValues => new List<SF_User>
        {
            new SF_User
            {
                Id = "Id",
                AboutMe = "AboutMe",
                Alias = "Alias",
                City = "City",
                CommunityNickName = "CommunityNickName",
                CompanyName = "CompanyName",
                Country = "Country",
                Department = "Department",
                Email = "Email",
                Fax = "Fax",
                FirstName = "FirstName",
                IsActive = true,
                LastName = "LastName",
                MobilePhone = "MobilePhone",
                Name = "Name",
                Phone = "Phone",
                PostalCode = "PostalCode",
                State = "State",
                Street = "Street",
                Title = "Title",
                Username = "UserName"
            }
        };

        private static List<string> SfUserJsonWithNullValues => new List<string>
        {
            " \"Id\": null",
            " \"AboutMe\": null",
            " \"Alias\": null",
            " \"City\": null",
            " \"CommunityNickName\": null",
            " \"CompanyName\": null",
            " \"Country\": null",
            " \"Department\": null",
            " \"Email\": null",
            " \"Fax\": null",
            " \"FirstName\": null",
            " \"IsActive\": false",
            " \"LastName\": null",
            " \"MobilePhone\": null",
            " \"Name\": null",
            " \"Phone\": null",
            " \"PostalCode\": null",
            " \"State\": null",
            " \"Street\": null",
            " \"Title\": null",
            " \"Username\": null"
        };

        private static List<SF_User> GetSfUserListWithNullValues()
        {
            return new List<SF_User>
            {
                new SF_User
                {
                    Id  = "null",
                    AboutMe  = string.Empty,
                    Alias  = string.Empty,
                    City  = string.Empty,
                    CommunityNickName  = string.Empty,
                    CompanyName  = string.Empty,
                    Country  = string.Empty,
                    Department  = string.Empty,
                    Email  = string.Empty,
                    Fax  = string.Empty,
                    FirstName  = string.Empty,
                    IsActive  = false,
                    LastName  = string.Empty,
                    MobilePhone  = string.Empty,
                    Name  = string.Empty,
                    Phone  = string.Empty,
                    PostalCode  = string.Empty,
                    State  = string.Empty,
                    Street  = string.Empty,
                    Title  = string.Empty,
                    Username  = string.Empty,
                }
            };
        }
    }
}