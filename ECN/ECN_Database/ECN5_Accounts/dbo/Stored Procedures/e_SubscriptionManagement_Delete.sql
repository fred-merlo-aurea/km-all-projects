CREATE PROCEDURE [dbo].[e_SubscriptionManagement_Delete]
	@SMID int
AS
	UPDATE SubscriptionManagement
	SET IsDeleted = 1
	WHERE SubscriptionManagementID = @SMID
