create proc Maint_MastersuppressionReversal
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
select distinct bab.EmailID, 0
from 
ECN_ACTIVITY..BlastActivityBounces bab with (NOLOCK) join
ecn5_communicator..emails e  with (NOLOCK) on e.emailID = bab.EmailID 
where 
customerID = 3522 and 
not 
(		
		bouncemessage like '%5.1.1%'  or
		bouncemessage like '%user unknown%' or 
		bouncemessage like '%no such user%' or
		bouncemessage like '%mailbox unavailable%' or
		bouncemessage like '%no such domain%' or
		bouncemessage like '%no mailbox here%'   or
		BounceMessage like '%dd this user doesnt have a yahoo.com account%'
)		
and
e.EmailID in (select emailID from ecn5_communicator..emailGroups where GroupID = 137025)
group by bab.EmailID
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



