CREATE PROCEDURE [dbo].[e_SubscriberDemographicInvalid_SelectForFileAudit]
	@ProcessCode varchar(50),
	@SourceFileID int,
	@StartDate DateTime = null,
	@EndDate DateTime = null
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @Query varchar(3000) = ''

	IF (LEN(@ProcessCode) > 0)
		BEGIN
			set @Query = ' Select sdi.* from SubscriberDemographicInvalid sdi With(NoLock) ' +
						 ' join SubscriberInvalid si With(NoLock) on sdi.SIRecordIdentifier = si.SIRecordIdentifier where si.ProcessCode = ''' + @ProcessCode + ''''
			if (@StartDate is not null)
				set @Query = @Query + ' and si.DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and si.DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END
	ELSE IF (@SourceFileID > 0)
		BEGIN
			set @Query = ' Select sdi.* from SubscriberDemographicInvalid sdi With(NoLock) ' +
						 ' join SubscriberInvalid si With(NoLock) on sdi.SIRecordIdentifier = si.SIRecordIdentifier where si.SourceFileID = ' + Cast(@SourceFileID as varchar(20))
			if (@StartDate is not null)
				set @Query = @Query + ' and si.DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and si.DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END
	ELSE
		BEGIN
			set @Query = ' Select sdi.* from SubscriberDemographicInvalid sdi With(NoLock) ' +
						 ' join SubscriberInvalid si With(NoLock) on sdi.SIRecordIdentifier = si.SIRecordIdentifier '
			if (@StartDate is not null)
				set @Query = @Query + ' where si.DateCreated > ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and si.DateCreated < ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END

	EXEC(@Query)

END