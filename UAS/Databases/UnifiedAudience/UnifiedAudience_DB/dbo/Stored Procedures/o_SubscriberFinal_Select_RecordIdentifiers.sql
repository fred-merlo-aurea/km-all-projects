create procedure o_SubscriberFinal_Select_RecordIdentifiers
@ProcessCode varchar(50)
as
	begin
		set nocount on
		select st.SORecordIdentifier, sf.STRecordIdentifier, sf.SFRecordIdentifier,sf.IGrp_No
		from SubscriberFinal sf with(nolock)
		join SubscriberTransformed st with(nolock) on sf.STRecordIdentifier = st.STRecordIdentifier
		where sf.ProcessCode = @ProcessCode
	end
go
