CREATE  PROC [dbo].[e_GlobalMasterSuppressionList_Delete] 
(
	@UserID int,
    @EmailAddress varchar(100)
)
AS 
BEGIN
	Update ChannelMasterSuppressionList SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE EmailAddress = @EmailAddress
END