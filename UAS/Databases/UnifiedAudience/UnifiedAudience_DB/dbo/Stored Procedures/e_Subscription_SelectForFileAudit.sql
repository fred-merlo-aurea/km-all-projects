CREATE PROCEDURE [dbo].[e_Subscription_SelectForFileAudit]
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
			set @Query = ' Select sub.* from Subscriptions sub With(NoLock) ' +
						 ' join SubscriberFinal sf With(NoLock) on sub.IGrp_No = sf.IGrp_No where sf.ProcessCode = ''' + @ProcessCode + ''''
			if (@StartDate is not null)
				set @Query = @Query + ' and sf.DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''

			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and sf.DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END
	ELSE IF (@SourceFileID > 0)
		BEGIN
			set @Query = ' Select sub.* from Subscriptions sub With(NoLock) ' +
						 ' join SubscriberFinal sf With(NoLock) on sub.IGrp_No = sf.IGrp_No where sf.SourceFileID = ' + Cast(@SourceFileID as varchar(20))
			if (@StartDate is not null)
				set @Query = @Query + ' and sf.DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''

			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and sf.DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END
	ELSE
		BEGIN
			set @Query = ' Select sub.* from Subscriptions sub With(NoLock) ' +
						 ' join SubscriberFinal sf With(NoLock) on sub.IGrp_No = sf.IGrp_No '
			if (@StartDate is not null)
				set @Query = @Query + ' where sf.DateCreated > ''' + Convert(varchar(25), @StartDate, 121) + ''''

			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and sf.DateCreated < ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END

	EXEC(@Query)

END