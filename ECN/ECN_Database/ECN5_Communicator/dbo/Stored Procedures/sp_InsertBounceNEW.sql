CREATE Procedure [dbo].[sp_InsertBounceNEW](  
 @xmlDocument Text,
 @defaultThreshold int   
)    
as   
BEGIN  
  
	set nocount on    

	--INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_InsertBounceNEW', GETDATE())

	exec [e_EmailActivityLog_InsertBounce]  @xmlDocument, @defaultThreshold
	  
--	DECLARE @docHandle int    
	  
--	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlDocument
--	declare @e  Table   
--	(  
--	EmailID int,  
--	BlastID int,
--	ActionNotes varchar(500),
--	ActionValue varchar(255),
--	customerID int,  
--	mastergroupID int, 
--	threshold int,
--	bounceScore int
--	)    
--	Insert into @e
--    SELECT e.EmailID, BlastID, ActionNotes, ActionValue, e.customerID, g.GroupID, (case when ISNULL(c.BounceThreshold,0) > 0 then c.BounceThreshold else (case when ISNULL(bc.BounceThreshold,0) > 0 then bc.BounceThreshold else @defaultThreshold end) end), 
--NULL FROM OPENXML(@docHandle, N'/ROOT/BOUNCE') 
--	WITH 
--	(
--		EmailID INT '@EmailID', 
--		BlastID INT '@BlastID', 
--		ActionValue varchar(255) '@BounceWeight',
--		ActionNotes varchar(500) '@Signature'
--	) inn join emails e on e.emailID = inn.emailID 
--		  join [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c on e.CustomerID = c.CustomerID 
--		  join [ECN5_ACCOUNTS].[DBO].[BASECHANNEL] bc on c.BaseChannelID = bc.BaseChannelID 
--		  left join groups g on e.customerID = g.customerID  and g.groupname = 'master supression'	
	  
--	EXEC sp_xml_removedocument @docHandle
	
--	update @e
--	set ActionValue = 'blocks'
--	WHERE 
--	ActionValue in ('hard','hardbounce','softbounce')	and
--	(
--	ActionNotes like '%block%' or
--	ActionNotes like '%banned%' or
--	ActionNotes like '%blacklist%' or
--	ActionNotes like '%access denied%' or
--	ActionNotes like '%transaction failed%' or
--	ActionNotes like '%permanently deferred%' or
--	--ActionNotes like '%rejected%' or
--	--ActionNotes like '%delisted%' or
--	ActionNotes like '%mail refused%' or
--	ActionNotes like '%you are not allowed to%' or
--	ActionNotes like '%not authorized%' or
--	ActionNotes like '%found on one or more dnsbls see%' or
--	ActionNotes like '%rbl restriction%' or
--	ActionNotes like '%poor reputation%'  or
--	ActionNotes like '%rejected due to spam%'or
--	ActionNotes like '%http://postmaster.info.aol.com/errors/554rlyb1.html%' or
--	ActionNotes like '%http://postmaster.info.aol.com/errors/554conb1.html%' or
--	ActionNotes like '%http://postmaster.info.aol.com/errors/421dynt1.html%' or
--	ActionNotes like '%invalid recipient - refer to error codes section at http://postmaster.cox.net/confluence/display/postmaster/error%' or
--	ActionNotes like '%aol.com esmtp not accepting connections%' or
--	ActionNotes like '%barracuda%' or
--	ActionNotes like '%mxlogic.net%'
	
--	)
	
--	update @e
--	set ActionValue = 'hardbounce'
--	WHERE 
--	ActionValue ='softbounce'	and
--	(
--	ActionNotes like '%cannot be delivered. this account has been disabled or discontinued%' 
--	)
	
--	insert into Emailactivitylog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue, ActionNotes, Processed)     
--	SELECT EmailID, BlastID, 'bounce', getDate(), ActionValue, ActionNotes, 'Y' 
--	FROM @e   

--	update emails
--	set bouncescore = ISNULL(bouncescore,0) + inn.bcount
--	from emails join 
--	(
--		SELECT EmailID, count(EmailID) as bcount 
--		FROM @e 
--		where ActionValue in ('hard','hardbounce','dnserror') 
--		group by EmailID
--	) inn on emails.emailID = inn.emailID
	
		
--	--get the bounce score and create comment
--	update @e   
--	set BounceScore = e.BounceScore, ActionNotes = 'AUTO MASTERSUPPRESSED: Bounce Score is ' + CONVERT(varchar,e.BounceScore) + '. Bounce Threshold is ' + CONVERT(varchar,(e1.threshold - 1))
--	from @e e1 join emails e on e1.emailID = e.emailID

--	--all of the below used to be e1.bounceScore > e1.threshold and we have changed to >=
--	if Exists (select top 1 EmailID from @e e1 where e1.bounceScore >= e1.threshold)
--	Begin
--		-- update emailgroups for existing records  
--		-- for Mastersuppersiongroup  = 'S'  
--		-- for other groups = 'U' 
--		update emailgroups set SubscribeTypeCode  = case  when e1.mastergroupID = eg.groupID then 'S' else 'U' end, LastChanged = getdate()   
--		from  emailgroups eg join @e e1 on e1.emailID = eg.emailID
--		where e1.bounceScore >= e1.threshold
		
--		-- insert emailgroups if master suppression not exists  
--		insert into EmailGroups (EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn, LastChanged)  
--		select distinct e1.EmailID, e1.mastergroupID, 'html', 'S', getdate(), getdate() from @e e1 left outer join emailgroups eg on e1.emailID = eg.emailID and e1.mastergroupID = eg.groupID  
--		where eg.emailgroupID is null and e1.mastergroupID is not null and e1.bounceScore >= e1.threshold
		
--		-- insert emailActivityLog if not exists  
--		insert into Emailactivitylog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue, ActionNotes, Processed)   
--		select  distinct  e1.EmailID, e1.BlastID, 'MASTSUP_UNSUB', getdate(), 'U', e1.ActionNotes, 'n'    
--		from  @e e1 left outer join Emailactivitylog eal  on e1.emailID = eal.emailID and e1.BlastID = eal.BlastID and ActionTypeCode = 'MASTSUP_UNSUB'  and eal.ActionValue = 'U'  
--		where eal.EAID is null and e1.bounceScore >= e1.threshold
--	End    
END
