create procedure e_SubscriberDemographicFinal_Select_ProcessCode
@ProcessCode varchar(50)
as
	begin
		set nocount on
		select sdf.*
		from SubscriberDemographicFinal sdf with(nolock)
		join subscriberFinal sf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier
		where sf.ProcessCode = @ProcessCode
	end	
go
