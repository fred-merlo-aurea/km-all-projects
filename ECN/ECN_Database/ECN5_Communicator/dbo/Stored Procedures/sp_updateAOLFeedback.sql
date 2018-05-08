CREATE proc [dbo].[sp_updateAOLFeedback] 
(
	@EmailID  	int, 
	@BlastID 		int
)
as
	declare @CustomerID int,
			 @grpMasterSuppID int

Begin
	
	select  @customerID = customerID from Emails where emailID = @EmailID  

	select  @grpMasterSuppID = g.groupID  from groups g where CustomerID =  @customerID and g.MasterSupression = 1	

	-- Mark as Unsubscribe in all groups
	update EmailGroups set SubscribeTypeCode  = 'U', LastChanged = getdate()  where EmailID = @EmailID and groupID <> @grpMasterSuppID

	if len(@customerID) > 0 and len(@grpMasterSuppID) > 0
	begin

	
		-- add this email to Master supression group.
		if exists(select emailID from emailgroups where emailID = @emailID and groupID = @grpMasterSuppID)
			update EmailGroups set SubscribeTypeCode  = 'S'  where EmailID = @EmailID and groupID = @grpMasterSuppID
		else
			insert into EmailGroups (EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn, LastChanged) values
			(@EmailID, @grpMasterSuppID, 'html', 'S', getdate(), getdate())
	
		-- insert into Activitylog as unsubscribed for that blast.
		if not exists(select EAID from Emailactivitylog where emailID = @emailID and BlastID = @BlastID and ActionTypeCode = 'FEEDBACK_UNSUB'  and ActionValue = 'U')
			insert into Emailactivitylog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue, ActionNotes, Processed) values
			(@EmailID, @BlastID, 'FEEDBACK_UNSUB', getdate(), 'U', 'Update from AOL FeedBack email', 'n')
	end
	
End
