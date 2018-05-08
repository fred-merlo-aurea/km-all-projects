CREATE PROCEDURE [dbo].[e_SubscriberDemographicOriginal_SelectForFileAudit]
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
			set @Query = ' Select sdo.* from SubscriberDemographicOriginal sdo With(NoLock) ' +
						 ' join SubscriberOriginal so With(NoLock) on sdo.SORecordIdentifier = so.SORecordIdentifier where so.ProcessCode = ''' + @ProcessCode + ''''
			if (@StartDate is not null)
				set @Query = @Query + ' and so.DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and so.DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END
	ELSE IF (@SourceFileID > 0)
		BEGIN
			set @Query = ' Select sdo.* from SubscriberDemographicOriginal sdo With(NoLock) ' +
						 ' join SubscriberOriginal so With(NoLock) on sdo.SORecordIdentifier = so.SORecordIdentifier where so.SourceFileID = ' + Cast(@SourceFileID as varchar(20))
			if (@StartDate is not null)
				set @Query = @Query + ' and so.DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and so.DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END
	ELSE
		BEGIN
			set @Query = ' Select sdo.* from SubscriberDemographicOriginal sdo With(NoLock) ' +
						 ' join SubscriberOriginal so With(NoLock) on sdo.SORecordIdentifier = so.SORecordIdentifier '
			if (@StartDate is not null)
				set @Query = @Query + ' where so.DateCreated > ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and so.DateCreated < ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END

	EXEC(@Query)

END