create procedure o_SubscriberFinal_Select_AdmsResultCount
@processCode varchar(50)
as
	begin
		set nocount on
		--Original
		select
		(select count(so.SORecordIdentifier)  
		from SubscriberOriginal so with(nolock)
		where so.ProcessCode = @processCode) as 'OriginalProfileCount',
		(select count(sdo.SORecordIdentifier)  
		from SubscriberDemographicOriginal sdo with(nolock)
		join SubscriberOriginal so with(nolock) on so.SORecordIdentifier = sdo.SORecordIdentifier
		where so.ProcessCode = @processCode) as 'OriginalDemoCount',
		--Transformed
		(select count(st.STRecordIdentifier)
		from SubscriberTransformed st with(nolock)
		where st.ProcessCode = @processCode) as 'TransformedProfileCount',
		(select count(sdt.STRecordIdentifier)
		from SubscriberDemographicTransformed sdt with(nolock)
		join SubscriberTransformed st with(nolock) on st.STRecordIdentifier = sdt.STRecordIdentifier
		where st.ProcessCode = @processCode) as 'TransformedDemoCount',
		--Invalid
		(select count(si.SIRecordIdentifier) 
		from SubscriberInvalid si with(nolock)
		where si.ProcessCode = @processCode) as 'InvalidProfileCount',
		(select count(sdi.SIRecordIdentifier)  
		from SubscriberDemographicInvalid sdi with(nolock)
		join SubscriberInvalid si with(nolock) on si.SIRecordIdentifier = sdi.SIRecordIdentifier
		where si.ProcessCode = @processCode) as 'InvalidDemoCount',
		--Archive
		(select count(sa.SARecordIdentifier)
		from SubscriberArchive sa with(nolock)
		where sa.ProcessCode = @processCode) as 'ArchiveProfileCount',
		(select count(sda.SARecordIdentifier)
		from SubscriberDemographicArchive sda with(nolock)
		join SubscriberArchive sa with(nolock) on sa.SARecordIdentifier = sda.SARecordIdentifier
		where sa.ProcessCode = @processCode) as 'ArchiveDemoCount',
		--Final
		(select count(sf.SFRecordIdentifier)
		from SubscriberFinal sf with(nolock)
		where sf.ProcessCode = @processCode) as 'FinalProfileCount',
		(select count(sdf.SFRecordIdentifier)
		from SubscriberDemographicFinal sdf with(nolock)
		join SubscriberFinal sf with(nolock) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier
		where sf.ProcessCode = @processCode) as 'FinalDemoCount',
		--
		(select count(sf.SFRecordIdentifier)
		from SubscriberFinal sf with(nolock)
		where sf.ProcessCode = @processCode and sf.IsNewRecord = 'false') as 'MatchedRecordCount',
		(select count(s.SubscriptionID)
		from Subscriptions s with(nolock)) as 'UadConsensusCount'
	end
go