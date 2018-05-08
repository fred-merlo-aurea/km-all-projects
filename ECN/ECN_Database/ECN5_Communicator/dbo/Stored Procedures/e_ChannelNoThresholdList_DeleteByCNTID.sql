CREATE  PROC [dbo].[e_ChannelNoThresholdList_DeleteByCNTID] 
(
	@UserID int,
    @BaseChannelID int,
    @CNTID int
)
AS 
BEGIN
	Update ChannelNoThresholdList SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE BaseChannelID = @BaseChannelID AND CNTID = @CNTID
END
