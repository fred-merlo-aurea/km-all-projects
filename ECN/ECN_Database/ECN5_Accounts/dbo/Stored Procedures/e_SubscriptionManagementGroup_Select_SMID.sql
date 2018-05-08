CREATE PROCEDURE [dbo].[e_SubscriptionManagementGroup_Select_SMID]
	@SMID int
AS
	SELECT * 
	FROM SubscriptionManagementGroup
	WHERE SubscriptionManagementPageID = @SMID and IsDeleted = 0
