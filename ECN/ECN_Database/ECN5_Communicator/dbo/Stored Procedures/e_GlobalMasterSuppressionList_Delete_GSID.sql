CREATE  PROC [dbo].[e_GlobalMasterSuppressionList_Delete_GSID] 
(
	@UserID int,
    @GSID int
)
AS 
BEGIN
	Update GlobalMasterSuppressionList SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE GSID = @GSID
END