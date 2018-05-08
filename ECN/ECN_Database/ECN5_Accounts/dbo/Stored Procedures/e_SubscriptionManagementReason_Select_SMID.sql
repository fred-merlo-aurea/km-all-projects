CREATE PROCEDURE [dbo].[e_SubscriptionManagementReason_Select_SMID]
	@SMID int
AS
	SELECT * 
	FROM SubscriptionManagementReason smr with(nolock) 
	where smr.SubscriptionManagementID = @SMID and smr.IsDeleted = 0