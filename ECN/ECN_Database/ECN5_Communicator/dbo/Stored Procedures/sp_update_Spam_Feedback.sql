﻿CREATE proc [dbo].[sp_update_Spam_Feedback] 
(
	@ebIDs 	varchar(8000), 
	@ActionTypeCode varchar(100),
	@ActionNotes varchar(255)
)
as

INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_update_Spam_Feedback', GETDATE())

declare  	@CustomerID int,
			@grpMasterSuppID int,
			@EmailID int,
			@blastID int
		 
Begin
	
	set nocount on

	declare @e  Table 
	(
		emailID int,
		blastID	int,
		customerID	int,
		mastergroupID	int
	)

	insert into @e (emailID, blastID)
	select substring(Items, 1, charindex('|', items)-1), substring(Items, charindex('|', Items)+1, len(Items)) from dbo.fn_split(@ebIDs,',')

	update @e 
	set customerID = e.customerID	
	from @e e1 join emails e on e1.emailID = e.emailID

	update @e 
	set mastergroupID = g.groupID	
	from @e e1 join groups g on e1.customerID = g.customerID  and g.MasterSupression = 1

	-- update emailgroups for existing records
	-- for Mastersuppersiongroup  = 'S'
	-- for other groups = 'U'
	update emailgroups set SubscribeTypeCode  = case  when e1.mastergroupID = eg.groupID then 'S' else 'U' end, LastChanged = getdate() 
	from 	emailgroups eg join @e e1 on e1.emailID = eg.emailID
	
	-- insert emailgroups if master suppression not exists
	insert into EmailGroups (EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn, LastChanged)
	select distinct e1.EmailID, e1.mastergroupID, 'html', 'S', getdate(), getdate() from @e e1 left outer join emailgroups eg on e1.emailID = eg.emailID and e1.mastergroupID = eg.groupID
	where eg.emailgroupID is null and e1.mastergroupID is not null

	-- insert emailActivityLog if not exists
	insert into Emailactivitylog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue, ActionNotes, Processed) 
	select 	distinct 	e1.EmailID, e1.BlastID, @ActionTypeCode, getdate(), 'U', @ActionNotes, 'n'  
	from 	@e e1 
	--left outer join Emailactivitylog eal  on e1.emailID = eal.emailID and e1.BlastID = eal.BlastID and ActionTypeCode = @ActionTypeCode  and ActionValue = 'U'
	--where eal.EAID is null 


End