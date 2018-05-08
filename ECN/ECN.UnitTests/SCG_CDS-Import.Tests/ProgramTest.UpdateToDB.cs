using System;
using System.Configuration;
using System.Configuration.Fakes;
using System.Data;
using System.Data.Fakes;
using System.Text;
using NUnit.Framework;
using Shouldly;
using ECN.TestHelpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_Entities.Accounts;
using SCG_CDS_Import.Fakes;

namespace SCG_CDS_Import.Tests
{
    public partial class ProgramTest
    {
        private const string WriteXmlToLog = "WriteXMLtoLog";
        private const string False = "False";
        private const string MethodUpdateToDB = "UpdateToDb";
        public readonly string ExpectedNullReferenceException = "-- Message --" + Environment.NewLine +
                                                                "Object reference not set to an instance of an object." + Environment.NewLine +
                                                                "-- Stack Trace --" + Environment.NewLine;
        
        [Test]
        public void UpdateToDB_CustomerGetRaisesException_ConstructCorrectString()
        {
            // Arrange
            ShimProgram.UpdateToDbStringStringImportFile = null;
            ShimConfigurationManager.AppSettingsGet = null;

            var builder = new StringBuilder();
            ShimProgram.WriteToImportFileLogString = s => { builder.Append(s); };
            ConfigurationManager.AppSettings[WriteXmlToLog] = False;

            // Act
            typeof(Program).CallMethod(MethodUpdateToDB, new object[] {string.Empty, string.Empty, new ImportFile()});

            // Assert
            builder.ToString().ShouldContain(ExpectedNullReferenceException);
        }

        [Test]
        public void UpdateToDB_DataRowRaiseException_ConstructCorrectString()
        {
            // Arrange
            ShimProgram.UpdateToDbStringStringImportFile = null;
            ShimConfigurationManager.AppSettingsGet = null;

            var builder = new StringBuilder();
            ShimProgram.WriteToImportFileLogString = s => { builder.Append(s); };
            ShimCustomer.GetByCustomerIDInt32Boolean = (a, b) => new Customer {BaseChannelID = 1};
            ShimBaseChannel.GetByBaseChannelIDInt32 = (a) => new BaseChannel();
            ECN_Framework_DataLayer.Communicator.Fakes.ShimEmailGroup.ImportEmailsInt32Int32Int32StringStringStringStringBooleanStringString =
                (a,b,c,d,e,f,g,h,i,j) => new DataTable();

            ShimDataTable.AllInstances.RowsGet = _ => throw new NullReferenceException();
            ConfigurationManager.AppSettings[WriteXmlToLog] = False;

            // Act
            typeof(Program).CallMethod(MethodUpdateToDB, new object[] {string.Empty, string.Empty, new ImportFile()});

            // Assert
            builder.ToString().ShouldContain(ExpectedNullReferenceException);
        }
    }
}
