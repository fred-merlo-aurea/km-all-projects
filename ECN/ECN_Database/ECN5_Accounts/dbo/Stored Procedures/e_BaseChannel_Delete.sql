create PROCEDURE [dbo].[e_BaseChannel_Delete]   
@BaseChannelID int,
@UserID int

AS
	Update BaseChannel SET IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate = GETDATE()  where  BaseChannelID = @BaseChannelID
