CREATE  PROC [dbo].[e_MessageType_Delete_Single] 
(
	@UserID int = NULL,
    @MessageTypeID int = NULL,
    @BaseChannelID int = NULL
)
AS 
BEGIN
	Update MessageType SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE BaseChannelID = @BaseChannelID AND MessageTypeID = @MessageTypeID
END
