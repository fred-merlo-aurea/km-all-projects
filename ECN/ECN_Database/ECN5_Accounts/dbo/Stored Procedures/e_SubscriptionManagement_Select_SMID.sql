CREATE PROCEDURE [dbo].[e_SubscriptionManagement_Select_SMID]
	@SMID int
AS
	SELECT * 
	FROM SubscriptionManagement
	WHERE SubscriptionManagementID = @SMID and IsDeleted = 0
