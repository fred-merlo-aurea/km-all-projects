CREATE proc sPMSReversalforBlockedEmails
as
declare @emailID int,
		@customerID int,
		@j int,
		@msgroupID int,
		@inMasterSuppression bit
		
set @j = 0

set nocount on


Create table #tmp_AutoMS_reverse (EmailID int not null, CustomerID int, MSGroupID int, BounceScore int, inMasterSuppression bit)

ALTER TABLE #tmp_AutoMS_reverse 
ADD PRIMARY KEY (EmailID)

insert into #tmp_AutoMS_reverse (EmailID, inMasterSuppression)
select distinct EmailID , 0
from ecn_Activity..BlastActivitybounces b with (NOLOCK) 
where b.BounceCodeID in (5,6, 17) 
and
 (
	BounceMessage like '%block%' or
	BounceMessage like '%banned%' or
	BounceMessage like '%black%' or
	BounceMessage like '%denied%' or
	BounceMessage like '%transaction failed%' or
	BounceMessage like '%rejected%' or
	BounceMessage like '%mail refused%' or
	BounceMessage like '%you are not allowed to%' or
	BounceMessage like '%not authorized%' or
	BounceMessage like '%found on one or more dnsbls see%' or
	BounceMessage like '%rbl restriction%' or
	BounceMessage like '%poor reputation%' or
	BounceMessage like '%delisted%' or
	BounceMessage like '%mxlogic%' or
	BounceMessage = 'smtp;550 #5.1.0 address rejected.'
)	

select COUNT(*) from #tmp_AutoMS_reverse

update #tmp_AutoMS_reverse
set customerID = e.customerID, bouncescore = e.bouncescore
from #tmp_AutoMS_reverse t join ECN5_COMMUNICATOR..Emails e on t.emailID = e.EmailID

update #tmp_AutoMS_reverse 
set		inMasterSuppression = case when eg.emailgroupID > 0 then 1 else 0 end, 
		MSGroupID = eg.groupID
from #tmp_AutoMS_reverse t join 
ECN5_COMMUNICATOR..EmailGroups eg on t.emailID = eg.EmailID join (select groupID from ECN5_COMMUNICATOR..groups where MasterSupression = 1) m on eg.GroupID = m.groupID

--select bc.basechannelID, bc.basechannelname, c.customerID, c.CustomerName, c.ActiveFlag, COUNT(emailID)
--from #tmp_AutoMS_reverse t join ecn5_accounts..customers c on t.customerID = c.CustomerID join ecn5_accounts..BaseChannels bc on bc.BaseChannelID = c.BaseChannelID
--where inMasterSuppression= 1
--group by bc.basechannelID, bc.basechannelname, c.customerID, c.CustomerName, c.ActiveFlag
--order by 2,4
--compute sum(count(emailID))


DECLARE c_EAL CURSOR FOR 
select emailID, msgroupID, inMasterSuppression from #tmp_AutoMS_reverse

OPEN c_EAL  

FETCH NEXT FROM c_EAL INTO @emailID, @msgroupID, @inMasterSuppression

WHILE @@FETCH_STATUS = 0  
BEGIN  

	if @inMasterSuppression = 1
	Begin
		delete from ECN_ACTIVITY.dbo.BlastActivityUnSubscribes 
		where EmailID = @emailID and UnsubscribeCodeID = 2  and Comments like '%AUTO MASTERSUPPRESSED%'
		
		delete from ecn5_communicator.dbo.EmailActivityLog 
		where EmailID = @emailID and ActionTypeCode = 'MASTSUP_UNSUB'
		
		delete from ecn5_communicator.dbo.emailgroups
		where emailID = @emailID  and GroupID = @msgroupID

		update ecn5_communicator.dbo.emailgroups
		set SubscribeTypeCode = 'S', LastChanged = GETDATE()
		where EmailID = @emailID and SubscribeTypeCode = 'M'
	end

	update ecn5_communicator.dbo.Emails set bouncescore = 0 where emailID = @emailID
	
	set @j = @j + 1

	print (convert(varchar,@j) + ' / ' + convert(varchar, getdate(), 108)) 
	
	FETCH NEXT FROM c_EAL INTO @emailID, @msgroupID, @inMasterSuppression
End

CLOSE c_EAL  
DEALLOCATE c_EAL 			

drop table #tmp_AutoMS_reverse

