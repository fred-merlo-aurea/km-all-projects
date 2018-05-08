create procedure ccp_Meister_AmountCollected
@SourceFileId int = 0,
@ProcessCode varchar(50) = '',
@ClientId int = 0
as
BEGIN

	set nocount on

	--execute this AFTER e_ImportToUAD sproc
	
	--get a list of all the IGrp_No for the pubcode
	declare @IGrp table
	(
		IGrp_No uniqueidentifier not null,
		SubscriptionId int null,
		AmountCollected DECIMAL(12,2) null,
		PubCode varchar(50)
	)
	Insert into @IGrp (IGrp_No,AmountCollected,PubCode)
	select distinct sf.IGrp_No,
	SUM(CASE WHEN c.Responsevalue='' THEN 0.00
						  WHEN c.Responsevalue like '%[^0-9.]%' THEN 0.00 
						  ELSE CAST(c.Responsevalue AS DECIMAL(12,2)) END)
	,sf.PubCode
	from SubscriberFinal sf with(nolock)
	join Subscriptions s on sf.IGrp_No = s.IGRP_NO
	join Pubs p with(nolock) on sf.PubCode = p.PubCode 
	join PubSubscriptions ps with(nolock) on p.PubID = ps.PubID and s.SubscriptionId = ps.SubscriptionID
	join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID and ps.SubscriptionID = psd.SubscriptionID
	join CodeSheet c with(nolock) on psd.CodesheetID = c.CodeSheetID
	where ProcessCode = @ProcessCode
	and sf.IGRP_RANK = 'M'
	and c.ResponseGroup = 'AMOUNT'
	group by sf.IGrp_No, sf.PubCode
	
	Update i 
	set i.SubscriptionId = s.SubscriptionID
	from @IGrp i
	join Subscriptions s on i.IGrp_No = s.IGRP_NO
	
		--create MasterGroup AMOUNT COLLECTED on the M record
	--first insert any Sum value into Mastercodesheet table if it doesn't exist
	select * 
	into #mcs
	from Mastercodesheet
	where ISNUMERIC(MasterValue) = 1
	
	declare @mgid int = (select MasterGroupID from MasterGroups where Name like 'AMOUNT COLLECTED')
	insert into Mastercodesheet (MasterValue,MasterDesc,MasterGroupID,MasterDesc1,EnableSearching)
	select g.AmountCollected,'$' + cast(g.AmountCollected as varchar(50)), @mgid,'',1
	from @IGrp g
	left join #mcs m on g.AmountCollected = Cast(m.MasterValue as decimal(12,2))
	where m.MasterID is null

	--now insert into SubscriberMasterValues								
	insert into SubscriberMasterValues (MasterGroupID,SubscriptionID,MastercodesheetValues)
	select mc.MasterGroupID,g.SubscriptionId,mc.MasterID
	from @IGrp g
	join #mcs mc with(nolock) on g.AmountCollected = Cast(mc.MasterValue as decimal(12,2))
	left join SubscriberMasterValues smv on  mc.MasterGroupID = smv.MasterGroupID and g.SubscriptionId = smv.SubscriptionID
	where g.SubscriptionId is not null
	and smv.MasterGroupID is null
	
	drop table #mcs

END
go