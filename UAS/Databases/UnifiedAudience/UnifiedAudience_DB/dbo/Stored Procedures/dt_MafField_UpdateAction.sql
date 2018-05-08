create procedure dt_MafField_UpdateAction
@processCode varchar(50)
as
	begin
		set nocount on
		select distinct MafField,'Replace' as 'UpdateAction'
		from SubscriberDemographicTransformed dt with(nolock)
		join SubscriberTransformed st with(nolock) on dt.STRecordIdentifier = st.STRecordIdentifier
		where st.ProcessCode = @processCode
		and dt.DemographicUpdateCodeId in (select CodeId from UAD_Lookup..Code where codeTypeId in (select codetypeid from uad_Lookup..CodeType where CodeTypeName= 'Demographic Update') and codeName = 'Replace')
		union
		select distinct MafField,'Overwrite' as 'UpdateAction'
		from SubscriberDemographicTransformed dt with(nolock)
		join SubscriberTransformed st with(nolock) on dt.STRecordIdentifier = st.STRecordIdentifier
		where st.ProcessCode = @processCode
		and dt.DemographicUpdateCodeId in (select CodeId from UAD_Lookup..Code where codeTypeId in (select codetypeid from uad_Lookup..CodeType where CodeTypeName= 'Demographic Update') and codeName = 'Overwrite')
	end
go