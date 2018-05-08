CREATE PROCEDURE dbo.e_FeatureLog_Insert
@ApplicationID int = -1,
@ProductLine nvarchar(400) = '',
@TargetApp nvarchar(200) = '',
@EnteredBy nvarchar(200) = '',
@EnteredDate datetime,
@RequestBy nvarchar(200) = '',
@RequestDate date,
@FeatureName nvarchar(800) = '',
@FetureDescription text = '',
@TargetReleaseDate date,
@IsQuoted bit = 'false',
@QuotedHours real = 0,
@IsApporoved bit = 'false',
@ApprovedBy  nvarchar(200) = '',
@ApprovedDate date,
@IsStarted bit = 'false',
@StartedDate date,
@IsCompleted bit = 'false',
@CompletedDate date,
@DevLead  nvarchar(200) = '',
@DevNotes text = '',
@FeaturePriority int = -1,
@DateAdded datetime,
@DateUpdated datetime,
@UpdatedBy  nvarchar(200) = ''
AS
INSERT INTO FeatureLog (ApplicationID,ProductLine,TargetApp,EnteredBy,EnteredDate,RequestBy,RequestDate,FeatureName,FetureDescription,TargetReleaseDate,IsQuoted,
QuotedHours,IsApporoved,ApprovedBy,ApprovedDate,IsStarted,StartedDate,IsCompleted,CompletedDate,DevLead,DevNotes,FeaturePriority,DateAdded,DateUpdated,UpdatedBy)

Values(@ApplicationID,@ProductLine,@TargetApp,@EnteredBy,@EnteredDate,@RequestBy,@RequestDate,@FeatureName,@FetureDescription,@TargetReleaseDate,@IsQuoted,
@QuotedHours,@IsApporoved,@ApprovedBy,@ApprovedDate,@IsStarted,@StartedDate,@IsCompleted,@CompletedDate,@DevLead,@DevNotes
,@FeaturePriority,@DateAdded,@DateUpdated,@UpdatedBy);SELECT @@IDENTITY;
