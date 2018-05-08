create procedure e_FileProcessingStat_NightlyInsert
@ProcessDate date = ''
as
BEGIN

	SET NOCOUNT ON

	if(len(@ProcessDate) = 0)
		set @ProcessDate = GETDATE()
		
	--first let's delete anything we may have
	delete FileProcessingStat where ProcessDate = @ProcessDate
	
	insert into FileProcessingStat (SourceFileId,ProfileCount,DemographicCount,ProcessDate)
	select st.SourceFileId, 
		COUNT(st.STRecordIdentifier) as 'ProfileCount',
		IsNull(COUNT(sdt.SubscriberDemographicTransformedID),0) as 'DemographicCount',
		CAST(st.DateCreated as date) as 'ProcessDate'
	from SubscriberTransformed st with(nolock)
		left outer join SubscriberDemographicTransformed sdt with(nolock) on st.STRecordIdentifier = sdt.STRecordIdentifier
	where CAST(st.DateCreated as date) = @ProcessDate
	group by st.SourceFileId,CAST(st.DateCreated as date)

END
go