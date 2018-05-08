CREATE PROCEDURE [dbo].[e_SubscriptionDetails_Delete_MasterID]
	@MasterID int
AS	
BEGIN
	
	SET NOCOUNT ON
	
	DELETE 
	FROM SubscriptionDetails 
	WHERE MasterID = @MasterID

END