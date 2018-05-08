CREATE PROCEDURE [dbo].[e_SubscriberDemographicArchive_SelectForFileAudit]
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
			set @Query = ' Select sda.* from SubscriberDemographicArchive sda With(NoLock) ' +
						 ' join SubscriberArchive sa With(NoLock) on sda.SARecordIdentifier = sa.SARecordIdentifier where sa.ProcessCode = ''' + @ProcessCode + ''''
			if (@StartDate is not null)
				set @Query = @Query + ' and sa.DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and sa.DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END
	ELSE IF (@SourceFileID > 0)
		BEGIN
			set @Query = ' Select sda.* from SubscriberDemographicArchive sda With(NoLock) ' +
						 ' join SubscriberArchive sa With(NoLock) on sda.SARecordIdentifier = sa.SARecordIdentifier where sa.SourceFileID = ' + Cast(@SourceFileID as varchar(20))
			if (@StartDate is not null)
				set @Query = @Query + ' and sa.DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and sa.DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END
	ELSE
		BEGIN
			set @Query = ' Select sda.* from SubscriberDemographicArchive sda With(NoLock) ' +
						 ' join SubscriberArchive sa With(NoLock) on sda.SARecordIdentifier = sa.SARecordIdentifier '
			if (@StartDate is not null)
				set @Query = @Query + ' where sa.DateCreated > ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and sa.DateCreated < ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END

	EXEC(@Query)

END