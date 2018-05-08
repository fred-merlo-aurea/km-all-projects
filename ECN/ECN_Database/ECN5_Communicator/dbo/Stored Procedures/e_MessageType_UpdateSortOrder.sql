CREATE  PROC [dbo].[e_MessageType_UpdateSortOrder] 
(
	@MessageTypeID int = NULL,
    @BaseChannelID int = NULL,
    @UserID int = NULL,
    @SortOrder int = NULL
)
AS 
BEGIN
	UPDATE MessageType SET SortOrder = @SortOrder, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
	WHERE BaseChannelID = @BaseChannelID AND MessageTypeID = @MessageTypeID
END
