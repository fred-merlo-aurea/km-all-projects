CREATE Procedure [dbo].[MovedToActivity_spInsertGABounceNEW](  
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
	) inn join emails e on e.emailID = inn.emailID 
		  join [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c on e.CustomerID = c.CustomerID 
		  join [ECN5_ACCOUNTS].[DBO].[BASECHANNEL] bc on c.BaseChannelID = bc.BaseChannelID 
		  left join groups g on e.customerID = g.customerID  and g.MasterSupression = 1
	  
	EXEC sp_xml_removedocument @docHandle
	
	insert into Emailactivitylog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue, ActionNotes, Processed)     
	SELECT EmailID, BlastID, 'bounce', getDate(), ActionValue, ActionNotes, 'Y' 
	FROM @e   

	update emails
	set bouncescore = ISNULL(bouncescore,0) + inn.bcount
	from emails join 
	(
		SELECT EmailID, count(EmailID) as bcount 
		FROM @e 
		where ActionValue in ('hard','hardbounce') 
		group by EmailID
	) inn on emails.emailID = inn.emailID
	
		
	--get the bounce score and create comment
	update @e   
	set BounceScore = e.BounceScore, ActionNotes = 'AUTO MASTERSUPPRESSED: Bounce Score is ' + CONVERT(varchar,e.BounceScore) + '. Bounce Threshold is ' + CONVERT(varchar,e1.threshold)
	from @e e1 join emails e on e1.emailID = e.emailID
	
	-- update emailgroups for existing records  
	-- for Mastersuppersiongroup  = 'S'  
	-- for other groups = 'U'
	if Exists (select top 1 EmailID from @e e1 where e1.bounceScore > e1.threshold)
	Begin 
		update emailgroups set SubscribeTypeCode  = case  when e1.mastergroupID = eg.groupID then 'S' else 'U' end, LastChanged = getdate()   
		from  emailgroups eg join @e e1 on e1.emailID = eg.emailID
		where e1.bounceScore > e1.threshold
		
		-- insert emailgroups if master suppression not exists  
		insert into EmailGroups (EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn, LastChanged)  
		select distinct e1.EmailID, e1.mastergroupID, 'html', 'S', getdate(), getdate() from @e e1 left outer join emailgroups eg on e1.emailID = eg.emailID and e1.mastergroupID = eg.groupID  
		where eg.emailgroupID is null and e1.mastergroupID is not null and e1.bounceScore > e1.threshold
		
		-- insert emailActivityLog if not exists  
		insert into Emailactivitylog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue, ActionNotes, Processed)   
		select  distinct  e1.EmailID, e1.BlastID, 'MASTSUP_UNSUB', getdate(), 'U', e1.ActionNotes, 'n'    
		from  @e e1 left outer join ecn_Activity.dbo.BlastActivityUnSubscribes baus  on e1.emailID = baus.emailID and e1.BlastID = baus.BlastID and UnsubscribeCodeID= @MASTSUP_UNSUB_CodeID
		where baus.EAID is null and e1.bounceScore > e1.threshold
    End
END
