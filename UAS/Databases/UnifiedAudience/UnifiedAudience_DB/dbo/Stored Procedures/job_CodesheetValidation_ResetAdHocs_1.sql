CREATE procedure job_CodesheetValidation_ResetAdHocs
@ClientId int,
@ProcessCode varchar(50) = ''
as
BEGIN   

	SET NOCOUNT ON 

	create table #STRecords (STRecordIdentifier uniqueidentifier)
	
	insert into #STRecords 
	select st.STRecordIdentifier
	from SubscriberDemographicTransformed sdt with(nolock)
		join SubscriberTransformed st with(nolock) on sdt.STRecordIdentifier = st.STRecordIdentifier
		join UAS..AdHocDimensionGroup ag with(nolock) on sdt.MAFField = ag.CreatedDimension 
		join UAS..AdHocDimension a with(nolock) on ag.AdHocDimensionGroupId = a.AdHocDimensionGroupId 
	where st.ProcessCode = @ProcessCode 
		and ag.ClientID = @ClientId
		and sdt.Value = a.DimensionValue
		and ag.IsActive = 'true'
		and a.IsActive = 'true'
		
	delete ImportError
	where ImportErrorID in
	(
		select ImportErrorID
		from ImportError ie with(nolock) 
			join SubscriberTransformed st with(nolock) on ie.SourceFileID = st.SourceFileID and ie.RowNumber = st.ImportRowNumber and st.ProcessCode = ie.ProcessCode and st.DateCreated = ie.DateCreated 
			join SubscriberDemographicTransformed sdt with(nolock) on sdt.STRecordIdentifier = st.STRecordIdentifier
			join #STRecords r with(nolock) on st.STRecordIdentifier = r.STRecordIdentifier
		where st.ProcessCode = @ProcessCode 
			and sdt.MAFField = ie.MAFField
			and sdt.Value = ie.BadDataRow
	)

	--Reset AdHocs
	update sdt
	set NotExists = 'false', NotExistReason=''
	from SubscriberDemographicTransformed as sdt
		join SubscriberTransformed st on sdt.STRecordIdentifier = st.STRecordIdentifier
		join UAS..AdHocDimensionGroup ag on sdt.MAFField = ag.CreatedDimension 
		join UAS..AdHocDimension a on ag.AdHocDimensionGroupId = a.AdHocDimensionGroupId 
	where st.ProcessCode = @ProcessCode 
		and ag.ClientID = @ClientId
		and sdt.Value = a.DimensionValue
		and ag.IsActive = 'true'
		and a.IsActive = 'true'

END