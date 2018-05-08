CREATE  PROC [dbo].[e_EmailGroup_Delete_GroupID_EmailID] 
(
	@UserID int = NULL,
    @GroupID int = NULL,
    @EmailID int =NULL
)
AS 
BEGIN
	insert into EmailHistory(OldEmailID, [Action], NewEmailID, OldGroupID, ActionTime) values (@EmailID, 'DeleteFromGroup', NULL, @GroupID, GETDATE())

	DELETE From [EmailGroups] 
	WHERE GroupID = @GroupID and EmailID= @EmailID
END