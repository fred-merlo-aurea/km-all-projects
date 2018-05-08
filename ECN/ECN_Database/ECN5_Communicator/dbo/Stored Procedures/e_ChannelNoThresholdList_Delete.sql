CREATE  PROC [dbo].[e_ChannelNoThresholdList_Delete] 
(
	@UserID int,
    @BaseChannelID int,
    @EmailAddress varchar(100)
)
AS 
BEGIN
	Update ChannelNoThresholdList SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE BaseChannelID = @BaseChannelID AND EmailAddress = @EmailAddress
END