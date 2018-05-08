using System.Collections.Generic;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml;
using ECN.Framework.BusinessLayer.Helpers;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_DataLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using KMPlatform.Entity;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class EmailGroupsTest
    {
        private PrivateType _testedClass;
        private const string TestedClassName = "ECN_Framework_BusinessLayer.Communicator.EmailGroup";
        private const string TestedClassAssemblyName = "ECN_Framework_BusinessLayer";
        private const string TestedMethod_ImportEmailToGroups = "ImportEmailToGroups";
        private const string TestedMethod_ImportEmailToGroups_NoAccessCheck = "ImportEmailToGroups_NoAccessCheck";
        private const string Groups = "Groups";
        private const string EmailValue = "Email";
        private const string EmailMarketing = "EMAILMARKETING";
        private const int UserId = 1;
        private const string ValidXml = "<ImportData><Group emailAddress='abc@test.com'><Data customer='1'/></Group></ImportData>";

        [SetUp]
        public void TestInitialize()
        {
            _testedClass = new PrivateType(TestedClassAssemblyName, TestedClassName);
            SetupShims();
        }

        [Test]
        public void ImportEmailToGroups_InvalidData_ShouldThrowException()
        {
            // Act, Assert
            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ImportEmailToGroups, new object[] { "", UserId });
            });

            exception.InnerException.ShouldBeOfType<XmlException>();
        }

        [Test]
        public void ImportEmailToGroups_ValidXml_ShouldNotThrowException()
        {
            // Act, Assert
            Should.NotThrow(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ImportEmailToGroups, new object[] { ValidXml, UserId });
            });
        }

        [Test]
        public void ImportEmails_NoAccessCheck_InvalidData_ShouldThrowException()
        {
            // Act, Assert
            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ImportEmailToGroups_NoAccessCheck, new object[] { "", UserId });
            });

            exception.InnerException.ShouldBeOfType<XmlException>();
        }

        [Test]
        public void ImportEmails_NoAccessCheck_ValidXml_ShouldNotThrowException()
        {
            // Act, Assert
            Should.NotThrow(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ImportEmailToGroups_NoAccessCheck, new object[] { ValidXml, UserId });
            });
        }

        private void SetupShims()
        {
            var serviceFeatures = new List<ServiceFeature>();
            var serviceFeature = new ServiceFeature
            {
                SFCode = Groups
            };
            serviceFeatures.Add(serviceFeature);
            serviceFeature = new ServiceFeature
            {
                SFCode = EmailValue
            };
            serviceFeatures.Add(serviceFeature);

            var services = new List<Service>();
            var service = new Service
            {
                ServiceCode = EmailMarketing,
                ServiceFeatures = serviceFeatures
            };
            services.Add(service);

            var user = new User
            {
                IsActive = true,
                IsPlatformAdministrator = true,
                CurrentClient = new Client
                {
                    Services = services
                }
            };

            ECN_Framework_BusinessLayer.Communicator.AccessCheck.Initialize(new UserAdapter(), new CustomerAdapter());
            ShimUser.GetByUserIDInt32Boolean = (_, __) => user;
            ShimSqlConnection.AllInstances.Close = (_) => { };
            ShimSqlConnection.AllInstances.DisposeBoolean = (_, __) => { };
            ShimSqlCommand.AllInstances.ConnectionGet = (_) => new ShimSqlConnection();
            ShimDataFunctions.ExecuteReaderSqlCommandString = (_, __) => new ShimSqlDataReader();
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (_, __) => true;
            ShimGroup.GetMasterSuppressionGroupInt32 = (_) => new Group();
        }
    }
}
