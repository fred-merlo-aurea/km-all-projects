CREATE  PROC [dbo].[e_Email_MergeProfiles] 
(
	@OldEmailID int,
	@NewEmailID int
)
AS 
BEGIN

	insert into EmailHistory(OldEmailID, [Action], NewEmailID, OldGroupID, ActionTime) values (@OldEmailID, 'Merge', @NewEmailID, NULL, GETDATE())
	
	CREATE TABLE #tmp_grp (GroupID int)
	
	insert into #tmp_grp 	
	select GroupID from EmailGroups
	where EmailID=@OldEmailID OR EmailID=@NewEmailID
	group by GroupID
	having COUNT(GroupID)= 2
	
	--Delete EmailGroup records of old profile for each group where both old and new profile exist
	delete from EmailGroups where EmailID=@OldEmailID
	and GroupID in (select GroupID from #tmp_grp)
	
	--Delete EmailDataValues of old profile if new profile already has values defined
	if	exists (select groupdatafieldsID from emaildatavalues where (emailID = @OldEmailID or emailID = @NewEmailID) and
	not exists (select groupdatafieldsID from groupdatafields where groupdatafieldsID = emaildatavalues.groupdatafieldsID and isnull(DatafieldsetID,0) > 0) 
	group by groupdatafieldsID having count(groupdatafieldsID) > 1)
	Begin
		delete from emaildatavalues 
		where groupdatafieldsID in (select groupdatafieldsID from emaildatavalues where (emailID = @OldEmailID or emailID = @NewEmailID) 
		group by groupdatafieldsID having count(groupdatafieldsID) > 1)
		and emailID = @OldEmailID
	end
	
	update Emails set DateUpdated = GETDATE() where EmailID = @NewEmailID
	update emailgroups set emailID = @NewEmailID, LastChanged = GETDATE() where emailID = @OldEmailID	
	update emaildatavalues set emailID = @NewEmailID where emailID = @OldEmailID	
	update emailactivitylog set emailID = @NewEmailID where emailID = @OldEmailID
	update ecn_activity..BlastActivityUnSubscribes set emailID = @NewEmailID where emailID = @OldEmailID
	update ecn_activity..BlastActivitySuppressed set emailID = @NewEmailID where emailID = @OldEmailID
	update ecn_activity..BlastActivityOpens set emailID = @NewEmailID where emailID = @OldEmailID
	update ecn_activity..BlastActivityClicks set emailID = @NewEmailID where emailID = @OldEmailID
	update ecn_activity..BlastActivityBounces set emailID = @NewEmailID where emailID = @OldEmailID
	update ecn_activity..BlastActivityConversion set emailID = @NewEmailID where emailID = @OldEmailID
	update ecn_activity..BlastActivityRefer set emailID = @NewEmailID where emailID = @OldEmailID
	update ecn_activity..BlastActivitySends set emailID = @NewEmailID where emailID = @OldEmailID
	update ecn_activity..BlastActivityResends set emailID = @NewEmailID where emailID = @OldEmailID

	delete from emails where emailID = @OldEmailID	
	
	DROP TABLE #tmp_grp	
END