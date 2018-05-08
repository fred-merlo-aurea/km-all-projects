CREATE PROCEDURE [dbo].[e_Notification_Select_NotificationID]
	@NotificationID int
AS
BEGIN
	SELECT *
	FROM Notification  WITH(NOLOCK)
	WHERE NotificationID = @NotificationID and IsDeleted=0
END
