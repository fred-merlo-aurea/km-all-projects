CREATE  PROC [dbo].[e_ChannelMasterSuppressionList_Delete_CMSID] 
(
	@UserID int,
    @BaseChannelID int,
    @CMSID int
)
AS 
BEGIN
	Update ChannelMasterSuppressionList SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE BaseChannelID = @BaseChannelID AND CMSID = @CMSID
END