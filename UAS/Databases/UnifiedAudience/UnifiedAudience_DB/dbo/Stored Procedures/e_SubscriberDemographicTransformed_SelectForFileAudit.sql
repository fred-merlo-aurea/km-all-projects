CREATE PROCEDURE [dbo].[e_SubscriberDemographicTransformed_SelectForFileAudit]
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
			set @Query = ' Select sdt.* from SubscriberDemographicTransformed sdt With(NoLock) ' +
						 ' join SubscriberTransformed st With(NoLock) on sdt.STRecordIdentifier = st.STRecordIdentifier where st.ProcessCode = ''' + @ProcessCode + ''''
			if (@StartDate is not null)
				set @Query = @Query + ' and st.DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and st.DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END
	ELSE IF (@SourceFileID > 0)
		BEGIN
			set @Query = ' Select sdt.* from SubscriberDemographicTransformed sdt With(NoLock) ' +
						 ' join SubscriberTransformed st With(NoLock) on sdt.STRecordIdentifier = st.STRecordIdentifier where st.SourceFileID = ' + Cast(@SourceFileID as varchar(20))
			if (@StartDate is not null)
				set @Query = @Query + ' and st.DateCreated >= ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and st.DateCreated <= ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END
	ELSE
		BEGIN
			set @Query = ' Select sdt.* from SubscriberDemographicTransformed sdt With(NoLock) ' +
						 ' join SubscriberTransformed st With(NoLock) on sdt.STRecordIdentifier = st.STRecordIdentifier '
			if (@StartDate is not null)
				set @Query = @Query + ' where st.DateCreated > ''' + Convert(varchar(25), @StartDate, 121) + ''''
			if (@StartDate is not null and @EndDate is not null)
				set @Query = @Query + ' and st.DateCreated < ''' + Convert(varchar(25), @EndDate, 121) + ''''
		END

	EXEC(@Query)

END