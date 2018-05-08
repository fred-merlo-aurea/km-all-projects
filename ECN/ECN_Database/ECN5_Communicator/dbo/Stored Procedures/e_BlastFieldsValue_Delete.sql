CREATE  PROC [dbo].[e_BlastFieldsValue_Delete] 
(
	@BlastFieldsValueID int,
    @UserID int
)
AS 
BEGIN
	update BlastFieldsValue set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE()
	where BlastFieldsValueID=@BlastFieldsValueID
END