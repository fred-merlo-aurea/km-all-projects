create proc [dbo].[sp_SavePubGroups](
@PubID int, 
@GroupID int)
as
BEGIN

	SET NOCOUNT ON

	 INSERT INTO [PubGroups] ([PubID], [GroupID]) 
	 VALUES (@PubID, @GroupID)

End