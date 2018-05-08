using System;
using System.Collections.Generic;
using System.Text;
using ecn.common.classes.Fakes;
using ECN_Framework_Common.Objects;
using KM.Common.Entity.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using CommunicatorClasses = ecn.communicator.classes;

namespace ECN.BlastEngine.Tests
{
    public partial class BlastEngineTest
    {
        private const string HandleSingleBlastMethodName = "HandleSingleBlast";
        private StringBuilder _logCriticalExceptionText;

        [Test]
        public void HandleSingleBlast_WhenRefBlastColumnIsNotNull_ExecutesSqlStatements()
        {
            // Arrange
            const int blastId = 1;
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            var exceptionLogged = false;
            var dataTable = GetDataTable();
            SetFakesForHandleBlastMethod();
            appSettings.Add("LogStatistics", bool.TrueString);
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimDataFunctions.GetDataTableString = (cmd) => dataTable;

            // Act
            _privateECNBlastEngineObj.Invoke(HandleSingleBlastMethodName, emailFunctions);

            // Assert
            exceptionLogged.ShouldSatisfyAllConditions(
                () => exceptionLogged.ShouldBeFalse(),
                () => _sqlCommandExceuted.ShouldContain($"exec sp_getBlastSingleEmailStatus {blastId}"),
                () => _sqlCommandExceuted.ShouldContain($"SELECT RefTriggerID FROM TriggerPlans WHERE TriggerPlanID = {dataTable.Rows[0]["LayoutPlanID"]}"),
                () => _sqlCommandExceuted.ShouldContain($"SELECT COUNT(EmailID) AS 'OpensCount'  FROM ECN_ACTIVITY..BlastActivityOpens " +
                                                        $" WHERE BlastID = {blastId} AND EmailID = {dataTable.Rows[0]["EmailID"]}"),
                () => _sqlCommandExceuted.ShouldContain($"DELETE FROM BlastSingles WHERE BlastSingleID = {blastId}"));
        }

        [Test]
        public void HandleSingleBlast_WhenRefBlastColumnIsNotNullAndTriggerIDIsZero_ExecutesSqlStatements()
        {
            // Arrange
            const int blastId = 1;
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            var exceptionLogged = false;
            var dataTable = GetDataTable();
            SetFakesForHandleBlastMethod();
            appSettings.Add("LogStatistics", bool.TrueString);
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimDataFunctions.GetDataTableString = (cmd) => dataTable;
            ShimDataFunctions.ExecuteScalarString = (query) =>
            {
                _sqlCommandExceuted += query;
                if (query.Contains("SELECT RefTriggerID FROM TriggerPlans"))
                {
                    return 0;
                }
                return 1;
            };

            // Act
            _privateECNBlastEngineObj.Invoke(HandleSingleBlastMethodName, emailFunctions);

            // Assert
            exceptionLogged.ShouldSatisfyAllConditions(
                () => exceptionLogged.ShouldBeFalse(),
                () => _sqlCommandExceuted.ShouldContain($"exec sp_getBlastSingleEmailStatus {blastId}"),
                () => _sqlCommandExceuted.ShouldContain($"SELECT RefTriggerID FROM TriggerPlans WHERE TriggerPlanID = {dataTable.Rows[0]["LayoutPlanID"]}"),
                () => _sqlCommandExceuted.ShouldContain($"UPDATE BlastSingles SET StartTime = GETDATE() WHERE " +
                                                            $"BlastSingleID = {dataTable.Rows[0]["BlastSingleID"]}"),
                () => _sqlCommandExceuted.ShouldContain($"Update BlastSingles set Processed='y' , " +
                                                            $"EndTime = GETDATE() where BlastSingleID={dataTable.Rows[0]["BlastSingleID"]}"),
                () => _sqlCommandExceuted.ShouldContain($"Update [Blast] set SendTime=GetDate() where BlastID={blastId}"),
                () => _sqlCommandExceuted.ShouldContain($"select count(SendID) from BlastActivitySends where BlastID={blastId}"),
                () => _sqlCommandExceuted.ShouldContain($"update [blast] set attempttotal=1,successtotal=1,sendtotal=1 where blastID={blastId}"));
        }

        [Test]
        public void HandleSingleBlast_WhenRefBlastColumnIsNotNullAndOpensCountIsZero_ExecutesSqlStatements()
        {
            // Arrange
            const int blastId = 1;
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            var exceptionLogged = false;
            _sqlCommandExceuted = string.Empty;
            var dataTable = GetDataTable();
            SetFakesForHandleBlastMethod();
            appSettings.Add("LogStatistics", bool.TrueString);
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimDataFunctions.GetDataTableString = (cmd) => dataTable;
            ShimDataFunctions.ExecuteScalarString = (query) =>
            {
                _sqlCommandExceuted += query;
                if (query.Contains("SELECT COUNT(EmailID) AS 'OpensCount'"))
                {
                    return 0;
                }
                return 1;
            };

            // Act
            _privateECNBlastEngineObj.Invoke(HandleSingleBlastMethodName, emailFunctions);

            // Assert
            exceptionLogged.ShouldSatisfyAllConditions(
                () => exceptionLogged.ShouldBeFalse(),
                () => _sqlCommandExceuted.ShouldContain($"exec sp_getBlastSingleEmailStatus {blastId}"),
                () => _sqlCommandExceuted.ShouldContain($"SELECT RefTriggerID FROM TriggerPlans WHERE TriggerPlanID = {dataTable.Rows[0]["LayoutPlanID"]}"),
                () => _sqlCommandExceuted.ShouldContain($"UPDATE BlastSingles SET StartTime = GETDATE() WHERE " +
                            $"BlastSingleID = {dataTable.Rows[0]["BlastSingleID"]}"),
                () => _sqlCommandExceuted.ShouldContain($"SELECT COUNT(EmailID) AS 'OpensCount'  FROM ECN_ACTIVITY..BlastActivityOpens  " +
                            $"WHERE BlastID = {blastId} AND EmailID = {dataTable.Rows[0]["EmailID"]}"),
                () => _sqlCommandExceuted.ShouldContain($"SELECT COUNT(EmailID) AS 'OpensCount'  FROM ECN5_Communicator..EmailActivityLog eal " +
                            $"with(nolock)  WHERE eal.BlastID = {blastId} AND EmailID = {dataTable.Rows[0]["EmailID"]} AND ActionTypeCode = 'open'"),
                () => _sqlCommandExceuted.ShouldContain($"Update BlastSingles set Processed='y', " +
                            $"EndTime = GETDATE() where BlastSingleID={dataTable.Rows[0]["BlastSingleID"]}"),
                () => _sqlCommandExceuted.ShouldContain($"Update [Blast] set SendTime=GetDate() where BlastID={blastId}"),
                () => _sqlCommandExceuted.ShouldContain($"select count(SendID) from BlastActivitySends where BlastID={blastId}"),
                () => _sqlCommandExceuted.ShouldContain($"update [blast] set attempttotal=1,successtotal=1,sendtotal=1 where blastID={blastId}"));
        }

        [Test]
        public void HandleSingleBlast_WhenGetDataTableThrowsException_LogsException()
        {
            // Arrange
            SetFakesForHandleBlastMethod();
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            var exceptionLogged = false;
            _logCriticalExceptionText = new StringBuilder();
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _logCriticalExceptionText.AppendLine(note);
                    exceptionLogged = true;
                    return 1;
                };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                throw new InvalidOperationException(UTException);
            };

            // Act
            _privateECNBlastEngineObj.Invoke(HandleSingleBlastMethodName, emailFunctions);

            // Assert
            exceptionLogged.ShouldSatisfyAllConditions(
                () => exceptionLogged.ShouldBeTrue(),
                () => _logCriticalExceptionText.ToString().ShouldContain($"An exception Happened when handling Blast Engine ID = {engineID} "+
                                                                            $"and Blast Single ID = {int.MinValue}"),
                () => _logCriticalExceptionText.ToString().ShouldContain($"Exception Message: {UTException}"),
                () => _sqlCommandExceuted.ShouldNotBeNullOrWhiteSpace(),
                () => _sqlCommandExceuted.ShouldContain("Update [BlastSingles] set Processed='e' where BlastSingleID = "));
        }

        [Test]
        public void HandleSingleBlast_WhenGetDataTableThrowsECNException_LogsException()
        {
            // Arrange
            SetFakesForHandleBlastMethod();
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            var exceptionLogged = false;
            _logCriticalExceptionText = new StringBuilder();
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _logCriticalExceptionText.AppendLine(note);
                    exceptionLogged = true;
                    return 1;
                };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                throw new ECNException(new List<ECNError> {
                    new ECNError { ErrorMessage = UTException }
                });
            };

            // Act
            _privateECNBlastEngineObj.Invoke(HandleSingleBlastMethodName, emailFunctions);

            // Assert
            exceptionLogged.ShouldSatisfyAllConditions(
                () => exceptionLogged.ShouldBeTrue(),
                () => _logCriticalExceptionText.ToString().ShouldContain($"An exception Happened when handling Blast Engine ID = {engineID} " +
                                                                            $"and Blast Single ID = {int.MinValue}"),
                () => _logCriticalExceptionText.ToString().ShouldContain($"Validation Error: <br/>FormsSpecificAPI: {UTException}"),
                () => _sqlCommandExceuted.ShouldNotBeNullOrWhiteSpace(),
                () => _sqlCommandExceuted.ShouldContain("Update [BlastSingles] set Processed='e' where BlastSingleID = "));
        }

        [Test]
        public void HandleSingleBlast_WhenRefBlastIsNull_UpdatesSingleBlasts()
        {
            // Arrange
            const int blastId = 1;
            SetFakesForHandleBlastMethod();
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            var exceptionLogged = false;
            _logCriticalExceptionText = new StringBuilder();
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _logCriticalExceptionText.AppendLine(note);
                    exceptionLogged = true;
                    return 1;
                };

            // Act
            _privateECNBlastEngineObj.Invoke(HandleSingleBlastMethodName, emailFunctions);

            // Assert
            exceptionLogged.ShouldBeFalse();
            _sqlCommandExceuted.ShouldNotBeNullOrWhiteSpace();
            _sqlCommandExceuted.ShouldContain($"Update BlastSingles set Processed='U' where BlastSingleID={blastId}");
        }
    }
}
