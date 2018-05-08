CREATE Procedure [dbo].[spInsertGABounceNEW](  
 @xmlDocument Text,
 @defaultThreshold int   
)    
as   
BEGIN  
  
	set nocount on    

	
	declare @MASTSUP_UNSUB_CodeID int
	select @MASTSUP_UNSUB_CodeID = UnsubscribeCodeID from ecn_Activity.dbo.UnsubscribeCodes where UnsubscribeCode = 'MASTSUP_UNSUB'
	  
	DECLARE @docHandle int    
	  
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlDocument
	declare @e  Table   
	(  
	EmailID int,  
	BlastID int,
	ActionNotes varchar(500),
	ActionValue varchar(255),
	customerID int,  
	mastergroupID int, 
	threshold int,
	bounceScore int
	)    
	Insert into @e
    SELECT e.EmailID, BlastID, ActionNotes, ActionValue, e.customerID, g.GroupID, (case when ISNULL(c.BounceThreshold,0) > 0 then c.BounceThreshold else (case when ISNULL(bc.BounceThreshold,0) > 0 then bc.BounceThreshold else @defaultThreshold end) end), NULL FROM OPENXML(@docHandle, N'/ROOT/BOUNCE') 
	WITH 
	(
		EmailID INT '@EmailID', 
		BlastID INT '@BlastID', 
		ActionValue varchar(255) '@BounceWeight',
		ActionNotes varchar(500) '@Signature'
	) inn join ecn5_communicator.dbo.emails e on e.emailID = inn.emailID 
		  join ecn5_accounts..[CUSTOMER] c on e.CustomerID = c.CustomerID 
		  join ecn5_accounts..[BASECHANNEL] bc on c.BaseChannelID = bc.BaseChannelID 
		  left join ecn5_communicator.dbo.groups g on e.customerID = g.customerID  and g.MasterSupression = 1	
	  
	EXEC sp_xml_removedocument @docHandle
	
	insert into ECN5_COMMUNICATOR..Emailactivitylog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue, ActionNotes, Processed)     
	SELECT EmailID, BlastID, 'bounce', getDate(), ActionValue, ActionNotes, 'Y' 
	FROM @e   

	update ecn5_communicator.dbo.emails
	set bouncescore = ISNULL(bouncescore,0) + inn.bcount
	from ecn5_communicator.dbo.emails join 
	(
		SELECT EmailID, count(EmailID) as bcount 
		FROM @e 
		where ActionValue in ('hard','hardbounce') 
		group by EmailID
	) inn on emails.emailID = inn.emailID
	
		
	--get the bounce score and create comment
	update @e   
	set BounceScore = e.BounceScore, ActionNotes = 'AUTO MASTERSUPPRESSED: Bounce Score is ' + CONVERT(varchar,e.BounceScore) + '. Bounce Threshold is ' + CONVERT(varchar,e1.threshold)
	from @e e1 join ecn5_communicator.dbo.emails e on e1.emailID = e.emailID
	
	-- update emailgroups for existing records  
	-- for Mastersuppersiongroup  = 'S'  
	-- for other groups = 'U'
	if Exists (select top 1 EmailID from @e e1 where e1.bounceScore >= e1.threshold)
	Begin 
		update ecn5_communicator.dbo.emailgroups set SubscribeTypeCode  = case  when e1.mastergroupID = eg.groupID then 'S' else 'U' end, LastChanged = getdate()   
		from  ecn5_communicator.dbo.emailgroups eg join @e e1 on e1.emailID = eg.emailID
		where e1.bounceScore >= e1.threshold
		
		-- insert emailgroups if master suppression not exists  
		insert into ecn5_communicator.dbo.EmailGroups (EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn, LastChanged)  
		select distinct e1.EmailID, e1.mastergroupID, 'html', 'S', getdate(), getdate() 
		from @e e1 left outer join ecn5_communicator.dbo.emailgroups eg on e1.emailID = eg.emailID and e1.mastergroupID = eg.groupID  
		where eg.emailgroupID is null and e1.mastergroupID is not null and e1.bounceScore >= e1.threshold
		
		-- insert emailActivityLog if not exists  
		insert into ecn5_communicator.dbo.Emailactivitylog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue, ActionNotes, Processed)   
		select  distinct  e1.EmailID, e1.BlastID, 'MASTSUP_UNSUB', getdate(), 'U', e1.ActionNotes, 'n'    
		from  @e e1 left outer join ecn_Activity.dbo.BlastActivityUnSubscribes baus  on e1.emailID = baus.emailID and e1.BlastID = baus.BlastID and UnsubscribeCodeID= @MASTSUP_UNSUB_CodeID
		where baus.EAID is null and e1.bounceScore >= e1.threshold
    End
END
