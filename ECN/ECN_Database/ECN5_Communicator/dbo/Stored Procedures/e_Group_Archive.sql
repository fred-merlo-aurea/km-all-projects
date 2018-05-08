CREATE PROCEDURE [dbo].[e_Group_Archive]
	@GroupID int,
	@Archive bit,
	@UserID int
AS
	UPDATE Groups 
	SET Archived = @Archive, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
	where GroupID = @GroupID
