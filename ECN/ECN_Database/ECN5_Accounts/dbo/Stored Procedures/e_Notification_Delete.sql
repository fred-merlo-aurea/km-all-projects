CREATE PROCEDURE [dbo].[e_Notification_Delete]
	@NotificationID int,
	@UserID int
AS 
BEGIN
	update Notification set IsDeleted=1, UpdatedDate= GETDATE(), UpdatedUserID= @UserID
	where NotificationID= @NotificationID
END