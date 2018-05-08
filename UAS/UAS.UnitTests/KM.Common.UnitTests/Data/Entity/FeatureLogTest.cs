using System;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Entity;
using KM.Common.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Data.Entity
{
    [TestFixture]
    public class FeatureLogTest
    {
        private const int FeatureLogApplicationId = 42;
        private const string FeatureLogProductLine = "ProductLine";
        private const string FeatureLogTargetApp = "TargetApp";
        private const string FeatureLogEnteredBy = "EnteredBy";
        private const string FeatureLogRequestBy = "RequestBy"; 
        private const string FeatureLogUpdateBy = "UpdateBy";
        private const string FeatureLogFeatureName = "FeatureName";
        private const double FeatureLogQuotedHours = 42.24;
        private const string FeatureLogFeatureDescription = "FeatureDescription";
        private const bool FeatureLogIsStarted = true;
        private const int FeatureLogFeaturePriority = 24;
        private const string FeatureLogDevLead = "DevLead";
        private const string FeatureLogDevNotes = "DevNotes";
        private const bool FeatureLogIsCompleted = true;
        private const string FeatureLogApprovedBy = "ApproveBy";
        private const bool FeatureLogIsApporoved = false;
        private const bool FeatureLogIsQuoted = true; 
        private const int FeatureLogFLogId = 602; 
        private const string FeatureLogApplicationIdKey = "@ApplicationID";
        private const string FeatureLogProductLineKey = "@ProductLine";
        private const string FeatureLogTargetAppKey = "@TargetApp";
        private const string FeatureLogEnteredByKey = "@EnteredBy";
        private const string FeatureLogEnteredDateKey = "@EnteredDate";
        private const string FeatureLogRequestByKey = "@RequestBy";
        private const string FeatureLogRequestDateKey = "@RequestDate";
        private const string FeatureLogFeatureNameKey = "@FeatureName";
        private const string FeatureLogFetureDescriptionKey = "@FetureDescription";
        private const string FeatureLogTargetReleaseDateKey = "@TargetReleaseDate";
        private const string FeatureLogIsQuotedKey = "@IsQuoted";
        private const string FeatureLogQuotedHoursKey = "@QuotedHours";
        private const string FeatureLogIsApporovedKey = "@IsApporoved";
        private const string FeatureLogApprovedByKey = "@ApprovedBy";
        private const string FeatureLogApprovedDateKey = "@ApprovedDate";
        private const string FeatureLogIsStartedKey = "@IsStarted";
        private const string FeatureLogStartedDateKey = "@StartedDate";
        private const string FeatureLogIsCompletedKey = "@IsCompleted";
        private const string FeatureLogCompletedDateKey = "@CompletedDate";
        private const string FeatureLogDevLeadKey = "@DevLead";
        private const string FeatureLogDevNotesKey = "@DevNotes";
        private const string FeatureLogFeaturePriorityKey = "@FeaturePriority";
        private const string FeatureLogDateAddedKey = "@DateAdded"; 
        private const string FeatureLogDateUpdatedKey = "@DateUpdated"; 
        private const string FeatureLogUpdatedByKey = "@UpdatedBy"; 
        private const string FeatureLogFLogIdKey = "@FLogID";
        private const string InsertCommandText = "e_FeatureLog_Insert";
        private const string UpdateCommandText = "e_FeatureLog_Update";

        private readonly DateTime featureLogRequestDate = DateTime.MinValue;
        private readonly DateTime featureLogTargetReleaseDate = DateTime.MaxValue;
        private readonly DateTime featureLogCompletedDate = DateTime.MinValue;
        private readonly DateTime featureLogApprovedDate = DateTime.MaxValue;
        private readonly DateTime featureLogEnteredDate = DateTime.MaxValue;
        private readonly DateTime featureLogStartedDate = DateTime.MinValue;
        private readonly DateTime featureLogDateAdded = DateTime.MaxValue; 
        private readonly DateTime featureLogDateUpdated = DateTime.MaxValue;

        private IDisposable context;

        [SetUp]
        public void SetUp()
        {
            context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public void Insert_FeatureLog_CommandParametersSet()
        {
            // Arrange
            string actualCommandText = null;
            CommandType? actualCommandType = null;
            SqlParameterCollection actualParameterCollection = null;
            var featureLog = CreateFeatureLogForInsert();

            ShimDataFunctions.ExecuteScalarSqlCommandBoolean = (sqlCommand, setDefaultConnection) =>
                {
                    actualCommandText = sqlCommand.CommandText;
                    actualCommandType = sqlCommand.CommandType;
                    actualParameterCollection = sqlCommand.Parameters;
                    return 0;
                };

            // Act
            var result = FeatureLog.Insert(featureLog);
            
            // Assert
            result.ShouldBe(0);
            actualCommandText.ShouldSatisfyAllConditions(
                () => actualCommandText.ShouldBe(InsertCommandText),
                () => actualCommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualParameterCollection.Count.ShouldBe(23),
                () => actualParameterCollection[FeatureLogApplicationIdKey].Value.ShouldBe(FeatureLogApplicationId),
                () => actualParameterCollection[FeatureLogProductLineKey].Value.ShouldBe(FeatureLogProductLine),
                () => actualParameterCollection[FeatureLogTargetAppKey].Value.ShouldBe(FeatureLogTargetApp),
                () => actualParameterCollection[FeatureLogEnteredByKey].Value.ShouldBe(FeatureLogEnteredBy),
                () => actualParameterCollection[FeatureLogRequestByKey].Value.ShouldBe(FeatureLogRequestBy),
                () => actualParameterCollection[FeatureLogFeatureNameKey].Value.ShouldBe(FeatureLogFeatureName),
                () => actualParameterCollection[FeatureLogQuotedHoursKey].Value.ShouldBe(FeatureLogQuotedHours),
                () => actualParameterCollection[FeatureLogFetureDescriptionKey].Value.ShouldBe(FeatureLogFeatureDescription),
                () => actualParameterCollection[FeatureLogIsStartedKey].Value.ShouldBe(FeatureLogIsStarted),
                () => actualParameterCollection[FeatureLogFeaturePriorityKey].Value.ShouldBe(FeatureLogFeaturePriority),
                () => actualParameterCollection[FeatureLogDevLeadKey].Value.ShouldBe(FeatureLogDevLead),
                () => actualParameterCollection[FeatureLogDevNotesKey].Value.ShouldBe(FeatureLogDevNotes),
                () => actualParameterCollection[FeatureLogIsCompletedKey].Value.ShouldBe(FeatureLogIsCompleted),
                () => actualParameterCollection[FeatureLogApprovedByKey].Value.ShouldBe(FeatureLogApprovedBy),
                () => actualParameterCollection[FeatureLogIsApporovedKey].Value.ShouldBe(FeatureLogIsApporoved),
                () => actualParameterCollection[FeatureLogIsQuotedKey].Value.ShouldBe(FeatureLogIsQuoted),
                () => actualParameterCollection[FeatureLogRequestDateKey].Value.ShouldBe(featureLogRequestDate),
                () => actualParameterCollection[FeatureLogTargetReleaseDateKey].Value.ShouldBe(featureLogTargetReleaseDate),
                () => actualParameterCollection[FeatureLogCompletedDateKey].Value.ShouldBe(featureLogCompletedDate),
                () => actualParameterCollection[FeatureLogApprovedDateKey].Value.ShouldBe(featureLogApprovedDate),
                () => actualParameterCollection[FeatureLogEnteredDateKey].Value.ShouldBe(featureLogEnteredDate),
                () => actualParameterCollection[FeatureLogStartedDateKey].Value.ShouldBe(featureLogStartedDate),
                () => actualParameterCollection[FeatureLogDateAddedKey].Value.ShouldBe(featureLogDateAdded));
        }

        [Test]
        public void Update_FeatureLog_CommandParametersSet()
        {
            // Arrange
            string actualCommandText = null;
            CommandType? actualCommandType = null;
            SqlParameterCollection actualParameterCollection = null;
            var featureLog = CreateFeatureLogForInsert();
            
            ShimDataFunctions.ExecuteNonQuerySqlCommand = sqlCommand =>
                {
                    actualCommandText = sqlCommand.CommandText;
                    actualCommandType = sqlCommand.CommandType;
                    actualParameterCollection = sqlCommand.Parameters;
                    return true;
                };

            // Act
            var result = FeatureLog.Update(featureLog);
            
            // Assert
            result.ShouldBeTrue();
            actualCommandText.ShouldSatisfyAllConditions(
                () => actualCommandText.ShouldBe(UpdateCommandText),
                () => actualCommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualParameterCollection.Count.ShouldBe(23),
                () => actualParameterCollection[FeatureLogFLogIdKey].Value.ShouldBe(FeatureLogFLogId),
                () => actualParameterCollection[FeatureLogApplicationIdKey].Value.ShouldBe(FeatureLogApplicationId),
                () => actualParameterCollection[FeatureLogProductLineKey].Value.ShouldBe(FeatureLogProductLine),
                () => actualParameterCollection[FeatureLogTargetAppKey].Value.ShouldBe(FeatureLogTargetApp),
                () => actualParameterCollection[FeatureLogRequestByKey].Value.ShouldBe(FeatureLogRequestBy),
                () => actualParameterCollection[FeatureLogFeatureNameKey].Value.ShouldBe(FeatureLogFeatureName),
                () => actualParameterCollection[FeatureLogQuotedHoursKey].Value.ShouldBe(FeatureLogQuotedHours),
                () => actualParameterCollection[FeatureLogFetureDescriptionKey].Value.ShouldBe(FeatureLogFeatureDescription),
                () => actualParameterCollection[FeatureLogIsStartedKey].Value.ShouldBe(FeatureLogIsStarted),
                () => actualParameterCollection[FeatureLogFeaturePriorityKey].Value.ShouldBe(FeatureLogFeaturePriority),
                () => actualParameterCollection[FeatureLogDevLeadKey].Value.ShouldBe(FeatureLogDevLead),
                () => actualParameterCollection[FeatureLogDevNotesKey].Value.ShouldBe(FeatureLogDevNotes),
                () => actualParameterCollection[FeatureLogIsCompletedKey].Value.ShouldBe(FeatureLogIsCompleted),
                () => actualParameterCollection[FeatureLogApprovedByKey].Value.ShouldBe(FeatureLogApprovedBy),
                () => actualParameterCollection[FeatureLogIsApporovedKey].Value.ShouldBe(FeatureLogIsApporoved),
                () => actualParameterCollection[FeatureLogIsQuotedKey].Value.ShouldBe(FeatureLogIsQuoted),
                () => actualParameterCollection[FeatureLogRequestDateKey].Value.ShouldBe(featureLogRequestDate),
                () => actualParameterCollection[FeatureLogTargetReleaseDateKey].Value.ShouldBe(featureLogTargetReleaseDate),
                () => actualParameterCollection[FeatureLogCompletedDateKey].Value.ShouldBe(featureLogCompletedDate),
                () => actualParameterCollection[FeatureLogApprovedDateKey].Value.ShouldBe(featureLogApprovedDate),
                () => actualParameterCollection[FeatureLogStartedDateKey].Value.ShouldBe(featureLogStartedDate),
                () => actualParameterCollection[FeatureLogUpdatedByKey].Value.ShouldBe(FeatureLogUpdateBy),
                () => actualParameterCollection[FeatureLogDateUpdatedKey].Value.ShouldBe(featureLogDateUpdated));
        }

        private FeatureLog CreateFeatureLogForInsert()
        {
            var featureLog = new FeatureLog
            {
                FLogID = FeatureLogFLogId,
                ApplicationID = FeatureLogApplicationId,
                ProductLine = FeatureLogProductLine,
                TargetApp = FeatureLogTargetApp,
                EnteredBy = FeatureLogEnteredBy,
                EnteredDate = featureLogEnteredDate,
                RequestBy = FeatureLogRequestBy,
                RequestDate = featureLogRequestDate,
                FeatureName = FeatureLogFeatureName,
                FetureDescription = FeatureLogFeatureDescription,
                TargetReleaseDate = featureLogTargetReleaseDate,
                IsQuoted = FeatureLogIsQuoted,
                QuotedHours = FeatureLogQuotedHours,
                IsApporoved = FeatureLogIsApporoved,
                ApprovedBy = FeatureLogApprovedBy,
                ApprovedDate = featureLogApprovedDate,
                IsStarted = FeatureLogIsStarted,
                StartedDate = featureLogStartedDate,
                IsCompleted = FeatureLogIsCompleted,
                CompletedDate = featureLogCompletedDate,
                DevLead = FeatureLogDevLead,
                DevNotes = FeatureLogDevNotes,
                FeaturePriority = FeatureLogFeaturePriority,
                DateAdded = featureLogDateAdded,
                DateUpdated = featureLogDateUpdated,
                UpdatedBy = FeatureLogUpdateBy
            };

            return featureLog;
        }
    }
}
