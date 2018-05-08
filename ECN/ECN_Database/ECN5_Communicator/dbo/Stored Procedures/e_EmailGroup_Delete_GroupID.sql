CREATE  PROC [dbo].[e_EmailGroup_Delete_GroupID] 
(
	@UserID int = NULL,
    @GroupID int = NULL
)
AS 
BEGIN
	Delete [EmailGroups] WHERE GroupID = @GroupID
END
