CREATE PROCEDURE [dbo].[e_SubscriberTransformed_SelectForFileAudit]
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
			set @Query = ' Select * from SubscriberTransformed With(NoLock) where ProcessCode = ''' + @ProcessCode + ''''
			if (@StartDate is not null)
				set @Query = @Query + ' and DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''

			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''

		END
	ELSE IF (@SourceFileID > 0)
		BEGIN
			set @Query = ' Select * from SubscriberTransformed With(NoLock) where SourceFileID = ' + Cast(@SourceFileID as varchar(20))
			if (@StartDate is not null)
				set @Query = @Query + ' and DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''

			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''

		END
	ELSE
		BEGIN
			set @Query = ' Select * from SubscriberTransformed With(NoLock) '
			if (@StartDate is not null)
				set @Query = @Query + ' where DateCreated > ''' + Convert(varchar(25), @StartDate, 121) + ''''

			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and DateCreated < ''' + Convert(varchar(25), @EndDate, 121) + ''''

		END

	EXEC(@Query)

END