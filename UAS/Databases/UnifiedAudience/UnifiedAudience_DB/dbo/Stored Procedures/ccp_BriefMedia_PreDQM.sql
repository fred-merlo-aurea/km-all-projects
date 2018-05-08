create procedure ccp_BriefMedia_PreDQM
@SourceFileId int = 0,
@ProcessCode varchar(50) = '',
@ClientId int = 0
as
BEGIN

	set nocount on

	--1. SET country_of_licensure = state_of_licensure
	--	WHERE ISNULL(state_of_Licensure,'')!='' and isnull(country,'')!='' and (country not like ('United State%') or country not like ('U.S%')) and isProcessed = 0
	declare @sol table ( STRecordIdentifier uniqueidentifier, Value varchar(max))
	insert into @sol (STRecordIdentifier,Value)
	select sdt.STRecordIdentifier,sdt.Value
	from SubscriberDemographicTransformed sdt
	join SubscriberTransformed st on sdt.STRecordIdentifier = st.STRecordIdentifier 
	where st.ProcessCode = @ProcessCode
	and sdt.MAFField = 'STATE_OF_LICENSURE'
	
	update sdt
	set sdt.Value = s.Value
	from SubscriberDemographicTransformed sdt
	join SubscriberTransformed st on sdt.STRecordIdentifier = st.STRecordIdentifier 
	join @sol s on sdt.STRecordIdentifier = s.STRecordIdentifier
	where st.ProcessCode = @ProcessCode
	and sdt.MAFField = 'country_of_licensure'
	and isnull(s.Value,'')!='' and isnull(st.Country,'')!='' and (st.Country not like ('United State%') or st.Country not like ('U.S%')) 

END
go